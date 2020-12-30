using System;
using System.Collections.Generic;
using System.Text;
using Agents.Net.Designer.Tests.Tools.Agents;
using Autofac;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.Tools.Modules
{
    internal abstract class TestBaseModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessageBoard>().As<IMessageBoard>().InstancePerLifetimeScope();
            builder.RegisterType<InitializeMessageCatcher>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MessagePulseAgent>().As<Agent>().InstancePerLifetimeScope();
        }
    }
}