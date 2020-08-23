using System.IO;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Serialization.Agents
{
    [Consumes(typeof(FileConnectionVerified))]
    [Produces(typeof(FileConnected))]
    public class JsonFileCreator : Agent
    {        public JsonFileCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            FileConnectionVerified fileVerified = messageData.Get<FileConnectionVerified>();
            if (fileVerified.FileExist)
            {
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(fileVerified.FileName));
            File.Create(fileVerified.FileName).Dispose();
            OnMessage(new FileConnected(fileVerified.FileName, true, messageData));
        }
    }
}
