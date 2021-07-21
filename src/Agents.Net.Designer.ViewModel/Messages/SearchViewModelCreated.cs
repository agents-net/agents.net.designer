#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class SearchViewModelCreated : Message
    {
        public SearchViewModelCreated(Message predecessorMessage, SearchViewModel viewModel)
            : base(predecessorMessage)
        {
            ViewModel = viewModel;
        }

        public SearchViewModelCreated(IEnumerable<Message> predecessorMessages, SearchViewModel viewModel)
            : base(predecessorMessages)
        {
            ViewModel = viewModel;
        }
        
        public SearchViewModel ViewModel { get; }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}