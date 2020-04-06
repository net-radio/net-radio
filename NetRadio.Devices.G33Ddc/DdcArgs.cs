using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.Devices.G3XDdc
{
    public class DdcArgs:EventArgs
    {
        public uint Index { get; private set; }
        public DdcInfo Info { get; private set; }

        internal DdcArgs(uint index, DdcInfo info)
        {
            Index = index;
            Info = info;
        }
    }
}
