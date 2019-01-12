using Autofac;
using Autofac.Integration.Mef;
using DependencyScanner.Api.Interfaces;
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

        //protected override void Load(ContainerBuilder builder)
        //{
        //    var catalog = new DirectoryCatalog(_pluginDir);

        //    foreach (var item in catalog)
        //    {
        //    }

        //    var test = catalog;

        //    //builder.RegisterAssemblyTypes(catalog)
        //    //    .Where(t => t.GetInterface(typeof(IPlugin).Name) != null)
        //    //    .As<IPlugin>()
        //    //    .InstancePerLifetimeScope();

        //    base.Load(builder);
        //}

        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = Directory.GetFiles(_pluginDir, "*.dll", SearchOption.AllDirectories)
                .Select(a => Path.GetFullPath(a))
                .Select(a => Assembly.LoadFile(a))
                .ToArray();

            // register all plugins
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.GetInterfaces().Contains(typeof(IPlugin)))
                //.Where(t => t.GetInterface(typeof(IPlugin).Name) != null)
                .As<IPlugin>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyModules(assemblies);

            base.Load(builder);
        }
    }
}
