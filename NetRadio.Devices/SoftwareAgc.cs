namespace NetRadio.Devices
{
    /// <summary>
    /// Represents software based automatic gain control parameters.
    /// </summary>
    public class SoftwareAgc
    {
        /// <summary>
        /// Gets or Sets AGC reference level.
        /// </summary>
        public double ReferenceLevel { get; set; }
        //public int ReferenceLevel { get; set; }

        /// <summary>
        /// Gets or Sets attack time.
        /// </summary>
        public double AttackTime { get; set; }
        //public uint AttackTime { get; set; }

        /// <summary>
        /// Gets or Sets decaay time.
        /// </summary>
        public double DecayTime { get; set; }
        //public uint DecayTime { get; set; }

        /// <summary>
        /// Creates a Software based automatic gain control parameter container.
        /// </summary>
        public SoftwareAgc()
        {

        }

    }
}
