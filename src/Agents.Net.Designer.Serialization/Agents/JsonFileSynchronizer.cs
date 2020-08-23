using System;
using System.Collections.Generic;
using System.IO;
using Agents.Net;
using Agents.Net.Designer.Json.Messages;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Json.Agents
{
    [Consumes(typeof(FileConnected))]
    [Consumes(typeof(JsonTextUpdated))]
    [Produces(typeof(FileSynchronized))]
    public class JsonFileSynchronizer : Agent
    {
        private MessageCollector<FileConnected, JsonTextUpdated> collector;

        public JsonFileSynchronizer(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<FileConnected, JsonTextUpdated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<FileConnected, JsonTextUpdated> set)
        {
            File.WriteAllText(set.Message1.FileName, set.Message2.Text);
            OnMessage(new FileSynchronized(set.Message1.FileName, set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.TryGet(out FileConnected connected) &&
                !connected.WasCreated)
            {
                collector = new MessageCollector<FileConnected, JsonTextUpdated>(OnMessagesCollected);
            }
            collector.Push(messageData);
        }
    }
}
