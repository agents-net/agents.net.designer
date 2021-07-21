#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.IO;
using Agents.Net.Designer.FileSystem.Messages;

namespace Agents.Net.Designer.FileSystem.Agents
{
    [Consumes(typeof(ConnectFileRequested))]
    [Produces(typeof(FileConnectionVerified))]
    public class FileVerifier : FileSystemAgent
    {
        public FileVerifier(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void SafeExecuteCore(Message messageData)
        {
            ConnectFileRequested fileRequested = messageData.Get<ConnectFileRequested>();
            if (!IsValidPath(fileRequested.FileName))
            {
                return;
            }
            
            OnMessage(new FileConnectionVerified(fileRequested.FileName, File.Exists(fileRequested.FileName), messageData));
        }

        private bool IsValidPath(string path, bool allowRelativePaths = false)
        {
            bool isValid = true;

            try
            {
                string fullPath = Path.GetFullPath(path);

                if (allowRelativePaths)
                {
                    isValid = Path.IsPathRooted(path);
                }
                else
                {
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }
            }
            catch(Exception ex)
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
