using System;
using System.Diagnostics;
using System.Threading;
using NAudio.Wave;
using NetRadio.Devices;

namespace NetRadio.Signal
{
    /// <summary>
    /// Live Audio player based on underlying audio providers.
    /// </summary>
    public class LiveAudioPlayer:IDisposable,IAudioPlayer<LiveAudioPlayer>
    {
        /// <summary>
        /// Occures when FFT calculation is finished.
        /// </summary>
        public event EventHandler<FftEventArgs> FftCalculated;
        /// <summary>
        /// Occures when maximum intensity is calculated.
        /// </summary>
        public event EventHandler<MaxSampleEventArgs> MaximumCalculated;

        private readonly IAudioProvider _provider;
        private readonly BufferedWaveProvider _buffer;
        private readonly IWavePlayer _waveOut;
        private readonly WaveFormat _format;
        private SampleAggregator _aggregator;
        private readonly ManualResetEvent _bufferLock = new ManualResetEvent(false);
        private float _bufferedLength;
        private readonly int _byteRate;
        private readonly float _preBufferLength;

        /// <summary>
        /// Rasises <see cref="FftCalculated"/>
        /// </summary>
        /// <param name="e">FFT data.</param>
        protected virtual void OnFftCalculated(FftEventArgs e)
        {
            EventHandler<FftEventArgs> handler = FftCalculated;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Rasises <see cref="MaximumCalculated"/>
        /// </summary>
        /// <param name="e">Max sample data.</param>
        protected virtual void OnMaximumCalculated(MaxSampleEventArgs e)
        {
            EventHandler<MaxSampleEventArgs> handler = MaximumCalculated;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Creates Live audio player.
        /// </summary>
        /// <param name="provider">Specified audio provider.</param>
        /// <param name="preBuffer">Audioprebuffer in milli seconds</param>
        /// <param name="fftAnalysis">Determines wheather FFT analyzer is enabled or not.</param>
        public LiveAudioPlayer(IAudioProvider provider, float preBuffer = 0.3F, bool fftAnalysis = false)//default 0.3F as 300 ms
        {
            _provider = provider;
            _preBufferLength = preBuffer;
            
            _format = new WaveFormat(provider.SamplingRate(), provider.Bits(), provider.ChannelCount()); // must match the waveformat of the raw audio
            _waveOut =  new WaveOut(); //new DirectSoundOut(); //new WaveOut();
            _buffer = new BufferedWaveProvider(_format);
            _buffer.DiscardOnBufferOverflow = true;
            
            if (fftAnalysis)
            {
                InitializeAggregator();
                _waveOut.Init(_aggregator);
            }
            else
                _waveOut.Init(_buffer);

            _byteRate = (provider.Bits() / 8) * provider.ChannelCount() * provider.SamplingRate();
            _provider.DataChunkRecieved += AudioChunkRecieved;
        }

        private void InitializeAggregator()
        {
            _aggregator = new SampleAggregator(_buffer.ToSampleProvider());
            _aggregator.NotificationCount = _format.SampleRate/10;
            _aggregator.PerformFft = true;
            _aggregator.FftCalculated += (s, a) => OnFftCalculated(a);
            _aggregator.MaximumCalculated += (s, a) => OnMaximumCalculated(a);
        }

        private void AudioChunkRecieved(object sender, ChunkArgs e)
        {
            _buffer.AddSamples(e.RawData, 0, e.RawData.Length);

            if (_bufferedLength < _preBufferLength)
                _bufferedLength += e.RawData.Length / (float)_byteRate;
            else
                _bufferLock.Set();

        }

        /// <summary>
        /// Gets audio pre buffred length
        /// </summary>
        /// <returns></returns>
        public float PreBufferLength()
        {
            return _preBufferLength;
        }

        /// <summary>
        /// Gets audio bit rate.
        /// </summary>
        /// <returns></returns>
        public int BitRate()
        {
            return _byteRate * 8;
        }

        public LiveAudioPlayer Play()
        {
            try
            {
                _provider.Start();
                _bufferLock.WaitOne();
                _waveOut.Play();
            }
            catch (Exception)
            {
                Debug.Print("Pressure on Play trigger, so many pause and replay!");
            }
            return this;
        }

        public LiveAudioPlayer Stop()
        {
            _waveOut.Stop();
            _provider.Stop();
            _buffer.ClearBuffer();
            _bufferedLength = 0;
            _bufferLock.Reset();

            return this;
        }

        /// <summary>
        /// Sets Master volume of Audio stack.
        /// </summary>
        /// <param name="volume">Specified volume (0.0 - 1.0)</param>
        /// <returns>Return Audio player.</returns>
        public LiveAudioPlayer Volume(float volume)
        {
            // ReSharper disable once CSharpWarnings::CS0618
            _waveOut.Volume = volume;
            return this;
        }

        public float VolumeLevel
        {
            get { return _waveOut.Volume; }
            set { _waveOut.Volume = value; }
        }

        public void Dispose()
        {
            _waveOut.Stop();
            _provider.Stop();
        }
    }
}
