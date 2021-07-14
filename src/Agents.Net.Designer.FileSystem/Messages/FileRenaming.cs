using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class FileRenaming : FileSystemMessage
    {
        public FileRenaming(string path, string newName, Message predecessorMessage)
			: base(path, predecessorMessage)
        {
            NewName = newName;
        }

        public FileRenaming(string path, string newName, IEnumerable<Message> predecessorMessages)
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
