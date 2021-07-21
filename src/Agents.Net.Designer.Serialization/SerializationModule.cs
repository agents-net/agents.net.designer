#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using Agents.Net.Designer.Serialization.Agents;
using Autofac;

namespace Agents.Net.Designer.Serialization
{
    public class SerializationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileSynchronizationCoordinator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonFileCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonFileLoader>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonFileSynchronizer>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonModelParser>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<JsonModelSerializer>().As<Agent>().InstancePerLifetimeScope();
        }
    }
}
