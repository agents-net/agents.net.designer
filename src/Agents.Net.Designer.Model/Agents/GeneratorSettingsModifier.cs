using System;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Consumes(typeof(ModelVersionCreated))]
    [Produces(typeof(ModificationResult))]
    public class GeneratorSettingsModifier : Agent
    {
        private readonly MessageCollector<ModifyModel, ModelVersionCreated> collector;

        public GeneratorSettingsModifier(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<ModifyModel, ModelVersionCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ModifyModel, ModelVersionCreated> set)
        {
            set.MarkAsConsumed(set.Message1);
            if (set.Message1.Target is not GeneratorSettings generatorSettings)
            {
                return;
            }
            
            GeneratorSettings updatedModel;
            switch (set.Message1.Property)
            {
                case GeneratorSettingsPackageNamespaceProperty _:
                    updatedModel = new GeneratorSettings(set.Message1.NewValue.AssertTypeOf<string>(),
                                                         generatorSettings.GenerateAutofacModule);
                    break;
                case GeneratorSettingsGenerateAutofacProperty _:
                    updatedModel = new GeneratorSettings(generatorSettings.PackageNamespace,
                                                         set.Message1.NewValue.AssertTypeOf<bool>());
                    break;
                default:
                    throw new InvalidOperationException($"Property {set.Message1.Property} unknown for agent model.");
            }

            CommunityModel updatedCommunity = new(updatedModel,
                                                  set.Message2.Model.Agents,
                                                  set.Message2.Model.Messages);
            OnMessage(new ModificationResult(updatedCommunity, set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
