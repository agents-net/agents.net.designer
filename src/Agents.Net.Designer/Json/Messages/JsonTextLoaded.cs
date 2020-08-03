using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Json.Messages
{
    public class JsonTextLoaded : Message
    {
        public JsonTextLoaded(string text, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            Text = text;
        }

        public JsonTextLoaded(string text, IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            Text = text;
        }

        public string Text { get; }

        protected override string DataToString()
        {
            return $"{nameof(Text)}: {Text.Length}";
        }
    }
}
