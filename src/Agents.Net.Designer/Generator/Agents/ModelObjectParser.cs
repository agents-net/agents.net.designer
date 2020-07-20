using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Generator.Messages;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.Generator.Agents
{
    public class ModelObjectParser : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition ModelObjectParserDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      AgentModelSelectedForGeneration.AgentModelSelectedForGenerationDefinition,
                                      MessageModelSelectedForGeneration.MessageModelSelectedForGenerationDefinition
                                  },
                                  new []
                                  {
                                      GeneratingAgent.GeneratingAgentDefinition,
                                      GeneratingMessage.GeneratingMessageDefinition
                                  });

        #endregion

        public ModelObjectParser(IMessageBoard messageBoard) : base(ModelObjectParserDefinition, messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.TryGet(out AgentModelSelectedForGeneration agentModel))
            {
                PrepareAgent(agentModel);
            }
            else
            {
                PrepareMessage(messageData.Get<MessageModelSelectedForGeneration>());
            }
        }

        private void PrepareMessage(MessageModelSelectedForGeneration messageModel)
        {
            OnMessage(new GeneratingMessage(messageModel, 
                                            new GeneratingFile(messageModel.Message.Name, 
                                                               messageModel.Message.Namespace
                                                                  .ExtendNamespace(messageModel.Get<ModelSelectedForGeneration>()
                                                                                      .Model),
                                                messageModel.Get<ModelSelectedForGeneration>().GenerationPath, messageModel)));
        }

        private void PrepareAgent(AgentModelSelectedForGeneration agentModel)
        {
            string name = agentModel.Agent.Name;
            CommunityModel communityModel = agentModel.Get<ModelSelectedForGeneration>().Model;
            string agentNamespace = agentModel.Agent.Namespace.ExtendNamespace(communityModel);
            string path = agentModel.Get<ModelSelectedForGeneration>().GenerationPath;
            List<string> consumingMessages = new List<string>();
            List<string> producingMessages = new List<string>();
            HashSet<string> dependencies = new HashSet<string>();
            AddMessages(agentModel.ProducingMessages, producingMessages);
            AddMessages(agentModel.ConsumingMessages, consumingMessages);

            OnMessage(new GeneratingAgent(consumingMessages.ToArray(),
                                          producingMessages.ToArray(),
                                          dependencies.ToArray(),
                                          agentModel,
                                          new GeneratingFile(name, agentNamespace, path, agentModel)));

            void AddMessages(IEnumerable<MessageModel> messageModels, List<string> messageNames)
            {
                foreach (MessageModel messageModel in messageModels)
                {
                    messageNames.Add(messageModel.Name);
                    string messageNamespace = messageModel.Namespace.ExtendNamespace(communityModel);
                    if (messageNamespace != agentNamespace && messageNamespace != "Agents.Net")
                    {
                        dependencies.Add(messageNamespace);
                    }
                }
            }
        }
    }
}
