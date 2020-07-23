using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(AddAgentRequested))]
    [Consumes(typeof(AddMessageRequested))]
    [Consumes(typeof(AddGeneratorSettingsRequested))]
    [Consumes(typeof(ModelCreated))]
    [Produces(typeof(ModelUpdated))]
    public class CommandModelUpdater : Agent
    {        private readonly HashSet<Message> processedMessages = new HashSet<Message>();
        private readonly MessageCollector<AddAgentRequested, ModelCreated> addAgentCollector;
        private readonly MessageCollector<AddMessageRequested, ModelCreated> addMessageCollector;
        private readonly MessageCollector<AddGeneratorSettingsRequested, ModelCreated> addGeneratorSettingsCollector;

        public CommandModelUpdater(IMessageBoard messageBoard) : base(messageBoard)
        {
            addAgentCollector = new MessageCollector<AddAgentRequested, ModelCreated>(OnMessagesCollected);
            addMessageCollector = new MessageCollector<AddMessageRequested, ModelCreated>(OnMessagesCollected);
            addGeneratorSettingsCollector = new MessageCollector<AddGeneratorSettingsRequested, ModelCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<AddGeneratorSettingsRequested, ModelCreated> set)
        {
            lock (processedMessages)
            {
                if (!processedMessages.Add(set.Message1) ||
                    set.Message2.Model.GeneratorSettings != null)
                {
                    return;
                }
            }

            CommunityModel updatedModel = set.Message2.Model.Clone();
            updatedModel.GeneratorSettings = new GeneratorSettings();
            OnMessage(new ModelUpdated(updatedModel,set));
        }

        private void OnMessagesCollected(MessageCollection<AddMessageRequested, ModelCreated> set)
        {
            lock (processedMessages)
            {
                if (!processedMessages.Add(set.Message1))
                {
                    return;
                }
            }

            CommunityModel updatedModel = set.Message2.Model.Clone();
            updatedModel.Messages = updatedModel.Messages
                                                .Concat(new[] {new MessageModel{Name = "MessageX"}})
                                                .ToArray();
            OnMessage(new ModelUpdated(updatedModel,set));
        }

        private void OnMessagesCollected(MessageCollection<AddAgentRequested, ModelCreated> set)
        {
            lock (processedMessages)
            {
                if (!processedMessages.Add(set.Message1))
                {
                    return;
                }
            }

            CommunityModel updatedModel = set.Message2.Model.Clone();
            updatedModel.Agents = updatedModel.Agents
                                                .Concat(new[] {new AgentModel(){Name = "AgentX"}})
                                                .ToArray();
            OnMessage(new ModelUpdated(updatedModel,set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            addAgentCollector.TryPush(messageData);
            addMessageCollector.TryPush(messageData);
            addGeneratorSettingsCollector.TryPush(messageData);
        }
    }
}
