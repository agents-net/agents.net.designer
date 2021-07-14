using System;
using System.Collections.Generic;
using System.Text;
using Agents.Net.Designer.CodeGenerator;
using Agents.Net.Designer.FileSystem;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Serialization;
using Agents.Net.Designer.Tests.Tools.Agents;
using Agents.Net.Designer.ViewModel;
using Autofac;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.Tools.Modules
{
    internal class CompleteModule : TestBaseModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<CodeGeneratorModule>();
            builder.RegisterModule<ModelModule>();
            builder.RegisterModule<Serialization.SerializationModule>();
            builder.RegisterModule<ViewModelModule>();
            SerializationModule.RegisterAgents(builder);
        }
    }
}
