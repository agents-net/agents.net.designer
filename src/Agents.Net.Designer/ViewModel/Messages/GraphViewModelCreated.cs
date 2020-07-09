using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class GraphViewModelCreated : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition GraphViewModelCreatedDefinition { get; } =
            new MessageDefinition(nameof(GraphViewModelCreated));

        #endregion

        public GraphViewModelCreated(GraphViewModel viewModel, Message predecessorMessage,
                                     params Message[] childMessages)
            : base(predecessorMessage, GraphViewModelCreatedDefinition, childMessages)
        {
            ViewModel = viewModel;
        }

        public GraphViewModelCreated(GraphViewModel viewModel, IEnumerable<Message> predecessorMessages,
                                     params Message[] childMessages)
            : base(predecessorMessages, GraphViewModelCreatedDefinition, childMessages)
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
