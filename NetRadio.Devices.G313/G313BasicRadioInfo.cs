namespace NetRadio.Devices.G313
{
    /// <summary>
    /// Represents WinRadio G313 receivers Information
    /// </summary>
    public class G313BasicRadioInfo:BasicRadioInfo
    {
        /// <summary>
        /// Gets minimum frequency supported by the receiver
        /// </summary>
        public ulong MinFrequency { get; protected set; }

        /// <summary>
        /// Gets maximum frequency supported by the receiver
        /// </summary>
        public ulong MaxFrequency { get; protected set; }
    }
}
