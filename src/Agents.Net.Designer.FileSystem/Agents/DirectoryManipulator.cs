using System;
using System.IO;
using Agents.Net;
using Agents.Net.Designer.FileSystem.Messages;

namespace Agents.Net.Designer.FileSystem.Agents
{
    [Consumes(typeof(DirectoryCreating))]
    [Consumes(typeof(DirectoryDeleting))]
    [Consumes(typeof(DirectoryRenaming))]
    [Produces(typeof(DirectoryCreated))]
    [Produces(typeof(DirectoryDeleted))]
    [Produces(typeof(DirectoryRenamed))]
    [Produces(typeof(FileSystemException))]
    public class DirectoryManipulator : FileSystemAgent
    {
        public DirectoryManipulator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void SafeExecuteCore(Message messageData)
        {
            if (messageData.TryGet(out DirectoryCreating creating))
            {
                Directory.CreateDirectory(creating.Path);
                OnMessage(new DirectoryCreated(creating.Path, messageData));
            }
            else if(messageData.TryGet(out DirectoryDeleting deleting))
            {
                Directory.Delete(deleting.Path, deleting.Recursive);
                OnMessage(new DirectoryDeleted(deleting.Path, messageData));
            }
            else if(messageData.TryGet(out DirectoryRenaming renaming))
            {
                string directory = Path.GetDirectoryName(renaming.Path) ?? string.Empty;
                string newPath = $"{directory}{new string(Path.DirectorySeparatorChar, 1)}{renaming.NewName}";
                Directory.Move(renaming.Path, newPath);
                OnMessage(new DirectoryRenamed(newPath, messageData));
            }
        }
    }
}
