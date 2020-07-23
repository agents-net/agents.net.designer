using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class JsonViewModelCreated : Message
    {        public JsonViewModelCreated(JsonViewModel viewModel, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            ViewModel = viewModel;
        }

        public JsonViewModelCreated(JsonViewModel viewModel, IEnumerable<Message> predecessorMessages,
                                    params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            ViewModel = viewModel;
        }

        public JsonViewModel ViewModel { get; }

        protected override string DataToString()
        {
            return $"{nameof(ViewModel)}: {ViewModel}";
        }
    }
}
