using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class AddMessageDecoratorRequested : Message
    {
        public AddMessageDecoratorRequested(Message predecessorMessage)
            : base(predecessorMessage)
        {
        }

        public AddMessageDecoratorRequested(IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
