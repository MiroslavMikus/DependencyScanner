using Autofac;
using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyScanner.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GitInfo>().As<IGitInfo>();
        }
    }
}
