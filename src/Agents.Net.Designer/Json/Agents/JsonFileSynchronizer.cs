using System;
using System.Collections.Generic;
using System.IO;
using Agents.Net;
using Agents.Net.Designer.Json.Messages;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Json.Agents
{
    [Consumes(typeof(FileConnected))]
    [Consumes(typeof(JsonModelSourceChanged))]
    [Produces(typeof(FileSynchronized))]
    public class JsonFileSynchronizer : Agent
    {
        private readonly MessageCollector<FileConnected, JsonModelSourceChanged> collector;

        public JsonFileSynchronizer(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<FileConnected, JsonModelSourceChanged>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<FileConnected, JsonModelSourceChanged> set)
        {
            set.MarkAsConsumed(set.Message1);
            File.WriteAllText(set.Message1.FileName, set.Message2.JsonModel);
            OnMessage(new FileSynchronized(set.Message1.FileName, set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
