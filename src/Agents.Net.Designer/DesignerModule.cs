using Agents.Net.Designer.Generator.Agents;
using Agents.Net.Designer.Json.Agents;
using Agents.Net.Designer.MicrosoftGraph.Agents;
using Agents.Net.Designer.Model.Agents;
using Agents.Net.Designer.Templates.Agents;
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
            //TODO Feature: Statusbar Synchronized/Connected position etc.
            //TODO Feature: Json schema error as text box validation error.
            //TODO Feature: Avalon Edit with line breaks and highlighting.
            //TODO Feature: Block ui text update events until graph synchronized and file saved; exporting svg; generate files
            //TODO Feature: Connect to image
            //TODO Feature: Progressbar for file generation
            //TODO Feature: Include amodels -> Package view (Like one model per assembly)
            builder.RegisterType<MainWindow>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<MessageBoard>().As<IMessageBoard>().InstancePerLifetimeScope();
            builder.RegisterType<GraphViewModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MainWindowDataContextProvider>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<GraphViewModelUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonModelParser>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<GraphCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MainWindowObserver>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<SelectedGraphObjectTranslator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonModelSerializer>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModelCommandExecuter>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<FileVerifier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonFileCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonFileLoader>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonFileSynchronizer>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModelAnalyser>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<DirectoryCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MessageModelObjectParser>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<FilesGeneratedAggregator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<AgentFileGenerator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<PathCompiler>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TemplateFileLoader>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TemplatesAggregator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TemplatesFinder>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<GraphToSvgConverter>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<AutofacModuleGenerator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<CommunityModelModifier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TreeViewModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TreeViewModelBuilder>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModelLoader>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<DetailsViewModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<DetailsViewModelUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<AgentViewModelWatcher>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<AgentModelModifier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<AgentViewModelUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ViewModelChangeApplier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TreeItemSelector>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<SelectionDirectionDecorator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<AutomaticMessageModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<CommunityViewModelUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModelIdUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<FileSynchronizationCoordinator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MessageViewModelWatcher>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MessageViewModelUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MessageModelModifier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<CommunityViewModelWatcher>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<GeneratorSettingsModifier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ContainingPackageSynchronizer>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<SelectedTreeItemTranslator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<NewItemsSelector>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<DeleteSelectedModelObject>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<AgentModelObjectParser>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MessageFileGenerator>().As<Agent>().InstancePerLifetimeScope();
        }
    }
}
