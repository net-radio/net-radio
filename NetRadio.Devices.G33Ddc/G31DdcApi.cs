using NetRadio.Devices.G3XDdc.Annotations;

namespace NetRadio.Devices.G3XDdc
{
    class G31DdcApi:NativeLoader
    {
        private const string DLL = "G33DDCAPI.dll";

        [UsedImplicitly][ApiCall]
        public  NativeDelegates.CloseDevice CloseDevice {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetADCNoiseBlanker GetAdcNoiseBlanker {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetADCNoiseBlankerThreshold GetAdcNoiseBlankerThreshold {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetAGC GetAgc {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetAGCParams GetAgcParams {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetAttenuator GetAttenuator {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetAudioFilter GetAudioFilter {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetAudioFilterLength GetAudioFilterLength {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetAudioFilterParams GetAudioFilterParams {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetAudioGain GetAudioGain {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetCurrentGain GetCurrentGain {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDDC1 GetDdc1 {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDDC1Frequency GetDdc1Frequency {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDDC2 GetDdc2 {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDDC2Frequency GetDdc2Frequency {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDDC2NoiseBlanker GetDdc2NoiseBlanker {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDDC2NoiseBlankerExcessValue GetDdc2NoiseBlankerExcessValue {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDDC2NoiseBlankerThreshold GetDdc2NoiseBlankerThreshold {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDDCInfo GetDdcInfo {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDemodulatorFilterBandwidth GetDemodulatorFilterBandwidth {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDemodulatorFilterLength GetDemodulatorFilterLength {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDemodulatorFilterShift GetDemodulatorFilterShift {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDemodulatorFrequency GetDemodulatorFrequency {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDemodulatorMode GetDemodulatorMode {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDemodulatorParam GetDemodulatorParam {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDemodulatorState GetDemodulatorState {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDeviceInfo GetDeviceInfo {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDithering GetDithering {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetFrequency GetFrequency {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetGain GetGain {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetInverted GetInverted {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetLED GetLed {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetMaxAGCGain GetMaxAgcGain {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetNotchFilter GetNotchFilter {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetNotchFilterBandwidth GetNotchFilterBandwidth {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetNotchFilterFrequency GetNotchFilterFrequency {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetNotchFilterLength GetNotchFilterLength {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetPower GetPower {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetSignalLevel GetSignalLevel {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetSpectrumCompensation GetSpectrumCompensation {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDDC1Count Getddc1Count {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.IsDeviceConnected IsDeviceConnected {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.IsDRMUnlocked IsDrmUnlocked {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.GetDeviceList GetDeviceList {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.OpenDevice OpenDevice {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.PauseAudioPlayback PauseAudioPlayback {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.PauseDDC1Playback PauseDdc1Playback {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.ResumeAudioPlayback ResumeAudioPlayback {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.ResumeDDC1Playback ResumeDdc1Playback {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetADCNoiseBlanker SetAdcNoiseBlanker {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetADCNoiseBlankerThreshold SetAdcNoiseBlankerThreshold {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetAGC SetAgc {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetAGCParams SetAgcParams {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetAttenuator SetAttenuator {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetAudioFilter SetAudioFilter {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetAudioFilterLength SetAudioFilterLength {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetAudioFilterParams SetAudioFilterParams {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetAudioGain SetAudioGain {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetCallbacks SetCallbacks {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetDDC1 SetDdc1 {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetDDC1Frequency SetDdc1Frequency {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetDDC2Frequency SetDdc2Frequency {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetDDC2NoiseBlanker SetDdc2NoiseBlanker {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetDDC2NoiseBlankerThreshold SetDdc2NoiseBlankerThreshold {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetDemodulatorFilterBandwidth SetDemodulatorFilterBandwidth {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetDemodulatorFilterLength SetDemodulatorFilterLength {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetDemodulatorFilterShift SetDemodulatorFilterShift {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetDemodulatorFrequency SetDemodulatorFrequency {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetDemodulatorMode SetDemodulatorMode {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetDemodulatorParam SetDemodulatorParam {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetDithering SetDithering {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetDRMKey SetDrmKey {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetFrequency SetFrequency {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetGain SetGain {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetInverted SetInverted {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetLED SetLed {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetMaxAGCGain SetMaxAgcGain {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetNotchFilter SetNotchFilter {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetNotchFilterBandwidth SetNotchFilterBandwidth {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetNotchFilterFrequency SetNotchFilterFrequency {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetNotchFilterLength SetNotchFilterLength {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.SetPower SetPower {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.StartAudio StartAudio {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.StartAudioPlayback StartAudioPlayback {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.StartDDC1 StartDdc1 {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.StartDDC1Playback StartDdc1Playback {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.StartDDC2 StartDdc2 {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.StartIF StartIf {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.StopAudio StopAudio {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.StopDDC1 StopDdc1 {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.StopDDC2 StopDdc2 {get;protected set;}
        [UsedImplicitly][ApiCall]
        public  NativeDelegates.StopIF StopIf {get;protected set;}

        public G31DdcApi( string path = null) 
            : base(DLL, path)
        {
            BindApiCalls();
        }
    }
}
