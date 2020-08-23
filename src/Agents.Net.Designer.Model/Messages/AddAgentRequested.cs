﻿using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class AddAgentRequested : Message
    {
            : base(predecessorMessage, childMessages:childMessages)
        {
        }

        public AddAgentRequested(IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}