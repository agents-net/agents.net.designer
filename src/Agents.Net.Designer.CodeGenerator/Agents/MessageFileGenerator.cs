using System;
using Agents.Net.Designer.CodeGenerator.Messages;
using Agents.Net.Designer.CodeGenerator.Templates.Messages;

namespace Agents.Net.Designer.CodeGenerator.Agents
{
    [Consumes(typeof(TemplatesLoaded))]
    [Consumes(typeof(GeneratingFile), Implicitly = true)]
    [Consumes(typeof(GeneratingMessage))]
    [Consumes(typeof(GeneratingMessageDecorator), Implicitly = true)]
    [Produces(typeof(FileGenerated))]
    public class MessageFileGenerator : Agent
    {
        private readonly MessageCollector<TemplatesLoaded, GeneratingFile> collector;

        public MessageFileGenerator(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<TemplatesLoaded, GeneratingFile>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<TemplatesLoaded, GeneratingFile> set)
        {
            set.MarkAsConsumed(set.Message2);

            bool decorator = set.Message2.Is<GeneratingMessageDecorator>();
            GenerateMessage(set.Message2,
                            decorator
                              ? set.Message1.Templates["MessageDecoratorTemplate"]
                              : set.Message1.Templates["MessageTemplate"]);
            OnMessage(new FileGenerated(
                          new FileGenerationResult(decorator ? FileType.MessageDecorator : FileType.Message,
                                                   set.Message2.Name, set.Message2.Namespace, set.Message2.Path), set));
        }

        private void GenerateMessage(GeneratingFile file, string template)
        {
            if (file.TryGet(out GeneratingMessageDecorator decorator))
            {
                string @using = string.IsNullOrEmpty(decorator.Dependency)
                                    ? string.Empty
                                    : $"{Environment.NewLine}using {decorator.Dependency};";
                template = template.Replace("$using$", @using)
                                   .Replace("$decoratedmessage$", decorator.DecoratedMessageName);
            }
            file.GenerateFile(template);
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
