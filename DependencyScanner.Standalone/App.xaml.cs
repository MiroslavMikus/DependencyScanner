using Autofac;
using DependencyScanner.Core;
using DependencyScanner.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DependencyScanner.Standalone
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();

            // Services
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
