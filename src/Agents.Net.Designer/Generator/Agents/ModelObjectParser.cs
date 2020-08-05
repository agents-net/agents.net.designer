using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Generator.Messages;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.Generator.Agents
{
    [Consumes(typeof(AgentModelSelectedForGeneration))]
    [Consumes(typeof(MessageModelSelectedForGeneration))]
    [Consumes(typeof(ModelSelectedForGeneration), Implicitly = true)]
    [Consumes(typeof(InterceptorAgentModelSelectedForGeneration), Implicitly = true)]
    [Produces(typeof(GeneratingAgent))]
    [Produces(typeof(GeneratingInterceptorAgent))]
    [Produces(typeof(GeneratingMessage))]
    [Produces(typeof(GeneratingFile))]
    public class ModelObjectParser : Agent
    {        public ModelObjectParser(IMessageBoard messageBoard) : base(messageBoard)
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
                                                               messageModel.Message.FullNamespace(),
                                                messageModel.Get<ModelSelectedForGeneration>().GenerationPath, messageModel)));
        }

        private void PrepareAgent(AgentModelSelectedForGeneration agentModel)
        {
            string name = agentModel.Agent.Name;
            string agentNamespace = agentModel.Agent.FullNamespace();
            string path = agentModel.Get<ModelSelectedForGeneration>().GenerationPath;
            List<string> consumingMessages = new List<string>();
            List<string> producingMessages = new List<string>();
            HashSet<string> dependencies = new HashSet<string>();
            AddMessages(agentModel.ProducingMessages, producingMessages);
            AddMessages(agentModel.ConsumingMessages, consumingMessages);

            Message message = new GeneratingAgent(consumingMessages.ToArray(),
                                                  producingMessages.ToArray(),
                                                  dependencies.ToArray(),
                                                  agentModel,
                                                  new GeneratingFile(name, agentNamespace, path,
                                                                     agentModel));
            if (agentModel.TryGet(out InterceptorAgentModelSelectedForGeneration interceptorAgent))
            {
                List<string> interceptingMessages = new List<string>();
                AddMessages(interceptorAgent.InterceptingMessages, interceptingMessages);
                message = new GeneratingInterceptorAgent(interceptingMessages.ToArray(), agentModel, message);
            }
            OnMessage(message);

            void AddMessages(IEnumerable<MessageModel> messageModels, List<string> messageNames)
            {
                foreach (MessageModel messageModel in messageModels)
                {
                    messageNames.Add(messageModel.Name);
                    string messageNamespace = messageModel.FullNamespace();
                    if (messageNamespace != agentNamespace && messageNamespace != "Agents.Net")
                    {
                        dependencies.Add(messageNamespace);
                    }
                }
            }
        }
    }
}
