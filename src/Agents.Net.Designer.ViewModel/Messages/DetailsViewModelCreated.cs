using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class DetailsViewModelCreated : Message
    {
        public DetailsViewModelCreated(DetailsViewModel viewModel, Message predecessorMessage,
                                       params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            ViewModel = viewModel;
        }

        public DetailsViewModelCreated(DetailsViewModel viewModel, IEnumerable<Message> predecessorMessages,
                                       params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
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