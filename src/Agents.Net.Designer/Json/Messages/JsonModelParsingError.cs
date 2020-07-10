using System.Collections.Generic;
using System.Linq;
using Agents.Net;

namespace Agents.Net.Designer.Json.Messages
{
    public class JsonModelParsingError : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition JsonModelParsingErrorDefinition { get; } =
            new MessageDefinition(nameof(JsonModelParsingError));

        #endregion

        public JsonModelParsingError(IEnumerable<string> messages, Message predecessorMessage,
                                     params Message[] childMessages)
            : base(predecessorMessage, JsonModelParsingErrorDefinition, childMessages)
        {
            Messages = messages;
        }

        public JsonModelParsingError(IEnumerable<string> messages, IEnumerable<Message> predecessorMessages,
                                     params Message[] childMessages)
            : base(predecessorMessages, JsonModelParsingErrorDefinition, childMessages)
        {
            Messages = messages;
        }

        public IEnumerable<string> Messages { get; }

        protected override string DataToString()
        {
            return $"{nameof(Messages)}: {Messages.Count()}";
        }
    }
}
