using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Templates.Messages
{
    public class TemplateFileFound : Message
    {
        public TemplateFileFound(string path, Message predecessorMessage)
            : base(predecessorMessage)
        {
            Path = path;
        }

        public TemplateFileFound(string path, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
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
