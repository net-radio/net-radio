namespace NetRadio.Signal
{
    /// <summary>
    /// Software based FFT analyzer parameters.
    /// </summary>
    public class BinParameters
    {
        /// <summary>
        /// Gets or Sets sampling rate.
        /// </summary>
        public uint SamplingRate { get; set; }

        /// <summary>
        /// Gets or Sets carrier frequency.
        /// </summary>
        public uint CarrierFrequency { get; set; }

        /// <summary>
        /// Gets or Sets minimum intensity based on dB.
        /// </summary>
        public double MinimumIntensityDb { get; set; }

        /// <summary>
        /// Gets or Sets FFT calibration amplitude factor.
        /// </summary>
        public double CalibrationAmplitude { get; set; }

        /// <summary>
        /// Gets or Sets wheather results should be in absolute domain or relative domain.
        /// </summary>
        public bool Relative { get; set; }

        /// <summary>
        /// Gets or Sets down-sampling factor.
        /// </summary>
        public int BinsPerPoint { get; set; }

        /// <summary>
        /// Gets or Sets FFT calibration amplitude offset.
        /// </summary>
        public int CalibrationOffset { get; set; }

        /// <summary>
        /// Sets FFT calibration amplitude offset.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns <see cref="BinParameters"/> parameters.</returns>
        public BinParameters SetCalibrationOffset(int value)
        {
            CalibrationOffset = value;
            return this;
        }

        /// <summary>
        /// Sets sampling rate.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns <see cref="BinParameters"/> parameters.</returns>
        public BinParameters SetSamplingRate(uint value)
        {
            SamplingRate = value;
            return this;
        }

        /// <summary>
        /// Sets carrier frequency.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns <see cref="BinParameters"/> parameters.</returns>
        public BinParameters SetCarrierFrequency(uint value)
        {
            CarrierFrequency = value;
            return this;
        }

        /// <summary>
        /// Sets minimum intensity based on dB.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns <see cref="BinParameters"/> parameters.</returns>
        public BinParameters SetMinimumIntensityDb(double value)
        {
            MinimumIntensityDb = value;
            return this;
        }

        /// <summary>
        /// Sets FFT calibration amplitude factor.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns <see cref="BinParameters"/> parameters.</returns>
        public BinParameters SetCalibrationAmplitude(double value)
        {
            CalibrationAmplitude = value;
            return this;
        }


        /// <summary>
        /// Sets wheather results should be in absolute domain or relative domain.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns <see cref="BinParameters"/> parameters.</returns>
        public BinParameters SetRelative(bool value)
        {
            Relative = value;
            return this;
        }

        /// <summary>
        /// Sets down-sampling factor.
        /// </summary>
        /// <param name="value">Specified value.</param>
        /// <returns>Returns <see cref="BinParameters"/> parameters.</returns>
        public BinParameters SetBinsPerPoint(int value)
        {
            BinsPerPoint = value;
            return this;
        }

        /// <summary>
        /// Creates bin parameters with default values.
        /// </summary>
        public BinParameters()
        {
            MinimumIntensityDb = -100;
            CalibrationAmplitude = 1.0;
            Relative = false;
            BinsPerPoint = 1;
        }
    }
}
