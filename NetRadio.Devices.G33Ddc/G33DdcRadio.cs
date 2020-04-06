using System;
using System.Runtime.InteropServices;
using NetRadio.Devices.Exceptions;

namespace NetRadio.Devices.G3XDdc
{
    public class G33DdcRadio:Radio<G33DdcRadio>,IProviders<G33DdcRadio>
    {
        private readonly G31DdcApi _api=new G31DdcApi();
        private readonly Ddc1 _ddc1;
        private readonly Ddc2[] _ddc2;

        private readonly NativeDefinitions.G31DDC_DDC1_STREAM_CALLBACK _ddc1StreamCallback;
        private readonly NativeDefinitions.G31DDC_CALLBACK _callbacks;
        private readonly GCHandle _callbacksHandle;

        public event EventHandler<IfCallbackArgs> IfDataRecieved;
        internal event EventHandler<Ddc1CallbackArgs> Ddc1DataRecieved;
        internal event EventHandler<Ddc2CallbackArgs> Ddc2DataRecieved;
        internal event EventHandler<Ddc2PreprocessedCallbackArgs> Ddc2PreprocessedDataRecieved;
        internal event EventHandler<AudioCallbackArgs> AudioDataRecieved;
        internal event EventHandler<AudioPlaybackArgs> AudioDataRequested;
        internal event EventHandler<Ddc1PlaybackArgs> ddc1DataRequested; 
 
        private static byte[] CopyArrayByte(IntPtr buffer, uint bufferSize)
        {
            var rawData = new byte[bufferSize];
            Marshal.Copy(buffer, rawData, 0, (int)bufferSize);
            return rawData;
        }

        private static float[] CopyArrayFloat(IntPtr buffer, uint bufferSize)
        {
            var rawData = new float[bufferSize];
            Marshal.Copy(buffer, rawData, 0, (int)bufferSize);
            return rawData;
        }

        private void IfCallback(IntPtr buffer, uint numberOfSamples, ushort maxAdcAmplitude, uint adcSamplingRate,
            uint userData)
        {
            var bytes = numberOfSamples*2;
            var data = CopyArrayByte(buffer, bytes);
            var ifData = new IfCallbackArgs(maxAdcAmplitude, adcSamplingRate, data);

            OnIfDataRecieved(ifData);
        }

        protected void OnIfDataRecieved(IfCallbackArgs ifData)
        {
            if (IfDataRecieved != null)
                IfDataRecieved(this, ifData);
        }

        protected void OnDdc1DataRecieved(Ddc1CallbackArgs ddc1Data)
        {
            if (Ddc1DataRecieved != null)
                Ddc1DataRecieved(this, ddc1Data);
        }

        protected void OnDdc2DataRecieved(Ddc2CallbackArgs ddc2Data)
        {
            if (Ddc2DataRecieved != null)
                Ddc2DataRecieved(this, ddc2Data);
        }

        protected void OnDdc2PreprocessedDataRecieved(Ddc2PreprocessedCallbackArgs ddc2Data)
        {
            if (Ddc2PreprocessedDataRecieved != null)
                Ddc2PreprocessedDataRecieved(this, ddc2Data);
        }

        protected void OnAudioDataRecieved(AudioCallbackArgs audioData)
        {
            if (AudioDataRecieved != null)
                AudioDataRecieved(this, audioData);
        }

        protected void OnAudioDataRequested(AudioPlaybackArgs audioData)
        {
            if (AudioDataRequested != null)
                AudioDataRequested(this, audioData);
        }

        protected void OnDdc1DataRequested(Ddc1PlaybackArgs ddc1Data)
        {
            if (ddc1DataRequested != null)
                ddc1DataRequested(this, ddc1Data);
        }

