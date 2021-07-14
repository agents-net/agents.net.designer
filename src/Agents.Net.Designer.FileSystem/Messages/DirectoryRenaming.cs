using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class DirectoryRenaming : FileSystemMessage
    {
        public DirectoryRenaming(string path, string newName, Message predecessorMessage,
                                 params Message[] childMessages)
			: base(path, predecessorMessage)
        {
            NewName = newName;
        }

        public DirectoryRenaming(string path, string newName, IEnumerable<Message> predecessorMessages,
                                 params Message[] childMessages)
			: base(path, predecessorMessages)
        {
            NewName = newName;
        }
        
        public string NewName { get; }

        protected override string DataToString()
        {
            return $"{nameof(NewName)}: {NewName}; {base.DataToString()}";
        }
    }
}
