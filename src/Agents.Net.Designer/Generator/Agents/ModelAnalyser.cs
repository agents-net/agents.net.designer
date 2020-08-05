using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Generator.Messages;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Generator.Agents
{
    [Consumes(typeof(GenerateFilesRequested))]
    [Consumes(typeof(ModelUpdated))]
    [Produces(typeof(ModelInvalid))]
    [Produces(typeof(AgentModelSelectedForGeneration))]
    [Produces(typeof(InterceptorAgentModelSelectedForGeneration))]
    [Produces(typeof(ModelSelectedForGeneration))]
    [Produces(typeof(MessageModelSelectedForGeneration))]
    [Produces(typeof(GeneratorSettingsDefined))]
    public class ModelAnalyser : Agent
    {
        private readonly MessageCollector<GenerateFilesRequested, ModelUpdated> collector;

        public ModelAnalyser(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<GenerateFilesRequested, ModelUpdated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<GenerateFilesRequested, ModelUpdated> set)
        {
            set.MarkAsConsumed(set.Message1);
            List<Message> messages = new List<Message>();
            List<string> errors = new List<string>();
            foreach (AgentModel agentModel in set.Message2.Model.Agents)
            {
                List<MessageModel> consuming = new List<MessageModel>();
                List<MessageModel> producing = new List<MessageModel>();
                FindMessageModels(agentModel.ConsumingMessages, agentModel, "consuming", consuming,
                                  set.Message2.Model.Messages, errors);
                FindMessageModels(agentModel.ProducedMessages, agentModel, "producing", producing,
                                  set.Message2.Model.Messages, errors);
                Message message = new AgentModelSelectedForGeneration(agentModel, consuming.ToArray(),
                                                                      producing.ToArray(), set,
                                                                      new ModelSelectedForGeneration(
                                                                          set.Message1.Path, set));
                if (agentModel is InterceptorAgentModel interceptorAgent)
                {
                    List<MessageModel> intercepting = new List<MessageModel>();
                    FindMessageModels(interceptorAgent.InterceptingMessages, agentModel, "intercepting", intercepting,
                                      set.Message2.Model.Messages, errors);
                    message = new InterceptorAgentModelSelectedForGeneration(intercepting.ToArray(), set, message);
                }
                messages.Add(message);
            }

            foreach (MessageModel modelMessage in set.Message2.Model.Messages)
            {
                if (modelMessage.BuildIn)
                {
                    continue;
                }
                messages.Add(new MessageModelSelectedForGeneration(modelMessage, set,
                                     new ModelSelectedForGeneration(set.Message1.Path, set)));
            }

            if (errors.Any())
            {
                OnMessage(new ModelInvalid(errors.ToArray(), set));
            }
            else
            {
                OnMessage(new GeneratorSettingsDefined(set.Message2.Model.GeneratorSettings??new GeneratorSettings(), set.Message1.Path, set));
                OnMessages(messages);
            }
        }

        private void FindMessageModels(IEnumerable<Guid> messageDefinitions, AgentModel agentModel, string type,
                                       List<MessageModel> messageModels, MessageModel[] availableMessages, 
                                       List<string> errors)
        {
            foreach (Guid message in messageDefinitions)
            {
                MessageModel messageDefinition = availableMessages.FirstOrDefault(m => m.Id == message);
                if (messageDefinition == null)
                {
                    errors.Add($"No message definition found for agent definition {agentModel.FullName()} with " + $"defined {type} message {message}");
                }
                else
                {
                    messageModels.Add(messageDefinition);
                }
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
