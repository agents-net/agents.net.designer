using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Templates.Messages
{
    public class TemplatesLoaded : Message
    {        public TemplatesLoaded(Dictionary<string, string> templates, Message predecessorMessage,
                               params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            Templates = templates;
        }

        public TemplatesLoaded(Dictionary<string, string> templates, IEnumerable<Message> predecessorMessages,
                               params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            Templates = templates;
        }

        public Dictionary<string, string> Templates { get; }

        protected override string DataToString()
        {
            return $"{nameof(Templates)}: {string.Join(", ",Templates.Keys)}";
        }
    }
}
