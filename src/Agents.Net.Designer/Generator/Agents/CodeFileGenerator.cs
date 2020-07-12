using System;
using System.IO;
using System.Linq;
using System.Text;
using Agents.Net;
using Agents.Net.Designer.Generator.Messages;
using Agents.Net.Designer.Templates.Messages;

namespace Agents.Net.Designer.Generator.Agents
{
    public class CodeFileGenerator : Agent
    {
        private const int Indentation = 4;

        #region Definition

        [AgentDefinition]
        public static AgentDefinition CodeFileGeneratorDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      TemplatesLoaded.TemplatesLoadedDefinition,
                                      GeneratingFile.GeneratingFileDefinition
                                  },
                                  new []
                                  {
                                      FileGenerated.FileGeneratedDefinition
                                  });

        #endregion

        private readonly MessageCollector<TemplatesLoaded, GeneratingFile> collector;

        public CodeFileGenerator(IMessageBoard messageBoard) : base(CodeFileGeneratorDefinition, messageBoard)
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
                GenerateAgent(set.Message2, set.Message1.Templates["AgentTemplate"], agent);
            }
            OnMessage(new FileGenerated(set.Message2.Path, set));
        }

        private void GenerateAgent(GeneratingFile file, string template, GeneratingAgent agent)
        {
            string dependencies = string.Join(Environment.NewLine, agent.Dependencies.Select(d => $"using {d};"));
            int templatePosition = template.IndexOf("$consumingmessages$", StringComparison.Ordinal);
            int previousLineBreak = template.LastIndexOf('\n', templatePosition);
            int baseEmptySpace = templatePosition - previousLineBreak - 1;
            string consumingMessages = AggregateMessages(agent.ConsumingMessages);
            string producingMessages = AggregateMessages(agent.ProducingMessages);
            string content = template.Replace("$dependecies$", dependencies, StringComparison.Ordinal)
                                     .Replace("$consumingmessages$", consumingMessages, StringComparison.Ordinal)
                                     .Replace("$producingmessages$", producingMessages, StringComparison.Ordinal);
            GenerateFile(file, content);

            string AggregateMessages(string[] messages)
            {
                if (messages.Length == 0)
                {
                    return "Array.Empty<MessageDefinition>()";
                }

                string baseSpace = new string(' ', baseEmptySpace);
                string indentedSpace = baseSpace + new string(' ', Indentation);
                StringBuilder result = new StringBuilder($"new []{Environment.NewLine}{baseSpace}{{{Environment.NewLine}");
                result.AppendLine(string.Join($",{Environment.NewLine}",
                                              messages.Select(m => $"{indentedSpace}{m}.{m}Definition")));
                result.Append($"{baseSpace}}}");
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
