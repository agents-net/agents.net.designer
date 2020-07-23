using System;
using System.IO;
using System.Threading;
using Agents.Net;
using Agents.Net.Designer.Json.Messages;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Json.Agents
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
