using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Nmg.ManagedCanToUsb
{
    public class FrameArg:EventArgs
    {
        public Frame Frame { get; private set; }

        internal FrameArg(Frame frame)
        {
            Frame = frame;
        }
    }
}
