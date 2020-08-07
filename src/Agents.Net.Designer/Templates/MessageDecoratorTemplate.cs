using System.Collections.Generic;
using Agents.Net;$using$

namespace $rootnamespace$
{
    public class $itemname$ : MessageDecorator
    {
        private $itemname$(Message decoratedMessage, IEnumerable<Message> additionalPredecessors = null)
            : base(decoratedMessage, additionalPredecessors)
        {
        }

        public static $itemname$ Decorate($decoratedmessage$ decoratedMessage,
                                          IEnumerable<Message> additionalPredecessors = null)
        {
            return new $itemname$(decoratedMessage, additionalPredecessors);
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
