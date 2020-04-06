using System;
using System.Runtime.InteropServices;

namespace NetRadio.Devices.G3XDdc
{
    public class Ddc1
    {
        private readonly G31DdcApi _api;
        private readonly G31DdcRadio _radio;
        private DdcArgs _ddcInfo;

        public event EventHandler<DdcArgs> DdcChanged;
        public event EventHandler<Ddc1CallbackArgs> DataRecieved;
 
        internal Ddc1(G31DdcRadio radio)
        {
            _api = radio.Api();
            _radio = radio;

            UpdateDdcArgs();

            _radio.Ddc1DataRecieved += (s, e) => OnDataRecieved(e);
        }

        public DdcArgs DdcArgs()
        {
            return _ddcInfo;
        }

        protected void UpdateDdcArgs()
        {
            uint index;
            var info = Ddc(out index);
            _ddcInfo= new DdcArgs(index, info);

            OnDdcChanged();
        }

        protected void OnDataRecieved(Ddc1CallbackArgs ddc1Data)
        {
            if (DataRecieved != null)
                DataRecieved(this, ddc1Data);
        }

        protected void OnDdcChanged()
        {
            if (DdcChanged == null)
                return;
          
            DdcChanged(this, _ddcInfo);
        }

        public G31DdcRadio Radio()
        {
            return _radio;
        }

        public uint DdcCount()
        {
            uint res = 0;
            _radio.Check(_api.Getddc1Count(_radio.GetHandle(), ref res));
            return res;
        }

        public bool TryDdc(uint index)
        {
            var res=TryStop();
            res= _api.SetDdc1(_radio.GetHandle(), index) && res;

            UpdateDdcArgs();

            return res;
        }

        public Ddc1 Ddc(uint index)
        {
            _radio.Check(TryDdc(index));
            return this;
        }

        public DdcInfo Ddc(out uint index)
        {
            index = 0;
            var size = Marshal.SizeOf(typeof(NativeDefinitions.G3XDDC_DDC_INFO));
            var ptr = Marshal.AllocHGlobal(size);
            try
            {
                _radio.Check(_api.GetDdc1(_radio.GetHandle(), ref index, ptr));
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

        public bool TryFrequency(uint frequency)
        {
            return _api.SetDdc1Frequency(_radio.GetHandle(), frequency);
        }

        public Ddc1 Frequency(uint frequency)
        {
            _radio.Check(_api.SetDdc1Frequency(_radio.GetHandle(), frequency));
            return this;
        }

        public uint Frequency()
        {
            uint res = 0;
            _radio.Check(_api.GetDdc1Frequency(_radio.GetHandle(), ref res));
            return res;
        }

        public bool TryStart(uint samplesPerBuffer = 64)
        {
            return _api.StartDdc1(_radio.GetHandle(), samplesPerBuffer);
        }

        public Ddc1 Start(uint sampledPerBuffer = 64)
        {
            _radio.Check(_api.StartDdc1(_radio.GetHandle(), sampledPerBuffer));
            return this;
        }

        public bool TryStop()
        {
            return _api.StopDdc1(_radio.GetHandle());
        }

        public Ddc1 Stop()
        {
            _radio.Check(_api.StopDdc1(_radio.GetHandle()));
            return this;
        }

        public bool TryStartPlayback(uint samplesPerBuffer = 64,
            IqBitsPerSample bitsPerSample = IqBitsPerSample.Default)
        {
            return _api.StartDdc1Playback(_radio.GetHandle(), samplesPerBuffer, (uint)bitsPerSample);
        }

        public Ddc1 StartPlayback(uint samplesPerBuffer = 64,
            IqBitsPerSample bitsPerSample = IqBitsPerSample.Default)
        {
            _radio.Check(_api.StartDdc1Playback(_radio.GetHandle(), samplesPerBuffer, (uint)bitsPerSample));
            return this;
        }

        public bool TryPausePlayback()
        {
            return _api.PauseDdc1Playback(_radio.GetHandle());
        }

        public Ddc1 PauseDdc1Playback()
        {
            _radio.Check(_api.PauseDdc1Playback(_radio.GetHandle()));
            return this;
        }

        public bool TryResumeDdc1Playback()
        {
            return _api.ResumeDdc1Playback(_radio.GetHandle());
        }

        public Ddc1 ResumeDdc1Playback()
        {
            _radio.Check(_api.ResumeDdc1Playback(_radio.GetHandle()));
            return this;
        }
    }
}
