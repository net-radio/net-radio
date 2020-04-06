using System.Runtime.InteropServices;

namespace NetRadio.Devices.G3XDdc
{
    public class Demodulator
    {
        private readonly G31DdcApi _api;
        private readonly G31DdcRadio _radio;
        private readonly Ddc2 _ddc2;

        internal Demodulator(Ddc2 ddc2)
        {
            _ddc2 = ddc2;
            _radio = ddc2.Radio();
            _api = _radio.Api();
        }

        public bool TryNotchFilter(bool active)
        {
            return _api.SetNotchFilter(_radio.GetHandle(), _ddc2.Channel(), active);
        }

        public Demodulator NotchFilter(bool active)
        {
            _radio.Check(_api.SetNotchFilter(_radio.GetHandle(), _ddc2.Channel(), active));
            return this;
        }

        public bool TryNotchFilter(int frequency)
        {
            return _api.SetNotchFilterFrequency(_radio.GetHandle(), _ddc2.Channel(), frequency);
        }

        public Demodulator NotchFilter(int frequency)
        {
            _radio.Check(_api.SetNotchFilterFrequency(_radio.GetHandle(), _ddc2.Channel(), frequency));
            return this;
        }

        public bool TryNotchFilter(uint bandwidth)
        {
            return _api.SetNotchFilterBandwidth(_radio.GetHandle(), _ddc2.Channel(), bandwidth);
        }

        public Demodulator NotchFilter(uint bandwidth)
        {
            _radio.Check(_api.SetNotchFilterBandwidth(_radio.GetHandle(), _ddc2.Channel(), bandwidth));
            return this;
        }

        public bool TryNotchFilterLength(uint length)
        {
            return _api.SetNotchFilterLength(_radio.GetHandle(), _ddc2.Channel(), length);
        }

        public Demodulator NotchFilterLength(uint length)
        {
            _radio.Check(_api.SetNotchFilterLength(_radio.GetHandle(), _ddc2.Channel(), length));
            return this;
        }

        public uint NotchFilterLength()
        {
            uint length = 0;
            _radio.Check(_api.GetNotchFilterLength(_radio.GetHandle(), _ddc2.Channel(), ref length));
            return length;
        }

        public NotchFilter NotchFilter()
        {
            int freqeuncy = 0;
            uint bandwidth = 0;
            bool actvie = false;

            _radio.Check(_api.GetNotchFilter(_radio.GetHandle(),_ddc2.Channel(),ref actvie));
            _radio.Check(_api.GetNotchFilterBandwidth(_radio.GetHandle(),_ddc2.Channel(),ref bandwidth));
            _radio.Check(_api.GetNotchFilterFrequency(_radio.GetHandle(),_ddc2.Channel(),ref freqeuncy));

            return new NotchFilter {Active = actvie, Bandwidth = bandwidth, Frequency = freqeuncy};
        }

        public bool TryNotchFilter(NotchFilter filter)
        {
            var res = _api.SetNotchFilter(_radio.GetHandle(), _ddc2.Channel(), filter.Active);
            res &= _api.SetNotchFilterBandwidth(_radio.GetHandle(), _ddc2.Channel(), filter.Bandwidth);
            res &= _api.SetNotchFilterFrequency(_radio.GetHandle(), _ddc2.Channel(), filter.Frequency);

            return res;
        }

        public Demodulator NotchFilter(NotchFilter filter)
        {
            _radio.Check(_api.SetNotchFilter(_radio.GetHandle(), _ddc2.Channel(), filter.Active));
            _radio.Check(_api.SetNotchFilterBandwidth(_radio.GetHandle(), _ddc2.Channel(), filter.Bandwidth));
            _radio.Check(_api.SetNotchFilterFrequency(_radio.GetHandle(), _ddc2.Channel(), filter.Frequency));

            return this;
        }

        public bool TryAgcState(bool active)
        {
            return _api.SetAgc(_radio.GetHandle(), _ddc2.Channel(), active);
        }

        public Demodulator AgcState(bool active)
        {
            _radio.Check(_api.SetAgc(_radio.GetHandle(),_ddc2.Channel(),active));
            return this;
        }

        public bool AgcState()
        {
            bool active = false;
            _radio.Check(_api.GetAgc(_radio.GetHandle(),_ddc2.Channel(),ref active));
            return active;
        }

        public bool TryAgc(SoftwareAgc agc)
        {
            return _api.SetAgcParams(_radio.GetHandle(), _ddc2.Channel(), agc.AttackTime, agc.DecayTime,
                agc.ReferenceLevel);
        }

