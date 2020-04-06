using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.Devices
{
    public class SignalLevel
    {
        public float Peak { get;private set; }
        public float Rms { get; private set; }

        public SignalLevel(float peak, float rms)
        {
            Peak = peak;
            Rms = rms;
        }
    }
}
