using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class TreeViewModelCreated : Message
    {
        public TreeViewModelCreated(TreeViewModel viewModel, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            ViewModel = viewModel;
        }

        public TreeViewModelCreated(TreeViewModel viewModel, IEnumerable<Message> predecessorMessages,
                                    params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            ViewModel = viewModel;
        }
        
        public TreeViewModel ViewModel { get; }

        protected override string DataToString()
        {
            return $"{nameof(ViewModel)}: {ViewModel}";
        }
    }
}