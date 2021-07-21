#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Tests.Tools.Agents;
using Autofac;

namespace Agents.Net.Designer.Tests.Tools.Modules
{
    internal class SerializationModule : TestBaseModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<ModelModule>();
            builder.RegisterModule<Serialization.SerializationModule>();
            RegisterAgents(builder);
        }

        public static void RegisterAgents(ContainerBuilder builder)
        {
            builder.RegisterType<TestFileSynchronizer>().As<Agent>().InstancePerLifetimeScope();
        }
    }
}
