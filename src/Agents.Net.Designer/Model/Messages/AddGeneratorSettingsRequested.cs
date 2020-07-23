﻿using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class AddGeneratorSettingsRequested : Message
    {
            : base(predecessorMessage, childMessages:childMessages)
        {
        }

        public AddGeneratorSettingsRequested(IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}