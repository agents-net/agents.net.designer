using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net.Designer.CodeGenerator.Messages;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.CodeGenerator.Agents
{
    [Consumes(typeof(GenerateFilesRequested))]
    [Consumes(typeof(ModelVersionCreated))]
    [Produces(typeof(ModelInvalid))]
    [Produces(typeof(AgentModelSelectedForGeneration))]
    [Produces(typeof(InterceptorAgentModelSelectedForGeneration))]
    [Produces(typeof(MessageDecoratorSelectedForGeneration))]
    [Produces(typeof(ModelSelectedForGeneration))]
    [Produces(typeof(MessageModelSelectedForGeneration))]
    [Produces(typeof(GeneratorSettingsDefined))]
    public class ModelAnalyser : Agent
    {
        private readonly MessageCollector<GenerateFilesRequested, ModelVersionCreated> collector;

        public ModelAnalyser(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<GenerateFilesRequested, ModelVersionCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<GenerateFilesRequested, ModelVersionCreated> set)
        {
            set.MarkAsConsumed(set.Message1);
            List<Message> messages = new();
            List<string> errors = new();
            foreach (AgentModel agentModel in set.Message2.Model.Agents)
            {
                List<MessageModel> consuming = new();
                List<MessageModel> producing = new();
                FindMessageModels(agentModel.ConsumingMessages, agentModel, "consuming", consuming,
                                  set.Message2.Model.Messages, errors);
                FindMessageModels(agentModel.ProducedMessages, agentModel, "producing", producing,
                                  set.Message2.Model.Messages, errors);
                Message message = AgentModelSelectedForGeneration.Decorate(new ModelSelectedForGeneration(
                                                                               set.Message1.Path, set), agentModel,
                                                                           consuming.ToArray(), producing.ToArray());
                if (agentModel is InterceptorAgentModel interceptorAgent)
                {
                    List<MessageModel> intercepting = new();
                    FindMessageModels(interceptorAgent.InterceptingMessages, agentModel, "intercepting", intercepting,
                                      set.Message2.Model.Messages, errors);
                    message = InterceptorAgentModelSelectedForGeneration.Decorate((AgentModelSelectedForGeneration) message, intercepting.ToArray());
                }
                messages.Add(message);
            }

            foreach (MessageModel modelMessage in set.Message2.Model.Messages)
            {
                if (modelMessage.BuildIn || modelMessage.IsGenericInstance)
                {
                    continue;
                }

                Message message =
                    MessageModelSelectedForGeneration.Decorate(new ModelSelectedForGeneration(set.Message1.Path, set),
                                                               modelMessage);
                if (modelMessage is MessageDecoratorModel decoratorModel &&
                    decoratorModel.DecoratedMessage != default)
                {
                    MessageModel decoratedMessage = GetMessageModel(decoratorModel.DecoratedMessage,
                                                                    set.Message2.Model.Messages,
                                                                    ()=> errors.Add($"No message definition found for message decorator definition {modelMessage.FullName()} with " +
                                                                                    $"defined message decorator {decoratorModel.DecoratedMessage}"));
                    message = MessageDecoratorSelectedForGeneration.Decorate((MessageModelSelectedForGeneration) message, decoratedMessage);
                }
                messages.Add(message);
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

        private MessageModel GetMessageModel(Guid messageId, MessageModel[] availableMessages, Action errorAction)
        {
            MessageModel messageDefinition = availableMessages.FirstOrDefault(m => m.Id == messageId);
            if (messageDefinition == null)
            {
                errorAction();
                return null;
            }
            return messageDefinition;
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
                    errors.Add($"No message definition found for agent definition {agentModel.FullName()} with " + 
                               $"defined {type} message {message}");
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
