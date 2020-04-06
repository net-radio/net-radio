using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Dsp;
using NetRadio.Signal.Utilities;

namespace NetRadio.Signal
{
    public class ApproximateFrequencyBins : FastFrequencyBins
    {
        public ApproximateFrequencyBins(FftEventArgs args, BinParameters parameters, bool real = true)
            : base(args, parameters, real)
        {
        }

        public ApproximateFrequencyBins(BinParameters parameters, int fftLength, bool real)
            : base(parameters, fftLength, real)
        {
        }

        protected sealed override void FillBins(Complex[] fftResults, int start, int stop, int indexBias)
        {
            var a = A;
            var b = B;

            for (var n = start; n < stop; n += Parameters.BinsPerPoint)
            {
                //var yPos = IntensityDb(fftResults[n]);
                var c = fftResults[n];
                var yPos = a * FastMath.Log10(FastMath.Sqrt(c.X * c.X + c.Y * c.Y)) - b;
                BinsIntensity[n + indexBias] = yPos;
            }
        }
    }
}
