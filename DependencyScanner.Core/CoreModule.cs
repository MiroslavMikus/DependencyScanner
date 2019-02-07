using Autofac;
using DependencyScanner.Api.Interfaces;
using DependencyScanner.Core.Model;

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
