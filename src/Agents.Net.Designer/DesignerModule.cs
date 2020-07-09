using Agents.Net.Designer.View;
using Agents.Net.Designer.View.Agents;
using Agents.Net.Designer.ViewModel.Agents;
using Autofac;

namespace Agents.Net.Designer
{
    public class DesignerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindow>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<MessageBoard>().As<IMessageBoard>().InstancePerLifetimeScope();
            builder.RegisterType<Community>().As<Community>().InstancePerLifetimeScope();
            builder.RegisterType<JsonViewModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<GraphViewModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MainWindowDataContextProvider>().As<Agent>().InstancePerLifetimeScope();
        }
    }
}
