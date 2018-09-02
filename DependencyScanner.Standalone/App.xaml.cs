using Autofac;
using DependencyScanner.Core;
using DependencyScanner.Core.GitClient;
using DependencyScanner.Core.NugetReference;
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
        internal static string GetProgramdataPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), AppName);

        public App()
        {
            ProfileOptimization.SetProfileRoot(GetProgramdataPath());
            ProfileOptimization.StartProfile("Startup.Profile");

            DispatcherHelper.Initialize();

            string[] args = Environment.GetCommandLineArgs();

            if (args.Contains("cleansettings"))
            {
                Settings.Default.Reset();
                DependencyScanner.ViewModel.Properties.Settings.Default.Reset();
            }

            Settings.Default.Upgrade();
            DependencyScanner.ViewModel.Properties.Settings.Default.Upgrade();

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

            builder.RegisterModule(new AppModule());

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
