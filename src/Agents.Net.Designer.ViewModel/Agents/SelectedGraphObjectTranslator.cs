#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(SelectedGraphObjectChanged))]
    [Produces(typeof(SelectedModelObjectChanged))]
    public class SelectedGraphObjectTranslator : Agent
    {
        public SelectedGraphObjectTranslator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            SelectedGraphObjectChanged selectedGraphObjectChanged = messageData.Get<SelectedGraphObjectChanged>();
            if (selectedGraphObjectChanged.SelectedObject is Node node)
            {
                OnMessage(new SelectedModelObjectChanged(node.UserData, messageData, SelectionSource.Graph));
            }
        }
    }
}
