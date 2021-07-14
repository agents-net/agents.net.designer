using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class FileRenamed : FileSystemMessage
    {
        public FileRenamed(string path, Message predecessorMessage)
			: base(path, predecessorMessage)
        {
        }

        public FileRenamed(string path, IEnumerable<Message> predecessorMessages)
			: base(path, predecessorMessages)
        {
        }
    }
}
