#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class DirectoryRenaming : FileSystemMessage
    {
        public DirectoryRenaming(string path, string newName, Message predecessorMessage)
			: base(path, predecessorMessage)
        {
            NewName = newName;
        }

        public DirectoryRenaming(string path, string newName, IEnumerable<Message> predecessorMessages)
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
