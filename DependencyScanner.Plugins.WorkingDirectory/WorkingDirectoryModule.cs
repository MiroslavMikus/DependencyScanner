using Autofac;

namespace Dependency.Scanner.Plugins.Wd
{
    public class WorkingDirectoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WorkingDirectoryViewModel>();
        }
    }
}
