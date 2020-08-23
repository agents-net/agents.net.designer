using System;
using Agents.Net.Designer.ViewModel;

namespace Agents.Net.Designer.WpfView
{
    public class SelectedTreeViewItemChangedArgs : EventArgs
    {
        public SelectedTreeViewItemChangedArgs(TreeViewItem selectedItem)
        {
            SelectedItem = selectedItem;
        }

        public TreeViewItem SelectedItem { get; }
    }
}