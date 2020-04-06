using System;

namespace NetRadio.Devices
{
    /// <summary>
    /// Represents un-processed stream data.
    /// </summary>
    public class ChunkArgs:EventArgs
    {
        /// <summary>
        /// Gets gathered stream raw data.
        /// </summary>
        public byte[] RawData { get; private set; }

        /// <summary>
        /// Gets stream sampling rate.
        /// </summary>
        public uint SamplingRate { get; private set; }

        public ChunkArgs(byte[] rawData,uint samplingRate)
        {
            SamplingRate = samplingRate;
            RawData = rawData;
        }
    }
}
