#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class GeneratingInterceptorAgent : MessageDecorator
    {
        private GeneratingInterceptorAgent(string[] interceptingMessages, Message decoratedMessage)
            : base(decoratedMessage)
        {
            InterceptingMessages = interceptingMessages;
        }

        public static GeneratingInterceptorAgent Decorate(GeneratingAgent agent, string[] interceptingMessages)
        {
            return new(interceptingMessages, agent);
        }

        public string[] InterceptingMessages { get; }

        protected override string DataToString()
        {
            return $"{nameof(InterceptingMessages)}: {string.Join(", ",InterceptingMessages)}; ";
        }
    }
}
