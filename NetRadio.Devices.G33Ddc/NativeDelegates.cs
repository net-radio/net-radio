using System;
using System.Runtime.InteropServices;

namespace NetRadio.Devices.G3XDdc
{
    // ReSharper disable InconsistentNaming
    class NativeDelegates
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int GetDeviceList(IntPtr list, uint bufferSize);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int OpenDevice(string serial);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool CloseDevice(int hDevice);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool IsDeviceConnected(int hDevice);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDeviceInfo(int hDevice, IntPtr info, uint bufferSize);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetPower(int hDevice, bool power);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetPower(int hDevice, ref bool power);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetAttenuator(int hDevice, uint attenuator);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetAttenuator(int hDevice, ref uint attenuator);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetDithering(int hDevice, bool dithering);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDithering(int hDevice, ref bool dithering);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetADCNoiseBlanker(int hDevice, bool blanker);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetADCNoiseBlanker(int hDevice, ref bool blanker);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetADCNoiseBlankerThreshold(int hDevice, ushort threshold);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetADCNoiseBlankerThreshold(int hDevice, ref ushort threshold);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetLED(int hDevice, uint led);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetLED(int hDevice, ref uint led);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetInverted(int hDevice, bool inverted);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetInverted(int hDevice, ref bool inverted);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetDDC2NoiseBlanker(int hDevice, uint channel, bool enabled);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDDC2NoiseBlanker(int hDevice, uint channel, ref bool enabled);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetDDC2NoiseBlankerThreshold(int hDevcice, uint channel, double threshold);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDDC2NoiseBlankerThreshold(int hDevice, uint channel, ref double threshold);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDDC2NoiseBlankerExcessValue(int hDevice, uint channel, ref double value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool StartIF(int hDevice, ushort period);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool StopIF(int hDevice);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDDCInfo(int hDevice, uint ddcTypeIndex, IntPtr info);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDDC1Count(int hDevice, ref uint count);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetDDC1(int hDevice, uint ddcTypeIndex);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDDC1(int hDevice, ref uint ddcTypeIndex, IntPtr ddcInfo);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool StartDDC1(int hDevice, uint samplesPerBuffer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool StopDDC1(int hDevice);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool StartDDC1Playback(int hDevice, uint samplesPerBuffer, uint bitsPerSample);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool PauseDDC1Playback(int hDevice);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool ResumeDDC1Playback(int hDevice);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetDDC1Frequency(int hDevice, uint frequency);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDDC1Frequency(int hDevice, ref uint frequency);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDDC2(int hDevice, ref uint ddcTypeIndex, IntPtr ddcInfo);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool StartDDC2(int hDevice,uint channel, uint samplesPerBuffer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool StopDDC2(int hDevice,uint channel);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetDDC2Frequency(int hDevice, uint channel, int frequency);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDDC2Frequency(int hDevice, uint channel, ref int frequency);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetSignalLevel(int hDevice, uint channel, ref float peak, ref float rms);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetAGC(int hDevice, uint channel, bool enabled);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetAGC(int hDevice, uint channel, ref bool enabled);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetAGCParams(
            int hDevice, uint channel, double attackTime, double decayTime, double referenceLevel);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetAGCParams(
            int hDevice, uint channel, ref double attackTime, ref double decayTime, ref double referenceLevel);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetMaxAGCGain(int hDevice, uint channel, double maxGain);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetMaxAGCGain(int hDevice, uint channel, ref double maxGain);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetGain(int hDevice, uint channel, double gain);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetGain(int hDevice, uint channel, ref double gain);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetCurrentGain(int hDevice, uint channel, ref double gain);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetNotchFilter(int hDevice, uint channel, bool enabled);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetNotchFilter(int hDevice, uint channel, ref bool enabled);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetNotchFilterFrequency(int hDevice, uint channel, int frequency);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetNotchFilterFrequency(int hDevice, uint channel, ref int frequency);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetNotchFilterBandwidth(int hDevice, uint channel, uint bandwidth);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetNotchFilterBandwidth(int hDevice, uint channel, ref uint bandwidth);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetNotchFilterLength(int hDevice, uint channel, uint length);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetNotchFilterLength(int hDevice, uint channel, ref uint length);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetDemodulatorMode(int hDevice, uint channel, uint mode);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDemodulatorMode(int hDevice, uint channel, ref uint mode);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetDemodulatorFrequency(int hDevice, uint channel, int frequency);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDemodulatorFrequency(int hDevice, uint channel, ref int frequency);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetDemodulatorFilterBandwidth(int hDevice, uint channel, uint bandwidth);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDemodulatorFilterBandwidth(int hDevice, uint channel, ref uint bandwidth);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetDemodulatorFilterShift(int hDevice, uint channel, int shift);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDemodulatorFilterShift(int hDevice, uint channel, ref int shift);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetDemodulatorFilterLength(int hDevice, uint channel, uint length);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDemodulatorFilterLength(int hDevice, uint channel, ref uint length);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetDemodulatorParam(int hDevice, uint channel, uint code, IntPtr buffer, uint bufferSize);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDemodulatorParam(int hDevice, uint channel, uint code,IntPtr buffer, uint bufferSize);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetDemodulatorState(int hDevice, uint channel, uint code, IntPtr buffer, uint bufferSize);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool StartAudio(int hDevice, uint channel, uint samplesPerBuffer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool StopAudio(int hDevice, uint channel);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool StartAudioPlayback(int hDevice, uint channel, uint samplesPerBuffer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool PauseAudioPlayback(int hDevice, uint channel);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool ResumeAudioPlayback(int hDevice, uint channel);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetAudioFilter(int hDevice, uint channel, bool enabled);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetAudioFilter(int hDevice, uint channel, ref bool enabled);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetAudioFilterParams(
            int hDevice, uint channel, uint cutOffLow, uint cutOffHigh, double deEmphasis);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetAudioFilterParams(
            int hDevice, uint channel, ref uint cutOffLow, ref uint cutOffHigh, ref double deEmphasis);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetAudioFilterLength(int hDevice, uint channel, uint length);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetAudioFilterLength(int hDevice, uint channel, ref uint length);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetAudioGain(int hDevice, uint channel, double gain);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetAudioGain(int hDevice, uint channel, ref double gain);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetDRMKey(int hDevice, string drmPath);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool IsDRMUnlocked(int hDevice);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetSpectrumCompensation(
            int hDevice, uint centerFrequency, uint width, IntPtr buffer, uint count);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetFrequency(int hDevice, uint channel, uint frequency);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetFrequency(int hDevice, uint channel, ref uint frequency);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetCallbacks(int hDevice, ref NativeDefinitions.G31DDC_CALLBACK callbacks, IntPtr userData);
    }
}
