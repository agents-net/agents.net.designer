using System;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(SelectedModelObjectChanged))]
    [Consumes(typeof(DeleteItemRequested))]
    [Produces(typeof(ModifyModel))]
    public class DeleteSelectedModelObject : Agent
    {
        private readonly MessageCollector<SelectedModelObjectChanged, DeleteItemRequested> collector;

        public DeleteSelectedModelObject(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<SelectedModelObjectChanged, DeleteItemRequested>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<SelectedModelObjectChanged, DeleteItemRequested> set)
        {
            set.MarkAsConsumed(set.Message2);
            CommunityModel model = null;
            PropertySpecifier property = null;
            if (set.Message1.SelectedObject is AgentModel agent)
            {
                model = agent.ContainingPackage;
                property = new PackageAgentsProperty();
            }
            else if (set.Message1.SelectedObject is MessageModel message)
            {
                model = message.ContainingPackage;
                property = new PackageMessagesProperty();
            }

            if (model != null)
            {
                OnMessage(new ModifyModel(ModelModification.Remove, 
                                          set.Message1.SelectedObject, 
                                          null,
                                          model, 
                                          property, 
                                          set));
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
