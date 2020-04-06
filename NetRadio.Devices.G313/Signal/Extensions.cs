using System;
using NetRadio.Signal;

namespace NetRadio.Devices.G313.Signal
{
    /// <summary>
    /// Primitive facilities for FFT initialization.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets BinPrameters with default values.
        /// </summary>
        /// <param name="radio">Radio context.</param>
        /// <returns>Returns <see cref="BinParameters"/> specified parameters.</returns>
        public static BinParameters BinParametersDefault(this G313Radio radio)
        {
            var parameters = new BinParameters
            {
                BinsPerPoint = 4,
                CalibrationAmplitude =1.1, //1.5,//1.0 in sweeper is better
                CalibrationOffset=20,//8,
                CarrierFrequency = radio.Frequency(),
                MinimumIntensityDb = -100,
                Relative = true,
                SamplingRate = radio.Demodulator().IfSamplingRate()
            };

            return parameters;
        }

        /// <summary>
        /// Gets BinPrameters with high resolution FFT settings.
        /// </summary>
        /// <param name="radio">Radio context.</param>
        /// <returns>Returns <see cref="BinParameters"/> specified parameters.</returns>
        public static BinParameters BinParametersDetailed(this G313Radio radio)
        {
            var parameters = new BinParameters
            {
                BinsPerPoint = 1,
                CalibrationAmplitude = 1.0,//1.0 in sweeper is better
                CarrierFrequency = radio.Frequency(),
                MinimumIntensityDb = -100,
                Relative = true,
                SamplingRate = radio.Demodulator().IfSamplingRate()
            };

            return parameters;
        }

        /// <summary>
        /// Gets BinPrameters with configurable resolution settings.
        /// </summary>
        /// <param name="radio">Radio context.</param>
        /// <param name="fftlength">Length of FFT</param>
        /// <param name="points">Number of Required points.</param>
        /// <returns>Returns <see cref="BinParameters"/> specified parameters.</returns>
        public static BinParameters BinParametersVideoFilter(this G313Radio radio, int fftlength, int points)
        {
            var binsPerPoints = (int) Math.Ceiling(fftlength/(float) points);
            return radio.BinParametersDefault().SetBinsPerPoint(binsPerPoints);
        }

        /// <summary>
        /// Gets usable samples based provided FFT length and required points.
        /// </summary>
        /// <param name="radio">Radio context.</param>
        /// <param name="fftlength">Length of FFT.</param>
        /// <param name="points">Required points.</param>
        /// <returns>Returns usable points.</returns>
        public static int UsableDownSamples(this G313Radio radio, int fftlength, int points)
        {
            var binsPerPoints = (int)Math.Ceiling(fftlength / (float)points);
            var res=(fftlength/binsPerPoints)/2;
            return res;
        }

        /// <summary>
        /// Gets BinPrameters suitable for wide spectrum analysis.
        /// </summary>
        /// <param name="radio">Radio context.</param>
        /// <returns>Returns <see cref="BinParameters"/> specified parameters.</returns>
        public static BinParameters BinParametersWideSpectrum(this G313Radio radio)
        {
            return radio.BinParametersDetailed().SetCalibrationAmplitude(1.0);//.SetCalibrationOffset(10);
        }

        /// <summary>
        /// Gets default sweep parameters.
        /// </summary>
        /// <param name="radio">Radio context.</param>
        /// <param name="from">Frist frequency.</param>
        /// <param name="to">Last frequency.</param>
        /// <returns>Returns <see cref="BinParameters"/> specified parameters.</returns>
        public static SweepParameters SweepParametersDefault(this G313Radio radio, uint from, uint to)
        {
            var parameters = new SweepParameters
            {
                AutoConfigure = true,
                From = from,
                IfGain = 50,
                Precision =31.25,//15.7, //2.93,
                SweepSpan = 10000,
                To = to
            };

            return parameters;
        }

        /// <summary>
        /// provides software baes Sweeper unit for WinRadio G313 devices.
        /// </summary>
        /// <param name="radio">Radio context.</param>
        /// <param name="parameters">Specified Sweep parameters.</param>
        /// <returns>Returns <see cref="G313Sweeper"/> sweeper context.</returns>
        public static G313Sweeper Sweeper(this G313Radio radio, SweepParameters parameters)
        {
            return new G313Sweeper(radio, parameters);
        }

        /// <summary>
        /// provides software baes Sweeper unit for WinRadio G313 devices.
        /// </summary>
        /// <param name="radio">Radio context.</param>
        /// <param name="from">First frequency.</param>
        /// <param name="to">Last frequency.</param>
        /// <returns>Returns <see cref="G313Sweeper"/> sweeper context.</returns>
        public static G313Sweeper Sweeper(this G313Radio radio,uint from,uint to)
        {
            return new G313Sweeper(radio, radio.SweepParametersDefault(from, to));
        }
    }
}
