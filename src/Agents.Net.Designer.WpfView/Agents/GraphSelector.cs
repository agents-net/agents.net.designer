using System;
using System.Linq;
using System.Reflection;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;
using Agents.Net.Designer.WpfView.Messages;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.WpfView.Agents
{
    [Consumes(typeof(MainWindowCreated))]
    [Consumes(typeof(SelectGraphObjectRequested))]
    [Consumes(typeof(SelectedModelObjectChanged), Implicitly = true)]
    [Produces(typeof(ViewModelChangeApplying))]
    public class GraphSelector : Agent
    {
        private readonly MessageCollector<MainWindowCreated, SelectedModelObjectChanged> collector;
        public GraphSelector(IMessageBoard messageBoard)
            : base(messageBoard)
        {
            collector = new MessageCollector<MainWindowCreated, SelectedModelObjectChanged>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<MainWindowCreated, SelectedModelObjectChanged> set)
        {
            set.MarkAsConsumed(set.Message2);

            if (set.Message2.SelectedObject == null)
            {
                return;
            }

            IViewerNode node = set.Message1.Window.GraphViewer.Entities
                                  .OfType<IViewerNode>()
                                  .FirstOrDefault(e => e.DrawingObject.UserData == set.Message2.SelectedObject);
            if (node == null)
            {
                return;
            }

            OnMessage(new ViewModelChangeApplying(() =>
            {
                set.Message1.Window.GraphViewer.NodeToCenterWithScale(node.Node, 1);
            }, set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}