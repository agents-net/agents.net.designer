#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Threading;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Consumes(typeof(ModelVersionCreated))]
    [Produces(typeof(SelectedModelObjectChanged))]
    public class NewItemsSelector : Agent
    {
        private object selectable;
        public NewItemsSelector(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.TryGet(out ModifyModel modifyModel))
            {
                if (modifyModel.Modification.ModificationType != ModificationType.Add ||
                    modifyModel.Modification.Target is not CommunityModel ||
                    !modifyModel.IsLast)
                {
                    return;
                }

                selectable = modifyModel.Modification.NewValue;
                return;
            }

            object lastSelectable = Interlocked.Exchange(ref selectable, null);
            if (lastSelectable == null)
            {
                return;
            }
            OnMessage(new SelectedModelObjectChanged(lastSelectable, messageData, SelectionSource.Internal));
        }
    }
}
