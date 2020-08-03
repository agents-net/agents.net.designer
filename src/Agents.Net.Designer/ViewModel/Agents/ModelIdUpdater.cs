using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Agents.Net;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(ModelUpdated))]
    [Consumes(typeof(TreeViewModelCreated))]
    public class ModelIdUpdater : Agent
    {
        private readonly MessageCollector<TreeViewModelCreated, ModelUpdated> collector;

        public ModelIdUpdater(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<TreeViewModelCreated, ModelUpdated>(OnMessagesCollected);
        }

        private CommunityModel lastModel;

        private void OnMessagesCollected(MessageCollection<TreeViewModelCreated, ModelUpdated> set)
        {
            set.MarkAsConsumed(set.Message2);
            CommunityModel oldModel = Interlocked.Exchange(ref lastModel, set.Message2.Model);
            if (oldModel == null)
            {
                return;
            }

            AgentModel[] newAgents = set.Message2.Model.Agents.Except(oldModel.Agents).ToArray();
            MessageModel[] newMessages = set.Message2.Model.Messages.Except(oldModel.Messages).ToArray();

            Stack<TreeViewItem> unvisited = new Stack<TreeViewItem>(new []{set.Message1.ViewModel.Community});
            while (unvisited.Any())
            {
                TreeViewItem item = unvisited.Pop();
                if (item is AgentViewModel agentViewModel &&
                    agentViewModel.ModelId == default)
                {
                    agentViewModel.ModelId =
                        newAgents.FirstOrDefault(a => a.FullName() == agentViewModel.FullName)
                                 ?.Id ?? default;
                }
                if (item is MessageViewModel messageViewModel &&
                    messageViewModel.ModelId == default)
                {
                    messageViewModel.ModelId =
                        newMessages.FirstOrDefault(a => a.FullName() == messageViewModel.FullName)
                                 ?.Id ?? default;
                }

                foreach (TreeViewItem child in item.Items)
                {
                    unvisited.Push(child);
                }
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
