#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Consumes(typeof(ModificationResult))]
    [Consumes(typeof(TreeViewModelUpdated))]
    [Consumes(typeof(GraphViewModelUpdated))]
    [Produces(typeof(ModelModified))]
    public class ModelModificationCompleter : Agent
    {
        private MessageCollector<ModificationResult, TreeViewModelUpdated, GraphViewModelUpdated> collector;
        public ModelModificationCompleter(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.Is<ModifyModel>())
            {
                collector ??= new MessageCollector<ModificationResult, TreeViewModelUpdated, GraphViewModelUpdated>(OnMessagesCollected);
                return;
            }
            collector?.Push(messageData);
        }

        private void OnMessagesCollected(MessageCollection<ModificationResult, TreeViewModelUpdated, GraphViewModelUpdated> set)
        {
            OnMessage(new ModelModified(set, set.Message1.Model));
        }
    }
}