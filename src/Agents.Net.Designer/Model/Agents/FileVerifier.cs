using System;
using System.IO;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    public class FileVerifier : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition FileVerifierDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      ConnectFileRequested.ConnectFileRequestedDefinition
                                  },
                                  new []
                                  {
                                      FileConnectionVerified.FileConnectionVerifiedDefinition
                                  });

        #endregion

        public FileVerifier(IMessageBoard messageBoard) : base(FileVerifierDefinition, messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
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
