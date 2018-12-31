using Autofac;
using DependencyScanner.Core;
using DependencyScanner.Core.GitClient;
using DependencyScanner.Core.Interfaces;
using DependencyScanner.Core.NugetReference;
using DependencyScanner.Core.Nuspec;
using DependencyScanner.Standalone.Components;
using DependencyScanner.Standalone.Services;
using DependencyScanner.Standalone.Setting;
using GalaSoft.MvvmLight.Messaging;
using LiteDB;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DependencyScanner.Standalone
{
    public class AppModule : Autofac.Module
    {
        private readonly static string DebugPath = GetProgramdataPath("Debug.txt");
        private readonly static string LogPath = GetProgramdataPath("Info.txt");
        private readonly static string FatalPath = GetProgramdataPath("Fatal.txt");

        protected override void Load(ContainerBuilder builder)
        {
            LiteDatabase database = new LiteDatabase(GetProgramdataPath("Storage.db"));

            ISettingsManager settingsManager = new SettingsManager(database);

            // Services
            var eventSink = new EventSink(null); // -> catch all logger events and provide errors and fatals to the UI

            builder.RegisterInstance(eventSink);

            builder.Register(a =>
            {
                var logger = new LoggerConfiguration()
#if DEBUG
                            .MinimumLevel.Debug()
                            .WriteTo.Logger(l => l.MinimumLevel.Debug().WriteTo.File(DebugPath))
#else
                            .MinimumLevel.Information()
                            .WriteTo.Logger(l => l.MinimumLevel.Information().WriteTo.File(LogPath))
#endif
                            .WriteTo.Logger(l => l.WriteTo.File(FatalPath), Serilog.Events.LogEventLevel.Fatal)
                            .WriteTo.EventSink(sink: eventSink)
                            .CreateLogger();

                Log.Logger = logger;

                return logger;
            }).As<ILogger>().SingleInstance();
            builder.RegisterType<Messenger>().AsImplementedInterfaces().InstancePerLifetimeScope();

            // Core Services
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IService)))
                .Where(t => t.GetInterface(typeof(IService).Name) != null)
                .Except<ReportStorage>()
                .Except<NugetScanFacade>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // ViewModel Services
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(MainViewModel)))
                 .Where(t => t.GetInterface(typeof(IService).Name) != null)
                 .AsSelf()
                 .AsImplementedInterfaces()
                 .InstancePerLifetimeScope();

            builder.RegisterType<ReportStorage>()
                .WithParameter(new TypedParameter(typeof(string), GetProgramdataPath("Reports")))
                .InstancePerLifetimeScope();

            builder.RegisterType<NugetScanFacade>()
                .WithParameter(new TypedParameter(typeof(string), App.ProductVersion))
                .InstancePerLifetimeScope();

            //View Models from DependencyScanner.ViewModel assembly
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(MainViewModel)))
                .Where(t => t.Name.EndsWith("ViewModel"))
                .InstancePerLifetimeScope();

            // register all plugins
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(MainViewModel)))
                .Where(t => t.GetInterface(typeof(IPlugin).Name) != null)
                .Where(t => !t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IPlugin<>))) // exclude generic plugins
                .As<IPlugin>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(MainViewModel)))
                .Where(t => t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IPlugin<>)))
                .As<IPlugin>()
                .InstancePerLifetimeScope()
                .OnActivating(a =>
                {
                    ReadSettings(a, settingsManager);
                })
                .OnRelease(a =>
                {
                    SaveSettings(a, settingsManager);
                });

            // LiteDb
            builder.RegisterInstance(database);
            //builder.RegisterInstance<LiteDatabase>(new LiteDatabase(GetProgramdataPath("Storage.db")))
            //    .AsSelf();

            // Settings
            builder.RegisterInstance(settingsManager)
                .As<ISettingsManager>();
            //builder.RegisterType<SettingsManager>()
            //    .As<ISettingsManager>()
            //    .InstancePerLifetimeScope();

            // View
            builder.Register(a => new MainWindow()
            {
#if DEBUG
                DataContext = a.Resolve<MainViewModel>(new TypedParameter(typeof(string), DebugPath))
#else
                DataContext = a.Resolve<MainViewModel>(new TypedParameter(typeof(string), LogPath))
#endif
            });
        }

        private static void SaveSettings(object a, ISettingsManager settingsManager)
        {
            var iPluginType = a.GetType().GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IPlugin<>));

            if (iPluginType != null)
            {
                var settingsType = iPluginType.GetGenericArguments()[0];

                var plugin = a as IPlugin<ISettings>;

                settingsManager.Save(plugin.Settings, plugin.CollectionKey, settingsType);
            }
        }

        private static void ReadSettings(Autofac.Core.IActivatingEventArgs<object> a, ISettingsManager settingsManager)
        {
            var iPluginType = a.Instance.GetType().GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IPlugin<>));

            if (iPluginType != null)
            {
                var settingsType = iPluginType.GetGenericArguments()[0];

                var plugin = a.Instance as IPlugin<ISettings>;

                var settings = (ISettings)(settingsManager.Load(plugin.CollectionKey, settingsType));

                plugin.SetSettings(settings);
            }
        }

        private static string GetProgramdataPath(string fileName) => Path.Combine(App.GetProgramdataPath(), fileName);
    }
}
