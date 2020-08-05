using System;
using System.IO;
using System.Linq;
using System.Text;
using Agents.Net;
using Agents.Net.Designer.Generator.Messages;
using Agents.Net.Designer.Templates.Messages;

namespace Agents.Net.Designer.Generator.Agents
{
    [Consumes(typeof(TemplatesLoaded))]
    [Consumes(typeof(GeneratingFile))]
    [Consumes(typeof(GeneratingMessage), Implicitly = true)]
    [Consumes(typeof(GeneratingAgent), Implicitly = true)]
    [Consumes(typeof(GeneratingInterceptorAgent), Implicitly = true)]
    [Produces(typeof(FileGenerated))]
    public class CodeFileGenerator : Agent
    {
        private const int Indentation = 4;
        private readonly MessageCollector<TemplatesLoaded, GeneratingFile> collector;

        public CodeFileGenerator(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<TemplatesLoaded, GeneratingFile>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<TemplatesLoaded, GeneratingFile> set)
        {
            if (set.Message2.TryGet(out GeneratingMessage _))
            {
                GenerateFile(set.Message2, set.Message1.Templates["MessageTemplate"]);
            }
            else
            {
                GeneratingAgent agent = set.Message2.Get<GeneratingAgent>();
                GenerateAgent(set.Message2,
                              agent.Is<GeneratingInterceptorAgent>()
                                  ? set.Message1.Templates["InterceptorAgentTemplate"]
                                  : set.Message1.Templates["AgentTemplate"], agent);
            }
            OnMessage(new FileGenerated(set.Message2.Path, set));
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
            string content = template.Replace("$dependecies$", dependencies, StringComparison.Ordinal)
                                     .Replace("$attributes$", attributes, StringComparison.Ordinal);
            GenerateFile(file, content);

            string AggregateMessages(string[] messages, string attributeName, bool prefixNewline)
            {
                if (messages.Length == 0)
                {
                    return string.Empty;
                }

                string space = new string(' ', emptySpace);
                
                StringBuilder result = prefixNewline ? new StringBuilder(Environment.NewLine): new StringBuilder();
                result.Append(string.Join(Environment.NewLine, messages.Select(m => $"{space}[{attributeName}(typeof({m}))]")));
                return result.ToString();
            }
        }

        private void GenerateFile(GeneratingFile file, string template)
        {
            string content = template.Replace("$rootnamespace$", file.Namespace, StringComparison.Ordinal)
                                     .Replace("$itemname$", file.Name, StringComparison.Ordinal);
            File.WriteAllText(file.Path, content, Encoding.UTF8);
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
