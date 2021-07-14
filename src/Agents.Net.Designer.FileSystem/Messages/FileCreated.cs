using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class FileCreated : FileSystemMessage
    {
        public FileCreated(string path, Message predecessorMessage)
			: base(path, predecessorMessage)
        {
        }

        public FileCreated(string path, IEnumerable<Message> predecessorMessages)
			: base(path, predecessorMessages)
        {
        }
        
        
    }
}
