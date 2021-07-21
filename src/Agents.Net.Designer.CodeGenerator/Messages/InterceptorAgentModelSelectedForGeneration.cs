#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class InterceptorAgentModelSelectedForGeneration : MessageDecorator
    {
        private InterceptorAgentModelSelectedForGeneration(MessageModel[] interceptingMessages, Message predecessorMessage)
            : base(predecessorMessage)
        {
            InterceptingMessages = interceptingMessages;
        }

        public static InterceptorAgentModelSelectedForGeneration Decorate(
            AgentModelSelectedForGeneration message, MessageModel[] interceptingMessages)
        {
            return new(interceptingMessages, message);
        }

        public MessageModel[] InterceptingMessages { get; }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
