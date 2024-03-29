#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;

namespace Agents.Net.Designer.WpfView
{
    public class EnumCollectionExtension : MarkupExtension
    {
        public Type EnumType { get; set; }
 
        public override object ProvideValue(IServiceProvider _)
        {
            if(EnumType != null)
            {
                return CreateEnumValueList(EnumType);
            }
            return default(object);
        }
 
        private List<object> CreateEnumValueList(Type enumType)
        {
            return Enum.GetNames(enumType)
                       .Select(name => Enum.Parse(enumType, name))
                       .ToList();
        }
    }
}