using System.Collections.Generic;

namespace Agents.Net.Designer.Serialization.Messages
{
    public class JsonTextLoaded : Message
    {
        public JsonTextLoaded(string text, Message predecessorMessage)
            : base(predecessorMessage)
        {
            Text = text;
        }

        public JsonTextLoaded(string text, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
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
