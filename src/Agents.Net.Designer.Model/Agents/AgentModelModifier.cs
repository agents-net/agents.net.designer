using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Produces(typeof(ModificationResult))]
    public class AgentModelModifier : Agent
    {
        public AgentModelModifier(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        private string[] ModifyEvents(ModifyModel modifyModel, string[] events)
        {
            if (events == null)
            {
                events = new string[0];
            }
            switch (modifyModel.Modification.ModificationType)
            {
                case ModificationType.Add:
                    return events.Concat(new[] {modifyModel.Modification.NewValue.AssertTypeOf<string>()})
                                 .ToArray();
                case ModificationType.Remove:
                    return events.Except(new[] {modifyModel.Modification.OldValue.AssertTypeOf<string>()})
                                 .ToArray();
                case ModificationType.Change:
                    return events.Except(new[] {modifyModel.Modification.OldValue.AssertTypeOf<string>()})
                                 .Concat(new[] {modifyModel.Modification.NewValue.AssertTypeOf<string>()})
                                 .ToArray();
                default:
                    throw new InvalidOperationException($"What? {modifyModel.Modification.ModificationType}");
            }
        }

        private Guid[] ModifyMessages(ModifyModel modifyModel, Guid[] originalMessages)
        {
            switch (modifyModel.Modification.ModificationType)
            {
                case ModificationType.Add:
                    return AddMessage(originalMessages, modifyModel.Modification.NewValue.AssertTypeOf<Guid>());
                case ModificationType.Remove:
                    return RemoveMessage(originalMessages, modifyModel.Modification.OldValue.AssertTypeOf<Guid>());
                case ModificationType.Change:
                    return RemoveMessage(AddMessage(originalMessages,
                                                    modifyModel.Modification.NewValue.AssertTypeOf<Guid>()),
                                         modifyModel.Modification.OldValue.AssertTypeOf<Guid>());
                default:
                    throw new InvalidOperationException($"What? {modifyModel.Modification.ModificationType}");
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
            ModifyModel modifyModel = messageData.Get<ModifyModel>();
            CommunityModel model = modifyModel.CurrentVersion;
            if (modifyModel.Modification.Target is not AgentModel agentModel)
            {
                return;
            }
            
            AgentModel updatedModel;
            switch (modifyModel.Modification.Property)
            {
                case AgentNameProperty _:
                    updatedModel = agentModel.Clone(name:modifyModel.Modification.NewValue.AssertTypeOf<string>());
                    break;
                case AgentNamespaceProperty _:
                    updatedModel = agentModel.Clone(@namespace:modifyModel.Modification.NewValue.AssertTypeOf<string>());
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
                    throw new InvalidOperationException($"Property {modifyModel.Modification.Property} unknown for agent model.");
            }

            CommunityModel updatedCommunity = new(model.GeneratorSettings,
                                                  ReplaceAgent(),
                                                  model.Messages);
            OnMessage(new ModificationResult(updatedCommunity, messageData));
            
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
    }
}