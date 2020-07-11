using System;
using System.Collections.Generic;
using System.IO;
using Agents.Net;
using Agents.Net.Designer.Json.Messages;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Json.Agents
{
    public class JsonFileSynchronizer : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition JsonFileSynchronizerDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      FileConnected.FileConnectedDefinition,
                                      JsonModelSourceChanged.JsonModelSourceChangedDefinition
                                  },
                                  new []
                                  {
                                      FileSynchronized.FileSynchronizedDefinition
                                  });

        #endregion

        private readonly MessageCollector<FileConnected, JsonModelSourceChanged> collector;
        private readonly HashSet<FileConnected> firstTimeExecutions = new HashSet<FileConnected>();

        public JsonFileSynchronizer(IMessageBoard messageBoard) : base(JsonFileSynchronizerDefinition, messageBoard)
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
