using Autofac;
using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core;
using DependencyScanner.Core.GitClient;
using DependencyScanner.Core.Interfaces;
using DependencyScanner.Core.NugetReference;
using DependencyScanner.Core.Nuspec;
using DependencyScanner.Core.Tools;
using DependencyScanner.Standalone.Properties;
using DependencyScanner.Standalone.Setting;
using DependencyScanner.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
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

        internal ILifetimeScope GlobalScope { get; private set; }

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

                File.Delete(AppModule.GetProgramdataPath("Storage.db"));
            }

            Settings.Default.Upgrade();

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            SetColors();

            var startResult = WatchExtensions.Measure(() =>
            {
                GlobalScope = BuildScope();

                var window = GlobalScope.Resolve<MainWindow>();

                window.Show();
            });

            Log.Information($"App started. Composition tooks: {startResult.TotalMilliseconds} ms.");
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

            builder.RegisterModule(new CompositionModule());

            builder.RegisterModule(new AppModule());

            return builder.Build();
        }

        private static void SetColors()
        {
            var theme = ThemeManager.GetAppTheme(Settings.Default.Theme_Name);
            var accent = ThemeManager.GetAccent(Settings.Default.Accent_Name);
            ThemeManager.ChangeAppStyle(Current, accent, theme);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Logger.Information("Closing app");

            // Save settings
            var manager = GlobalScope.Resolve<ISettingsManager>();

            foreach (var settings in GlobalScope.Resolve<IEnumerable<ISettings>>())
            {
                manager.Save(settings);
            }

            GlobalScope.Dispose();

            base.OnExit(e);
        }

        private static string GetProductVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        }
    }
}
