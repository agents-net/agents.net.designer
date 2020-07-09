using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class JsonViewModelCreated : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition JsonViewModelCreatedDefinition { get; } =
            new MessageDefinition(nameof(JsonViewModelCreated));

        #endregion

        public JsonViewModelCreated(JsonViewModel viewModel, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, JsonViewModelCreatedDefinition, childMessages)
        {
            ViewModel = viewModel;
        }

        public JsonViewModelCreated(JsonViewModel viewModel, IEnumerable<Message> predecessorMessages,
                                    params Message[] childMessages)
            : base(predecessorMessages, JsonViewModelCreatedDefinition, childMessages)
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
