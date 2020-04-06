using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.Devices.G3XDdc
{
    public class CallbackArgs:EventArgs
    {
        public uint SamplingRate { get; protected set; }
        public uint BitCount { get; protected set; }
    }
}
