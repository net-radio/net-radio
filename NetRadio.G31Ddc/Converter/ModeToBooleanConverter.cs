using NetRadio.Devices.G3XDdc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NetRadio.G31Ddc.Converter
{
    [ValueConversion(typeof(DemodulatorMode), typeof(bool), ParameterType = typeof(DemodulatorMode))]
    public class ModeToBooleanConverter : ConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            var state = (bool)value;
            return state ? parameter : null;
        }
    }
}
