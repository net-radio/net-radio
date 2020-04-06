namespace NetRadio.Devices.G313.Signal
{
    /// <summary>
    /// Represents Sweeped frequency band.
    /// </summary>
    public class SweepedFrequency
    {
        /// <summary>
        /// Gets Sweeped frequency.
        /// </summary>
        public double Frequency { get;private set; }

        /// <summary>
        /// Gets minimum intensity during sweep operation.
        /// </summary>
        public double Min { get; private set; }

        /// <summary>
        /// Gets maximum instensity duting sweep operation.
        /// </summary>
        public double Max { get; private set; }

        /// <summary>
        /// Gets current intensity.
        /// </summary>
        public double Current { get; private set; }

        internal SweepedFrequency(double frequency, double min, double max, double current)
        {
            Frequency = frequency;
            Min = min;
            Max = max;
            Current = current;
        }
    }
}
