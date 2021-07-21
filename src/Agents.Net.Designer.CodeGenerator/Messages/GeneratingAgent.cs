#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class GeneratingAgent : MessageDecorator
    {
        private GeneratingAgent(string[] consumingMessages,
                               string[] producingMessages, string[] dependencies, Message decoratedMessage)
            : base(decoratedMessage)
        {
            ConsumingMessages = consumingMessages;
            ProducingMessages = producingMessages;
            Dependencies = dependencies;
        }
        
        public string[] ConsumingMessages { get; }
        public string[] ProducingMessages { get; }
        public string[] Dependencies { get; }

        public static GeneratingAgent Decorate(GeneratingFile file, string[] consumingMessages,
                                               string[] producingMessages, string[] dependencies)
        {
            return new(consumingMessages, producingMessages, dependencies, file);
        }

        protected override string DataToString()
        {
            return $"{nameof(ConsumingMessages)}: {string.Join(", ",ConsumingMessages)}; " +
                   $"{nameof(ProducingMessages)}: {string.Join(", ",ProducingMessages)}; " +
                   $"{nameof(Dependencies)}: {string.Join(", ",Dependencies)}; ";
        }
    }
}
