using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetRadio.Devices.G3XDdc
{
    public class AudioPlaybackArgs:AudioCallbackArgs
    {
        public bool End { get; set; }

        internal AudioPlaybackArgs(uint channel, float[] data, float[] filteredData)
            : base(channel, data, filteredData)
        {
            
        }
    }
}
