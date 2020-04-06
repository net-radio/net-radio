using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WpfControlLibrary.DataTypes.Enums;

namespace WpfControlLibrary.Converters
{
    [ValueConversion(typeof(uint), typeof(double), ParameterType = typeof(MetricPrefix))]
    class FrequencyConverter : ConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Debug.WriteLine(value.GetType().Name);

            double result = (uint)value / 1e3;
            return System.Convert.ChangeType(result, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Debug.WriteLine(value.GetType().Name);
            object val = null;

            try
            {
                var f = 1e3 * double.Parse(value.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                val = System.Convert.ChangeType(f, targetType);
            }
            catch (OverflowException ex)
            {
                var type = targetType;
                FieldInfo fi = type.GetField("MaxValue");
                if (fi != null && fi.IsLiteral && !fi.IsInitOnly)
                {
                    val = fi.GetRawConstantValue();
                }
            }
            catch (FormatException ex)
            {
                // ignore
            }

            return val;
        }
    }
}
