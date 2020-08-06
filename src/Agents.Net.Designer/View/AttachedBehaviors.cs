using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Agents.Net.Designer.ViewModel;
using TreeViewItem = System.Windows.Controls.TreeViewItem;

namespace Agents.Net.Designer.View
{
    public static class AttachedBehaviors
    {
        public static readonly DependencyProperty UpdateTextOnDropDownClosedProperty = DependencyProperty.RegisterAttached(
            "UpdateTextOnDropDownClosed", typeof (bool), typeof (AttachedBehaviors), new PropertyMetadata(default(bool), OnUpdateTextOnDropDownClosedPropertyChanged));

        public static void SetUpdateTextOnDropDownClosed(DependencyObject element, bool value)
        {
            element.SetValue(UpdateTextOnDropDownClosedProperty, value);
        }

        public static bool GetUpdateTextOnDropDownClosed(DependencyObject element)
        {
            return (bool) element.GetValue(UpdateTextOnDropDownClosedProperty);
        }

        private static void OnUpdateTextOnDropDownClosedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ComboBox comboBox))
            {
                return;
            }

            if (!((bool) e.OldValue) &&
                ((bool) e.NewValue))
            {
                comboBox.Unloaded += ComboBoxOnUnloaded;
                comboBox.DropDownClosed += ComboBoxOnDropDownClosed;
            }
        }

        private static void ComboBoxOnDropDownClosed(object sender, EventArgs e)
        {
            if (!(sender is ComboBox comboBox) ||
                comboBox.SelectedItem == null ||
                string.IsNullOrEmpty(comboBox.Text))
            {
                return;
            }
            
            comboBox.GetBindingExpression(ComboBox.TextProperty)?.UpdateSource();
        }

        private static void ComboBoxOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!(sender is ComboBox comboBox))
            {
                return;
            }
            
            comboBox.Unloaded -= ComboBoxOnUnloaded;
            comboBox.DropDownClosed -= ComboBoxOnDropDownClosed;
        }

        public static readonly DependencyProperty IsBringSelectedIntoViewProperty = DependencyProperty.RegisterAttached(
            "IsBringSelectedIntoView", typeof (bool), typeof (AttachedBehaviors), new PropertyMetadata(default(bool), PropertyChangedCallback));

        public static void SetIsBringSelectedIntoView(DependencyObject element, bool value)
        {
            element.SetValue(IsBringSelectedIntoViewProperty, value);
        }

        public static bool GetIsBringSelectedIntoView(DependencyObject element)
        {
            return (bool) element.GetValue(IsBringSelectedIntoViewProperty);
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var treeViewItem = dependencyObject as TreeViewItem;
            if (treeViewItem == null)
            {
                return;
            }

            if (!((bool) dependencyPropertyChangedEventArgs.OldValue) &&
                ((bool) dependencyPropertyChangedEventArgs.NewValue))
            {
                treeViewItem.Unloaded += TreeViewItemOnUnloaded;
                treeViewItem.Selected += TreeViewItemOnSelected;
            }
        }

        private static void TreeViewItemOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var treeViewItem = sender as TreeViewItem;
            if (treeViewItem == null)
            {
                return;
            }

            treeViewItem.Unloaded -= TreeViewItemOnUnloaded;
            treeViewItem.Selected -= TreeViewItemOnSelected;
        }

        private static void TreeViewItemOnSelected(object sender, RoutedEventArgs routedEventArgs)
        {
            var treeViewItem = sender as TreeViewItem;
            treeViewItem?.BringIntoView();
        }
        
        public static readonly DependencyProperty UpdatePropertySourceWhenEnterPressedProperty = DependencyProperty.RegisterAttached(
            "UpdatePropertySourceWhenEnterPressed", typeof(DependencyProperty), typeof(AttachedBehaviors), new PropertyMetadata(null, OnUpdatePropertySourceWhenEnterPressedPropertyChanged));

        static AttachedBehaviors()
        {

        }

        public static void SetUpdatePropertySourceWhenEnterPressed(DependencyObject dp, DependencyProperty value)
        {
            dp.SetValue(UpdatePropertySourceWhenEnterPressedProperty, value);
        }

        public static DependencyProperty GetUpdatePropertySourceWhenEnterPressed(DependencyObject dp)
        {
            return (DependencyProperty)dp.GetValue(UpdatePropertySourceWhenEnterPressedProperty);
        }

        private static void OnUpdatePropertySourceWhenEnterPressedPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            UIElement element = dp as UIElement;

            if (element == null)
            {
                return;
            }

            if (e.OldValue != null)
            {
                element.PreviewKeyDown -= HandlePreviewKeyDown;
            }

            if (e.NewValue != null)
            {
                element.PreviewKeyDown += new KeyEventHandler(HandlePreviewKeyDown);
            }
        }

        static void HandlePreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DoUpdateSource(e.Source);
            }
        }

        static void DoUpdateSource(object source)
        {
            DependencyProperty property =
                GetUpdatePropertySourceWhenEnterPressed(source as DependencyObject);

            if (property == null)
            {
                return;
            }

            UIElement elt = source as UIElement;

            if (elt == null)
            {
                return;
            }

            BindingExpression binding = BindingOperations.GetBindingExpression(elt, property);

            if (binding != null)
            {
                binding.UpdateSource();
            }
        }
    }
}