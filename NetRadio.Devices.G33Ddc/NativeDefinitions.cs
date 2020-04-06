using System;
using System.Runtime.InteropServices;

namespace NetRadio.Devices.G3XDdc
{
    // ReSharper disable InconsistentNaming
    internal class NativeDefinitions
    {
        /// Buffer: FLOAT*
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int G31DDC_AUDIO_PLAYBACK_STREAM_CALLBACK(
            uint channel, IntPtr buffer, uint numberOfSamples, uint userData);

        /// Buffer: FLOAT*
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void G31DDC_AUDIO_STREAM_CALLBACK(
            uint channel, IntPtr buffer, IntPtr bufferFiltered, uint numberOfSamples, uint userData);

        //Buffer: void*
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int G31DDC_DDC1_PLAYBACK_STREAM_CALLBACK(
            IntPtr buffer, uint numberOfSamples, uint bitsPerSample, uint userData);

        //Buffer: void*
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void G31DDC_DDC1_STREAM_CALLBACK(
            IntPtr buffer, uint numberOfSamples, uint bitsPerSample, uint userData);

        /// Buffer: FLOAT*
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void G31DDC_DDC2_PREPROCESSED_STREAM_CALLBACK(
            uint channel, IntPtr buffer, uint numberOfSamples, float sLevelPeak, float sLevelRms, uint userData);

        /// Buffer: FLOAT*
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void G31DDC_DDC2_STREAM_CALLBACK(
            uint channel, IntPtr buffer, uint numberOfSamples, uint userData);

        //Buffer: SHORT*
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void G31DDC_IF_CALLBACK(
            IntPtr buffer, uint numberOfSamples, ushort maxAdcAmplitude, uint adcSamplingRate, uint userData);

        public enum DemodulatorMode : uint
        {
            G3XDDC_MODE_CW = 0,
            G3XDDC_MODE_AM = 1,
            G3XDDC_MODE_FM = 2,
            G3XDDC_MODE_LSB = 4,
            G3XDDC_MODE_USB = 5,
            G3XDDC_MODE_AMS = 8,
            G3XDDC_MODE_DRM = 18
        }

        public enum AmsSideband : uint
        {
            G3XDDC_SIDE_BAND_LOWER = 0x01,
            G3XDDC_SIDE_BAND_UPPER = 0x02,
            G3XDDC_SIDE_BAND_BOTH = (G3XDDC_SIDE_BAND_LOWER | G3XDDC_SIDE_BAND_UPPER)
        }

        public enum DemodulatorParameter : uint
        {
            G3XDDC_DEMODULATOR_PARAM_AMS_SIDE_BAND = 0x00000001,
            G3XDDC_DEMODULATOR_PARAM_AMS_CAPTURE_RANGE = 0x00000002,
            G3XDDC_DEMODULATOR_PARAM_CW_FREQUENCY = 0x00000003,
            G3XDDC_DEMODULATOR_PARAM_DRM_AUDIO_SERVICE = 0x00000004,
            G3XDDC_DEMODULATOR_PARAM_DRM_MULTIMEDIA_SERVICE = 0x00000005
        }

        public enum DemodulatorState : uint
        {
            G3XDDC_DEMODULATOR_STATE_AMS_LOCK = 0x00000001,
            G3XDDC_DEMODULATOR_STATE_AMS_FREQUENCY = 0x00000002,
            G3XDDC_DEMODULATOR_STATE_AM_DEPTH = 0x00000003,
            G3XDDC_DEMODULATOR_STATE_TUNE_ERROR = 0x00000004,
            G3XDDC_DEMODULATOR_STATE_DRM_STATUS = 0x00000005,
            G3XDDC_DEMODULATOR_STATE_FM_DEVIATION = 0x00000006
        }


        public enum LedMode : uint
        {
            G3XDDC_FRONT_PANEL_LED_MODE_DIAG = 0,
            G3XDDC_FRONT_PANEL_LED_MODE_ON = 1,
            G3XDDC_FRONT_PANEL_LED_MODE_OFF = 2
        };

