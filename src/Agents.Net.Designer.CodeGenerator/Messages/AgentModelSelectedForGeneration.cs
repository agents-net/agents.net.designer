﻿#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class AgentModelSelectedForGeneration : MessageDecorator
    {
        private AgentModelSelectedForGeneration(AgentModel agent, MessageModel[] consumingMessages,
                                               MessageModel[] producingMessages, Message decoratedMessage)
            : base(decoratedMessage)
        {
            Agent = agent;
            ConsumingMessages = consumingMessages;
            ProducingMessages = producingMessages;
        }

        public static AgentModelSelectedForGeneration Decorate(ModelSelectedForGeneration message, AgentModel agent,
                                                               MessageModel[] consumingMessages,
                                                               MessageModel[] producingMessages)
        {
            return new(agent, consumingMessages, producingMessages,
                       message);
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
