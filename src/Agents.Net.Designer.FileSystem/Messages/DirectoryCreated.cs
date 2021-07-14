using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class DirectoryCreated : FileSystemMessage
    {
        public DirectoryCreated(string path, Message predecessorMessage)
			: base(path, predecessorMessage)
        {
        }

        public DirectoryCreated(string path, IEnumerable<Message> predecessorMessages)
			: base(path, predecessorMessages)
        {
        }
    }
}
