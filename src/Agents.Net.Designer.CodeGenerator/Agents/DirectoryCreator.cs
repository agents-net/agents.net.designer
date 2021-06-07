using System.Collections.Generic;
using System.IO;
using Agents.Net.Designer.CodeGenerator.Messages;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.CodeGenerator.Agents
{
    [Intercepts(typeof(GeneratingFile))]
    [Intercepts(typeof(GenerateFilesRequested))]
    public class DirectoryCreator : InterceptorAgent
    {
        public DirectoryCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        private readonly HashSet<string> createdDirectories = new();

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
