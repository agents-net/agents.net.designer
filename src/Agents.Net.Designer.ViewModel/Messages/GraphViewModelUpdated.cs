using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class GraphViewModelUpdated : Message
    {
        public GraphViewModelUpdated(Message predecessorMessage)
            : base(predecessorMessage)
        {
        }

        public GraphViewModelUpdated(IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