        public Demodulator Agc(SoftwareAgc agc)
        {
            _radio.Check(_api.SetAgcParams(_radio.GetHandle(), _ddc2.Channel(), agc.AttackTime, agc.DecayTime,
                agc.ReferenceLevel));
            return this;
        }

        public SoftwareAgc Agc()
        {
            double attackTime = 0;
            double decayTime = 0;
            double referenceLevel = 0;

            _radio.Check(_api.GetAgcParams(_radio.GetHandle(),_ddc2.Channel(),ref attackTime,ref decayTime,ref referenceLevel));
            return new SoftwareAgc {AttackTime = attackTime, DecayTime = decayTime, ReferenceLevel = referenceLevel};
        }

        public bool TryMaxAgcGain(double maxGain)
        {
            return _api.SetMaxAgcGain(_radio.GetHandle(), _ddc2.Channel(), maxGain);
        }

        public Demodulator MaxAgcGain(double maxGain)
        {
            _radio.Check(_api.SetMaxAgcGain(_radio.GetHandle(), _ddc2.Channel(), maxGain));
            return this;
        }

        public double MaxAgcGain()
        {
            double maxGain = 0;
            _radio.Check(_api.GetMaxAgcGain(_radio.GetHandle(),_ddc2.Channel(),ref maxGain));

            return maxGain;
        }

        public bool TryGain(double gain)
        {
            return _api.SetGain(_radio.GetHandle(), _ddc2.Channel(), gain);
        }

        public Demodulator Gain(double gain)
        {
            _radio.Check(_api.SetGain(_radio.GetHandle(), _ddc2.Channel(), gain));
            return this;
        }

        public double Gain()
        {
            double gain = 0;
            _radio.Check(_api.GetGain(_radio.GetHandle(),_ddc2.Channel(),ref gain));
            return gain;
        }

        public double CurrentGain()
        {
            double gain = 0;
            _radio.Check(_api.GetCurrentGain(_radio.GetHandle(), _ddc2.Channel(), ref gain));
            return gain;
        }

        public bool TryBandwidth(uint bandwidth)
        {
            return _api.SetDemodulatorFilterBandwidth(_radio.GetHandle(), _ddc2.Channel(), bandwidth);
        }

        public Demodulator Bandwidth(uint bandwidth)
        {
            _radio.Check(_api.SetDemodulatorFilterBandwidth(_radio.GetHandle(), _ddc2.Channel(), bandwidth));
            return this;
        }

        public uint Bandwidth()
        {
            uint bandwidth = 0;

            _radio.Check(_api.GetDemodulatorFilterBandwidth(_radio.GetHandle(),_ddc2.Channel(),ref bandwidth));
            return bandwidth;
        }

        public bool TryShift(int shift)
        {
            return _api.SetDemodulatorFilterShift(_radio.GetHandle(), _ddc2.Channel(), shift);
        }

        public Demodulator Shift(int shift)
        {
            _radio.Check(_api.SetDemodulatorFilterShift(_radio.GetHandle(), _ddc2.Channel(), shift));
            return this;
        }

        public int Shift()
        {
            int shift = 0;

            _radio.Check(_api.GetDemodulatorFilterShift(_radio.GetHandle(), _ddc2.Channel(), ref shift));
            return shift;
        }

        public bool TryFilterLength(uint length)
        {
            return _api.SetDemodulatorFilterLength(_radio.GetHandle(), _ddc2.Channel(), length);
        }

        public Demodulator FilterLength(uint length)
        {
            _radio.Check(_api.SetDemodulatorFilterLength(_radio.GetHandle(), _ddc2.Channel(), length));
            return this;
        }

        public uint FilterLength()
        {
            uint length = 0;

            _radio.Check(_api.GetDemodulatorFilterLength(_radio.GetHandle(), _ddc2.Channel(), ref length));
            return length;
        }

        public bool TryFrequency(int frequency)
        {
            return _api.SetDemodulatorFrequency(_radio.GetHandle(), _ddc2.Channel(), frequency);
        }

        public Demodulator Frequency(int frequency)
        {
            _radio.Check(_api.SetDemodulatorFrequency(_radio.GetHandle(), _ddc2.Channel(), frequency));
            return this;
        }

        public int Frequency()
        {
            int frequency = 0;

            _radio.Check(_api.GetDemodulatorFrequency(_radio.GetHandle(), _ddc2.Channel(), ref frequency));
            return frequency;
        }

        public bool TryMode(DemodulatorMode mode)
        {
            return _api.SetDemodulatorMode(_radio.GetHandle(), _ddc2.Channel(), (uint) mode);
        }

