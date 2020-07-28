using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Agents;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Consumes(typeof(TreeViewModelCreated))]
    [Produces(typeof(ViewModelChangeApplying))]
    public class AgentViewModelUpdater : Agent
    {
        private readonly MessageCollector<TreeViewModelCreated, ModifyModel> collector;
        public AgentViewModelUpdater(IMessageBoard messageBoard)
            : base(messageBoard)
        {
            collector = new MessageCollector<TreeViewModelCreated, ModifyModel>(OnMessagesCollected);
        }

        //TODO Continue with next property
        private void OnMessagesCollected(MessageCollection<TreeViewModelCreated, ModifyModel> set)
        {
            set.MarkAsConsumed(set.Message2);
            if (!(set.Message2.Target is AgentModel oldModel))
            {
                return;
            }

            AgentViewModel changingViewModel = (AgentViewModel) set.Message1.ViewModel.Community.FindViewItemById(oldModel.Id);
            OnMessage(new ViewModelChangeApplying(() =>
            {
                switch (set.Message2.Property)
                {
                    case AgentNameProperty _:
                        changingViewModel.Name = set.Message2.NewValue.AssertTypeOf<string>();
                        changingViewModel.FullName = $"{changingViewModel.Namespace}.{changingViewModel.Name}";
                        break;
                    case AgentNamespaceProperty _:
                        changingViewModel.Namespace = set.Message2.NewValue.AssertTypeOf<string>();
                        changingViewModel.FullName = $"{changingViewModel.Namespace}.{changingViewModel.Name}";
                        RestructureViewModel(changingViewModel, set.Message1.ViewModel);
                        break;
                }
            }, set));
        }

        private void RestructureViewModel(AgentViewModel changingViewModel, TreeViewModel viewModel)
        {
            viewModel.Community.RemoveItem(changingViewModel);
            viewModel.Community.AddItem(changingViewModel);
            changingViewModel.IsSelected = true;
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}