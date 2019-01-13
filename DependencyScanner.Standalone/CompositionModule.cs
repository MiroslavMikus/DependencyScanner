using Autofac;
using Autofac.Integration.Mef;
using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Nuspec;
using DependencyScanner.Standalone.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Standalone
{
    /// <summary>
    /// Search and register all <see cref="IPlugin"/>, <see cref="ISettings"/>, <see cref="IService"/>
    /// </summary>
    public class CompositionModule : Autofac.Module
    {
        private const string _pluginDir = "plugins";

        protected override void Load(ContainerBuilder builder)
        {
            var pluginAssemblies = Directory.GetFiles(_pluginDir, "*.dll", SearchOption.AllDirectories)
                .Select(a => Path.GetFullPath(a))
                .Select(a => Assembly.LoadFile(a))
                .ToArray();

            var allAssemblies = pluginAssemblies
                .Concat(new[]
                {
                    Assembly.GetAssembly(typeof(MainWindow)),
                    Assembly.GetAssembly(typeof(NuspecComparer))
                })
                .ToArray();

            // register all plugins

            builder.RegisterAssemblyTypes(allAssemblies)
                .Where(t => t.GetInterfaces().Contains(typeof(IPlugin)) && !t.IsAbstract)
                .As<IPlugin>()
                .InstancePerLifetimeScope();

            // register all services
            builder.RegisterAssemblyTypes(allAssemblies)
                .Where(t => t.GetInterfaces().Contains(typeof(IService)) && !t.IsAbstract)
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // load settings from all assemblies
            foreach (var t in allAssemblies.SelectMany(a => AppModule.GetTypesFromAssembly<ISettings>(a)).Where(a => !a.IsAbstract))
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

            // register all additional modules
            builder.RegisterAssemblyModules(pluginAssemblies);

            base.Load(builder);
        }
    }
}
