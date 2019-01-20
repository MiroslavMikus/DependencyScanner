using Autofac;
using DependencyScanner.Api.Model;
using DependencyScanner.Plugins.Wd.Model;

namespace Dependency.Scanner.Plugins.Wd
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
