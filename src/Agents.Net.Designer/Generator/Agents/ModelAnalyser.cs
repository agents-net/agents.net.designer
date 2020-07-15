using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Generator.Messages;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Generator.Agents
{
    public class ModelAnalyser : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition ModelAnalyserDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      GenerateFilesRequested.GenerateFilesRequestedDefinition,
                                      ModelCreated.ModelCreatedDefinition
                                  },
                                  new []
                                  {
                                      ModelInvalid.ModelInvalidDefinition
                                  });

        #endregion

        private readonly MessageCollector<GenerateFilesRequested, ModelCreated> collector;
        private readonly HashSet<GenerateFilesRequested> processedCommands = new HashSet<GenerateFilesRequested>();

        public ModelAnalyser(IMessageBoard messageBoard) : base(ModelAnalyserDefinition, messageBoard)
        {
            collector = new MessageCollector<GenerateFilesRequested, ModelCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<GenerateFilesRequested, ModelCreated> set)
        {
            lock (processedCommands)
            {
                if (!processedCommands.Add(set.Message1))
                {
                    return;
                }
            }

            List<Message> messages = new List<Message>();
            List<string> errors = new List<string>();
            foreach (AgentModel agentModel in set.Message2.Model.Agents)
            {
                List<MessageModel> consuming = new List<MessageModel>();
                List<MessageModel> producing = new List<MessageModel>();
                FindMessageModels(agentModel.ConsumingMessages, agentModel, "consuming", consuming);
                FindMessageModels(agentModel.ProducedMessages, agentModel, "producing", producing);
                messages.Add(new AgentModelSelectedForGeneration(agentModel,consuming.ToArray(),producing.ToArray(),set, 
                                     new ModelSelectedForGeneration(set.Message1.Path, set.Message2.Model, set)));
            }

            foreach (MessageModel modelMessage in set.Message2.Model.Messages)
            {
                messages.Add(new MessageModelSelectedForGeneration(modelMessage, set,
                                     new ModelSelectedForGeneration(set.Message1.Path, set.Message2.Model, set)));
            }

            if (errors.Any())
            {
                OnMessage(new ModelInvalid(errors.ToArray(), set));
            }
            else
            {
                OnMessages(messages);
            }

            void FindMessageModels(IEnumerable<string> messageDefinitions, AgentModel agentModel, string type, List<MessageModel> messageModels)
            {
                foreach (string message in messageDefinitions)
                {
                    MessageModel messageDefinition = set.Message2.Model.Messages
                                              .FirstOrDefault(m => m.FullName(set.Message2.Model)
                                                                    .EndsWith(message,
                                                                              StringComparison.Ordinal));
                    if (messageDefinition == null)
                    {
                        errors.Add(
                            $"No message definition found for agent definition {agentModel.FullName(set.Message2.Model)} with " +
                            $"defined {type} message {message}");
                    }
                    else
                    {
                        messageModels.Add(messageDefinition);
                    }
                }
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
