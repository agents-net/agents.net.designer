using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class SelectedTreeViewItemChanged : Message
    {
        public SelectedTreeViewItemChanged(TreeViewItem selectedItem, Message predecessorMessage)
            : base(predecessorMessage)
        {
            SelectedItem = selectedItem;
        }

        public SelectedTreeViewItemChanged(TreeViewItem selectedItem, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
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