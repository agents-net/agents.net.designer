using System.Collections.Generic;
using Agents.Net;

namespace $rootnamespace$
{
    public class $itemname$ : Message
    {
        public $itemname$(Message predecessorMessage): base(predecessorMessage)
        {
        }

        public $itemname$(IEnumerable<Message> predecessorMessages): base(predecessorMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
