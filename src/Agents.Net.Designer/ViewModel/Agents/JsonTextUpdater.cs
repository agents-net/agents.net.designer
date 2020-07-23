using System;
using Agents.Net;
using Agents.Net.Designer.Json.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(JsonTextUpdated))]
    [Consumes(typeof(JsonViewModelCreated))]
    public class JsonTextUpdater : Agent
    {        private readonly MessageCollector<JsonTextUpdated, JsonViewModelCreated> collector;

        public JsonTextUpdater(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<JsonTextUpdated, JsonViewModelCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<JsonTextUpdated, JsonViewModelCreated> set)
        {
            set.Message2.ViewModel.Text = set.Message1.Text;
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
