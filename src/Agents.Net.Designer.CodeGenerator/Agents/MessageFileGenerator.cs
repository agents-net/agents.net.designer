using System;
using Agents.Net;
using Agents.Net.Designer.Generator.Messages;
using Agents.Net.Designer.Templates.Messages;

namespace Agents.Net.Designer.Generator.Agents
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

            GenerateMessage(set.Message2,
                            set.Message2.Is<GeneratingMessageDecorator>()
                              ? set.Message1.Templates["MessageDecoratorTemplate"]
                              : set.Message1.Templates["MessageTemplate"]);
            OnMessage(new FileGenerated(set.Message2.Path, set));
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
