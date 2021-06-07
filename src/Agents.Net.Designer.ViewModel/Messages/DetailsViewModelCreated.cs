using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class DetailsViewModelCreated : Message
    {
        public DetailsViewModelCreated(DetailsViewModel viewModel, Message predecessorMessage)
            : base(predecessorMessage)
        {
            ViewModel = viewModel;
        }

        public DetailsViewModelCreated(DetailsViewModel viewModel, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
            ViewModel = viewModel;
        }
        
        public DetailsViewModel ViewModel { get; }

        protected override string DataToString()
        {
            return $"{nameof(ViewModel)}: {ViewModel}";
        }
    }
}