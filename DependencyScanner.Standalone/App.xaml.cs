using Autofac;
using DependencyScanner.Core;
using DependencyScanner.Core.GitClient;
using DependencyScanner.Core.Nuspec;
using DependencyScanner.Standalone.Properties;
using DependencyScanner.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Windows;
using System.Windows.Threading;

namespace DependencyScanner.Standalone
{
    public partial class App : Application
    {
        private const string AppName = "DependencyScanner";

        public static readonly string ProductVersion = GetProductVersion();

        public static readonly string DebugPath = GetProgramdataPath("Debug.txt");
        public static readonly string LogPath = GetProgramdataPath("Info.txt");
        public static readonly string FatalPath = GetProgramdataPath("Fatal.txt");

        private static string GetProgramdataPath(string fileName) => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), AppName, fileName);

        public App()
        {
            ProfileOptimization.SetProfileRoot(GetProgramdataPath(""));
            ProfileOptimization.StartProfile("Startup.Profile");

            DispatcherHelper.Initialize();

            string[] args = Environment.GetCommandLineArgs();

            if (args.Contains("cleansettings"))
            {
                Settings.Default.Reset();
                DependencyScanner.ViewModel.Properties.Settings.Default.Reset();
            }

            Settings.Default.Upgrade();

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            SetColors();

            ILifetimeScope scope = BuildScope();

            var window = scope.Resolve<MainWindow>();

            window.Show();

            Log.Information("Starting app");
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Fatal(e.Exception, "DispatcherUnhandledException");
        }

        private void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal("CurrentDomain.UnhandledException -> {obj}", e.ExceptionObject);
        }

        private static ILifetimeScope BuildScope()
        {
            var builder = new ContainerBuilder();

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

            // View Models
            builder.RegisterType<MainViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<BrowseViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<ConsolidateSolutionsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<ConsolidateProjectsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<NuspecUpdaterViewModel>().InstancePerLifetimeScope();

            // View
            builder.Register(a => new MainWindow() { DataContext = a.Resolve<MainViewModel>() });

            var scope = builder.Build().BeginLifetimeScope();

            return scope;
        }

        private static void SetColors()
        {
            var theme = ThemeManager.GetAppTheme(Standalone.Properties.Settings.Default.Theme_Name);
            var accent = ThemeManager.GetAccent(Standalone.Properties.Settings.Default.Accent_Name);
            ThemeManager.ChangeAppStyle(Current, accent, theme);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Logger.Information("Closing app");

            base.OnExit(e);
        }

        private static string GetProductVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersionInfo.ProductVersion;
        }
    }
}
