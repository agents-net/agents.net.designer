using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class GraphViewModelCreated : Message
    {        public GraphViewModelCreated(GraphViewModel viewModel, Message predecessorMessage,
                                     params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            ViewModel = viewModel;
        }

        public GraphViewModelCreated(GraphViewModel viewModel, IEnumerable<Message> predecessorMessages,
                                     params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            ViewModel = viewModel;
        }

        public GraphViewModel ViewModel { get; }

        protected override string DataToString()
        {
            return $"{nameof(ViewModel)}: {ViewModel}";
        }
    }
}
