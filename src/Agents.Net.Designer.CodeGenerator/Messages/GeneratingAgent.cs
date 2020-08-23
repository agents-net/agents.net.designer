﻿using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class GeneratingAgent : Message
    {
                               string[] producingMessages, string[] dependencies, Message predecessorMessage,
                               params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            ConsumingMessages = consumingMessages;
            ProducingMessages = producingMessages;
            Dependencies = dependencies;
        }

        public GeneratingAgent(string[] consumingMessages,
                               string[] producingMessages, string[] dependencies,
                               IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            ConsumingMessages = consumingMessages;
            ProducingMessages = producingMessages;
            Dependencies = dependencies;
        }
        public string[] ConsumingMessages { get; }
        public string[] ProducingMessages { get; }
        public string[] Dependencies { get; }

        protected override string DataToString()
        {
            return $"{nameof(ConsumingMessages)}: {string.Join(", ",ConsumingMessages)}; " +
                   $"{nameof(ProducingMessages)}: {string.Join(", ",ProducingMessages)}; " +
                   $"{nameof(Dependencies)}: {string.Join(", ",Dependencies)}; ";
        }
    }
}