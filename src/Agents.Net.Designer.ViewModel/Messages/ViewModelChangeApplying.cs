#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class ViewModelChangeApplying : Message
    {
        public ViewModelChangeApplying(Action changeAction, Message predecessorMessage)
            : base(predecessorMessage)
        {
            ChangeAction = changeAction;
        }

        public ViewModelChangeApplying(Action changeAction, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
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