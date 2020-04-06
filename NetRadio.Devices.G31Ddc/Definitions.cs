using System;

namespace NetRadio.Devices.G3XDdc
{
    public enum G3XInterfaceType : uint
    {
        Pcie = NativeDefinitions.InterfaceType.G3XDDC_INTERFACE_TYPE_PCIE,
        Usb = NativeDefinitions.InterfaceType.G3XDDC_INTERFACE_TYPE_USB
    };

    public enum LedMode : uint
    {
        Diagnostic = NativeDefinitions.LedMode.G3XDDC_FRONT_PANEL_LED_MODE_DIAG,
        On = NativeDefinitions.LedMode.G3XDDC_FRONT_PANEL_LED_MODE_ON,
        Off = NativeDefinitions.LedMode.G3XDDC_FRONT_PANEL_LED_MODE_OFF,
    };

    public enum IqBitsPerSample : uint
    {
        Default = 0,
        Sample16Bit = 16,
        Sample32Bit = 32
    };

    public enum DemodulatorMode : uint
    {
        Cw = NativeDefinitions.DemodulatorMode.G3XDDC_MODE_CW,
        Am = NativeDefinitions.DemodulatorMode.G3XDDC_MODE_AM,
        Fm = NativeDefinitions.DemodulatorMode.G3XDDC_MODE_FM,
        Lsb = NativeDefinitions.DemodulatorMode.G3XDDC_MODE_LSB,
        Usb = NativeDefinitions.DemodulatorMode.G3XDDC_MODE_USB,
        Ams = NativeDefinitions.DemodulatorMode.G3XDDC_MODE_AMS,
        Drm = NativeDefinitions.DemodulatorMode.G3XDDC_MODE_DRM,
        Dsb = EnhancedDemodulatorMode.G3XDDC_MODE_DSB,
        Isb = EnhancedDemodulatorMode.G3XDDC_MODE_ISB,
    };

    public enum EnhancedDemodulatorMode : uint
    {
        G3XDDC_MODE_DSB = 100,
        G3XDDC_MODE_ISB = 101,
    }

    [Flags]
    public enum AmsSideband : uint
    {
        Lower = NativeDefinitions.AmsSideband.G3XDDC_SIDE_BAND_LOWER,
        Upper = NativeDefinitions.AmsSideband.G3XDDC_SIDE_BAND_UPPER,
        Both = Lower | Upper
    };

    public enum DrmMode
    {
        A = NativeDefinitions.DrmMode.G3XDDC_DRM_STATE_MODE_A,
        B = NativeDefinitions.DrmMode.G3XDDC_DRM_STATE_MODE_B,
        C = NativeDefinitions.DrmMode.G3XDDC_DRM_STATE_MODE_C,
        D = NativeDefinitions.DrmMode.G3XDDC_DRM_STATE_MODE_D,
        NotDetermined = NativeDefinitions.DrmMode.G3XDDC_DRM_STATE_MODE_NOT_DETERMINED_YET,
    };

    public enum DrmInterleaverMode : byte
    {
        Short = NativeDefinitions.DrmInterleaverMode.G3XDDC_DRM_STATE_INTERLEAVER_SHORT,
        Long = NativeDefinitions.DrmInterleaverMode.G3XDDC_DRM_STATE_INTERLEAVER_LONG,
    };

    public enum DrmMscQamType : byte
    {
        HierMix = NativeDefinitions.DrmQamType.G3XDDC_DRM_STATE_QAM_TYPE_HIER_MIX,
        HierSym = NativeDefinitions.DrmQamType.G3XDDC_DRM_STATE_QAM_TYPE_HIER_SYM,
        Std = NativeDefinitions.DrmQamType.G3XDDC_DRM_STATE_QAM_TYPE_STD,
    };

