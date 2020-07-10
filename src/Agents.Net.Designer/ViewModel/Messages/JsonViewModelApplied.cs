using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class JsonViewModelApplied : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition JsonViewModelAppliedDefinition { get; } =
            new MessageDefinition(nameof(JsonViewModelApplied));

        #endregion

        public JsonViewModelApplied(JsonViewModel viewModel, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, JsonViewModelAppliedDefinition, childMessages)
        {
            ViewModel = viewModel;
        }

        public JsonViewModelApplied(JsonViewModel viewModel, IEnumerable<Message> predecessorMessages,
                                    params Message[] childMessages)
            : base(predecessorMessages, JsonViewModelAppliedDefinition, childMessages)
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
