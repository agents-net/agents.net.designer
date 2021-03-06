#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Windows.Controls;
using System.Windows.Data;
using Agents.Net.Designer.ViewModel;

namespace Agents.Net.Designer.WpfView
{
    [ValueConversion(typeof(Button), typeof(DeleteItemEventArgs))]
    public class ButtonToDeleteItemEventArgsConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            Button button = (Button) value;

            ItemsControl ancestor = button.FindAncestorOrSelf<ItemsControl>();
            BindingExpression binding = ancestor?.GetBindingExpression(ItemsControl.ItemsSourceProperty);
            if (button?.DataContext == null ||
                binding == null)
            {
                return null;
            }

            DeleteItemEventArgs eventArgs = new(
                binding.ResolvedSourcePropertyName,
                button.DataContext);
            return eventArgs;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
