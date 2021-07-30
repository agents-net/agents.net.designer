#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.MicrosoftGraph.Messages
{
    public class GraphScopeChanged : Message
    {
        public GraphScopeChanged(Message predecessorMessage, GraphViewScope newScope)
            : base(predecessorMessage)
        {
            NewScope = newScope;
        }

        public GraphScopeChanged(IEnumerable<Message> predecessorMessages, GraphViewScope newScope)
            : base(predecessorMessages)
        {
            NewScope = newScope;
        }
        
        public GraphViewScope NewScope { get; }

        protected override string DataToString()
        {
            return $"{nameof(NewScope)}: {NewScope}";
        }
    }
}