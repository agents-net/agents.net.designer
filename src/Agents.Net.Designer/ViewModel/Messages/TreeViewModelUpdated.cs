using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class TreeViewModelUpdated : Message
    {
        public TreeViewModelUpdated(Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
        }

        public TreeViewModelUpdated(IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
