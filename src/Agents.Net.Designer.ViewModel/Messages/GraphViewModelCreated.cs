using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class GraphViewModelCreated : Message
    {
        public GraphViewModelCreated(GraphViewModel viewModel, Message predecessorMessage)
            : base(predecessorMessage)
        {
            ViewModel = viewModel;
        }

        public GraphViewModelCreated(GraphViewModel viewModel, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
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