        public enum InterfaceType : byte
        {
            G3XDDC_INTERFACE_TYPE_PCIE = 0,
            G3XDDC_INTERFACE_TYPE_USB = 1
        }

        public enum DrmMode
        {
            G3XDDC_DRM_STATE_MODE_NOT_DETERMINED_YET = -1,
            G3XDDC_DRM_STATE_MODE_A = 0,
            G3XDDC_DRM_STATE_MODE_B = 1,
            G3XDDC_DRM_STATE_MODE_C = 2,
            G3XDDC_DRM_STATE_MODE_D = 3
        }

        public enum DrmInterleaverMode : byte
        {
            G3XDDC_DRM_STATE_INTERLEAVER_LONG = 0, //long interleaver used (2 sec)
            G3XDDC_DRM_STATE_INTERLEAVER_SHORT = 1 //short interleaver used (400 msec)
        }

        public enum DrmQamType : byte
        {
            G3XDDC_DRM_STATE_QAM_TYPE_STD = 0, //standard
            G3XDDC_DRM_STATE_QAM_TYPE_HIER_SYM = 1, //hierarchical symmetrical
            G3XDDC_DRM_STATE_QAM_TYPE_HIER_MIX = 2 //hierarchical mixed
        }

        public enum DrmContentMode : byte
        {
            G3XDDC_DRM_STATE_SERVICE_CONTENT_EMPTY = 0x00, //service is not used/contains no data
            G3XDDC_DRM_STATE_SERVICE_CONTENT_AUDIO = 0x01, //service contains audio data
            G3XDDC_DRM_STATE_SERVICE_CONTENT_TEXTMSG = 0x02, //service contains text messages
            G3XDDC_DRM_STATE_SERVICE_CONTENT_MULTIMEDIA = 0x04, //service contains multimedia data
            G3XDDC_DRM_STATE_SERVICE_CONTENT_DATA = 0x08 //service contains application specific data
        }

        public enum DrmAudioCoding : byte
        {
            G3XDDC_DRM_STATE_AUDIO_CODING_AAC = 0, //audio coding is AAC
            G3XDDC_DRM_STATE_AUDIO_CODING_CELP = 1, //audio coding is CELP
            G3XDDC_DRM_STATE_AUDIO_CODING_HVXC = 2, //audio coding is HVXC
            G3XDDC_DRM_STATE_AUDIO_CODING_RFU = 3 //reserved for future use
        }

        public enum DrmAacMode
        {
            G3XDDC_DRM_STATE_AUDIO_MODE_AAC_MONO = 0, //mono
            G3XDDC_DRM_STATE_AUDIO_MODE_AAC_PARAM_STEREO = 1, //parametric stereo
            G3XDDC_DRM_STATE_AUDIO_MODE_AAC_STEREO = 2, //stereo
            G3XDDC_DRM_STATE_AUDIO_MODE_AAC_RFU = 3 //reserved for future use
        }

        public enum DrmCelpMode
        {
            G3XDDC_DRM_STATE_AUDIO_MODE_CELP_NO_CRC = 0, //audio data is without CRC
            G3XDDC_DRM_STATE_AUDIO_MODE_CELP_CRC = 1, //CRC used
            G3XDDC_DRM_STATE_AUDIO_MODE_CELP_RFU_10 = 2, //reserved for future use
            G3XDDC_DRM_STATE_AUDIO_MODE_CELP_RFU_11 = 3 //reserved for future use
        }

        public enum DrmHvxcMode
        {
            G3XDDC_DRM_STATE_AUDIO_MODE_HVXC_RFU_00 = 0, //reserved for future use
            G3XDDC_DRM_STATE_AUDIO_MODE_HVXC_RFU_01 = 1, //reserved for future use
            G3XDDC_DRM_STATE_AUDIO_MODE_HVXC_RFU_10 = 2, //reserved for future use
            G3XDDC_DRM_STATE_AUDIO_MODE_HVXC_RFU_11 = 3 //reserved for future use
        }

