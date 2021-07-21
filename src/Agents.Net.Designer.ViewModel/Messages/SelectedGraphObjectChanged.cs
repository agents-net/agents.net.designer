#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using Agents.Net;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class SelectedGraphObjectChanged : Message
    {
        public SelectedGraphObjectChanged(DrawingObject selectedObject, Message predecessorMessage)
            : base(predecessorMessage)
        {
            SelectedObject = selectedObject;
        }

        public SelectedGraphObjectChanged(DrawingObject selectedObject, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
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
