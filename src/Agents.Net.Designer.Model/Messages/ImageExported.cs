using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class ImageExported : Message
    {
        public ImageExported(string path, Message predecessorMessage)
            : base(predecessorMessage)
        {
            Path = path;
        }

        public ImageExported(string path, IEnumerable<Message> predecessorMessages)
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
