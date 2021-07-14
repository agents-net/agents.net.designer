using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class FileRenaming : FileSystemMessage
    {
        public FileRenaming(string path, string newName, Message predecessorMessage,
                            params Message[] childMessages)
			: base(path, predecessorMessage)
        {
            NewName = newName;
        }

        public FileRenaming(string path, string newName, IEnumerable<Message> predecessorMessages,
                            params Message[] childMessages)
			: base(path, predecessorMessages)
        {
            NewName = newName;
        }
        
        public string NewName { get; }

        public string NewPath =>
            $"{System.IO.Path.GetDirectoryName(Path) ?? string.Empty}{new string(System.IO.Path.DirectorySeparatorChar, 1)}{NewName}";

        protected override string DataToString()
        {
            return $"{nameof(NewName)}: {NewName}; {base.DataToString()}";
        }
    }
}
