using System;
using System.Collections.Generic;
using System.Text;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Intercepts(typeof(SelectedModelObjectChanged))]
    [Produces(typeof(SelectTreeObjectRequested))]
    [Produces(typeof(SelectGraphObjectRequested))]
    public class SelectionDirectionDecorator : InterceptorAgent
    {
        public SelectionDirectionDecorator(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            if (messageData.TryGetPredecessor(out SelectedGraphObjectChanged _))
            {
                SelectTreeObjectRequested.Decorate(messageData.Get<SelectedModelObjectChanged>());
            }
            else if (messageData.TryGetPredecessor(out SelectedTreeViewItemChanged _))
            {
                SelectGraphObjectRequested.Decorate(messageData.Get<SelectedModelObjectChanged>());
            }
            else
            {
                SelectTreeObjectRequested.Decorate(messageData.Get<SelectedModelObjectChanged>());
                SelectGraphObjectRequested.Decorate(messageData.Get<SelectedModelObjectChanged>());
            }
            return InterceptionAction.Continue;
        }
    }
}
