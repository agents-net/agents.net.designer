using System;
using System.Collections.Generic;
using System.Text;
using Agents.Net.Designer.CodeGenerator.Agents;
using Agents.Net.Designer.CodeGenerator.Templates.Agents;
using Autofac;

namespace Agents.Net.Designer.CodeGenerator
{
    public class CodeGeneratorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AgentFileGenerator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<AgentModelObjectParser>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<AutofacModuleGenerator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<FilesGeneratedAggregator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MessageFileGenerator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MessageModelObjectParser>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ModelAnalyser>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<PathCompiler>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TemplateFileLoader>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TemplatesAggregator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<TemplatesFinder>().As<Agent>().InstancePerLifetimeScope();
        }
    }
}
