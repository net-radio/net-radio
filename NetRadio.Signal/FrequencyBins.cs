using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NAudio.Dsp;

namespace NetRadio.Signal
{
    /// <summary>
    /// Software based FFT analyzer.
    /// </summary>
    public class FrequencyBins : IFrequencyBins
    {
        private readonly Dictionary<double, double> _bins = new Dictionary<double, double>();

        private readonly BinParameters _parameters;
        private readonly double _relativeFrequency;
        private readonly double _precision;
        private readonly bool _realMode;

        /// <summary>
        /// Gets down sampling level.
        /// </summary>
        public int DownSamplingLevel
        {
            get { return _parameters.BinsPerPoint; }
        }

        /// <summary>
        /// Gets Spectrum width.
        /// </summary>
        public int SpectralWidth
        {
            get { return _realMode ? (int) _parameters.SamplingRate/2 : (int) _parameters.SamplingRate; }
        }

        /// <summary>
        /// Gets minimum available intensity.
        /// </summary>
        public double MinimumIntensity
        {
            get { return _parameters.MinimumIntensityDb; }
        }

        /// <summary>
        /// Gets length of FFT
        /// </summary>
        public int FftLength
        {
            get { return _bins.Count*2; }
        }

        /// <summary>
        /// Gets number of FFT bins.
        /// </summary>
        public int BinsCount
        {
            get { return _bins.Count; }
        }

        /// <summary>
        /// Gets extracted frequencies.
        /// </summary>
        /// <returns></returns>
        public ICollection<double> Frequencies()
        {
            return _bins.Keys;
        }

        /// <summary>
        /// Gets intensity for specified frequency.
        /// </summary>
        /// <param name="frequency">Specified frequency.</param>
        /// <returns>Returns intensity.</returns>
        public double Intensity(double frequency)
        {
            return _bins.ContainsKey(frequency) ? _bins[frequency] : default(double);
        }

        /// <summary>
        /// Gets normalized intensity for specified frequency.
        /// </summary>
        /// <param name="frequency">Specified frequency.</param>
        /// <returns>Returns intensity.</returns>
        public double IntensityNormalized(double frequency)
        {
            return Intensity(frequency)/_parameters.MinimumIntensityDb;
        }

        /// <summary>
        /// Gets all extracted intensities.
        /// </summary>
        /// <returns>Returns a collection of extracted intensities.</returns>
        public ICollection<double> Intensities()
        {
            return _bins.Values;
        }

        /// <summary>
        /// Gets frequency of maximum intensity based on Hz.
        /// </summary>
        /// <returns>Returns extracted frequency.</returns>
        public double MaxIntensityAt()
        {
            var frequency = _bins.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            return frequency;
        }

        /// <summary>
        /// Gets maximum intensity based on dB.
        /// </summary>
        /// <returns>Returns intensity.</returns>
        public double MaxIntensity()
        {
            return _bins.Values.Max();
        }

        /// <summary>
        /// Gets minimum intensity based on dB.
        /// </summary>
        /// <returns>Returns intensity.</returns>
        public double MinIntensity()
        {
            return _bins.Values.Min();
        }

        /// <summary>
        /// Creates a FFT analyzer.
        /// </summary>
        /// <param name="fft">FFT raw data.</param>
        /// <param name="parameters">Specified bin parameters.</param>
        /// <param name="real"></param>
        public FrequencyBins(FftEventArgs args, BinParameters parameters,bool real=true)
        {
            _realMode = real;
            Complex[] fft = args.Result;
            _parameters = parameters;
            _relativeFrequency = _parameters.Relative ? real ? _parameters.SamplingRate / 4 : _parameters.SamplingRate / 2 : 0;
            _precision = _parameters.SamplingRate / (double)fft.Length;

            FillBins(fft,real);
        }

        public void Update(FftEventArgs args)
        {
            FillBins(args.Result);
        }

        private void FillBins(Complex[] fftResults, bool real = true)
        {
            var watch = new Stopwatch();
            watch.Start();

            var bias = _parameters.SamplingRate/2;

            if (!real)
            {
                FillBins(fftResults, fftResults.Length/2, fftResults.Length, -bias);
                FillBins(fftResults, 0, fftResults.Length/2, bias);
            }
            else
                FillBins(fftResults, 0, fftResults.Length / 2, 0);

            watch.Stop();
            var res = watch.ElapsedMilliseconds;
            Console.WriteLine("update bin:{0}", res);
        }


        private void FillBins(Complex[] fftResults, int start,int stop,long bias)
        {
            for (var n = start; n < stop; n += _parameters.BinsPerPoint)
            {
                // averaging out bins
                double yPos = 0;
                for (var b = 0; b < _parameters.BinsPerPoint; b++)
                    yPos += IntensityDb(fftResults[n + b]);

                // Update by Hossein 
                double frequency = n;
                // var frequency = n / (double)_parameters.BinsPerPoint;
                frequency *= _precision;
                frequency -= _relativeFrequency;
                frequency += _parameters.CarrierFrequency;
                frequency += bias;

                _bins[frequency] = yPos/_parameters.BinsPerPoint;
            }
        }

        private double IntensityDb(Complex c)
        {
            var intensityDb = 20*Math.Log10(Math.Sqrt(c.X*c.X + c.Y*c.Y));
            //intensityDb = intensityDb < _parameters.MinimumIntensityDb ? _parameters.MinimumIntensityDb : intensityDb;
            var percent = intensityDb / _parameters.MinimumIntensityDb;
            var calibratedDb = percent * _parameters.MinimumIntensityDb * _parameters.CalibrationAmplitude -
                               _parameters.MinimumIntensityDb * (_parameters.CalibrationAmplitude - 1.0);
            return calibratedDb+_parameters.CalibrationOffset;
        }
    }
}
