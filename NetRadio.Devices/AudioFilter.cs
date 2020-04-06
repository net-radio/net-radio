using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.Devices
{
    public class AudioFilter
    {
        public uint CutOffLow { get; set; }
        public uint CutOffHigh { get; set; }
        public double DeEmphasis { get; set; }
    }
}
