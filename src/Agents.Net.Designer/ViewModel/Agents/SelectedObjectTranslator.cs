using System;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(SelectedObjectChanged))]
    [Produces(typeof(SelectedModelObjectChanged))]
    public class SelectedObjectTranslator : Agent
    {        public SelectedObjectTranslator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            SelectedObjectChanged selectedObjectChanged = messageData.Get<SelectedObjectChanged>();
            if (selectedObjectChanged.SelectedObject is Node node)
            {
                OnMessage(new SelectedModelObjectChanged(node.UserData, messageData));
            }
        }
    }
}
