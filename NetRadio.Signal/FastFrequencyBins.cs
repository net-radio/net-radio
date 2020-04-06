using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NAudio.Dsp;
using NetRadio.Signal.Utilities;

namespace NetRadio.Signal
{
    /// <summary>
    /// 
    /// </summary>
    public class FastFrequencyBins:IFrequencyBins
    {
        protected readonly BinParameters Parameters;
        protected readonly double RelativeFrequency;
        protected readonly double Precision;
        protected readonly bool RealMode;
        protected readonly double[] BinsFrequency;
        protected readonly double[] BinsIntensity;

        protected readonly double A;
        protected readonly double B;

        /// <summary>
        /// Gets Spectrum width.
        /// </summary>
        public int SpectralWidth
        {
            get { return RealMode ? (int)Parameters.SamplingRate / 2 : (int)Parameters.SamplingRate; }
        }

        /// <summary>
        /// Gets minimum available intensity.
        /// </summary>
        public double MinimumIntensity
        {
            get { return Parameters.MinimumIntensityDb; }
        }

        /// <summary>
        /// Gets length of FFT
        /// </summary>
        public int FftLength
        {
            get { return BinsFrequency.Length*2; }
        }

        /// <summary>
        /// Gets number of FFT bins.
        /// </summary>
        public int BinsCount
        {
            get { return BinsFrequency.Length; }
        }

        /// <summary>
        /// Gets extracted frequencies.
        /// </summary>
        /// <returns></returns>
        public ICollection<double> Frequencies()
        {
            return BinsFrequency;
        }

        /// <summary>
        /// Gets intensity for specified frequency.
        /// </summary>
        /// <param name="frequency">Specified frequency.</param>
        /// <returns>Returns intensity.</returns>
        public double Intensity(double frequency)
        {
            for (var i = 0; i < BinsFrequency.Length; i++)
                if (BinsFrequency[i].Equals(frequency))
                    return BinsIntensity[i];

            return default(double);
        }

        /// <summary>
        /// Gets normalized intensity for specified frequency.
        /// </summary>
        /// <param name="frequency">Specified frequency.</param>
        /// <returns>Returns intensity.</returns>
        public double IntensityNormalized(double frequency)
        {
            return Intensity(frequency) / Parameters.MinimumIntensityDb;
        }

        /// <summary>
        /// Gets all extracted intensities.
        /// </summary>
        /// <returns>Returns a collection of extracted intensities.</returns>
        public ICollection<double> Intensities()
        {
            return BinsIntensity;
        }

        /// <summary>
        /// Gets maximum intensity based on dB.
        /// </summary>
        /// <returns>Returns intensity.</returns>
        public double MaxIntensity()
        {
            return BinsIntensity.Max();
            // return Intensities().ToList<double>().Where((x, i) => i % Parameters.BinsPerPoint == 0).Max;
        }

        /// <summary>
        /// Gets minimum intensity based on dB.
        /// </summary>
        /// <returns>Returns intensity.</returns>
        public double MinIntensity()
        {
            return BinsIntensity.Min();
        }

