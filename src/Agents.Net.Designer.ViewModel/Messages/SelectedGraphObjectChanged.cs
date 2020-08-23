using System.Collections.Generic;
using Agents.Net;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class SelectedGraphObjectChanged : Message
    {        public SelectedGraphObjectChanged(DrawingObject selectedObject, Message predecessorMessage,
                                     params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            SelectedObject = selectedObject;
        }

        public SelectedGraphObjectChanged(DrawingObject selectedObject, IEnumerable<Message> predecessorMessages,
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
