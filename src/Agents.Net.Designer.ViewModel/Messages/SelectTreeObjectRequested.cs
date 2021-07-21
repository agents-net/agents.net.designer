#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class SelectTreeObjectRequested : MessageDecorator
    {
        private SelectTreeObjectRequested(Message decoratedMessage, IEnumerable<Message> additionalPredecessors = null)
            : base(decoratedMessage, additionalPredecessors)
        {
        }

        public static SelectTreeObjectRequested Decorate(SelectedModelObjectChanged decoratedMessage,
                                                         IEnumerable<Message> additionalPredecessors = null)
        {
            return new(decoratedMessage, additionalPredecessors);
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
