using System.Collections.Generic;
using Agents.Net;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class SelectedObjectChanged : Message
    {        public SelectedObjectChanged(DrawingObject selectedObject, Message predecessorMessage,
                                     params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            SelectedObject = selectedObject;
        }

        public SelectedObjectChanged(DrawingObject selectedObject, IEnumerable<Message> predecessorMessages,
                                     params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            SelectedObject = selectedObject;
        }

        public DrawingObject SelectedObject { get; }

        protected override string DataToString()
        {
            return $"{nameof(SelectedObject)}: {SelectedObject}";
        }
    }
}
