using System;
using System.Windows.Data;
using NetRadio.Devices.G313;

namespace NetRadio.G313.Converter
{
    [ValueConversion(typeof(Agc),typeof(bool), ParameterType=typeof(Agc))]
    class AgcToBooleanConverter:ConverterBase,IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var state = (bool) value;
            return state ? (Agc) parameter : default(Agc);
        }
    }
}
