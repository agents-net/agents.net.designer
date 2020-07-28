using System;
using Agents.Net.Designer.Model.Messages;

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
                    updatedModel = new AgentModel(set.Message1.NewValue.AssertTypeOf<string>(),
                                                  agentModel.Namespace,
                                                  agentModel.ConsumingMessages,
                                                  agentModel.ProducedMessages,
                                                  agentModel.IncomingEvents,
                                                  agentModel.ProducedEvents,
                                                  agentModel.Id);
                    break;
                case AgentNamespaceProperty _:
                    string newNamespace = set.Message1.NewValue.AssertTypeOf<string>();
                    bool clipNamespace = !string.IsNullOrEmpty(set.Message2.Model.GeneratorSettings?.PackageNamespace) &&
                                         newNamespace.StartsWith(set.Message2.Model.GeneratorSettings.PackageNamespace);
                    updatedModel = new AgentModel(agentModel.Name,
                                                  clipNamespace
                                                  ?newNamespace.Substring(set.Message2.Model.GeneratorSettings.PackageNamespace.Length)
                                                  :newNamespace,
                                                  agentModel.ConsumingMessages,
                                                  agentModel.ProducedMessages,
                                                  agentModel.IncomingEvents,
                                                  agentModel.ProducedEvents,
                                                  agentModel.Id);
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

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}