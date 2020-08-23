using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Templates.Messages
{
    public class TemplateFileFound : Message
    {        public TemplateFileFound(string path, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            Path = path;
        }

        public TemplateFileFound(string path, IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
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
