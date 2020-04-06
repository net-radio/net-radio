using System;
using System.Runtime.InteropServices;
namespace NetRadio.Devices.G3XDdc
{
    public class Ddc2
    {
        private readonly G31DdcApi _api;
        private readonly G31DdcRadio _radio;
        private readonly uint _channel;
        private readonly Demodulator _demodulator;

        private DdcArgs _ddcInfo;

        public event EventHandler<DdcArgs> DdcChanged;
        public event EventHandler<Ddc2CallbackArgs> DataRecieved;
        public event EventHandler<Ddc2PreprocessedCallbackArgs> PreprocessedDataRecieved;
        public event EventHandler<AudioCallbackArgs> AudioDataRecieved;

        internal Ddc2(G31DdcRadio radio, uint channel)
        {
            _api = radio.Api();
            _radio = radio;
            _channel = channel;
            _demodulator = new Demodulator(this);

            UpdateDdcArgs();

            radio.Ddc1().DdcChanged += (s, e) => UpdateDdcArgs();

            radio.Ddc2DataRecieved += (s, e) => OnDataRecieved(e);
            radio.Ddc2PreprocessedDataRecieved += (s, e) => OnPreprocessedDataRecieved(e);
            radio.AudioDataRecieved += (s, e) => OnAudioDataRecieved(e);
        }

        public Demodulator Demodulator()
        {
            return _demodulator;
        }

        public DdcArgs DdcArgs()
        {
            return _ddcInfo;
        }

        protected void OnDataRecieved(Ddc2CallbackArgs ddc2Data)
        {
            if (ddc2Data.Channel != _channel)
                return;

            if (DataRecieved != null)
                DataRecieved(this, ddc2Data);
        }

        protected void OnPreprocessedDataRecieved(Ddc2PreprocessedCallbackArgs ddc2Data)
        {
            if (ddc2Data.Channel != _channel)
                return;

            if (PreprocessedDataRecieved != null)
                PreprocessedDataRecieved(this, ddc2Data);
        }

        protected void OnAudioDataRecieved(AudioCallbackArgs audioData)
        {
            if (audioData.Channel != _channel)
                return;

            if (AudioDataRecieved != null)
                AudioDataRecieved(this, audioData);
        }

        protected void OnDdcChanged()
        {
            if (DdcChanged == null)
                return;

            DdcChanged(this, _ddcInfo);
        }

        protected void UpdateDdcArgs()
        {
            uint index;
            var info = Ddc(out index);
            _ddcInfo = new DdcArgs(index, info);

            OnDdcChanged();
        }

        public uint Channel()
        {
            return _channel;
        }

        public G31DdcRadio Radio()
        {
            return _radio;
        }

        public bool TryFrequency(int frequency)
        {
            return _api.SetDdc2Frequency(_radio.GetHandle(), _channel, frequency);
        }

        public Ddc2 Frequency(int frequency)
        {
            _radio.Check(_api.SetDdc2Frequency(_radio.GetHandle(), _channel, frequency));
            return this;
        }

        public int Frequency()
        {
            int res = 0;
            _radio.Check(_api.GetDdc2Frequency(_radio.GetHandle(), _channel, ref res));
            return res;
        }

        public bool TryStart(uint samplesPerBuffer = 64)
        {
            return _api.StartDdc2(_radio.GetHandle(), _channel, samplesPerBuffer);
        }

        public Ddc2 Start(uint sampledPerBuffer = 64)
        {
            _radio.Check(_api.StartDdc2(_radio.GetHandle(), _channel, sampledPerBuffer));
            return this;
        }

        public bool TryStop()
        {
            return _api.StopDdc2(_radio.GetHandle(), _channel);
        }

        public Ddc2 Stop()
        {
            _radio.Check(_api.StopDdc2(_radio.GetHandle(), _channel));
            return this;
        }

        public NoiseBlanker NoiseBlanker()
        {
            var enabled = false;
            double threshold = 0;

            _radio.Check(_api.GetDdc2NoiseBlanker(_radio.GetHandle(), _channel, ref enabled));
            _radio.Check(_api.GetDdc2NoiseBlankerThreshold(_radio.GetHandle(), _channel, ref threshold));
            return new NoiseBlanker { Active = enabled, Threshold = threshold };
        }

        public Ddc2 NoiseBlanker(bool active)
        {
            _radio.Check(_api.SetDdc2NoiseBlanker(_radio.GetHandle(), _channel, active));
            return this;
        }

        public bool TryNoiseBlanker(bool active)
        {
            return _api.SetDdc2NoiseBlanker(_radio.GetHandle(), _channel, active);
        }

        public Ddc2 NoiseBlanker(double threshold)
        {
            _radio.Check(_api.SetDdc2NoiseBlankerThreshold(_radio.GetHandle(), _channel, threshold));
            return this;
        }

        public bool TryNoiseBlanker(double threshold)
        {
            return _api.SetDdc2NoiseBlankerThreshold(_radio.GetHandle(), _channel, threshold);
        }

        public Ddc2 NoiseBlanker(NoiseBlanker blanker)
        {
            _radio.Check(_api.SetDdc2NoiseBlanker(_radio.GetHandle(), _channel, blanker.Active));
            _radio.Check(_api.SetDdc2NoiseBlankerThreshold(_radio.GetHandle(), _channel, blanker.Threshold));
            return this;
        }

        public bool TryNoiseBlanker(NoiseBlanker blanker)
        {
            var res = _api.SetDdc2NoiseBlanker(_radio.GetHandle(), _channel, blanker.Active);
            res &= _api.SetDdc2NoiseBlankerThreshold(_radio.GetHandle(), _channel, blanker.Threshold);
            return res;
        }

        public double NoiseBlankerExcessValue()
        {
            double res = 0;
            _radio.Check(_api.GetDdc2NoiseBlankerExcessValue(_radio.GetHandle(), _channel, ref res));
            return res;
        }

        public SignalLevel SignalLevel()
        {
            float peak = 0;
            float rms = 0;

            _radio.Check(_api.GetSignalLevel(_radio.GetHandle(), _channel, ref peak, ref rms));
            return new SignalLevel(peak, rms);
        }

        public DdcInfo Ddc(out uint index)
        {
            index = 0;
            var size = Marshal.SizeOf(typeof(NativeDefinitions.G3XDDC_DDC_INFO));
            var ptr = Marshal.AllocHGlobal(size);
            try
            {
                _radio.Check(_api.GetDdc2(_radio.GetHandle(), ref index, ptr));
                var nativeInfo =
                    (NativeDefinitions.G3XDDC_DDC_INFO)
                        Marshal.PtrToStructure(ptr, typeof(NativeDefinitions.G3XDDC_DDC_INFO));
                return new DdcInfo(nativeInfo);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
