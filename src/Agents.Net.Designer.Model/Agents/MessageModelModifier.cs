using System;
using System.Collections.Generic;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(ModelVersionCreated))]
    [Consumes(typeof(ModifyModel))]
    [Produces(typeof(ModificationResult))]
    public class MessageModelModifier : Agent
    {
        private readonly MessageCollector<ModifyModel, ModelVersionCreated> collector;

        public MessageModelModifier(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<ModifyModel, ModelVersionCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ModifyModel, ModelVersionCreated> set)
        {
            set.MarkAsConsumed(set.Message1);
            set.MarkAsConsumed(set.Message2);
            
            ExecuteCollectedMessages(set.Message1, set.Message2.Model, set);
        }

        private void ExecuteCollectedMessages(ModifyModel modifyModel, CommunityModel model, IEnumerable<Message> set)
        {
            if (modifyModel.Target is not MessageModel messageModel)
            {
                return;
            }
            
            MessageModel updatedModel;
            switch (modifyModel.Property)
            {
                case MessageNameProperty _:
                    updatedModel = messageModel.Clone(name:modifyModel.NewValue.AssertTypeOf<string>());
                    break;
                case MessageNamespaceProperty _:
                    updatedModel = messageModel.Clone(@namespace:modifyModel.NewValue.AssertTypeOf<string>());
                    break;
                case MessageDecoratorDecoratedMessageProperty _:
                    MessageDecoratorModel decoratorModel = (MessageDecoratorModel) messageModel;
                    updatedModel = decoratorModel.Clone(modifyModel.NewValue.AssertTypeOf<Guid>());
                    break;
                default:
                    throw new InvalidOperationException($"Property {modifyModel.Property} unknown for agent model.");
            }

            CommunityModel updatedCommunity = new(model.GeneratorSettings,
                                                  model.Agents,
                                                  ReplaceMessage());
            OnMessage(new ModificationResult(updatedCommunity, set));
            
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

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
