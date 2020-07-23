using System;
using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModelInvalid : Message
    {        public ModelInvalid(string[] messages, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            Messages = messages;
        }

        public ModelInvalid(string[] messages, IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            Messages = messages;
        }

        public string[] Messages { get; }

        protected override string DataToString()
        {
            return $"{nameof(Messages)}:{Environment.NewLine}{string.Join(Environment.NewLine, Messages)}";
        }
    }
}
