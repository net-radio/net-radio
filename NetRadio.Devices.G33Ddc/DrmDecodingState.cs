using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.Devices.G3XDdc
{
    public class DrmDecodingState
    {
        public bool SyncFound { get; private set; }
        public bool FacDecoded { get; private set; }
        public bool SdcDecoded { get; private set; }
        public bool AudioDecoded { get; private set; }
        public short AudioFramesCount { get; private set; }
        public short AudioErrorsCount { get; private set; }

        internal DrmDecodingState(NativeDefinitions.DecodingState native)
        {
            SyncFound = native.SyncFound;
            FacDecoded = native.FACDecoded;
            SdcDecoded = native.SDCDecoded;
            AudioDecoded = native.AudioDecoded;
            AudioFramesCount = native.NumberOfAudioFrames;
            AudioErrorsCount = native.NumberOfAudioErrors;
        }
    }
}
