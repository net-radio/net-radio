using System;
using NetRadio.Devices;

namespace NetRadio.Signal
{
    /// <summary>
    /// Represents generic audio stream provider.
    /// </summary>
    public interface IAudioProvider
    {
        /// <summary>
        /// Occures when data is received from underlying stream.
        /// </summary>
        event EventHandler<ChunkArgs> DataChunkRecieved;

        /// <summary>
        /// Gets sampling rate of underlying stream.
        /// </summary>
        /// <returns>Returns sampling rate.</returns>
        int SamplingRate();

        /// <summary>
        /// Gets sampling precision if underlying stream.
        /// </summary>
        /// <returns>Returns precision.</returns>
        int Bits();

        /// <summary>
        /// Gets number of sampled channels through stream.
        /// </summary>
        /// <returns>Returns number of channels.</returns>
        int ChannelCount();

        /// <summary>
        /// Starts providing data from underlying stream.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops underlying data provider.
        /// </summary>
        void Stop();
    }
}
