#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.IO;
using Agents.Net;
using Agents.Net.Designer.FileSystem.Messages;

namespace Agents.Net.Designer.FileSystem.Agents
{
    [Consumes(typeof(FileCreating))]
    [Consumes(typeof(FileDeleting))]
    [Consumes(typeof(FileRenaming))]
    [Consumes(typeof(FileOpening))]
    [Produces(typeof(FileCreated))]
    [Produces(typeof(FileDeleted))]
    [Produces(typeof(FileRenamed))]
    [Produces(typeof(FileOpened))]
    [Produces(typeof(FileSystemException))]
    public class FileManipulator : FileSystemAgent
    {
        public FileManipulator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void SafeExecuteCore(Message messageData)
        {
            if (messageData.TryGet(out FileCreating creating))
            {
                File.Create(creating.Path).Dispose();
                OnMessage(new FileCreated(creating.Path, messageData));
            }
            else if(messageData.TryGet(out FileDeleting deleting))
            {
                File.Delete(deleting.Path);
                OnMessage(new FileDeleted(deleting.Path, messageData));
            }
            else if(messageData.TryGet(out FileOpening opening))
            {
                Stream fileStream = File.Open(opening.Path, opening.FileMode, opening.FileAccess,
                                              opening.FileShare);
                OnMessage(new FileOpened(opening.Path, fileStream, messageData));
            }
            else if(messageData.TryGet(out FileRenaming renaming))
            {
                File.Move(renaming.Path, renaming.NewPath);
                OnMessage(new FileRenamed(renaming.NewPath, messageData));
            }
        }
    }
}
