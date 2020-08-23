using System.Collections.Generic;
using Agents.Net;

namespace $rootnamespace$
{
    public class $itemname$ : Message
    {
        public $itemname$(Message predecessorMessage, params Message[] childMessages)
			: base(predecessorMessage, childMessages:childMessages)
        {
        }

        public $itemname$(IEnumerable<Message> predecessorMessages, params Message[] childMessages)
			: base(predecessorMessages, childMessages:childMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
