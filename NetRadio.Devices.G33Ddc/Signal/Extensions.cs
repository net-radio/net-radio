using NetRadio.Signal;
using System;

namespace NetRadio.Devices.G3XDdc.Signal
{
    /// <summary>
    /// Primitive facilities for FFT initialization.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets BinPrameters with high resolution FFT settings.
        /// </summary>
        /// <param name="radio">Radio context.</param>
        /// <returns>Returns <see cref="BinParameters"/> specified parameters.</returns>
        public static BinParameters BinParametersDetailed(this G33DdcRadio radio, uint channel)
        {
            uint index;
            DdcInfo info = radio.Ddc2Info(out index);

            var parameters = new BinParameters
            {
                BinsPerPoint = 1,
                CalibrationAmplitude = 1.0,//1.0 in sweeper is better
                CarrierFrequency = radio.AbsoluteFrequency(channel),
                MinimumIntensityDb = -100,
                Relative = true,
                SamplingRate = info.SampleRate // new Ddc2(radio, channel).DdcArgs().Info.SampleRate
            };

            return parameters;
        }

        /// <summary>
        /// Gets usable samples based provided FFT length and required points.
        /// </summary>
        /// <param name="radio">Radio context.</param>
        /// <param name="fftlength">Length of FFT.</param>
        /// <param name="points">Required points.</param>
        /// <returns>Returns usable points.</returns>
        public static int UsableDownSamples(this G33DdcRadio radio, int fftlength, int points)
        {
            var binsPerPoints = (int)Math.Ceiling(fftlength / (float)points);
            var resolution = (fftlength / binsPerPoints) / 2;
            return resolution;
        }


    }
}
