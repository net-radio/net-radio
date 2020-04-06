using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.Devices.G3XDdc
{
    public class Ddc1CallbackArgs:CallbackArgs
    {
        public byte[] Data { get; private set; }

        internal Ddc1CallbackArgs(uint bitCount,uint samplingRate, byte[] data)
        {
            Data = data;
            BitCount = bitCount;
            SamplingRate = samplingRate;
        }
    }
}
