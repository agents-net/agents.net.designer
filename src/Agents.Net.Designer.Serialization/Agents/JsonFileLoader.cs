﻿using System.IO;
using System.Text;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.Serialization.Messages;

namespace Agents.Net.Designer.Serialization.Agents
{
    [Consumes(typeof(FileConnectionVerified))]
    [Produces(typeof(FileConnected))]
    [Produces(typeof(JsonTextLoaded))]
    public class JsonFileLoader : Agent
    {
        public JsonFileLoader(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            FileConnectionVerified fileVerified = messageData.Get<FileConnectionVerified>();
            if (!fileVerified.FileExist)
            {
                return;
            }

            OnMessage(new JsonTextLoaded(File.ReadAllText(fileVerified.FileName, Encoding.UTF8), messageData));
            OnMessage(new FileConnected(fileVerified.FileName, false, messageData));
        }
    }
}
