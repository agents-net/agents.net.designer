#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

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
            builder.RegisterType<GeneratorSettingsModifier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MessageModelModifier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModelCommandExecuter>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModelLoader>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModificationBatchExecuter>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModelVersionCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<GenericsDetector>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModificationRequestExtender>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModificationRequestExtensionsCollector>().As<Agent>().InstancePerLifetimeScope();
        }
    }
}
