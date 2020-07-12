using System;
using Agents.Net;
$dependecies$

namespace $rootnamespace$
{
    public class $itemname$ : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition $itemname$Definition { get; }
            = new AgentDefinition($consumingmessages$,
                                  $producingmessages$);

        #endregion

        public $itemname$(IMessageBoard messageBoard) : base($itemname$Definition, messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            throw new NotImplementedException();
        }
    }
}
