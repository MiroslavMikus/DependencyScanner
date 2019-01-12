using Autofac;
using Autofac.Integration.Mef;
using DependencyScanner.Api.Interfaces;
using DependencyScanner.Standalone.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Standalone
{
    public class CompositionModule : Autofac.Module
    {
        private const string _pluginDir = "plugins";

        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = Directory.GetFiles(_pluginDir, "*.dll", SearchOption.AllDirectories)
                .Select(a => Path.GetFullPath(a))
                .Select(a => Assembly.LoadFile(a))
                .ToArray();

            var assembliesBuilder = builder.RegisterAssemblyTypes(assemblies);

            // register all plugins
            assembliesBuilder
                .Where(t => t.GetInterfaces().Contains(typeof(IPlugin)))
                .As<IPlugin>()
                .InstancePerLifetimeScope();

            // register all services
            assembliesBuilder
                 .Where(t => t.GetInterfaces().Contains(typeof(IService)))
                 .AsSelf()
                 .AsImplementedInterfaces()
                 .InstancePerLifetimeScope();

            // load settings from all assemblies
            foreach (var t in assemblies.SelectMany(a => AppModule.GetTypesFromAssembly<ISettings>(a)).Where(a => !a.IsAbstract))
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
            builder.RegisterAssemblyModules(assemblies);

            base.Load(builder);
        }
    }
}
