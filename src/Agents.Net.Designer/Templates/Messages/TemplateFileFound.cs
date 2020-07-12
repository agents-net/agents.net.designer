using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Templates.Messages
{
    public class TemplateFileFound : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition TemplateFileFoundDefinition { get; } =
            new MessageDefinition(nameof(TemplateFileFound));

        #endregion

        public TemplateFileFound(string path, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, TemplateFileFoundDefinition, childMessages)
        {
            Path = path;
        }

        public TemplateFileFound(string path, IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, TemplateFileFoundDefinition, childMessages)
        {
            Path = path;
        }

        public string Path { get; }

        protected override string DataToString()
        {
            return $"{nameof(Path)}: {Path}";
        }
    }
}
