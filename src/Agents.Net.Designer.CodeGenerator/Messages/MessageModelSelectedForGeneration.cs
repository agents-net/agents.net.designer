﻿using System.Collections.Generic;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class MessageModelSelectedForGeneration : Message
    {
                                                 params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            Message = message;
        }

        public MessageModelSelectedForGeneration(MessageModel message, IEnumerable<Message> predecessorMessages,
                                                 params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            Message = message;
        }

        public MessageModel Message { get; }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}