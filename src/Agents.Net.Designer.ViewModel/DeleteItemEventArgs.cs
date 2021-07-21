#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;

namespace Agents.Net.Designer.ViewModel
{
    public class DeleteItemEventArgs : EventArgs
    {
        public DeleteItemEventArgs(string targetProperty, object deletedItem)
        {
            TargetProperty = targetProperty;
            DeletedItem = deletedItem;
        }

        public string TargetProperty { get; }
        public object DeletedItem { get; }
    }
}
