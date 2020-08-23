using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Agents.Net.Designer.View
{
    public static class ViewExtensions
    {
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
