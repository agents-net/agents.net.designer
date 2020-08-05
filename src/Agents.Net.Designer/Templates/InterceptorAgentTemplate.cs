using System;
using Agents.Net;
$dependecies$

namespace $rootnamespace$
{
    $attributes$
    public class $itemname$ : InterceptorAgent
    {
        public $itemname$(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            throw new NotImplementedException();
        }
    }
}
