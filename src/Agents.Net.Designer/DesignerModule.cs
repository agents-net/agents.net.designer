using Agents.Net.Designer.CodeGenerator;
using Agents.Net.Designer.CodeGenerator.Agents;
using Agents.Net.Designer.CodeGenerator.Templates.Agents;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Agents;
using Agents.Net.Designer.Serialization;
using Agents.Net.Designer.Serialization.Agents;
using Agents.Net.Designer.ViewModel;
using Agents.Net.Designer.ViewModel.Agents;
using Agents.Net.Designer.ViewModel.MicrosoftGraph.Agents;
using Agents.Net.Designer.WpfView;
using Agents.Net.Designer.WpfView.Agents;
using Autofac;

namespace Agents.Net.Designer
{
    public class DesignerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //TODO exceptions are not logged properly
            //TODO Consuming message marked as implicit; Produing as multiple (Make a line below each message Name with toggle button and icon and tooltip; delete button centered over both lines)
            //TODO Feature: Statusbar Synchronized/Connected position etc.
            //TODO Feature: Connect to image
            //TODO Feature: Progressbar for file generation
            //TODO Feature: Include amodels -> Package view (Like one model per assembly)
            builder.RegisterType<MessageBoard>().As<IMessageBoard>().InstancePerLifetimeScope();
            builder.RegisterModule<CodeGeneratorModule>();
            builder.RegisterModule<ModelModule>();
            builder.RegisterModule<SerializationModule>();
            builder.RegisterModule<ViewModelModule>();
            builder.RegisterModule<WpfViewModule>();
        }
    }
}
