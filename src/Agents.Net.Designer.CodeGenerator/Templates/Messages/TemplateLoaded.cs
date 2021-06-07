using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Templates.Messages
{
    public class TemplateLoaded : Message
    {
        public TemplateLoaded(string name, string content, Message predecessorMessage)
            : base(predecessorMessage)
        {
            Name = name;
            Content = content;
        }

        public TemplateLoaded(string name, string content, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
            Name = name;
            Content = content;
        }

        public string Name { get; }
        public string Content { get; }

        protected override string DataToString()
        {
            return $"{nameof(Name)}: {Name}";
        }
    }
}
