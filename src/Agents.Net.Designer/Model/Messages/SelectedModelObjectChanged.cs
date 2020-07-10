using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class SelectedModelObjectChanged : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition SelectedModelObjectChangedDefinition { get; } =
            new MessageDefinition(nameof(SelectedModelObjectChanged));

        #endregion

        public SelectedModelObjectChanged(object selectedObject, Message predecessorMessage,
                                          params Message[] childMessages)
            : base(predecessorMessage, SelectedModelObjectChangedDefinition, childMessages)
        {
            SelectedObject = selectedObject;
        }

        public SelectedModelObjectChanged(object selectedObject, IEnumerable<Message> predecessorMessages,
                                          params Message[] childMessages)
            : base(predecessorMessages, SelectedModelObjectChangedDefinition, childMessages)
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
