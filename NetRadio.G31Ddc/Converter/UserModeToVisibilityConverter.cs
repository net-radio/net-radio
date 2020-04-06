using NetRadio.Devices.G3XDdc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NetRadio.G31Ddc.Converter
{
    [ValueConversion(typeof(DemodulatorMode), typeof(Visibility), ParameterType = typeof(DemodulatorMode))]
    class UserModeToVisibilityConverter : ConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            var state = (Visibility)value;
            return state.Equals(Visibility.Visible) ? parameter : null;
        }
    }
}
