using System;

namespace NetRadio.Signal
{
    /// <summary>
    /// Provides Rf domain specific facilities.
    /// </summary>
    public static class RfMath
    {
        /// <summary>
        /// Represetns S-Meter units
        /// </summary>
        public enum SUnits
        {
            /// <summary>
            /// Unit S1
            /// </summary>
            S1,
            /// <summary>
            /// Unit S2
            /// </summary>
            S2,
            /// <summary>
            /// Unit S3
            /// </summary>
            S3,
            /// <summary>
            /// Unit S4
            /// </summary>
            S4,
            /// <summary>
            /// Unit S5
            /// </summary>
            S5,
            /// <summary>
            /// Unit S6
            /// </summary>
            S6,
            /// <summary>
            /// Unit S7
            /// </summary>
            S7,
            /// <summary>
            /// Unit S8
            /// </summary>
            S8,
            /// <summary>
            /// Unit S9
            /// </summary>
            S9,
            /// <summary>
            /// Unit S9+10dB
            /// </summary>
            S9Plus10dB,
            /// <summary>
            /// Out of range state
            /// </summary>
            Invalid
        };

        /// <summary>
        /// Converts micro volts to decibel milli watts.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns calculated value.</returns>
        public static double MicroVoltsToDbm(double value)
        {
            var b = (Math.Pow((value/1000000), 2)/50);
            var c = 10*((Math.Log(b/0.001))/Math.Log(10)); // Divide by Math.log(10) to give the equivalent of log10(x).
            c = Math.Round(c*100)/100;
            return c;
        }

        /// <summary>
        /// Converts decibel milli watts to micro volts.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns calculated value.</returns>
        public static double DbmToMicroVolts(double value)
        {
            var e = 0.001*Math.Pow(10, (value/10));
            var f = Math.Sqrt(e*50)*1000000;
            f = Math.Round(f*100)/100;
            return f;
        }

        /// <summary>
        /// Converts watts to decibel milli watts.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns calculated value.</returns>
        public static double WattsToDbm(double value)
        {
            var h = 10*(Math.Log(value/0.001)/Math.Log(10));
                // Divide by Math.log(10) to give the equivalent of log10(x).
            h = Math.Round(h*100)/100;
            return h;
        }

        /// <summary>
        /// Converts decibel milli watts to watts.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns calculated value.</returns>
        public static double DbmToWatts(double value)
        {
            var j = 0.001*Math.Pow(10, (value/10));
            j = Math.Round(j*100)/100;
            return j;
        }

        /// <summary>
        /// Converts decibel milli watts to micro watts.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns calculated value.</returns>
        public static double DbmToMicroWatts(double value)
        {
            var j = 0.001*Math.Pow(10, (value/10));
            j = Math.Round(j*100*1000000)/100;
            return j;
        }

        /// <summary>
        /// Converts micro volts to S-meter units.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns calculated value.</returns>
        public static SUnits MicroVoltsToSUnit(double value)
        {
            if (value >= 160)
                return SUnits.S9Plus10dB;
            if (value >= 50.2)
                return SUnits.S9;
            if (value >= 25.1)
                return SUnits.S8;
            if (value >= 12.6)
                return SUnits.S7;
            if (value >= 6.3)
                return SUnits.S6;
            if (value >= 3.2)
                return SUnits.S5;
            if (value >= 1.6)
                return SUnits.S4;
            if (value >= 0.8)
                return SUnits.S3;
            if (value >= 0.4)
                return SUnits.S2;
            if (value >= 0.2)
                return SUnits.S1;

            return SUnits.Invalid;
        }

        /// <summary>
        /// Converts decibel milli watts S-meter units.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns calculated value.</returns>
        public static SUnits DbmToSUnit(double value)
        {
            if (value >= -63)
                return SUnits.S9Plus10dB;
            if (value >= -73)
                return SUnits.S9;
            if (value >= -79)
                return SUnits.S8;
            if (value >= -85)
                return SUnits.S7;
            if (value >= -91)
                return SUnits.S6;
            if (value >= -97)
                return SUnits.S5;
            if (value >= -103)
                return SUnits.S4;
            if (value >= -109)
                return SUnits.S3;
            if (value >= -115)
                return SUnits.S2;
            if (value >= -121)
                return SUnits.S1;

            return SUnits.Invalid;
        }

        /// <summary>
        /// Converts micro volts RMS to Peak.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns calculated value.</returns>
        public static double MicroVoltsRmsToPeak(double value)
        {
            return (4 * value) / Math.Sqrt(2);
        }

        /// <summary>
        /// Converts micro volts Peak to RMS.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns calculated value.</returns>
        public static double MicroVoltsPeakToRms(double value)
        {
            return (Math.Sqrt(2) / 2) * (value / 2);
        }

        //public static double MicroVoltsToDbm(double value)
        //{
        //    return 20 * (Log10(value / (Math.Sqrt(0.008 * 50 /*50 Ohm impedance*/))));
        //}

        //public static double DbmToMicroVolts(double value)
        //{
        //    return Math.Sqrt(0.008 * 50 /*50 Ohm impedance*/) * Math.Pow(10, (value / 20));
        //}

        /// <summary>
        /// Converts micro volts RMS to decibel milli watts.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns calculated value.</returns>
        public static double MicroVoltsRmsToDbm(double value)
        {
            var peak = MicroVoltsRmsToPeak(value);
            return MicroVoltsToDbm(peak);
        }

        //private static double Log10(double value)
        //{
        //    return Math.Log(value)/Math.Log(10);
        //}
    }
}
