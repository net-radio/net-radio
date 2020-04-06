using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.Devices.G3XDdc
{
    public class DdcInfo
    {
        public uint SampleRate { get; private set; }
        public uint Bandwidth { get; private set; }
        public IqBitsPerSample BitsPerSample { get; private set; }

        internal DdcInfo(NativeDefinitions.G3XDDC_DDC_INFO native)
        {
            SampleRate = native.SampleRate;
            Bandwidth = native.Bandwidth;
            BitsPerSample = (IqBitsPerSample)native.BitsPerSample;
        }

        public override string ToString()
        {
            return string.Format("BandWidth:{0} , Sample Rate:{1}", Bandwidth, SampleRate);
        }
    }
}
