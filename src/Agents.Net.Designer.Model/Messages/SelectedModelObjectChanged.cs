#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class SelectedModelObjectChanged : Message
    {
        public SelectedModelObjectChanged(object selectedObject, Message predecessorMessage, SelectionSource selectionSource)
            : base(predecessorMessage)
        {
            SelectedObject = selectedObject;
            SelectionSource = selectionSource;
        }

        public SelectedModelObjectChanged(object selectedObject, IEnumerable<Message> predecessorMessages, SelectionSource selectionSource)
            : base(predecessorMessages)
        {
            SelectedObject = selectedObject;
            SelectionSource = selectionSource;
        }

        public object SelectedObject { get; }
        
        public SelectionSource SelectionSource { get; }

        protected override string DataToString()
        {
            return $"{nameof(SelectedObject)}: {SelectedObject}; {nameof(SelectionSource)}: {SelectionSource}";
        }
    }

    public enum SelectionSource
    {
        Internal,
        Graph,
        Tree
    }
}
