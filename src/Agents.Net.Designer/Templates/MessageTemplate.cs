using System.Collections.Generic;
using Agents.Net;

namespace $rootnamespace$
{
    public class $itemname$ : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition $itemname$Definition { get; } =
            new MessageDefinition(nameof($itemname$));

        #endregion

        public $itemname$(Message predecessorMessage, params Message[] childMessages)
			: base(predecessorMessage, $itemname$Definition, childMessages)
        {
        }

        public $itemname$(IEnumerable<Message> predecessorMessages, params Message[] childMessages)
			: base(predecessorMessages, $itemname$Definition, childMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
