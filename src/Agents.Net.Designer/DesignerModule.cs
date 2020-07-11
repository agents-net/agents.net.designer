using Agents.Net.Designer.Json.Agents;
using Agents.Net.Designer.MicrosoftGraph.Agents;
using Agents.Net.Designer.Model.Agents;
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
            //TODO Feature: Export picture
            //TODO Feature: Generate classes
            //TODO Feature: Statusbar Synchronized/Connected position etc.
            //TODO Feature: Block ui text update events until graph synchronized and file saved
            builder.RegisterType<MainWindow>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<MessageBoard>().As<IMessageBoard>().InstancePerLifetimeScope();
            builder.RegisterType<Community>().As<Community>().InstancePerLifetimeScope();
            builder.RegisterType<JsonViewModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<GraphViewModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MainWindowDataContextProvider>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonTextObserver>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<GraphViewModelUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonModelParser>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<GraphCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonTextExampleLoader>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MainWindowObserver>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonModelValidator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<SelectedModelObjectToSelectedTextPosition>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<SelectedObjectTranslator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TextPositionUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonTextUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonModelSerializer>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<CommandModelUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<FileVerifier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonFileCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonFileLoader>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonFileSynchronizer>().As<Agent>().InstancePerLifetimeScope();
        }
    }
}
