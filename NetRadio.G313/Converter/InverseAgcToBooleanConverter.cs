using System;
using System.Globalization;

namespace NetRadio.G313.Converter
{
    class InverseAgcToBooleanConverter:AgcToBooleanConverter
    {
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ! (bool) base.ConvertBack(value, targetType, parameter, culture);
        }
    }
}
