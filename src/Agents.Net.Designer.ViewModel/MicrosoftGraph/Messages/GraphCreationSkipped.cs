#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.MicrosoftGraph.Messages
{
    public class GraphCreationSkipped : Message
    {
        public GraphCreationSkipped(Message predecessorMessage)
            : base(predecessorMessage)
        {
        }

        public GraphCreationSkipped(IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}