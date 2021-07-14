#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using Agents.Net.Designer.FileSystem.Messages;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.Tools.Agents
{
    [Consumes(typeof(FileOpening))]
    [Consumes(typeof(FileCreating))]
    [Produces(typeof(FileOpened))]
    [Consumes(typeof(FileCreated))]
    public class FileSystemSimulator : Agent
    {
        private readonly ConcurrentDictionary<string, string> fileContents = new();
        
        public FileSystemSimulator(IMessageBoard messageBoard, string name = null)
            : base(messageBoard, name)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.TryGet(out FileOpening opening))
            {
                CloseActionStream stream = new(result =>
                {
                    result.Seek(0, SeekOrigin.Begin);
                    using StreamReader reader = new(result, leaveOpen:true);
                    string newContent = reader.ReadToEnd();
                    fileContents.AddOrUpdate(opening.Path, newContent, (_, _) => newContent);
                });
                if (fileContents.TryGetValue(opening.Path, out string content))
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(content);
                    MemoryStream stringStream = new(byteArray);
                    stringStream.CopyTo(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                }
                OnMessage(new FileOpened(opening.Path, stream, opening));
            }
            else if (messageData.TryGet(out FileCreating creating))
            {
                fileContents.AddOrUpdate(opening.Path, string.Empty, (_, _) => string.Empty);
                OnMessage(new FileCreated(creating.Path, creating));
            }
        }

        public void SetFileContent(string path, string fileContent)
        {
            fileContents.AddOrUpdate(path, fileContent, (_,_) => fileContent);
        }
        
        public string GetFileContent(string fileName)
        {
            return fileContents.TryGetValue(fileName, out string content) ? content : string.Empty;
        }

        private class CloseActionStream : MemoryStream
        {
            private readonly Action<Stream> closeAction;

            public CloseActionStream(Action<Stream> closeAction)
            {
                this.closeAction = closeAction;
            }
            
            private bool IsDisposed { get; set; }

            protected override void Dispose(bool disposing)
            {
                if (disposing && !IsDisposed)
                {
                    closeAction(this);
                    IsDisposed = true;
                }
                base.Dispose(disposing);
            }
        }
    }
}