using System;
using NetRadio.Signal;

namespace NetRadio.Devices.G3XDdc.Signal
{
    public class G3XDdcIfProvider:IAudioProvider
    {
        private const int IF_SAMPLERATE = 100000000; //100Mhz

        public event EventHandler<ChunkArgs> DataChunkRecieved;

        private readonly G33DdcRadio _radio;

        public int SamplingRate()
        {
            return IF_SAMPLERATE;
        }

        public int Bits()
        {
            return 16;
        }

        public int ChannelCount()
        {
            return 1;
        }

        public void Start()
        {
            _radio.IfDataRecieved+=Radio_IfDataRecieved;
        }

        public void Stop()
        {
            _radio.IfDataRecieved -= Radio_IfDataRecieved;
        }

        private void Radio_IfDataRecieved(object sender, IfCallbackArgs e)
        {
            if (DataChunkRecieved == null)
                return;

            DataChunkRecieved(this,new ChunkArgs(e.Data, IF_SAMPLERATE));
        }

        public G3XDdcIfProvider(G33DdcRadio radio)
        {
            _radio = radio;
        }
    }
}
