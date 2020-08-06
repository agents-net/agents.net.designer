using System;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Consumes(typeof(ModelUpdated))]
    [Produces(typeof(ModelUpdated))]
    public class MessageModelModifier : Agent
    {
        private readonly MessageCollector<ModifyModel, ModelUpdated> collector;

        public MessageModelModifier(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<ModifyModel, ModelUpdated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ModifyModel, ModelUpdated> set)
        {
            set.MarkAsConsumed(set.Message1);
            if (!(set.Message1.Target is MessageModel messageModel))
            {
                return;
            }
            
            MessageModel updatedModel;
            switch (set.Message1.Property)
            {
                case MessageNameProperty _:
                    updatedModel = messageModel.Clone(name:set.Message1.NewValue.AssertTypeOf<string>());
                    break;
                case MessageNamespaceProperty _:
                    updatedModel = messageModel.Clone(@namespace:set.Message1.NewValue.AssertTypeOf<string>());
                    break;
                case MessageDecoratorDecoratedMessageProperty _:
                    MessageDecoratorModel decoratorModel = (MessageDecoratorModel) messageModel;
                    updatedModel = decoratorModel.Clone(set.Message1.NewValue.AssertTypeOf<Guid>());
                    break;
                default:
                    throw new InvalidOperationException($"Property {set.Message1.Property} unknown for agent model.");
            }

            CommunityModel updatedCommunity = new CommunityModel(set.Message2.Model.GeneratorSettings,
                                                                 set.Message2.Model.Agents,
                                                                 ReplaceMessage());
            OnMessage(new ModelUpdated(updatedCommunity, set));
            
            MessageModel[] ReplaceMessage()
            {
                MessageModel[] messages = new MessageModel[set.Message2.Model.Messages.Length];
                Array.Copy(set.Message2.Model.Messages, messages, messages.Length);
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
