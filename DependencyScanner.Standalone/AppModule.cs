using Autofac;
using DependencyScanner.Api.Interfaces;
using DependencyScanner.Api.Services;
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
using MahApps.Metro.Controls.Dialogs;
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

            builder.RegisterInstance(DialogCoordinator.Instance);

            builder.RegisterType<ReportStorage>()
                .WithParameter(new TypedParameter(typeof(string), GetProgramdataPath("Reports")))
                .InstancePerLifetimeScope();

            #region TODO move to specific plugin dll

            builder.RegisterType<NugetScanFacade>()
                .WithParameter(new TypedParameter(typeof(string), App.ProductVersion))
                .InstancePerLifetimeScope();

            //View Models from DependencyScanner.ViewModel assembly
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(MainViewModel)))
                .Where(t => t.Name.EndsWith("ViewModel"))
                .InstancePerLifetimeScope();

            #endregion

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

        internal static string GetProgramdataPath(string fileName)
        {
            var path = Path.Combine(App.GetProgramdataPath(), fileName);

            var dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return path;
        }

        internal static Type[] GetTypesFromAssembly<T>(Assembly assembly) where T : class
        {
            return assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(T))).ToArray();
        }
    }
}
