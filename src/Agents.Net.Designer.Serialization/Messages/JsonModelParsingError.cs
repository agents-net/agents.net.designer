using System.Collections.Generic;
using System.Linq;

namespace Agents.Net.Designer.Serialization.Messages
{
    public class JsonModelParsingError : Message
    {        public JsonModelParsingError(IEnumerable<string> messages, Message predecessorMessage,
                                     params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            Messages = messages;
        }

        public JsonModelParsingError(IEnumerable<string> messages, IEnumerable<Message> predecessorMessages,
                                     params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
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
