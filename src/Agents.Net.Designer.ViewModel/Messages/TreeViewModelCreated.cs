#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class TreeViewModelCreated : Message
    {
        public TreeViewModelCreated(TreeViewModel viewModel, Message predecessorMessage)
            : base(predecessorMessage)
        {
            ViewModel = viewModel;
        }

        public TreeViewModelCreated(TreeViewModel viewModel, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
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