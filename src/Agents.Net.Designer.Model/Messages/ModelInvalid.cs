#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModelInvalid : Message
    {
        public ModelInvalid(string[] messages, Message predecessorMessage)
            : base(predecessorMessage)
        {
            Messages = messages;
        }

        public ModelInvalid(string[] messages, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
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
