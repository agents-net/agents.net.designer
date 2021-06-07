using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class ExportImageRequested : Message
    {
        public ExportImageRequested(string path, Message predecessorMessage)
            : base(predecessorMessage)
        {
            Path = path;
        }

        public ExportImageRequested(string path, IEnumerable<Message> predecessorMessages)
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
