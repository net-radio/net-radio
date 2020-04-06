namespace NetRadio.Devices
{
    /// <summary>
    /// Represents noise blanker parameters.
    /// </summary>
    public class NoiseBlanker
    {
        /// <summary>
        /// Gets or Sets noise blanker threshold.
        /// </summary>
        public double Threshold { get; set; }
        //public uint Threshold { get; set; }

        /// <summary>
        /// Gets or Sets state of noise blanker.
        /// </summary>
        public bool Active { get; set; }
    }
}