        private void Ddc1StreamCallback(IntPtr buffer, uint numberOfSamples, uint bitsPerSample, uint userData)
        {
            var size = bitsPerSample/4; //bitspersample/8 *2 (for I/Q pair)
            var bytes = numberOfSamples*size;

            var data =  CopyArrayByte(buffer, bytes);

            OnDdc1DataRecieved(new Ddc1CallbackArgs(bitsPerSample*2,Ddc1().DdcArgs().Info.SampleRate,data));
        }

        private int Ddc1PlaybackStreamCallback(IntPtr buffer, uint numberOfSamples, uint bitsPerSample, uint userData)
        {
            var size = bitsPerSample / 4; //bitspersample/8 *2 (for I/Q pair)
            var bytes = numberOfSamples * size;

            var data = new byte[bytes];


            var e = new Ddc1PlaybackArgs(bitsPerSample*2, Ddc1().DdcArgs().Info.SampleRate, data);

            OnDdc1DataRequested(e);

            return e.End ? 0 : 1;
        }

        private void Ddc2StreamCallback(uint channel, IntPtr buffer, uint numberOfSamples, uint userData)
        {
            var size = numberOfSamples * 2; //32bit float*2 (for I/Q pair)

            var data = CopyArrayFloat(buffer, size);

            OnDdc2DataRecieved(new Ddc2CallbackArgs(32 /*32 bit float *2 (I/Q pair)*/,
                Ddc2(channel).DdcArgs().Info.SampleRate, channel, data));
        }

        private void Ddc2PreprocessedStreamCallback(uint channel, IntPtr buffer, uint numberOfSamples, float sLevelPeak,
            float sLevelRms, uint userData)
        {
            var size = numberOfSamples * 2; //32bit float*2 (for I/Q pair)

            var data = CopyArrayFloat(buffer, size);

            OnDdc2PreprocessedDataRecieved(new Ddc2PreprocessedCallbackArgs(32 /*32 bit float *2 (I/Q pair)*/,
                Ddc2(channel).DdcArgs().Info.SampleRate, channel, data,sLevelPeak,sLevelRms));
        }

        private void AudioStreamCallback(uint channel, IntPtr buffer, IntPtr bufferFiltered, uint numberOfSamples,
            uint userData)
        {
            var data = CopyArrayFloat(buffer, numberOfSamples);
            var filteredData = CopyArrayFloat(bufferFiltered, numberOfSamples);

            OnAudioDataRecieved(new AudioCallbackArgs(channel,data,filteredData));
        }

        private int AudioPlaybackStreamCallback(uint channel, IntPtr buffer, uint numberOfSamples, uint userData)
        {
            var data = new float[numberOfSamples];
            var filteredData = new float[numberOfSamples];

            var e = new AudioPlaybackArgs(channel, data, filteredData);

            OnAudioDataRequested(e);

            return e.End ? 0 : 1;
        }

        internal G31DdcApi Api()
        {
            return _api;
        }

        public G31DdcRadioInfo CachedInfo { get; private set; }

        public bool Connected()
        {
            var res = _api.IsDeviceConnected(GetHandle());
            Check();
            return res;
        }

        public LedMode LedMode()
        {
            uint res = 0;
            Check(_api.GetLed(GetHandle(),ref res));
            return (LedMode) res;
        }

        public G33DdcRadio LedMode(LedMode mode)
        {
            Check(_api.SetLed(GetHandle(),(uint)mode));
            return this;
        }

        public bool TryLedMode(LedMode mode)
        {
            return _api.SetLed(GetHandle(), (uint)mode);
        }

        public bool Power()
        {
            var res =false;
            Check(_api.GetPower(GetHandle(), ref res));
            return res;
        }

        public G33DdcRadio Power(bool power)
        {
            Check(_api.SetPower(GetHandle(), power));
            return this;
        }

        public bool TryPower(bool power)
        {
            return _api.SetPower(GetHandle(), power);
        }

