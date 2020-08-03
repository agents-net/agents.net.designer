using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class SelectedTreeViewItemChanged : Message
    {
        public SelectedTreeViewItemChanged(TreeViewItem selectedItem, Message predecessorMessage,
                                           params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            SelectedItem = selectedItem;
        }

        public SelectedTreeViewItemChanged(TreeViewItem selectedItem, IEnumerable<Message> predecessorMessages,
                                           params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            SelectedItem = selectedItem;
        }

        public TreeViewItem SelectedItem { get; }
        
        protected override string DataToString()
        {
            return $"{nameof(SelectedItem)}:{SelectedItem}";
        }
    }
}