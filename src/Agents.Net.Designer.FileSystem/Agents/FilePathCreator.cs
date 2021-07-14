using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Agents.Net;
using Agents.Net.Designer.FileSystem.Messages;
using Serilog;

namespace Agents.Net.Designer.FileSystem.Agents
{
    [Consumes(typeof(DirectoryCreated))]
    [Consumes(typeof(FileSystemException))]
    [Produces(typeof(DirectoryCreating))]
    [Intercepts(typeof(FileCreating))]
    [Intercepts(typeof(FileOpening))]
    [Intercepts(typeof(FileRenaming))]
    public class FilePathCreator : InterceptorAgent
    {
        public FilePathCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            directoryCreationGate.Check(messageData);
        }
        
        private readonly MessageGate<DirectoryCreating, DirectoryCreated> directoryCreationGate =
            new();
        private readonly Dictionary<string, List<InterceptionDelayToken>> delayTokens =
            new();

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            if (messageData.TryGet(out FileOpening opening) &&
                opening.FileMode != FileMode.Create &&
                opening.FileMode != FileMode.CreateNew &&
                opening.FileMode != FileMode.OpenOrCreate)
            {
                return InterceptionAction.Continue;
            }

            string path = messageData.Get<FileSystemMessage>().Path;
            if (messageData.TryGet(out FileRenaming renaming))
            {
                path = renaming.NewPath;
            }

            string directory;
            try
            {
                directory = Path.GetDirectoryName(path);
            }
            catch (PathTooLongException)
            {
                directory = null;
            }

            if (string.IsNullOrEmpty(directory) ||
                Directory.Exists(directory))
            {
                return InterceptionAction.Continue;
            }

            List<InterceptionDelayToken> otherAgentTokens;
            InterceptionAction action = InterceptionAction.Delay(out InterceptionDelayToken token);
            lock (delayTokens)
            {
                if (delayTokens.TryGetValue(directory, out List<InterceptionDelayToken> tokens))
                {
                    Log.Verbose($"Delay for directory creation {directory}.");
                    tokens.Add(token);
                    return action;
                }

                otherAgentTokens = new List<InterceptionDelayToken> {token};
                delayTokens.Add(directory, otherAgentTokens);
            }

            DirectoryCreating message = new(directory, messageData);
            Log.Verbose($"Start directory creation {directory}.");
            directoryCreationGate.SendAndContinue(message, OnMessage, result =>
            {
                Log.Verbose($"Directory {directory} created");
                lock (delayTokens)
                {
                    delayTokens.Remove(directory);
                }

                DelayTokenReleaseIntention releaseIntention = result.Result == MessageGateResultKind.Success
                                                                  ? DelayTokenReleaseIntention.Publish
                                                                  : DelayTokenReleaseIntention.DoNotPublish;
                foreach (InterceptionDelayToken delayToken in otherAgentTokens)
                {
                    delayToken.Release(releaseIntention);
                }
            });
            
            return action;
        }
    }
}
