namespace NetRadio.Devices.G313
{
    /// <summary>
    /// Represents Automatic gain control modes
    /// </summary>
    public enum Agc
    {
        /// <summary>
        /// AGC off
        /// </summary>
        Off = 0,
        /// <summary>
        /// AGC slow mode
        /// </summary>
        Slow = 1,
        /// <summary>
        /// AGC medium mode
        /// </summary>
        Medium = 2,
        /// <summary>
        /// AGC fast mode
        /// </summary>
        Fast = 3
    }

    /// <summary>
    /// Represents Internal RSSI settings of a receiver.
    /// </summary>
    public class InternalRssi
    {
        private const uint LOWER = 0x0000FFFF;
        private const uint HIGHER = 0xFFFF0000;

        private readonly uint _rawValue;

        /// <summary>
        /// Get raw  value.
        /// </summary>
        public uint Raw { get { return _rawValue; } }

        /// <summary>
        /// Gets RSSI value.
        /// </summary>
        public uint Rssi { get { return _rawValue & LOWER; } }

        /// <summary>
        /// Gets raw AGC value.
        /// </summary>
        public uint Agc { get { return (_rawValue & HIGHER) >> 16; } }

        /// <summary>
        /// Gets AGC mode based on raw value.
        /// </summary>
        /// <returns></returns>
        public Agc ToAgc()
        {
            return (Agc)Agc;
        }

        internal InternalRssi(uint rawValue)
        {
            _rawValue = rawValue;
        }
    }
}
