using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.Serialization.Messages
{
    public class FileSynchronizationProcessing : MessageDecorator
    {
        private FileSynchronizationProcessing(Message decoratedMessage, IEnumerable<Message> additionalPredecessors = null)
            : base(decoratedMessage, additionalPredecessors)
        {
        }

        public static FileSynchronizationProcessing Decorate(JsonTextUpdated message)
        {
            return new(message);
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}