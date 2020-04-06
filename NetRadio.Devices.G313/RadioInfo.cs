namespace NetRadio.Devices.G313
{
    /// <summary>
    /// Represents WinRadio receivers Information
    /// </summary>
    public class RadioInfo:G313BasicRadioInfo
    {
        /// <summary>
        /// Flag indicating that external frequency reference is available and can be used
        /// </summary>
        public bool ExternalFrequencyReference { get; protected set; }

        internal RadioInfo(NativeDefinitions.RadioInfo2 info)
        {
            Serial = info.szSerNum;
            Name = info.szProdName;
            MinFrequency = info.minFreq;
            MaxFrequency = info.maxFreq;

            ExternalFrequencyReference = info.features.ExtRef > 0;
        }
    }
}
