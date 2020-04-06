using System;

namespace NetRadio.Devices.G313
{
    /// <summary>
    /// Represents WinRadio receivers Information
    /// </summary>
    [Obsolete("use RadioInfo instead")]
    public class LegacyRadioInfo:G313BasicRadioInfo
    {
        /// <summary>
        /// Gets number of band filters on RF input of the receiver; also, number of valid entries in BandFrequency array
        /// </summary>
        public int BandFilterCount { get; protected set; }

        /// <summary>
        /// Gets crossover frequencies between band filters
        /// </summary>
        public uint[] BandFrequency { get; protected set; }

        /// <summary>
        /// Local oscillator offset
        /// </summary>
        public uint LocalOscillatorOffset { get; protected set; }

        /// <summary>
        /// Gets number of VCOs on PLL board; also, number of valid entries in VcoFrequncy, VcoDividers and VcoSelectBits arrays
        /// </summary>
        public int VcoCount { get; protected set; }

        /// <summary>
        /// Gets highest frequency for each VCO
        /// </summary>
        public uint[] VcoFrequncy { get;protected set; }

        /// <summary>
        /// Gets VCO dividers
        /// </summary>
        public ushort[] VcoDividers { get; protected set; }

        /// <summary>
        /// Gets VCO select bits
        /// </summary>
        public byte[] VcoSelectBits { get; protected set; }

        /// <summary>
        /// Reference Clock1 frequency [Hz]
        /// </summary>
        public uint ReferenceClock1 { get; protected set; }

        /// <summary>
        /// Reference Clock2 frequency [Hz], 0 if not fitted
        /// </summary>
        public uint ReferenceClock2 { get; protected set; }

        /// <summary>
        /// Gets DACs on IF1 module
        /// </summary>
        public byte[] DacOnIf1 { get; protected set; }

        /// <summary>
        /// Determines whether there is a second clock reference or not
        /// </summary>
        /// <returns>Returns true if second clock is present.</returns>
        public bool HasSecondClock()
        {
            return ReferenceClock2 != 0;
        }

        internal LegacyRadioInfo(NativeDefinitions.OldRadioInfo info)
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
        }
        /// <summary>
        /// Creates default LegacyRadioInfo
        /// </summary>
        protected LegacyRadioInfo()
        {

        }
    }
}
