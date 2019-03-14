using Autofac;
using DependencyScanner.Api.Model;
using DependencyScanner.Plugins.Wd.Components.Working_Directory;

namespace DependencyScanner.Plugins.Wd
{
    public class WorkingDirectoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WorkingDirectoryViewModel>();
            builder.RegisterType<WorkingDirectory>().As<IWorkingDirectory>();
        }
    }
}
