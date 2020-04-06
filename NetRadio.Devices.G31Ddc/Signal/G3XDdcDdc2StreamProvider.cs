using System;
using NetRadio.Signal;
using NetRadio.Signal.Utilities;

namespace NetRadio.Devices.G3XDdc.Signal
{
    public class G3XDdcDdc2StreamProvider:IAudioProvider,IFloatProvider
    {
       public event EventHandler<ChunkArgs> DataChunkRecieved;
       public event EventHandler<FloatArgs> FloatChunkRecieved;

        protected readonly Ddc2 Ddc2;

        public bool FloatProvider { get; set; }
        public bool RawProvider { get; set; }
        public bool SeperateIq { get; private set; }

        public int SamplingRate()
        {
            var iqRate = (int) Ddc2.DdcArgs().Info.SampleRate;
            return SeperateIq ? iqRate : iqRate*2;
        }

        public int Bits()
        {
            return 16;//(int)Ddc2.DdcArgs().Info.BitsPerSample;
        }

        public int ChannelCount()
        {
            return SeperateIq ? 2 : 1;
        }

        public virtual void Start()
        {
            Ddc2.DataRecieved+=Ddc2_DataRecieved;
        }

        public virtual void Stop()
        {
            Ddc2.DataRecieved -= Ddc2_DataRecieved;
        }

        protected void Ddc2_DataRecieved(object sender, Ddc2CallbackArgs e)
        {
            ProvideRawData(e);
            ProvideFloatData(e);
        }

        private void ProvideFloatData(Ddc2CallbackArgs e)
        {
            if (!FloatProvider || FloatChunkRecieved == null)
                return;

            var iqRate = e.SamplingRate;
            iqRate = SeperateIq ? iqRate : iqRate*2;

            FloatChunkRecieved(this, new FloatArgs(e.Data, iqRate));
        }

        private void ProvideRawData(Ddc2CallbackArgs e)
        {
            if (!RawProvider || DataChunkRecieved == null)
                return;

            var iqRate = e.SamplingRate;
            iqRate = SeperateIq ? iqRate : iqRate*2;

            var data = FloatConverter.Float32ToPcm16BufferReady(e.Data);

            DataChunkRecieved(this, new ChunkArgs(data, iqRate));
        }

        public G3XDdcDdc2StreamProvider(Ddc2 ddc2, bool seperateIq=false)
        {
            Ddc2 = ddc2;
            SeperateIq = seperateIq;
            FloatProvider = true;
            RawProvider = false;
        }
    }
}
