using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class FileCreating : FileSystemMessage
    {
        public FileCreating(string path, Message predecessorMessage)
			: base(path, predecessorMessage)
        {
        }

        public FileCreating(string path, IEnumerable<Message> predecessorMessages)
			: base(path, predecessorMessages)
        {
        }
    }
}
