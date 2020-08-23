using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;

namespace Agents.Net.Designer.View
{
    public abstract class BaseConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
