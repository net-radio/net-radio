namespace NetRadio.Devices
{
    /// <summary>
    /// Represetns notch filter parameters.
    /// </summary>
    public class NotchFilter
    {
        /// <summary>
        /// Gets or Sets notch filter frequency.
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// Gets or Sets notch filter bandwidth.
        /// </summary>
        public uint Bandwidth { get; set; }

        /// <summary>
        /// Gets or Sets notch filter state.
        /// </summary>
        public bool Active { get; set; }
    }
}
