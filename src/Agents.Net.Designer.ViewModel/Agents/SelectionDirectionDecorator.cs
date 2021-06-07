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
            SelectedModelObjectChanged modelObjectChanged = messageData.Get<SelectedModelObjectChanged>();
            switch (modelObjectChanged.SelectionSource)
            {
                case SelectionSource.Graph:
                    SelectTreeObjectRequested.Decorate(messageData.Get<SelectedModelObjectChanged>());
                    break;
                case SelectionSource.Tree:
                    SelectGraphObjectRequested.Decorate(messageData.Get<SelectedModelObjectChanged>());
                    break;
                case SelectionSource.Internal:
                default:
                    SelectTreeObjectRequested.Decorate(messageData.Get<SelectedModelObjectChanged>());
                    SelectGraphObjectRequested.Decorate(messageData.Get<SelectedModelObjectChanged>());
                    break;
            }
            return InterceptionAction.Continue;
        }
    }
}
