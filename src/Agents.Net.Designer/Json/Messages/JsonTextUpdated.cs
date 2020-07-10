using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Json.Messages
{
    public class JsonTextUpdated : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition JsonTextUpdatedDefinition { get; } =
            new MessageDefinition(nameof(JsonTextUpdated));

        #endregion

        public JsonTextUpdated(string text, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, JsonTextUpdatedDefinition, childMessages)
        {
            Text = text;
        }

        public JsonTextUpdated(string text, IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, JsonTextUpdatedDefinition, childMessages)
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
