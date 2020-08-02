using System;
using Agents.Net;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Consumes(typeof(TreeViewModelCreated))]
    [Produces(typeof(ViewModelChangeApplying))]
    public class CommunityViewModelUpdater : Agent
    {
        private readonly MessageCollector<TreeViewModelCreated, ModifyModel> collector;

        public CommunityViewModelUpdater(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<TreeViewModelCreated, ModifyModel>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<TreeViewModelCreated, ModifyModel> set)
        {
            set.MarkAsConsumed(set.Message2);
            if (!(set.Message2.Target is CommunityModel oldModel))
            {
                return;
            }

            CommunityViewModel changingViewModel = set.Message1.ViewModel.Community;
            OnMessage(new ViewModelChangeApplying(() =>
            {
                switch (set.Message2.Property)
                {
                    case MessagesProperty _:
                        ChangeMessages(set.Message2, changingViewModel, oldModel);
                        break;
                    case AgentsProperty _:
                        ChangeAgents(set.Message2, changingViewModel, oldModel);
                        break;
                }
            }, set));
        }

        private void ChangeAgents(ModifyModel modifyModel, CommunityViewModel changingViewModel,
                                  CommunityModel oldModel)
        {
            switch (modifyModel.ModificationType)
            {
                case ModelModification.Add:
                    AddAgent();
                    break;
                case ModelModification.Remove:
                    RemoveAgent();
                    break;
                case ModelModification.Change:
                    RemoveAgent();
                    AddAgent();
                    break;
            }

            void AddAgent()
            {
                AgentModel agentModel = modifyModel.NewValue.AssertTypeOf<AgentModel>();
                AgentViewModel viewModel = agentModel.CreateViewModel(oldModel, changingViewModel);
                changingViewModel.AddItem(viewModel);
            }

            void RemoveAgent()
            {
                AgentModel agentModel = modifyModel.OldValue.AssertTypeOf<AgentModel>();
                AgentViewModel viewModel = (AgentViewModel) changingViewModel.FindViewItemById(agentModel.Id);
                changingViewModel.RemoveItem(viewModel);
            }
        }

        //TODO Add modelid updater on model updated.
        private void ChangeMessages(ModifyModel modifyModel, CommunityViewModel changingViewModel,
                                    CommunityModel oldModel)
        {
            switch (modifyModel.ModificationType)
            {
                case ModelModification.Add:
                    AddMessage();
                    break;
                case ModelModification.Remove:
                    RemoveMessage();
                    break;
                case ModelModification.Change:
                    RemoveMessage();
                    AddMessage();
                    break;
            }

            void AddMessage()
            {
                MessageModel messageModel = modifyModel.NewValue.AssertTypeOf<MessageModel>();
                MessageViewModel viewModel = messageModel.CreateViewModel(oldModel);
                if (!messageModel.BuildIn)
                {
                    changingViewModel.AddItem(viewModel);
                }

                changingViewModel.FindItemByType<AgentViewModel>()?.AvailableItems.AvailableMessages.Add(viewModel);
            }

            void RemoveMessage()
            {
                MessageModel messageModel = modifyModel.OldValue.AssertTypeOf<MessageModel>();
                MessageViewModel viewModel = (MessageViewModel) changingViewModel.FindViewItemById(messageModel.Id);
                changingViewModel.RemoveItem(viewModel);
                changingViewModel.FindItemByType<AgentViewModel>()?.AvailableItems.AvailableMessages
                                 .Remove(viewModel);
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
