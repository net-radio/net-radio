namespace NetRadio.Devices.G313.Signal
{
    /// <summary>
    /// Represents Sweep parameters
    /// </summary>
    public class SweepParameters
    {
        /// <summary>
        /// Gets or Sets first frequencty.
        /// </summary>
        public uint From { get; set; }

        /// <summary>
        /// Gets or Sets last frequency.
        /// </summary>
        public uint To { get; set; }

        /// <summary>
        /// Gets or Sets sweepe precision.
        /// </summary>
        public double Precision { get; set; }

        /// <summary>
        /// Gets or Sets whether radio should automatically get prepared for sweep or not.
        /// </summary>
        public bool AutoConfigure { get; set; }

        /// <summary>
        /// Gets or Sets If Gain for sweep (applied when AutoConfigure is active).
        /// </summary>
        public int IfGain { get; set; }

        /// <summary>
        /// Gets or Sets sweep half band.
        /// </summary>
        public int SweepSpan { get; set; }

        /// <summary>
        /// Gets or Sets number of fft values which should get discarded in calculations.
        /// </summary>
        public int FftWarmup { get; set; }

        /// <summary>
        /// Sets first frequency.
        /// </summary>
        /// <param name="value">First frequency value.</param>
        /// <returns>Returns <see cref="SweepParameters"/> sweep parameter</returns>
        public SweepParameters SetFrom(uint value)
        {
            From = value;
            return this;
        }

        /// <summary>
        /// Sets Last frequency.
        /// </summary>
        /// <param name="value">Last frequency value.</param>
        /// <returns>Returns <see cref="SweepParameters"/> sweep parameter</returns>
        public SweepParameters SetTo(uint value)
        {
            To = value;
            return this;
        }

        /// <summary>
        /// Sets sweep precision.
        /// </summary>
        /// <param name="value">Presicion value.</param>
        /// <returns>Returns <see cref="SweepParameters"/> sweep parameter</returns>
        public SweepParameters SetPrecision(double value)
        {
            Precision = value;
            return this;
        }

        /// <summary>
        /// Sets IF gain.
        /// </summary>
        /// <param name="value">IF gain value.</param>
        /// <returns>Returns <see cref="SweepParameters"/> sweep parameter</returns>
        public SweepParameters SetIfGain(int value)
        {
            IfGain = value;
            return this;
        }

        /// <summary>
        /// Creates sweep parameters
        /// </summary>
        public SweepParameters()
        {
            FftWarmup = 5;
            SweepSpan = 10000;
            Precision = 31.25;
        }
    }
}
