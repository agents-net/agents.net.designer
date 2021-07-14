using Agents.Net.Designer.FileSystem.Agents;
using Autofac;

namespace Agents.Net.Designer.FileSystem
{
    public class FileSystemModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DirectoryManipulator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<FileManipulator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<FilePathCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<FileVerifier>().As<Agent>().InstancePerLifetimeScope();
        }
    }
}