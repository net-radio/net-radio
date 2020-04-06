using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.Devices.G3XDdc
{
    public class DrmStatus
    {
        public bool Valid { get; private set; }
        public DrmDecodingState DecodingState { get; private set; }
        public DrmMode Mode { get; private set; }
        public double RfBandwidth { get; private set; }
        public DrmInterleaverMode Interleaver { get; private set; }
        public short SdcQam { get; private set; }
        public short MscQam { get; private set; }
        public DrmMscQamType MscQamType { get; private set; }
        public double CodeRateH { get; private set; }
        public double CodeRateA { get; private set; }
        public double CodeRateB { get; private set; }
        public double EstimatedSnr { get; private set; }
        public string TextMessage { get; private set; }
        public DrmServiceInfo[] ServiceInfo { get; private set; }
        public DrmAudioDecoderInfo[] AudioDecoderInfo { get; private set; }

        internal DrmStatus(NativeDefinitions.G3XDDC_DRM_STATUS native)
        {
            Valid = native.Valid;
            DecodingState = new DrmDecodingState(native.DecodingState);
            Mode = (DrmMode) native.Mode;
            RfBandwidth = native.RFBandwidth;
            Interleaver = (DrmInterleaverMode) native.Interleaver;
            SdcQam = native.SDCQam;
            MscQam = native.MSCQam;
            MscQamType = (DrmMscQamType) native.MSCQamType;
            CodeRateH = native.CoderateH;
            CodeRateA = native.CoderateA;
            CodeRateB = native.CoderateB;
            EstimatedSnr = native.EstimatedSNR;
            TextMessage = native.TextMessage;
            ServiceInfo = new[]
            {
                new DrmServiceInfo(native.ServiceInfo[0]), new DrmServiceInfo(native.ServiceInfo[1]),
                new DrmServiceInfo(native.ServiceInfo[2]), new DrmServiceInfo(native.ServiceInfo[3])
            };
            AudioDecoderInfo = new[]
            {
                new DrmAudioDecoderInfo(native.AudioDecoderInfo[0]), new DrmAudioDecoderInfo(native.AudioDecoderInfo[1]),
                new DrmAudioDecoderInfo(native.AudioDecoderInfo[2]), new DrmAudioDecoderInfo(native.AudioDecoderInfo[3])
            };
        }
    }
}
