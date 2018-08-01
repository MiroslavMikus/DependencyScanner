using Autofac;
using DependencyScanner.Core;
using DependencyScanner.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DependencyScanner.Standalone
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly string LogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DependencyScanner", "Log.txt");

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // todo log here
        }

        private void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // todo log here
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // set colors
            var theme = ThemeManager.GetAppTheme(Standalone.Properties.Settings.Default.Theme_Name);
            var accent = ThemeManager.GetAccent(Standalone.Properties.Settings.Default.Accent_Name);
            ThemeManager.ChangeAppStyle(Current, accent, theme);

            var builder = new ContainerBuilder();

            // Services
            builder.Register(a =>
            {
                return new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.File(LogPath)
                            .CreateLogger();
            });

            builder.RegisterType<FileScanner>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<Messenger>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<SolutionComparer>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectComparer>().InstancePerLifetimeScope();

            // View Models
            builder.RegisterType<MainViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<BrowseViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<ConsolidateSolutionsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<ConsolidateProjectsViewModel>().InstancePerLifetimeScope();
            
            // View
            builder.Register(a => new MainWindow() { DataContext = a.Resolve<MainViewModel>() });

            var scope = builder.Build().BeginLifetimeScope();

            var window = scope.Resolve<MainWindow>();

            window.Show();
        }
    }
}
