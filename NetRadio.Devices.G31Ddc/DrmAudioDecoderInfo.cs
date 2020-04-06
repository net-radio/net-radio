using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.Devices.G3XDdc
{
    public class DrmAudioDecoderInfo
    {
        public bool Valid { get; private set; }
        public DrmAudioCoding Coding { get; private set; }
        public bool Sbr { get; private set; }

        public DrmAacMode? AacMode { get; private set; }
        public DrmHvxcMode? HvxcMode { get; private set; }
        public DrmCelpMode? CelpMode { get; private set; }

        internal DrmAudioDecoderInfo(NativeDefinitions.AudioDecoderInfo native)
        {
            Valid = native.Valid;
            Coding = (DrmAudioCoding) native.AudioCoding;
            Sbr = native.SBR;

            switch (Coding)
            {
                case DrmAudioCoding.Aac:
                    AacMode = (DrmAacMode) native.AudioMode;
                    break;
                case DrmAudioCoding.Celp:
                    CelpMode = (DrmCelpMode) native.AudioMode;
                    break;
                case DrmAudioCoding.Hvxc:
                    HvxcMode = (DrmHvxcMode) native.AudioMode;
                    break;
            }
        }
    }
}
