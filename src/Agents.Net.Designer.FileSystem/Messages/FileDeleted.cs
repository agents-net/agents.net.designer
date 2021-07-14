using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class FileDeleted : FileSystemMessage
    {
        public FileDeleted(string path, Message predecessorMessage)
			: base(path, predecessorMessage)
        {
        }

        public FileDeleted(string path, IEnumerable<Message> predecessorMessages)
			: base(path, predecessorMessages)
        {
        }
    }
}
