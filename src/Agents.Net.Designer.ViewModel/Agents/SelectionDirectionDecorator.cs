#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Intercepts(typeof(SelectedModelObjectChanged))]
    [Produces(typeof(SelectTreeObjectRequested))]
    [Produces(typeof(SelectGraphObjectRequested))]
    public class SelectionDirectionDecorator : InterceptorAgent
    {
        private object lastModelObject;
        
        public SelectionDirectionDecorator(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            SelectedModelObjectChanged modelObjectChanged = messageData.Get<SelectedModelObjectChanged>();
            object lastSelected = Interlocked.Exchange(ref lastModelObject, modelObjectChanged.SelectedObject);
            if (lastSelected == modelObjectChanged.SelectedObject)
            {
                //do not bounce back and forth
                return InterceptionAction.Continue;
            }
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
