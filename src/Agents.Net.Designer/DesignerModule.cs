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
            builder.RegisterType<SelectedObjectTranslator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonModelSerializer>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<CommandModelUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<FileVerifier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonFileCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonFileLoader>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonFileSynchronizer>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModelAnalyser>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<DirectoryCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModelObjectParser>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<FilesGeneratedAggregator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<CodeFileGenerator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<PathCompiler>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TemplateFileLoader>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TemplatesAggregator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TemplatesFinder>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<GraphToSvgConverter>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<AutofacModuleGenerator>().As<Agent>().InstancePerLifetimeScope();
        }
    }
}
