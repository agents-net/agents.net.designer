using System;
using System.Windows.Markup;

namespace Agents.Net.Designer.WpfView
{
    public abstract class BaseConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