        public AttenuatorValue Attenuator()
        {
            uint res = 0;
            Check(_api.GetAttenuator(GetHandle(), ref res));
            return (AttenuatorValue)res;
        }

        public G33DdcRadio Attenuator(AttenuatorValue value)
        {
            Check(_api.SetAttenuator(GetHandle(), (uint)value));
            return this;
        }

        public bool TryAttenuator(AttenuatorValue value)
        {
            return _api.SetAttenuator(GetHandle(), (uint)value);
        }

        public bool Dithering()
        {
            var res = false;
            Check(_api.GetDithering(GetHandle(), ref res));
            return res;
        }

        public G33DdcRadio Dithering(bool state)
        {
            Check(_api.SetDithering(GetHandle(), state));
            return this;
        }

        public bool TryDithering(bool state)
        {
            return _api.SetDithering(GetHandle(), state);
        }

        public NoiseBlanker AdcNoiseBlanker()
        {
            var enabled = false;
            ushort threshold = 0;

            Check(_api.GetAdcNoiseBlanker(GetHandle(), ref enabled));
            Check(_api.GetAdcNoiseBlankerThreshold(GetHandle(), ref threshold));
            return new NoiseBlanker {Active = enabled, Threshold = threshold};
        }

        public G33DdcRadio AdcNoiseBlanker(bool active)
        {
            Check(_api.SetAdcNoiseBlanker(GetHandle(), active));
            return this;
        }

        public bool TryAdcNoiseBlanker(bool active)
        {
            return _api.SetAdcNoiseBlanker(GetHandle(), active);
        }

        public G33DdcRadio AdcNoiseBlanker(ushort threshold)
        {
            Check(_api.SetAdcNoiseBlankerThreshold(GetHandle(), threshold));
            return this;
        }

        public bool TryAdcNoiseBlanker(ushort threshold)
        {
            return _api.SetAdcNoiseBlankerThreshold(GetHandle(), threshold);
        }

        public G33DdcRadio AdcNoiseBlanker(NoiseBlanker blanker)
        {
            Check(_api.SetAdcNoiseBlanker(GetHandle(), blanker.Active));
            Check(_api.SetAdcNoiseBlankerThreshold(GetHandle(), (ushort)blanker.Threshold));
            return this;
        }

        public bool TryAdcNoiseBlanker(NoiseBlanker blanker)
        {
            var res = _api.SetAdcNoiseBlanker(GetHandle(), blanker.Active);
            res&= _api.SetAdcNoiseBlankerThreshold(GetHandle(), (ushort)blanker.Threshold);
            return res;
        }

        public G33DdcRadio StartIf(ushort period)
        {
            Check(_api.StartIf(GetHandle(), period));
            return this;
        }

        public bool TryStartIf(ushort period)
        {
            return _api.StartIf(GetHandle(),period);
        }

        public G33DdcRadio StoptIf()
        {
            Check(_api.StopIf(GetHandle()));
            return this;
        }

        public bool TryStopIf()
        {
            return _api.StopIf(GetHandle());
        }

        public G33DdcRadio Inverted(bool active)
        {
            Check(_api.SetInverted(GetHandle(), active));
            return this;
        }

        public bool TryInverted(bool active)
        {
            return _api.SetInverted(GetHandle(), active);
        }

        public bool Inverted()
        {
            var res = false;
            Check(_api.GetInverted(GetHandle(), ref res));
            return res;
        }