        public Demodulator Mode(DemodulatorMode mode)
        {
            _radio.Check(_api.SetDemodulatorMode(_radio.GetHandle(), _ddc2.Channel(), (uint) mode));
            return this;
        }

        public DemodulatorMode Mode()
        {
            uint mode = 0;
            _radio.Check(_api.GetDemodulatorMode(_radio.GetHandle(),_ddc2.Channel(),ref mode));
            return (DemodulatorMode) mode;
        }

        public bool TryParamsAmsSideband(AmsSideband state)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));
            try
            {
                MarshalHelper.WriteUInt32(ptr, (uint) state);
                return _api.SetDemodulatorParam(_radio.GetHandle(), _ddc2.Channel(),
                    (uint) NativeDefinitions.DemodulatorParameter.G3XDDC_DEMODULATOR_PARAM_AMS_SIDE_BAND, ptr,
                    (uint) Marshal.SizeOf(typeof (uint)));
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public Demodulator ParamsAmsSideband(AmsSideband state)
        {
            _radio.Check(TryParamsAmsSideband(state));
            return this;
        }

        public AmsSideband ParamsAmsSideband()
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));
            try
            {
                _radio.Check(_api.GetDemodulatorParam(_radio.GetHandle(), _ddc2.Channel(),
                    (uint) NativeDefinitions.DemodulatorParameter.G3XDDC_DEMODULATOR_PARAM_AMS_SIDE_BAND, ptr,
                    (uint) Marshal.SizeOf(typeof (uint))));

                var res = MarshalHelper.ReadUInt32(ptr);
                return (AmsSideband) res;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public bool TryParamsAmsCaptureRange(uint value) //50-10000 Hz
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));
            try
            {
                MarshalHelper.WriteUInt32(ptr, value);
                return _api.SetDemodulatorParam(_radio.GetHandle(), _ddc2.Channel(),
                    (uint) NativeDefinitions.DemodulatorParameter.G3XDDC_DEMODULATOR_PARAM_AMS_CAPTURE_RANGE, ptr,
                    (uint) Marshal.SizeOf(typeof (uint)));
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public Demodulator ParamsAmsCaptureRange(uint value)
        {
            _radio.Check(TryParamsAmsCaptureRange(value));
            return this;
        }

        public uint ParamsAmsCaptureRange() //50-10000 Hz
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));

            try
            {
                _radio.Check(_api.GetDemodulatorParam(_radio.GetHandle(), _ddc2.Channel(),
                    (uint) NativeDefinitions.DemodulatorParameter.G3XDDC_DEMODULATOR_PARAM_AMS_CAPTURE_RANGE, ptr,
                    (uint) Marshal.SizeOf(typeof (uint))));

                var res = MarshalHelper.ReadUInt32(ptr);
                return res;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public bool TryParamsCwFrequency(int frequency)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            try
            {
                Marshal.WriteInt32(ptr, frequency);
                return _api.SetDemodulatorParam(_radio.GetHandle(), _ddc2.Channel(),
                    (uint) NativeDefinitions.DemodulatorParameter.G3XDDC_DEMODULATOR_PARAM_CW_FREQUENCY, ptr,
                    (uint) Marshal.SizeOf(typeof (int)));
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public Demodulator ParamsCwFrequency(int frequency)
        {
            _radio.Check(TryParamsCwFrequency(frequency));
            return this;
        }

        public int ParamsCwFrequency()
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            try
            {
                _radio.Check(_api.GetDemodulatorParam(_radio.GetHandle(), _ddc2.Channel(),
                    (uint) NativeDefinitions.DemodulatorParameter.G3XDDC_DEMODULATOR_PARAM_CW_FREQUENCY, ptr,
                    (uint) Marshal.SizeOf(typeof (int))));

                var res = Marshal.ReadInt32(ptr);
                return res;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public bool TryParamsDrmAudioServiceIndex(uint index)//0-4
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));
            try
            {
                MarshalHelper.WriteUInt32(ptr, index);
                return _api.SetDemodulatorParam(_radio.GetHandle(), _ddc2.Channel(),
                    (uint) NativeDefinitions.DemodulatorParameter.G3XDDC_DEMODULATOR_PARAM_DRM_AUDIO_SERVICE, ptr,
                    (uint) Marshal.SizeOf(typeof (uint)));
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public Demodulator ParamsDrmAudioServiceIndex(uint index)
        {
            _radio.Check(TryParamsDrmAudioServiceIndex(index));
            return this;
        }

        public uint ParamsDrmAudioServiceIndex()//0-4
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));
            try
            {
                _radio.Check(_api.GetDemodulatorParam(_radio.GetHandle(), _ddc2.Channel(),
                    (uint) NativeDefinitions.DemodulatorParameter.G3XDDC_DEMODULATOR_PARAM_DRM_AUDIO_SERVICE, ptr,
                    (uint) Marshal.SizeOf(typeof (uint))));

                var res = MarshalHelper.ReadUInt32(ptr);
                return res;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public bool TryParamsDrmMultimediaServiceIndex(uint index)//0-4
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));
            try
            {
                MarshalHelper.WriteUInt32(ptr, index);
                return _api.SetDemodulatorParam(_radio.GetHandle(), _ddc2.Channel(),
                    (uint) NativeDefinitions.DemodulatorParameter.G3XDDC_DEMODULATOR_PARAM_DRM_MULTIMEDIA_SERVICE, ptr,
                    (uint) Marshal.SizeOf(typeof (uint)));
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public Demodulator ParamsDrmMultimediaServiceIndex(uint index)
        {
            _radio.Check(TryParamsDrmMultimediaServiceIndex(index));
            return this;
        }

        public uint ParamsDrmMultimediaServiceIndex() //0-4
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (uint)));
            try
            {
                _radio.Check(_api.GetDemodulatorParam(_radio.GetHandle(), _ddc2.Channel(),
                    (uint) NativeDefinitions.DemodulatorParameter.G3XDDC_DEMODULATOR_PARAM_DRM_MULTIMEDIA_SERVICE, ptr,
                    (uint) Marshal.SizeOf(typeof (uint))));

                var res = MarshalHelper.ReadUInt32(ptr);
                return res;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public bool StateAmsLock()
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte)));
            try
            {
                _radio.Check(_api.GetDemodulatorState(_radio.GetHandle(), _ddc2.Channel(),
                    (uint)NativeDefinitions.DemodulatorState.G3XDDC_DEMODULATOR_STATE_AMS_LOCK, ptr,
                    (uint)Marshal.SizeOf(typeof(byte))));

                var res = Marshal.ReadByte(ptr);
                return res != 0;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public double StateAmsLockedFrequency()
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)));
            try
            {
                _radio.Check(_api.GetDemodulatorState(_radio.GetHandle(), _ddc2.Channel(),
                    (uint)NativeDefinitions.DemodulatorState.G3XDDC_DEMODULATOR_STATE_AMS_FREQUENCY, ptr,
                    (uint)Marshal.SizeOf(typeof(double))));

                var res = MarshalHelper.ReadDouble(ptr);
                return res;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public double StateAmsModulationDepth()//as %
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)));
            try
            {
                _radio.Check(_api.GetDemodulatorState(_radio.GetHandle(), _ddc2.Channel(),
                    (uint)NativeDefinitions.DemodulatorState.G3XDDC_DEMODULATOR_STATE_AM_DEPTH, ptr,
                    (uint)Marshal.SizeOf(typeof(double))));

                var res = MarshalHelper.ReadDouble(ptr);
                return res;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public int StateTuneError()
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            try
            {
                _radio.Check(_api.GetDemodulatorState(_radio.GetHandle(), _ddc2.Channel(),
                    (uint)NativeDefinitions.DemodulatorState.G3XDDC_DEMODULATOR_STATE_TUNE_ERROR, ptr,
                    (uint)Marshal.SizeOf(typeof(int))));

                var res = Marshal.ReadInt32(ptr);
                return res;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public uint StateFmDeviation()
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));
            try
            {
                _radio.Check(_api.GetDemodulatorState(_radio.GetHandle(), _ddc2.Channel(),
                    (uint)NativeDefinitions.DemodulatorState.G3XDDC_DEMODULATOR_STATE_FM_DEVIATION, ptr,
                    (uint)Marshal.SizeOf(typeof(uint))));

                var res = MarshalHelper.ReadUInt32(ptr);
                return res;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public DrmStatus StateDrmStatus()
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeDefinitions.G3XDDC_DRM_STATUS)));
            try
            {
                _radio.Check(_api.GetDemodulatorState(_radio.GetHandle(), _ddc2.Channel(),
                    (uint)NativeDefinitions.DemodulatorState.G3XDDC_DEMODULATOR_STATE_DRM_STATUS, ptr,
                    (uint)Marshal.SizeOf(typeof(NativeDefinitions.G3XDDC_DRM_STATUS))));

                var res = (NativeDefinitions.G3XDDC_DRM_STATUS)Marshal.PtrToStructure(ptr, typeof (NativeDefinitions.G3XDDC_DRM_STATUS));
                return new DrmStatus(res);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public bool TryStartAudio(uint samplesPerBuffer=64)
        {
            return _api.StartAudio(_radio.GetHandle(), _ddc2.Channel(), samplesPerBuffer);
        }

        public Demodulator StartAudio(uint samplesPerBuffer = 64)
        {
            _radio.Check(_api.StartAudio(_radio.GetHandle(), _ddc2.Channel(), samplesPerBuffer));
            return this;
        }

        public bool TryStopAudio()
        {
            return _api.StopAudio(_radio.GetHandle(), _ddc2.Channel());
        }

        public Demodulator StopAudio()
        {
            _radio.Check(_api.StopAudio(_radio.GetHandle(), _ddc2.Channel()));
            return this;
        }

        public bool TryStartAudioPlayback(uint samplesPerBuffer = 64)
        {
            return _api.StartAudioPlayback(_radio.GetHandle(), _ddc2.Channel(), samplesPerBuffer);
        }

        public Demodulator StartAudioPlayback(uint samplesPerBuffer = 64)
        {
            _radio.Check(_api.StartAudioPlayback(_radio.GetHandle(), _ddc2.Channel(), samplesPerBuffer));
            return this;
        }

        public bool TryPauseAudioPlayback()
        {
            return _api.PauseAudioPlayback(_radio.GetHandle(), _ddc2.Channel());
        }

        public Demodulator PauseAudio()
        {
            _radio.Check(_api.PauseAudioPlayback(_radio.GetHandle(), _ddc2.Channel()));
            return this;
        }

        public bool TryResumeAudioPlayback()
        {
            return _api.ResumeAudioPlayback(_radio.GetHandle(), _ddc2.Channel());
        }

        public Demodulator ResumeAudio()
        {
            _radio.Check(_api.ResumeAudioPlayback(_radio.GetHandle(), _ddc2.Channel()));
            return this;
        }

        public bool TryAudioGain(double gain)
        {
            return _api.SetAudioGain(_radio.GetHandle(), _ddc2.Channel(), gain);
        }

        public Demodulator AudioGain(double gain)
        {
            _radio.Check(_api.SetAudioGain(_radio.GetHandle(),_ddc2.Channel(),gain));
            return this;
        }

        public double AudioGain()
        {
            double gain = 0;
            _radio.Check(_api.GetAudioGain(_radio.GetHandle(),_ddc2.Channel(),ref gain));
            return gain;
        }

        public bool TryAudioFilterState(bool active)
        {
            return _api.SetAudioFilter(_radio.GetHandle(), _ddc2.Channel(), active);
        }

        public Demodulator AudioFilterState(bool active)
        {
            _radio.Check(_api.SetAudioFilter(_radio.GetHandle(), _ddc2.Channel(), active));
            return this;
        }

        public bool AudioFilterState()
        {
            bool active = false;
            _radio.Check(_api.GetAudioFilter(_radio.GetHandle(),_ddc2.Channel(),ref active));
            return active;
        }

        public bool TryAudioFilterLength(uint length)
        {
            return _api.SetAudioFilterLength(_radio.GetHandle(), _ddc2.Channel(), length);
        }

        public Demodulator AudioFilterLength(uint length)
        {
            _radio.Check(_api.SetAudioFilterLength(_radio.GetHandle(), _ddc2.Channel(), length));
            return this;
        }

        public uint AudioFilterLength()
        {
            uint length = 0;
            _radio.Check(_api.GetAudioFilterLength(_radio.GetHandle(), _ddc2.Channel(),ref length));
            return length;
        }

        public bool TryAudioFilter(AudioFilter filter)
        {
            return _api.SetAudioFilterParams(_radio.GetHandle(), _ddc2.Channel(), filter.CutOffLow, filter.CutOffHigh,
                filter.DeEmphasis);
        }

        public Demodulator AudioFilter(AudioFilter filter)
        {
            _radio.Check(_api.SetAudioFilterParams(_radio.GetHandle(), _ddc2.Channel(), filter.CutOffLow,
                filter.CutOffHigh,
                filter.DeEmphasis));
            return this;
        }

        public AudioFilter AudioFilter()
        {
            uint cutOffLow = 0;
            uint cutOffHigh = 0;
            double deEmphasis = 0;

            _radio.Check(_api.GetAudioFilterParams(_radio.GetHandle(), _ddc2.Channel(), ref cutOffLow, ref cutOffHigh,
                ref deEmphasis));
            return new AudioFilter {CutOffHigh = cutOffHigh, CutOffLow = cutOffLow, DeEmphasis = deEmphasis};
        }
    }
}