        [StructLayout(LayoutKind.Sequential)]
        public class AudioDecoderInfo
        {
            /// BOOL->int
            [MarshalAs(UnmanagedType.Bool)] 
            public bool Valid;

            /// BYTE->unsigned char
            public DrmAudioCoding AudioCoding;

            /// BOOL->int
            [MarshalAs(UnmanagedType.Bool)] 
            public bool SBR;

            /// INT32->int
            public int AudioMode;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public class DecodingState
        {
            /// BOOL->int
            [MarshalAs(UnmanagedType.Bool)] 
            public bool SyncFound;

            /// BOOL->int
            [MarshalAs(UnmanagedType.Bool)] 
            public bool FACDecoded;

            /// BOOL->int
            [MarshalAs(UnmanagedType.Bool)] 
            public bool SDCDecoded;

            /// BOOL->int
            [MarshalAs(UnmanagedType.Bool)] 
            public bool AudioDecoded;

            /// SHORT->short
            public short NumberOfAudioFrames;

            /// SHORT->short
            public short NumberOfAudioErrors;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal class G31DDC_DEVICE_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string DevicePath;
            public InterfaceType InterfaceType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)] public string SerialNumber;
            public uint ChannelCount;
            public uint DDCTypeCount;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct G31DDC_CALLBACK
        {
            public G31DDC_IF_CALLBACK IFCallback;
            public G31DDC_DDC1_STREAM_CALLBACK DDC1StreamCallback;
            public G31DDC_DDC1_PLAYBACK_STREAM_CALLBACK DDC1PlaybackStreamCallback;
            public G31DDC_DDC2_STREAM_CALLBACK DDC2StreamCallback;
            public G31DDC_DDC2_PREPROCESSED_STREAM_CALLBACK DDC2PreprocessedStreamCallback;
            public G31DDC_AUDIO_STREAM_CALLBACK AudioStreamCallback;
            public G31DDC_AUDIO_PLAYBACK_STREAM_CALLBACK AudioPlaybackStreamCallback;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class G3XDDC_DDC_INFO
        {
            public uint SampleRate;
            public uint Bandwidth;
            public uint BitsPerSample;
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public class G3XDDC_DRM_STATUS
        {
            /// BOOL->int
            [MarshalAs(UnmanagedType.Bool)] public bool Valid;

            /// Anonymous_31376de9_d453_45d8_8f33_9cebc37612d4
            public DecodingState DecodingState;

            /// INT32->int
            public DrmMode Mode;

            /// double
            public double RFBandwidth;

            /// BYTE->unsigned char
            public DrmInterleaverMode Interleaver;

            /// SHORT->short
            public short SDCQam;

            /// SHORT->short
            public short MSCQam;

            /// BYTE->unsigned char
            public DrmQamType MSCQamType;

            /// double
            public double CoderateH;

            /// double
            public double CoderateA;

            /// double
            public double CoderateB;

            /// double
            public double EstimatedSNR;

            /// WCHAR[145]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 145)] 
            public string TextMessage;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)] 
            public ServiceInfo[] ServiceInfo;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)] 
            public AudioDecoderInfo[] AudioDecoderInfo;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack=1)]
        public class ServiceInfo
        {
            /// BYTE->unsigned char
            public DrmContentMode Content;

            /// WCHAR[256]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] 
            public string DynamicLabel;

            /// WCHAR[256]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] 
            public string Country;

            /// WCHAR[256]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] 
            public string Language;

            /// WCHAR[256]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] 
            public string ProgramType;

            /// double
            public double AudioBitrate;

            /// double
            public double TextMsgBitrate;

            /// double
            public double MultimediaBitrate;

            /// double
            public double DataBitrate;
        }
    }
}