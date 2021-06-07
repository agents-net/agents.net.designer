using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(ModelVersionCreated))]
    [Consumes(typeof(ModifyModel))]
    [Produces(typeof(ModificationResult))]
    public class AgentModelModifier : Agent
    {
        private readonly MessageCollector<ModifyModel, ModelVersionCreated> collector;
        
        public AgentModelModifier(IMessageBoard messageBoard)
            : base(messageBoard)
        {
            collector = new MessageCollector<ModifyModel, ModelVersionCreated>(OnMessagesCollected);
        }

        private void ExecuteCollectedMessages(ModifyModel modifyModel, CommunityModel model, IEnumerable<Message> set)
        {
            if (modifyModel.Target is not AgentModel agentModel)
            {
                return;
            }
            
            AgentModel updatedModel;
            switch (modifyModel.Property)
            {
                case AgentNameProperty _:
                    updatedModel = agentModel.Clone(name:modifyModel.NewValue.AssertTypeOf<string>());
                    break;
                case AgentNamespaceProperty _:
                    updatedModel = agentModel.Clone(@namespace:modifyModel.NewValue.AssertTypeOf<string>());
                    break;
                case AgentConsumingMessagesProperty _:
                    updatedModel = agentModel.Clone(consumingMessages:ModifyMessages(modifyModel, agentModel.ConsumingMessages));
                    break;
                case AgentProducedMessagesProperty _:
                    updatedModel = agentModel.Clone(producedMessages:ModifyMessages(modifyModel, agentModel.ProducedMessages));
                    break;
                case InterceptorAgentInterceptingMessagesProperty _:
                    InterceptorAgentModel interceptorAgentModel = (InterceptorAgentModel) agentModel;
                    updatedModel = interceptorAgentModel.Clone(ModifyMessages(modifyModel, interceptorAgentModel.InterceptingMessages));
                    break;
                case AgentIncomingEventsProperty _:
                    updatedModel = agentModel.Clone(incomingEvents:ModifyEvents(modifyModel, agentModel.IncomingEvents));
                    break;
                case AgentProducedEventsProperty _:
                    updatedModel = agentModel.Clone(producedEvents:ModifyEvents(modifyModel, agentModel.ProducedEvents));
                    break;
                default:
                    throw new InvalidOperationException($"Property {modifyModel.Property} unknown for agent model.");
            }

            CommunityModel updatedCommunity = new(model.GeneratorSettings,
                                                  ReplaceAgent(),
                                                  model.Messages);
            OnMessage(new ModificationResult(updatedCommunity, set));
            
            AgentModel[] ReplaceAgent()
            {
                AgentModel[] agents = new AgentModel[model.Agents.Length];
                Array.Copy(model.Agents, agents, agents.Length);
                int agentIndex = agents.TakeWhile(a => a.Id != agentModel.Id).Count();
                if (agentIndex == agents.Length)
                {
                    throw new InvalidOperationException("Could not find agent model in community.");
                }

                agents[agentIndex] = updatedModel;
                return agents;
            }
        }

        private void OnMessagesCollected(MessageCollection<ModifyModel, ModelVersionCreated> set)
        {
            set.MarkAsConsumed(set.Message1);
            set.MarkAsConsumed(set.Message2);
            
            ExecuteCollectedMessages(set.Message1, set.Message2.Model, set);
        }

        private string[] ModifyEvents(ModifyModel modifyModel, string[] events)
        {
            if (events == null)
            {
                events = new string[0];
            }
            switch (modifyModel.ModificationType)
            {
                case ModelModification.Add:
                    return events.Concat(new[] {modifyModel.NewValue.AssertTypeOf<string>()})
                                 .ToArray();
                case ModelModification.Remove:
                    return events.Except(new[] {modifyModel.OldValue.AssertTypeOf<string>()})
                                 .ToArray();
                case ModelModification.Change:
                    return events.Except(new[] {modifyModel.OldValue.AssertTypeOf<string>()})
                                 .Concat(new[] {modifyModel.NewValue.AssertTypeOf<string>()})
                                 .ToArray();
                default:
                    throw new InvalidOperationException($"What? {modifyModel.ModificationType}");
            }
        }

        private Guid[] ModifyMessages(ModifyModel modifyModel, Guid[] originalMessages)
        {
            switch (modifyModel.ModificationType)
            {
                case ModelModification.Add:
                    return AddMessage(originalMessages, modifyModel.NewValue.AssertTypeOf<Guid>());
                case ModelModification.Remove:
                    return RemoveMessage(originalMessages, modifyModel.OldValue.AssertTypeOf<Guid>());
                case ModelModification.Change:
                    return RemoveMessage(AddMessage(originalMessages,
                                                    modifyModel.NewValue.AssertTypeOf<Guid>()),
                                         modifyModel.OldValue.AssertTypeOf<Guid>());
                default:
                    throw new InvalidOperationException($"What? {modifyModel.ModificationType}");
            }

            Guid[] AddMessage(Guid[] messages, Guid addedMessage)
            {
                return messages.Concat(new[] {addedMessage}).ToArray();
            }

            Guid[] RemoveMessage(Guid[] messages, Guid removedMessage)
            {
                return messages.Except(new[] {removedMessage}).ToArray();
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}