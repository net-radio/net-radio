using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.Devices.G3XDdc
{
    public class AudioCallbackArgs : CallbackArgs
    {
        public uint Channel { get; private set; }
        public float[] Data { get; private set; }
        public float[] FilteredData { get; private set; }

        internal AudioCallbackArgs(uint channel, float[] data, float[] filteredData)
        {
            SamplingRate = 48000; //48khz
            BitCount = 32; //32bit float mono
            Channel = channel;

            Data = data;
            FilteredData = filteredData;
        }
    }
}
