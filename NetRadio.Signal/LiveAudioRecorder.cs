using System;
using System.IO;
using System.Threading;
using NAudio.Wave;
using NetRadio.Devices;

namespace NetRadio.Signal
{
    /// <summary>
    /// Real-Time PCM recorder based on underlying audio provider.
    /// </summary>
    public class LiveAudioRecorder:IDisposable,IAudioRecorder<LiveAudioRecorder>
    {
         private readonly IAudioProvider _provider;
        private Stream _writer;
        private readonly WaveFormat _format;

        private readonly int _bitRate;

        private readonly ManualResetEvent _stopLock=new ManualResetEvent(false);

        /// <summary>
        /// Creates live audio recorder.
        /// </summary>
        /// <param name="provider">Specified audio provider.</param>
        public LiveAudioRecorder(IAudioProvider provider)
        {
            _provider = provider;

            _format = new WaveFormat(provider.SamplingRate(), provider.Bits(), provider.ChannelCount()); // must match the waveformat of the raw audio

            _bitRate = provider.Bits() * provider.ChannelCount() * provider.SamplingRate();
            _provider.DataChunkRecieved += AudioChunkRecieved;
        }

        private void AudioChunkRecieved(object sender, ChunkArgs e)
        {
            _stopLock.WaitOne();
            if (_writer != null)
                _writer.Write(e.RawData, 0, e.RawData.Length);
        }

        /// <summary>
        /// Gets audio bit rate.
        /// </summary>
        /// <returns></returns>
        public int BitRate()
        {
            return _bitRate * 8;
        }

        /// <summary>
        /// Writes data to recorder stream.
        /// </summary>
        /// <param name="stream">Specified recorder steam.</param>
        /// <param name="format">Specified wave format.</param>
        /// <returns>Returns recorder stream.</returns>
        protected virtual Stream CreateWriter(Stream stream, WaveFormat format)
        {
            return new WaveFileWriter(stream, format);
        }

        public LiveAudioRecorder Record(Stream stream)
        {
            _writer = CreateWriter(stream, _format);
            _stopLock.Set();
            _provider.Start();
            return this;
        }

        public LiveAudioRecorder Stop()
        {
            _stopLock.Reset();
            _provider.Stop();
            if (_writer == null)
                return this;
            _writer.Close();
            _writer.Dispose(); //for mp3 recorder
            _writer = null;
            _stopLock.Set();
            return this;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
