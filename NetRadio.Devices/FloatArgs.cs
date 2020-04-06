using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.Devices
{
    public class FloatArgs:EventArgs
    {
        /// <summary>
        /// Gets gathered stream raw data.
        /// </summary>
        public float[] RawData { get; private set; }

        /// <summary>
        /// Gets stream sampling rate.
        /// </summary>
        public uint SamplingRate { get; private set; }

        public int DataLength { get; private set; }

        public FloatArgs(float[] rawData, uint samplingRate)
            : this(rawData, samplingRate, rawData.Length)
        {
            
        }

        public FloatArgs(float[] rawData,uint samplingRate,int length)
        {
            SamplingRate = samplingRate;
            RawData = rawData;
            DataLength = length;
        }
    }
}
