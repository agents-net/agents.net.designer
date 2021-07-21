#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using System.IO;
using Agents.Net;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class FileOpened : FileSystemMessage
    {
        public FileOpened(string path, Stream data, Message predecessorMessage)
			: base(path, predecessorMessage)
        {
            Data = data;
        }

        public FileOpened(string path, Stream data, IEnumerable<Message> predecessorMessages)
			: base(path, predecessorMessages)
        {
            Data = data;
        }
        
        public Stream Data { get; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Data.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
