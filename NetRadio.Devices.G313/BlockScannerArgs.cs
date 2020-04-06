using System;

namespace NetRadio.Devices.G313
{
    /// <summary>
    /// Represetns block scanner processed parameters
    /// </summary>
    public class BlockScannerArgs:EventArgs
    {
        /// <summary>
        /// Gets state of the scan [0 - 1.0].
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// Gets raw signal strength.
        /// </summary>
        public int Strength { get; private set; }

        /// <summary>
        /// Gets minimum acceptable raw signal.
        /// </summary>
        public int ThresholdStrength { get; private set; }

        /// <summary>
        /// Gets scanned frequency band.
        /// </summary>
        public uint Frequency { get; private set; }

        /// <summary>
        /// Gets signal strength based on dBm.
        /// </summary>
        public int StrengthDbm { get; private set; }

        internal BlockScannerArgs(float progress, int strength, int threshold, uint frequency)
        {
            Progress = progress;
            Strength = strength;
            ThresholdStrength = threshold;
            Frequency = frequency;

            StrengthDbm = (Strength*1000/255 - 1300)/10;
        }
    }
}
