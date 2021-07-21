#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.Model.Messages
{
    public class ExtendedModificationRequest : MessageDecorator
    {
        private ExtendedModificationRequest(Message decoratedMessage, IEnumerable<Message> additionalPredecessors = null)
            : base(decoratedMessage, additionalPredecessors)
        {
        }

        public static ExtendedModificationRequest Decorate(ModificationRequest request)
        {
            return new(request);
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}