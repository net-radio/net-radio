using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.Audio.Converter
{
    class InverseAgcToBooleanConverter : AgcToBooleanConverter
    {
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)base.ConvertBack(value, targetType, parameter, culture);
        }
    }
}
