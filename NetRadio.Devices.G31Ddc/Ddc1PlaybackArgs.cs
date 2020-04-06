using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.Devices.G3XDdc
{
    public class Ddc1PlaybackArgs:Ddc1CallbackArgs
    {
        public bool End { get; set; }
        public uint SampleSize { get;private set; }

        internal Ddc1PlaybackArgs(uint bitCount, uint samplingRate, byte[] data) :
            base(bitCount,samplingRate,data)
        {
            SampleSize = bitCount/2;
        }
    }
}
