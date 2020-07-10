using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class GraphViewModelApplied : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition GraphViewModelAppliedDefinition { get; } =
            new MessageDefinition(nameof(GraphViewModelApplied));

        #endregion

        public GraphViewModelApplied(GraphViewModel viewModel, Message predecessorMessage,
                                     params Message[] childMessages)
            : base(predecessorMessage, GraphViewModelAppliedDefinition, childMessages)
        {
            ViewModel = viewModel;
        }

        public GraphViewModelApplied(GraphViewModel viewModel, IEnumerable<Message> predecessorMessages,
                                     params Message[] childMessages)
            : base(predecessorMessages, GraphViewModelAppliedDefinition, childMessages)
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
