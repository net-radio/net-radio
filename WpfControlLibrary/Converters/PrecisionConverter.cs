using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WpfControlLibrary.DataTypes.Enums;

namespace WpfControlLibrary.Converters
{
    [ValueConversion(typeof(sbyte), typeof(string), ParameterType = typeof(MetricPrefix))]
    class PrecisionConverter : ConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ChangeType(parameter + "Hz", targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ChangeType(parameter, targetType);
        }
    }
}