        /// <summary>
        /// Creates a FFT analyzer.
        /// </summary>
        /// <param name="fft">FFT raw data.</param>
        /// <param name="parameters">Specified bin parameters.</param>
        /// <param name="real"></param>
        public FastFrequencyBins(FftEventArgs args, BinParameters parameters, bool real = true)
            : this(parameters, args.Result.Length, real)
        {
            Update(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="fftLength"></param>
        /// <param name="real"></param>
        public FastFrequencyBins(BinParameters parameters, int fftLength,bool real)
        {
            RealMode = real;
            Parameters = parameters;

            A = Parameters.CalibrationAmplitude * 20;
            B = (Parameters.MinimumIntensityDb * (Parameters.CalibrationAmplitude - 1.0)) +
                 Parameters.CalibrationOffset;

            RelativeFrequency = Parameters.Relative ? real ? Parameters.SamplingRate/4 : Parameters.SamplingRate/2 : 0;
            Precision = Parameters.SamplingRate / (double)fftLength;

            var bufferLength = real ? fftLength/2 : fftLength;
            BinsFrequency = new double[bufferLength];
            BinsIntensity = new double[bufferLength];

            FillFrequencies();
        }

        public virtual void Update(FftEventArgs args)
        {
            FillBins(args.Result);
        }

        protected void FillFrequencies()
        {
            for (var n = 0; n < BinsFrequency.Length; n += Parameters.BinsPerPoint)
            {
                var frequency = n / (double)Parameters.BinsPerPoint;
                frequency *= Precision;
                frequency -= RelativeFrequency;
                frequency += Parameters.CarrierFrequency;
                BinsFrequency[n] = frequency;
            }
        }

        private void FillBins(Complex[] fftResults)
        {
            // FFT Shift
            var watch = new Stopwatch();
            watch.Start();

            if (!RealMode)
            {
                var bias = fftResults.Length / 2;
                FillBins(fftResults, bias, fftResults.Length, -bias);
                FillBins(fftResults, 0, bias, bias);
            }
            else
            {
                // fftResults.Reverse();
                FillBins(fftResults, 0, fftResults.Length / 2, 0);
                // var bias = fftResults.Length / 2;
                // FillBins(fftResults, bias, fftResults.Length, -bias);
            }

            watch.Stop();
            var res = watch.ElapsedMilliseconds;
            //Console.WriteLine("update bin:{0}",res);
        }

        protected virtual void FillBins(Complex[] fftResults,int start,int stop,int indexBias)
        {
            var a = A;
            var b = B;

            for (var n = start; n < stop; n += Parameters.BinsPerPoint)
            {
                //var yPos = IntensityDb(fftResults[n]);
                var c = fftResults[n];
                var yPos = a*Math.Log10(FastMath.Sqrt(c.X*c.X + c.Y*c.Y)) - b;
                BinsIntensity[n+indexBias] = yPos;
            }
        }

        private double IntensityDb(Complex c)
        {
            var intensityDb = 20*Math.Log10(FastMath.Sqrt(c.X*c.X + c.Y*c.Y));
            var percent = intensityDb / Parameters.MinimumIntensityDb;
            var calibratedDb = percent * Parameters.MinimumIntensityDb * Parameters.CalibrationAmplitude -
                               Parameters.MinimumIntensityDb * (Parameters.CalibrationAmplitude - 1.0);
            return calibratedDb+Parameters.CalibrationOffset;
        }

        private double IntensityDb2(Complex c)
        {
            return A * Math.Log10(FastMath.Sqrt(c.X * c.X + c.Y * c.Y)) - B;
            //return (20 * Math.Log10(FastMath.SqrtApproximate.Sqrt(c.X * c.X + c.Y * c.Y))  * _parameters.CalibrationAmplitude) -(_parameters.MinimumIntensityDb * (_parameters.CalibrationAmplitude - 1.0)) + _parameters.CalibrationOffset;
        }

        public double MaxFrequency()
        {
            var max = double.MinValue;
            var frequency = -1.0;

            for(var i=0;i<BinsIntensity.Length;i++)
                if (BinsIntensity[i] > max)
                {
                    max = BinsIntensity[i];
                    frequency = BinsFrequency[i];
                }

            return frequency;
        }

        public int MaxIntensityAt()
        {
            var max = double.MinValue;
            int index = -1;

            for (var i = 0; i < BinsIntensity.Length; i++ /*+= Parameters.BinsPerPoint*/)
                if (BinsIntensity[i] > max)
                {
                    max = BinsIntensity[i];
                    index = i;
                }

            return index;
        }

        public double IntensityAt(int index)
        {
            return BinsIntensity[index];
        }

        public double FrequencyAt(int index)
        {
            return BinsFrequency[index];
        }

        public double MinFrequency()
        {
            var min = double.MaxValue;
            var frequency = -1.0;

            for (var i = 0; i < BinsIntensity.Length; i++)
                if (BinsIntensity[i] < min)
                {
                    min = BinsIntensity[i];
                    frequency = BinsFrequency[i];
                }

            return frequency;
        }
    }
}
