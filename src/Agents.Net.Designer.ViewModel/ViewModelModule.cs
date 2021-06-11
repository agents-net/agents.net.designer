using System;
using System.Collections.Generic;
using System.Text;
using Agents.Net.Designer.ViewModel.Agents;
using Agents.Net.Designer.ViewModel.MicrosoftGraph.Agents;
using Autofac;

namespace Agents.Net.Designer.ViewModel
{
    public class ViewModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {            
            builder.RegisterType<AgentViewModelUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<AgentViewModelWatcher>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<CommunityViewModelUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<CommunityViewModelWatcher>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<DetailsViewModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<DetailsViewModelUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<GraphViewModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<GraphViewModelUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MessageViewModelUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MessageViewModelWatcher>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModelModificationCompleter>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModelIdUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<NewItemsSelector>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<SelectedGraphObjectTranslator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<SelectedTreeItemTranslator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<SelectionDirectionDecorator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TreeItemSelector>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TreeViewModelBuilder>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TreeViewModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<GraphCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<GraphToSvgConverter>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<SearchViewModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<SearchSourceUpdater>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<SearchViewModelObserver>().As<Agent>().InstancePerLifetimeScope();
        }
    }
}
