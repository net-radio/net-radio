using System;
using NetRadio.Signal;

namespace NetRadio.Devices.G3XDdc.Signal
{
    public class G3XDdcDdc1StreamProvider:IAudioProvider
    {
        public event EventHandler<ChunkArgs> DataChunkRecieved;

        private readonly Ddc1 _ddc1;

        public bool SeperateIq { get; private set; }

        public int SamplingRate()
        {
            var iqRate = (int) _ddc1.DdcArgs().Info.SampleRate;
            return SeperateIq ? iqRate : iqRate*2;
        }

        public int Bits()
        {
            return (int)_ddc1.DdcArgs().Info.BitsPerSample;
        }

        public int ChannelCount()
        {
            return SeperateIq ? 2 : 1;
        }

        public void Start()
        {
            _ddc1.DataRecieved+=Ddc1_DataRecieved;
        }

        public void Stop()
        {
            _ddc1.DataRecieved -= Ddc1_DataRecieved;
        }

        private void Ddc1_DataRecieved(object sender, Ddc1CallbackArgs e)
        {
            if (DataChunkRecieved == null)
                return;

            var iqRate = e.SamplingRate;
            iqRate= SeperateIq ? iqRate : iqRate * 2;

            DataChunkRecieved(this, new ChunkArgs(e.Data,iqRate));
        }

        public G3XDdcDdc1StreamProvider(Ddc1 ddc1, bool seperateIq=false)
        {
            _ddc1 = ddc1;
            SeperateIq = seperateIq;
        }
    }
}