        public DdcInfo DdcInfo(uint index)
        {
            var size = Marshal.SizeOf(typeof(NativeDefinitions.G3XDDC_DDC_INFO));
            var ptr = Marshal.AllocHGlobal(size);
            try
            {
                Check(_api.GetDdcInfo(GetHandle(), index, ptr));
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

        public Ddc1 Ddc1()
        {
            return _ddc1;
        }

        public DdcInfo Ddc2Info(out uint index)
        {
            index = 0;
            var size = Marshal.SizeOf(typeof(NativeDefinitions.G3XDDC_DDC_INFO));
            var ptr = Marshal.AllocHGlobal(size);
            try
            {
                Check(_api.GetDdc2(GetHandle(), ref index, ptr));
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

        public Ddc2 Ddc2(uint channel)
        {
            return _ddc2[channel];
        }

        public bool TryAbsoluteFrequency(uint frequency, uint ddc2Channel)
        {
            return _api.SetFrequency(GetHandle(), ddc2Channel, frequency);
        }

        public G33DdcRadio AbsoluteFrequency(uint frequency, uint ddc2Channel)
        {
            Check(_api.SetFrequency(GetHandle(),ddc2Channel,frequency));
            return this;
        }

        public uint AbsoluteFrequency(uint ddc2Channel)
        {
            uint frequency = 0;
            Check(_api.GetFrequency(GetHandle(), ddc2Channel, ref frequency));
            return frequency;
        }

        public bool DrmUnlocked()
        {
            return _api.IsDrmUnlocked(GetHandle());
        }

        public bool TryDrmKey(string path)
        {
            return _api.SetDrmKey(GetHandle(), path);
        }

        public G33DdcRadio DrmKey(string path)
        {
            Check(_api.SetDrmKey(GetHandle(),path));
            return this;
        }

        public G31DdcRadioInfo Info()
        {
            var size = Marshal.SizeOf(typeof (NativeDefinitions.G31DDC_DEVICE_INFO));
            var ptr = Marshal.AllocHGlobal(size);

            Check(_api.GetDeviceInfo(GetHandle(), ptr, (uint)size));

            var nativeInfo =
                (NativeDefinitions.G31DDC_DEVICE_INFO)
                    Marshal.PtrToStructure(ptr, typeof (NativeDefinitions.G31DDC_DEVICE_INFO));

            Marshal.FreeHGlobal(ptr);
            return new G31DdcRadioInfo(nativeInfo);
        }

        protected internal void Check(bool res)
        {
            if(!res)
                throw new OperationFailedException("method failed",this);
            Check();
        }
        protected internal void Check()
        {
            var error = Marshal.GetLastWin32Error();
            if (error != 0)
                throw new OperationFailedException("method failed", this);
        }

        protected internal int GetHandle()
        {
            return Handle.ToInt32();
        }

        public override void Dispose()
        {
            _callbacksHandle.Free();
            Check(_api.CloseDevice(GetHandle()));
        }

        public IRadioInfoProvider<G33DdcRadio> InfoProvider()
        {
            return new G33DdcRadioInfoProvider();
        }

        public IRadioProvider<G33DdcRadio> RadioProvider()
        {
            return new G33DdcRadioProvider();
        }

        private G33DdcRadio()
        {
        }

        internal G33DdcRadio(IntPtr handle)
            :this()
        {
            Handle = handle;
            CachedInfo = Info();

            _ddc1 = new Ddc1(this);
            _ddc2 = new[] { new Ddc2(this, 0), new Ddc2(this, 1), new Ddc2(this, 2) };

            _ddc1StreamCallback = Ddc1StreamCallback;

            _callbacks = new NativeDefinitions.G31DDC_CALLBACK
            {
                AudioPlaybackStreamCallback = AudioPlaybackStreamCallback,
                AudioStreamCallback = AudioStreamCallback,
                DDC1PlaybackStreamCallback = Ddc1PlaybackStreamCallback,
                DDC1StreamCallback = _ddc1StreamCallback,
                DDC2PreprocessedStreamCallback = Ddc2PreprocessedStreamCallback,
                DDC2StreamCallback = Ddc2StreamCallback,
                IFCallback = IfCallback
            };

            //_callbacksHandle = GCHandle.Alloc(_callbacks, GCHandleType.Pinned);


            Check(_api.SetCallbacks(GetHandle(), ref _callbacks, IntPtr.Zero));
        }
    }
}
