#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using System.Linq;

namespace Agents.Net.Designer.Serialization.Messages
{
    public class JsonModelParsingError : Message
    {
        public JsonModelParsingError(IEnumerable<string> messages, Message predecessorMessage)
            : base(predecessorMessage)
        {
            Messages = messages;
        }

        public JsonModelParsingError(IEnumerable<string> messages, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
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
