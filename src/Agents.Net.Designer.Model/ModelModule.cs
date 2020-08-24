using System;
using System.Collections.Generic;
using System.Text;
using Agents.Net.Designer.Model.Agents;
using Autofac;

namespace Agents.Net.Designer.Model
{
    public class ModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AgentModelModifier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<AutomaticMessageModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<CommunityModelModifier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ContainingPackageSynchronizer>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<DeleteSelectedModelObject>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<FileVerifier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<GeneratorSettingsModifier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MessageModelModifier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModelCommandExecuter>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModelLoader>().As<Agent>().InstancePerLifetimeScope();
        }
    }
}
