using System;
using System.Collections.Generic;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Produces(typeof(ModificationResult))]
    public class MessageModelModifier : Agent
    {
        public MessageModelModifier(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            ModifyModel modifyModel = messageData.Get<ModifyModel>();
            CommunityModel model = modifyModel.CurrentVersion;
            if (modifyModel.Modification.Target is not MessageModel messageModel)
            {
                return;
            }
            
            MessageModel updatedModel;
            switch (modifyModel.Modification.Property)
            {
                case MessageNameProperty _:
                    updatedModel = messageModel.Clone(name:modifyModel.Modification.NewValue.AssertTypeOf<string>());
                    break;
                case MessageNamespaceProperty _:
                    updatedModel = messageModel.Clone(@namespace:modifyModel.Modification.NewValue.AssertTypeOf<string>());
                    break;
                case MessageDecoratorDecoratedMessageProperty _:
                    MessageDecoratorModel decoratorModel = (MessageDecoratorModel) messageModel;
                    updatedModel = decoratorModel.Clone(modifyModel.Modification.NewValue.AssertTypeOf<Guid>());
                    break;
                default:
                    throw new InvalidOperationException($"Property {modifyModel.Modification.Property} unknown for agent model.");
            }

            CommunityModel updatedCommunity = new(model.GeneratorSettings,
                                                  model.Agents,
                                                  ReplaceMessage());
            OnMessage(new ModificationResult(updatedCommunity, messageData));
            
            MessageModel[] ReplaceMessage()
            {
                MessageModel[] messages = new MessageModel[model.Messages.Length];
                Array.Copy(model.Messages, messages, messages.Length);
                int messageIndex = Array.IndexOf(messages, messageModel);
                if (messageIndex < 0)
                {
                    throw new InvalidOperationException("Could not find agent model in community.");
                }

                messages[messageIndex] = updatedModel;
                return messages;
            }
        }
    }
}
