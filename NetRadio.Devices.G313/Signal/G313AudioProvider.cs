using System;
using NetRadio.Signal;

namespace NetRadio.Devices.G313.Signal
{
    /// <summary>
    /// Provides standard facilities to access WinRadio G313 audio stream
    /// </summary>
    public class G313AudioProvider:IAudioProvider
    {
        /// <summary>
        /// Occures when data received from stream.
        /// </summary>
        public event EventHandler<ChunkArgs> DataChunkRecieved;

        private readonly G313Radio _radio;

        /// <summary>
        /// Gets sampling rate of underlying stream.
        /// </summary>
        /// <returns>Returns sampling rate.</returns>
        public int SamplingRate()
        {
            return (int)_radio.Demodulator().AudioSamplingRate();
        }

        /// <summary>
        /// Gets sampling precision if underlying stream.
        /// </summary>
        /// <returns>Returns precision.</returns>
        public int Bits()
        {
            return 16;
        }

        /// <summary>
        /// Gets number of sampled channels through stream.
        /// </summary>
        /// <returns>Returns number of channels.</returns>
        public int ChannelCount()
        {
            return 2;
        }

        /// <summary>
        /// Starts providing data from underlying stream.
        /// </summary>
        public void Start()
        {
            _radio.Demodulator().AudioChunkRecieved += G313AudioProvider_AudioChunkRecieved;
        }

        void G313AudioProvider_AudioChunkRecieved(object sender, ChunkArgs e)
        {
            if (DataChunkRecieved == null)
                return;

            DataChunkRecieved(this, e);
        }

        /// <summary>
        /// Stops underlying data provider.
        /// </summary>
        public void Stop()
        {
            _radio.Demodulator().AudioChunkRecieved -= G313AudioProvider_AudioChunkRecieved;
        }

        /// <summary>
        /// Creates WinRadio G313 audio provider fro specified radio context.
        /// </summary>
        /// <param name="radio">Radio context.</param>
        public G313AudioProvider(G313Radio radio)
        {
            _radio = radio;
        }
    }
}
