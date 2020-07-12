using System;
using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModelInvalid : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition ModelInvalidDefinition { get; } =
            new MessageDefinition(nameof(ModelInvalid));

        #endregion

        public ModelInvalid(string[] messages, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, ModelInvalidDefinition, childMessages)
        {
            Messages = messages;
        }

        public ModelInvalid(string[] messages, IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, ModelInvalidDefinition, childMessages)
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
