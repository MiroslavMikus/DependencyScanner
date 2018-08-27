using Autofac;
using DependencyScanner.Core;
using DependencyScanner.Core.GitClient;
using DependencyScanner.Core.NugetReference;
using DependencyScanner.Core.Nuspec;
using DependencyScanner.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Serilog;
using System.IO;

namespace DependencyScanner.Standalone
{
    public class AppModule : Module
    {
        private readonly string DebugPath = GetProgramdataPath("Debug.txt");
        private readonly string LogPath = GetProgramdataPath("Info.txt");
        private readonly string FatalPath = GetProgramdataPath("Fatal.txt");

        protected override void Load(ContainerBuilder builder)
        {
            // Services
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
                            .CreateLogger();

                Log.Logger = logger;

                return logger;
            }).As<ILogger>().SingleInstance();

            builder.RegisterType<FileScanner>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<Messenger>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<SolutionComparer>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectComparer>().InstancePerLifetimeScope();
            builder.RegisterType<NuspecComparer>().InstancePerLifetimeScope();
            builder.RegisterType<GitEngine>().InstancePerLifetimeScope();
            builder.Register<NugetReferenceScan>(a => new NugetReferenceScan(App.GetProgramdataPath())).InstancePerLifetimeScope();

            // View Models
            builder.RegisterType<MainViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<BrowseViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<ConsolidateSolutionsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<ConsolidateProjectsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<NuspecUpdaterViewModel>().InstancePerLifetimeScope();

            // View
            builder.Register(a => new MainWindow() { DataContext = a.Resolve<MainViewModel>() });
        }

        private static string GetProgramdataPath(string fileName) => Path.Combine(App.GetProgramdataPath(), fileName);
    }
}
