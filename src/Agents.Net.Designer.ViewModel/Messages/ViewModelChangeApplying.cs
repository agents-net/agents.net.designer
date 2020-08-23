using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class ViewModelChangeApplying : Message
    {
        public ViewModelChangeApplying(Action changeAction, Message predecessorMessage, string name = null,
                                       params Message[] childMessages)
            : base(predecessorMessage, name, childMessages)
        {
            ChangeAction = changeAction;
        }

        public ViewModelChangeApplying(Action changeAction, IEnumerable<Message> predecessorMessages,
                                       string name = null,
                                       params Message[] childMessages)
            : base(predecessorMessages, name, childMessages)
        {
            ChangeAction = changeAction;
        }
        
        public Action ChangeAction { get; }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}