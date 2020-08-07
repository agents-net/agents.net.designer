using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Generator.Messages;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.Generator.Agents
{
    [Consumes(typeof(AgentModelSelectedForGeneration))]
    [Consumes(typeof(ModelSelectedForGeneration), Implicitly = true)]
    [Consumes(typeof(InterceptorAgentModelSelectedForGeneration), Implicitly = true)]
    [Produces(typeof(GeneratingAgent))]
    [Produces(typeof(GeneratingInterceptorAgent))]
    [Produces(typeof(GeneratingFile))]
    public class AgentModelObjectParser : Agent
    {
        public AgentModelObjectParser(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            AgentModelSelectedForGeneration agentModel = messageData.Get<AgentModelSelectedForGeneration>();
            string name = agentModel.Agent.Name;
            string agentNamespace = agentModel.Agent.FullNamespace();
            string path = agentModel.Get<ModelSelectedForGeneration>().GenerationPath;
            List<string> consumingMessages = new List<string>();
            List<string> producingMessages = new List<string>();
            List<string> interceptingMessages = new List<string>();
            HashSet<string> dependencies = new HashSet<string>();
            AddMessages(agentModel.ProducingMessages, producingMessages);
            AddMessages(agentModel.ConsumingMessages, consumingMessages);
            if (agentModel.TryGet(out InterceptorAgentModelSelectedForGeneration interceptorAgent))
            {
                AddMessages(interceptorAgent.InterceptingMessages, interceptingMessages);
            }

            Message message = new GeneratingAgent(consumingMessages.ToArray(),
                                                  producingMessages.ToArray(),
                                                  dependencies.ToArray(),
                                                  agentModel,
                                                  new GeneratingFile(name, agentNamespace, path,
                                                                     agentModel));
            if (agentModel.Is<InterceptorAgentModelSelectedForGeneration>())
            {
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
