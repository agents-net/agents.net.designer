using System;
using System.Linq;
using System.Text;
using Agents.Net.Designer.CodeGenerator.Messages;
using Agents.Net.Designer.CodeGenerator.Templates.Messages;

namespace Agents.Net.Designer.CodeGenerator.Agents
{
    [Consumes(typeof(TemplatesLoaded))]
    [Consumes(typeof(GeneratingFile), Implicitly = true)]
    [Consumes(typeof(GeneratingAgent))]
    [Consumes(typeof(GeneratingInterceptorAgent), Implicitly = true)]
    [Produces(typeof(FileGenerated))]
    public class AgentFileGenerator : Agent
    {
        private readonly MessageCollector<TemplatesLoaded, GeneratingFile> collector;

        public AgentFileGenerator(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<TemplatesLoaded, GeneratingFile>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<TemplatesLoaded, GeneratingFile> set)
        {
            set.MarkAsConsumed(set.Message2);

            GeneratingAgent agent = set.Message2.Get<GeneratingAgent>();
            bool interceptor = agent.Is<GeneratingInterceptorAgent>();
            GenerateAgent(set.Message2,
                          interceptor
                              ? set.Message1.Templates["InterceptorAgentTemplate"]
                              : set.Message1.Templates["AgentTemplate"], agent);
            OnMessage(new FileGenerated(
                          new FileGenerationResult(interceptor ? FileType.InterceptorAgent : FileType.Agent,
                                                   set.Message2.Name, set.Message2.Namespace, set.Message2.Path), set));
        }

        private void GenerateAgent(GeneratingFile file, string template, GeneratingAgent agent)
        {
            string dependencies = string.Join(Environment.NewLine, agent.Dependencies.Select(d => $"using {d};"));
            int templatePosition = template.IndexOf("$attributes$", StringComparison.Ordinal);
            int previousLineBreak = template.LastIndexOf('\n', templatePosition);
            int emptySpace = templatePosition - previousLineBreak - 1;
            string consumingMessages = AggregateMessages(agent.ConsumingMessages, "Consumes", false);
            string producingMessages = AggregateMessages(agent.ProducingMessages, "Produces", !string.IsNullOrEmpty(consumingMessages));
            string attributes = consumingMessages + producingMessages;
            if (agent.TryGet(out GeneratingInterceptorAgent interceptorAgent))
            {
                string interceptingMessages = AggregateMessages(interceptorAgent.InterceptingMessages, "Intercepts",
                                                                !string.IsNullOrEmpty(attributes));
                attributes += interceptingMessages;
            }
            if (!string.IsNullOrEmpty(attributes))
            {
                attributes = attributes.Substring(emptySpace);
            }
            string content = template.Replace("$dependecies$", dependencies)
                                     .Replace("$attributes$", attributes);
            file.GenerateFile(content);

            string AggregateMessages(string[] messages, string attributeName, bool prefixNewline)
            {
                if (messages.Length == 0)
                {
                    return string.Empty;
                }

                string space = new(' ', emptySpace);
                
                StringBuilder result = prefixNewline ? new StringBuilder(Environment.NewLine): new StringBuilder();
                result.Append(string.Join(Environment.NewLine, messages.Select(m => $"{space}[{attributeName}(typeof({m}))]")));
                return result.ToString();
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
