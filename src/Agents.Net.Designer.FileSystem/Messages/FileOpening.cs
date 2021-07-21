#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using System.IO;
using Agents.Net;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class FileOpening : FileSystemMessage
    {
        public FileOpening(string path, Message predecessorMessage, FileMode fileMode = FileMode.OpenOrCreate,
                           FileAccess fileAccess = FileAccess.ReadWrite, FileShare fileShare = FileShare.None)
			: base(path, predecessorMessage)
        {
            FileMode = fileMode;
            FileAccess = fileAccess;
            FileShare = fileShare;
        }

        public FileOpening(string path, IEnumerable<Message> predecessorMessages, FileMode fileMode = FileMode.OpenOrCreate,
                           FileAccess fileAccess = FileAccess.ReadWrite, FileShare fileShare = FileShare.None)
			: base(path, predecessorMessages)
        {
            FileMode = fileMode;
            FileAccess = fileAccess;
            FileShare = fileShare;
        }
        
        public FileMode FileMode { get; }
        
        public FileAccess FileAccess { get; }
        
        public FileShare FileShare { get; }

        protected override string DataToString()
        {
            return $"{nameof(FileMode)}: {FileMode}; {nameof(FileAccess)}: {FileAccess}; {nameof(FileShare)}: {FileShare}; {base.DataToString()}";
        }
    }
}
