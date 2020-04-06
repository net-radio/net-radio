using System.IO;
using NAudio.Lame;
using NAudio.Wave;

namespace NetRadio.Signal
{
    /// <summary>
    /// Real-Time MP3 recorder based on underlying audio provider.
    /// </summary>
    public class LiveMp3Recorder:LiveAudioRecorder,IAudioRecorder<LiveMp3Recorder>
    {
        private readonly int _mp3BitRate;

        /// <summary>
        /// Creates live audio recorder.
        /// </summary>
        /// <param name="provider">Specified audio provider.</param>
        /// <param name="bitRate">Specified MP3 bit rate.</param>
        public LiveMp3Recorder(IAudioProvider provider, int bitRate = 192000)//512000 lossless for 16000hz 16-bit 2 channel
            : base(provider)
        {
            _mp3BitRate = bitRate;
        }

        protected override Stream CreateWriter(Stream stream, WaveFormat format)
        {
            return new LameMP3FileWriter(stream, format, _mp3BitRate);
        }

        public new LiveMp3Recorder Record(Stream stream)
        {
            return base.Record(stream) as LiveMp3Recorder;
        }

        public new LiveMp3Recorder Stop()
        {
            return base.Stop() as LiveMp3Recorder;
        }
    }
}
