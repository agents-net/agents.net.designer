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
    {        private readonly MessageCollector<FileConnected, JsonModelSourceChanged> collector;
        private readonly HashSet<FileConnected> firstTimeExecutions = new HashSet<FileConnected>();

        public JsonFileSynchronizer(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<FileConnected, JsonModelSourceChanged>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<FileConnected, JsonModelSourceChanged> set)
        {
            lock (firstTimeExecutions)
            {
                if (firstTimeExecutions.Add(set.Message1) && !set.Message1.WasCreated)
                {
                    return;
                }
            }
            File.WriteAllText(set.Message1.FileName, set.Message2.JsonModel);
            OnMessage(new FileSynchronized(set.Message1.FileName, set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
