using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.Devices.G3XDdc
{
    public class Ddc2PreprocessedCallbackArgs:Ddc2CallbackArgs
    {
        public float LevelPeak { get; private set; }
        public float LevelRms { get; private set; }

        internal Ddc2PreprocessedCallbackArgs(uint bitCount, uint samplingRate, uint channel, float[] data,
            float levelPeak, float levelRms)
            : base(bitCount, samplingRate, channel, data)
        {
            LevelPeak = levelPeak;
            LevelRms = levelRms;
        }
    }
}
