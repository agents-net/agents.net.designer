using System.Collections.Generic;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class SelectGraphObjectRequested : MessageDecorator
    {
        private SelectGraphObjectRequested(Message decoratedMessage, IEnumerable<Message> additionalPredecessors = null)
            : base(decoratedMessage, additionalPredecessors)
        {
        }

        public static SelectGraphObjectRequested Decorate(SelectedModelObjectChanged decoratedMessage,
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
