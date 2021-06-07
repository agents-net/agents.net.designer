using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class TreeViewModelUpdated : Message
    {
        public TreeViewModelUpdated(Message predecessorMessage)
            : base(predecessorMessage)
        {
        }

        public TreeViewModelUpdated(IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