    public enum DrmContentType : byte
    {
        Audio = NativeDefinitions.DrmContentMode.G3XDDC_DRM_STATE_SERVICE_CONTENT_AUDIO,
        Data = NativeDefinitions.DrmContentMode.G3XDDC_DRM_STATE_SERVICE_CONTENT_DATA,
        Empty = NativeDefinitions.DrmContentMode.G3XDDC_DRM_STATE_SERVICE_CONTENT_EMPTY,
        Multimedia = NativeDefinitions.DrmContentMode.G3XDDC_DRM_STATE_SERVICE_CONTENT_MULTIMEDIA,
        TextMessage = NativeDefinitions.DrmContentMode.G3XDDC_DRM_STATE_SERVICE_CONTENT_TEXTMSG,
    };

    public enum DrmAudioCoding : byte
    {
        Aac = NativeDefinitions.DrmAudioCoding.G3XDDC_DRM_STATE_AUDIO_CODING_AAC,
        Celp = NativeDefinitions.DrmAudioCoding.G3XDDC_DRM_STATE_AUDIO_CODING_CELP,
        Hvxc = NativeDefinitions.DrmAudioCoding.G3XDDC_DRM_STATE_AUDIO_CODING_HVXC,
        //Rfu = NativeDefinitions.DrmAudioCoding.G3XDDC_DRM_STATE_AUDIO_CODING_RFU,
    };

    public enum DrmAacMode
    {
        Mono = NativeDefinitions.DrmAacMode.G3XDDC_DRM_STATE_AUDIO_MODE_AAC_MONO,
        ParametericStereo = NativeDefinitions.DrmAacMode.G3XDDC_DRM_STATE_AUDIO_MODE_AAC_PARAM_STEREO,
        Stereo = NativeDefinitions.DrmAacMode.G3XDDC_DRM_STATE_AUDIO_MODE_AAC_STEREO,
        //Rfu = NativeDefinitions.DrmAacMode.G3XDDC_DRM_STATE_AUDIO_MODE_AAC_RFU
    };

    public enum DrmCelpMode
    {
        NoCrc = NativeDefinitions.DrmCelpMode.G3XDDC_DRM_STATE_AUDIO_MODE_CELP_NO_CRC,
        Crc = NativeDefinitions.DrmCelpMode.G3XDDC_DRM_STATE_AUDIO_MODE_CELP_CRC,
        //Rfu0 = NativeDefinitions.DrmCelpMode.G3XDDC_DRM_STATE_AUDIO_MODE_CELP_RFU_10,
        //Rfu1 = NativeDefinitions.DrmCelpMode.G3XDDC_DRM_STATE_AUDIO_MODE_CELP_RFU_11,
    };

    public enum DrmHvxcMode
    {
        //Rfu0 = NativeDefinitions.DrmHvxcMode.G3XDDC_DRM_STATE_AUDIO_MODE_HVXC_RFU_00,
        //Rfu1 = NativeDefinitions.DrmHvxcMode.G3XDDC_DRM_STATE_AUDIO_MODE_HVXC_RFU_01,
        //Rfu2 = NativeDefinitions.DrmHvxcMode.G3XDDC_DRM_STATE_AUDIO_MODE_HVXC_RFU_10,
        //Rfu3 = NativeDefinitions.DrmHvxcMode.G3XDDC_DRM_STATE_AUDIO_MODE_HVXC_RFU_11
    };

    public enum AttenuatorValue : uint
    {
        A0 = 0,
        A3 = 3,
        A6 = 6,
        A9 = 9,
        A12 = 12,
        A15 = 15,
        A18 = 18,
        A21 = 21,
    };

    /// <summary>
    /// Represents Automatic gain control modes
    /// </summary>
    public enum Agc
    {
        /// <summary>
        /// AGC off
        /// </summary>
        Off = 0,
        /// <summary>
        /// AGC slow mode
        /// </summary>
        Slow = 1,
        /// <summary>
        /// AGC medium mode
        /// </summary>
        Medium = 2,
        /// <summary>
        /// AGC fast mode
        /// </summary>
        Fast = 3
    }
}
