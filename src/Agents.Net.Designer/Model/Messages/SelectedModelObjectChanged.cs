using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    //TODO Add Interceptor agents
    public class SelectedModelObjectChanged : Message
    {        public SelectedModelObjectChanged(object selectedObject, Message predecessorMessage,
                                          params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            SelectedObject = selectedObject;
        }

        public SelectedModelObjectChanged(object selectedObject, IEnumerable<Message> predecessorMessages,
                                          params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            SelectedObject = selectedObject;
        }

        public object SelectedObject { get; }

        protected override string DataToString()
        {
            return $"{nameof(SelectedObject)}: {SelectedObject}";
        }
    }
}
