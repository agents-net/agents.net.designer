using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Templates.Messages
{
    public class TemplateLoaded : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition TemplateLoadedDefinition { get; } =
            new MessageDefinition(nameof(TemplateLoaded));

        #endregion

        public TemplateLoaded(string name, string content, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, TemplateLoadedDefinition, childMessages)
        {
            Name = name;
            Content = content;
        }

        public TemplateLoaded(string name, string content, IEnumerable<Message> predecessorMessages,
                              params Message[] childMessages)
            : base(predecessorMessages, TemplateLoadedDefinition, childMessages)
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
