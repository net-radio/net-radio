using NetRadio.Devices.G3XDdc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NetRadio.G31Ddc.Converter
{
    [ValueConversion(typeof(Agc), typeof(bool), ParameterType = typeof(Agc))]
    class AgcToBooleanConverter : ConverterBase, IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var state = (bool)value;
            return state ? (Agc)parameter : default(Agc);
        }
    }
}
