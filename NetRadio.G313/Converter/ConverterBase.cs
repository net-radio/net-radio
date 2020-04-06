using System;
using System.Windows.Markup;

namespace NetRadio.G313.Converter
{
    class ConverterBase:MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
