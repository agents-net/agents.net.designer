using System;
using System.Linq;
using Agents.Net.Designer.Model.Messages;
using AngleSharp.Common;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Consumes(typeof(ModelUpdated))]
    [Produces(typeof(ModelUpdated))]
    public class AgentModelModifier : Agent
    {
        private readonly MessageCollector<ModifyModel, ModelUpdated> collector;
        
        public AgentModelModifier(IMessageBoard messageBoard)
            : base(messageBoard)
        {
            collector = new MessageCollector<ModifyModel, ModelUpdated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ModifyModel, ModelUpdated> set)
        {
            set.MarkAsConsumed(set.Message1);
            if (!(set.Message1.Target is AgentModel agentModel))
            {
                return;
            }
            
            AgentModel updatedModel;
            switch (set.Message1.Property)
            {
                case AgentNameProperty _:
                    updatedModel = agentModel.Clone(name:set.Message1.NewValue.AssertTypeOf<string>());
                    break;
                case AgentNamespaceProperty _:
                    updatedModel = agentModel.Clone(@namespace:set.Message1.NewValue.AssertTypeOf<string>());
                    break;
                case AgentConsumingMessagesProperty _:
                    updatedModel = agentModel.Clone(consumingMessages:ModifyMessages(set.Message1, agentModel.ConsumingMessages));
                    break;
                case AgentProducedMessagesProperty _:
                    updatedModel = agentModel.Clone(producedMessages:ModifyMessages(set.Message1, agentModel.ProducedMessages));
                    break;
                case InterceptorAgentInterceptingMessagesProperty _:
                    InterceptorAgentModel interceptorAgentModel = (InterceptorAgentModel) agentModel;
                    updatedModel = interceptorAgentModel.Clone(ModifyMessages(set.Message1, interceptorAgentModel.InterceptingMessages));
                    break;
                case AgentIncomingEventsProperty _:
                    updatedModel = agentModel.Clone(incomingEvents:ModifyEvents(set.Message1, agentModel.IncomingEvents));
                    break;
                case AgentProducedEventsProperty _:
                    updatedModel = agentModel.Clone(producedEvents:ModifyEvents(set.Message1, agentModel.ProducedEvents));
                    break;
                default:
                    throw new InvalidOperationException($"Property {set.Message1.Property} unknown for agent model.");
            }

            CommunityModel updatedCommunity = new CommunityModel(set.Message2.Model.GeneratorSettings,
                                                                 ReplaceAgent(),
                                                                 set.Message2.Model.Messages);
            OnMessage(new ModelUpdated(updatedCommunity, set));
            
            AgentModel[] ReplaceAgent()
            {
                AgentModel[] agents = new AgentModel[set.Message2.Model.Agents.Length];
                Array.Copy(set.Message2.Model.Agents, agents, agents.Length);
                int agentIndex = Array.IndexOf(agents, agentModel);
                if (agentIndex < 0)
                {
                    throw new InvalidOperationException("Could not find agent model in community.");
                }

                agents[agentIndex] = updatedModel;
                return agents;
            }
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