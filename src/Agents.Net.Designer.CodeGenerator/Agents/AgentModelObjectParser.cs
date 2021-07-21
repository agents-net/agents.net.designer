#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using System.Linq;
using Agents.Net.Designer.CodeGenerator.Messages;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.CodeGenerator.Agents
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
            List<string> consumingMessages = new();
            List<string> producingMessages = new();
            List<string> interceptingMessages = new();
            HashSet<string> dependencies = new();
            AddMessages(agentModel.ProducingMessages, producingMessages);
            AddMessages(agentModel.ConsumingMessages, consumingMessages);
            if (agentModel.TryGet(out InterceptorAgentModelSelectedForGeneration interceptorAgent))
            {
                AddMessages(interceptorAgent.InterceptingMessages, interceptingMessages);
            }

            Message message = GeneratingAgent.Decorate(new GeneratingFile(name, agentNamespace, path,
                                                                          agentModel),
                                                       consumingMessages.ToArray(),
                                                       producingMessages.ToArray(),
                                                       dependencies.ToArray());
            if (agentModel.Is<InterceptorAgentModelSelectedForGeneration>())
            {
                message = GeneratingInterceptorAgent.Decorate((GeneratingAgent) message, interceptingMessages.ToArray());
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
