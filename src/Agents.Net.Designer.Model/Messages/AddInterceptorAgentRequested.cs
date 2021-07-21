#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class AddInterceptorAgentRequested : Message
    {
        public AddInterceptorAgentRequested(Message predecessorMessage)
            : base(predecessorMessage)
        {
        }

        public AddInterceptorAgentRequested(IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
