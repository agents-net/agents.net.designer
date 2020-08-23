﻿using System.Collections.Generic;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class AgentModelSelectedForGeneration : Message
    {
                                               MessageModel[] producingMessages, Message predecessorMessage,
                                               params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            Agent = agent;
            ConsumingMessages = consumingMessages;
            ProducingMessages = producingMessages;
        }

        public AgentModelSelectedForGeneration(AgentModel agent, MessageModel[] consumingMessages,
                                               MessageModel[] producingMessages,
                                               IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            Agent = agent;
            ConsumingMessages = consumingMessages;
            ProducingMessages = producingMessages;
        }

        public AgentModel Agent { get; }

        public MessageModel[] ConsumingMessages { get; }

        public MessageModel[] ProducingMessages { get; }

        protected override string DataToString()
        {
            return $"{nameof(Agent)}: {Agent}";
        }
    }
}