﻿using System;
using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModelInvalid : Message
    {
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