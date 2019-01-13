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

            builder.RegisterType<Messenger>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IService)))
                .Where(t => t.GetInterface(typeof(IService).Name) != null)
                .Except<ReportStorage>()
                .Except<NugetScanFacade>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Load and register all settings
            foreach (var t in GetTypesFromAssembly<ISettings>(Assembly.GetAssembly(typeof(MainViewModel))).Where(a => !a.IsAbstract))
            {
                builder.Register(a =>
                {
                    var manager = a.Resolve<ISettingsManager>();

                    var settings = Activator.CreateInstance(t) as ISettings;

                    return manager.Load(settings.CollectionKey, t);
                })
                .SingleInstance()
                .As(new Type[] { t, typeof(ISettings) });
            }

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
                .InstancePerLifetimeScope();

            // LiteDb
            builder.RegisterInstance<LiteDatabase>(new LiteDatabase(GetProgramdataPath("Storage.db")))
                .AsSelf();

            // Settings
            builder.RegisterType<SettingsManager>()
                .As<ISettingsManager>()
                .InstancePerLifetimeScope();

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

        internal static string GetProgramdataPath(string fileName) => Path.Combine(App.GetProgramdataPath(), fileName);

        internal static Type[] GetTypesFromAssembly<T>(Assembly assembly) where T : class
        {
            return assembly.GetTypes().Where(t.GetInterfaces().Contains(typeof(T))).ToArray();
        }
    }
}
