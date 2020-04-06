using System;
using System.Windows.Data;
using NetRadio.Devices.G313;

namespace NetRadio.G313.Converter
{
    [ValueConversion(typeof (G313Demodulator.DemodulatorMode), typeof (bool),
        ParameterType = typeof (G313Demodulator.DemodulatorMode))]
    internal class ModeToBooleanConverter : ConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            var state = (bool) value;
            return state ?  parameter : null;
                //default(G313Demodulator.DemodulatorMode);
        }
    }
}
