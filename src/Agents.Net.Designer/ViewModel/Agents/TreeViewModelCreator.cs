using System;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(InitializeMessage))]
    [Produces(typeof(TreeViewModelCreated))]
    public class TreeViewModelCreator : Agent
    {
        public TreeViewModelCreator(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            OnMessage(new TreeViewModelCreated(new TreeViewModel(), messageData));
        }
    }
}