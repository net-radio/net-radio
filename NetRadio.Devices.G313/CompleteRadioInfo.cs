using System;

namespace NetRadio.Devices.G313
{
    /// <summary>
    /// Represents WinRadio receivers Information
    /// </summary>
    [Obsolete("use Radioinfo instead")]
    public class CompleteRadioInfo : LegacyRadioInfo
    {
        /// <summary>
        /// Gets raw AGC level at low power (-97dBm)
        /// </summary>
        public int RawAgcStart { get; protected set; }

        /// <summary>
        /// Gets raw AGC value 5dB above AGC start
        /// </summary>
        public int RawAgcMiddle { get; protected set; }

        /// <summary>
        /// Gets raw AGC value at -30dBm
        /// </summary>
        public int RawAgcEnd { get; protected set; }

        /// <summary>
        /// Gets the level where AGC starts *10dBm
        /// </summary>
        public int DropLevel { get; protected set; }

        /// <summary>
        /// Gets the level where raw RSSI value is 0x03FF *10dBm
        /// </summary>
        public int RssiTop { get; protected set; }

        /// <summary>
        /// Gets raw RSSI value at -97dBm
        /// </summary>
        public int RssiReference { get; protected set; }

        internal CompleteRadioInfo(NativeDefinitions.RadioInfo info)
        {
            Serial = info.szSerNum;
            Name = info.szProdName;
            MinFrequency = info.dwMinFreq;
            MaxFrequency = info.dwMaxFreq;
            BandFilterCount = info.bNumBands;
            BandFrequency = info.dwBandFreq;
            LocalOscillatorOffset = info.dwLOfreq;
            VcoCount = info.bNumVcos;
            VcoFrequncy = info.dwVcoFreq;
            VcoDividers = info.wVcoDiv;
            VcoSelectBits = info.bVcoBits;
            ReferenceClock1 = info.dwRefClk1;
            ReferenceClock2 = info.dwRefClk2;
            DacOnIf1 = info.IF1DAC;

            RawAgcStart = info.iAGCstart;
            RawAgcMiddle = info.iAGCmid;
            RawAgcEnd = info.iAGCend;
            DropLevel = info.iDropLevel;
            RssiTop = info.iRSSItop;
            RssiReference = info.iRSSIref;
        }
    }
}
