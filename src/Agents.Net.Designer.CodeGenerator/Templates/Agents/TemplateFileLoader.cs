using System.IO;
using Agents.Net.Designer.CodeGenerator.Templates.Messages;
using Agents.Net.Designer.FileSystem.Messages;

namespace Agents.Net.Designer.CodeGenerator.Templates.Agents
{
    [Consumes(typeof(TemplateFileFound))]
    [Produces(typeof(TemplateLoaded))]
    [Consumes(typeof(FileOpened))]
    [Consumes(typeof(FileSystemException))]
    [Produces(typeof(FileOpening))]
    public class TemplateFileLoader : Agent
    {
        private readonly MessageGate<FileOpening, FileOpened> fileGate = new();

        public TemplateFileLoader(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.TryGet(out TemplateFileFound fileFound))
            {
                string name = Path.GetFileNameWithoutExtension(fileFound.Path);
                MessageGateResult<FileOpened> result = fileGate.SendAndAwait(new FileOpening(fileFound.Path, messageData), OnMessage);
                if (result.Result != MessageGateResultKind.Success)
                {
                    return;
                }

                using StreamReader reader = new(result.EndMessage.Data);
                string content = reader.ReadToEnd();
                OnMessage(new TemplateLoaded(name, content, messageData));
            }
            else
            {
                fileGate.Check(messageData);
            }
        }
    }
}
