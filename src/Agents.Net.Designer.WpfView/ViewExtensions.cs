#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Agents.Net.Designer.WpfView
{
    public static class ViewExtensions
    {
        /// <summary>
        /// We extend ItemContainerGenerator rather than TreeView itself because there 
        /// are other hierarchical ItemsControls, like MenuItem. 
        /// </summary>
        /// <typeparam name="TItem">The type of the item displayed in the treeview 
        /// (or whatever). If your items are of multiple types, use a common base class 
        /// and/or write a Byzantine itemParentSelector</typeparam>
        /// <param name="rootContainerGenerator"></param>
        /// <param name="item">Item to find the container for</param>
        /// <param name="itemParentSelector">Lambda to return the parent property 
        /// value from a TItem</param>
        /// <returns></returns>
        public static ItemsControl ContainerFromItem<TItem>(
            this ItemContainerGenerator rootContainerGenerator,
            TItem item,
            Func<TItem, TItem> itemParentSelector)
        {
            //  Caller can pass in treeView.SelectedItem as TItem in cases where 
            //  treeView.SelectedItem is null. Seems to me correct behavior there is 
            //  "No container". 
            if (item == null)
            {
                return null;
            }

            if (itemParentSelector == null)
            {
                throw new ArgumentNullException(nameof(itemParentSelector));
            }

            var parentItem = itemParentSelector(item);

            //  When we run out of parents, we're a root level node so we query the 
            //  rootContainerGenerator itself for the top level child container, and 
            //  start unwinding back to the item the caller gave us. 
            if (parentItem == null)
            {
                //  Our item is the parent of our caller's item. 
                //  This is the parent of our caller's container. 
                return rootContainerGenerator.ContainerFromItem(item) as ItemsControl;
            }
            else
            {
                //  This gets parents by unwinding the stack back down from root
                var parentContainer =
                    ContainerFromItem<TItem>(rootContainerGenerator,
                                             parentItem, itemParentSelector);

                return parentContainer.ItemContainerGenerator.ContainerFromItem(item)
                           as ItemsControl;
            }
        }
        
        public static T FindAncestorOrSelf<T>(this DependencyObject obj)
            where T : DependencyObject
        {
            while (obj != null)
            {
                if (obj is T objTest)
                    return objTest;
                obj = GetParent(obj);
            }

            return null;
        }

        public static DependencyObject GetParent(this DependencyObject obj)
        {
            switch (obj)
            {
                case null:
                    return null;
                case ContentElement ce:
                {
                    DependencyObject parent = ContentOperations.GetParent(ce);
                    if (parent != null)
                        return parent;
                    return ce is FrameworkContentElement fce ? fce.Parent : null;
                }
                default:
                    return VisualTreeHelper.GetParent(obj);
            }
        }
    }
}
