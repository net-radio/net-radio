using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NetRadio.G31Ddc.Converter
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    class BooleanToVisibilityConverter : ConverterBase, IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var state = (Visibility)value;
            return state.Equals(Visibility.Visible);
        }       
    }
}
