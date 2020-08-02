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
                    updatedModel = new MessageModel(set.Message1.NewValue.AssertTypeOf<string>(),
                                                    messageModel.Namespace,
                                                    messageModel.Id,
                                                    messageModel.BuildIn);
                    break;
                case MessageNamespaceProperty _:
                    string newNamespace = set.Message1.NewValue.AssertTypeOf<string>();
                    bool clipNamespace = !string.IsNullOrEmpty(set.Message2.Model.GeneratorSettings?.PackageNamespace) &&
                                         newNamespace.StartsWith(set.Message2.Model.GeneratorSettings.PackageNamespace);
                    updatedModel = new MessageModel(messageModel.Name,
                                                    clipNamespace
                                                        ?newNamespace.Substring(set.Message2.Model.GeneratorSettings.PackageNamespace.Length)
                                                        :newNamespace,
                                                    messageModel.Id,
                                                    messageModel.BuildIn);
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
