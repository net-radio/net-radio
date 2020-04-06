using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRadio.Devices.G3XDdc.Annotations;
using NetRadio.Signal;
using NetRadio.Signal.Utilities;

namespace NetRadio.Devices.G3XDdc.Signal
{
    public class G3XDdcAudioProvider:IAudioProvider
    {
        public event EventHandler<ChunkArgs> DataChunkRecieved;

        private readonly Ddc2 _ddc2;

        public bool UseFilteredDate { get; set; }
        public bool RightChannel { get; set; }
        public bool LeftChannel { get; set; }

        public void Mute()
        {
            RightChannel = false;
            LeftChannel = false;
        }

        public void Unmute(bool left = true, bool right = true)
        {
            RightChannel = right;
            LeftChannel = left;
        }

        public int SamplingRate()
        {
            return 48000; //(int)_ddc2.DdcArgs().Info.SampleRate;
        }

        public int Bits()
        {
            return 16;
        }

        public int ChannelCount()
        {
            return 2; //changed from 1 to support virtual stereo for ISB/DSB
        }

        public void Start()
        {
            _ddc2.AudioDataRecieved+=Ddc2_AudioDataRecieved;
        }

        public void Stop()
        {
            _ddc2.AudioDataRecieved -= Ddc2_AudioDataRecieved;
        }

        private void Ddc2_AudioDataRecieved(object sender, AudioCallbackArgs e)
        {
            if (DataChunkRecieved == null)
                return;

            var data = UseFilteredDate
                ? FloatConverter.Float32ToPcm16VirtualStereoBufferReady(e.FilteredData, LeftChannel, RightChannel)
                : FloatConverter.Float32ToPcm16VirtualStereoBufferReady(e.Data, LeftChannel, RightChannel);

            DataChunkRecieved(this, new ChunkArgs(data, e.SamplingRate));
        }

        public G3XDdcAudioProvider(Ddc2 ddc2)
        {
            _ddc2 = ddc2;

            RightChannel = true;
            LeftChannel = true;
        }
    }
}
