#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.Tests.Tools.Agents
{
    [Consumes(typeof(ViewModelChangeApplying))]
    public class ViewModelChangeApplier : Agent
    {
        public ViewModelChangeApplier(IMessageBoard messageBoard, string name = null)
            : base(messageBoard, name)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            messageData.Get<ViewModelChangeApplying>().ChangeAction();
        }
    }
}