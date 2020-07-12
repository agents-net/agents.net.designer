using System;
using System.Collections.Generic;
using System.IO;
using Agents.Net;
using Agents.Net.Designer.Generator.Messages;

namespace Agents.Net.Designer.Generator.Agents
{
    public class DirectoryCreator : InterceptorAgent
    {
        #region Definition

        [AgentDefinition]
        public static InterceptorAgentDefinition DirectoryCreatorDefinition { get; }
            = new InterceptorAgentDefinition(new[]
                                             {
                                                 GeneratingFile.GeneratingFileDefinition,
                                                 GenerateFilesRequested.GenerateFilesRequestedDefinition
                                             },
                                             Array.Empty<MessageDefinition>());

        #endregion

        public DirectoryCreator(IMessageBoard messageBoard) : base(DirectoryCreatorDefinition, messageBoard)
        {
        }

        private readonly HashSet<string> createdDirectories = new HashSet<string>();

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            if (messageData.TryGet(out GenerateFilesRequested _))
            {
                lock (createdDirectories)
                {
                    createdDirectories.Clear();
                }
                return InterceptionAction.Continue;
            }

            GeneratingFile file = messageData.Get<GeneratingFile>();
            lock (createdDirectories)
            {
                if (!createdDirectories.Add(Path.GetDirectoryName(file.Path)))
                {
                    return InterceptionAction.Continue;
                }

                if (!Directory.Exists(Path.GetDirectoryName(file.Path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(file.Path));
                }
            }

            return InterceptionAction.Continue;
        }
    }
}
