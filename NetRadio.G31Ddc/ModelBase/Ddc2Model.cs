using System.IO;
using System.Threading.Tasks;
using NetRadio.Devices;
using NetRadio.Devices.G3XDdc;
using NetRadio.Devices.G3XDdc.Signal;
using NetRadio.Signal;

namespace NetRadio.G31Ddc
{
    public class Ddc2Model
    {
        private G3XDdcDdc2StreamProvider _ddc2Provider;
        private readonly G3XDdcAudioProvider _audioProvider;
        private readonly G3XDdcAudioProvider _recordProvider;

        public LiveFftAnalyzer FftAnalyzerDdc2 { get; private set; }
        public LiveAudioPlayer AudioPlayer { get; private set; }
        public LiveAudioRecorder AudioRecorder { get; private set; }

        public G31DdcRadio Radio { get; private set; }
        public uint Index { get; private set; }

        public uint AbsoluteFrequency
        {
            get { return Radio.AbsoluteFrequency(Index); }
            set { Radio.AbsoluteFrequency(value, Index); }
        }

        public int Ddc2Frequency
        {
            get { return Radio.Ddc2(Index).Frequency(); }
            set { Radio.Ddc2(Index).Frequency(value); }
        }

        public NoiseBlanker NoiseBlanker
        {
            get { return Radio.Ddc2(Index).NoiseBlanker(); }
            set { Radio.Ddc2(Index).NoiseBlanker(value); }
        }

        public DemodulatorMode DemodulatorMode
        {
            get { return Radio.Ddc2(Index).Demodulator().Mode(); }
            set { Radio.Ddc2(Index).Demodulator().Mode(value); }
        }

        public uint DemodulatorBandwidth
        {
            get { return Radio.Ddc2(Index).Demodulator().Bandwidth(); }
            set { Radio.Ddc2(Index).Demodulator().Bandwidth(value); }
        }

        public int DemodulatorFrequency
        {
            get { return Radio.Ddc2(Index).Demodulator().Frequency(); }
            set { Radio.Ddc2(Index).Demodulator().Frequency(value); }
        }

        public NotchFilter NotchFilter
        {
            get { return Radio.Ddc2(Index).Demodulator().NotchFilter(); }
            set { Radio.Ddc2(Index).Demodulator().NotchFilter(value); }
        }

        public bool AgcActive
        {
            get { return Radio.Ddc2(Index).Demodulator().AgcState(); }
            set { Radio.Ddc2(Index).Demodulator().AgcState(value); }
        }

        public SoftwareAgc Agc
        {
            get { return Radio.Ddc2(Index).Demodulator().Agc(); }
            set { Radio.Ddc2(Index).Demodulator().Agc(value); }
        }

        public double MaxAgcGain
        {
            get { return Radio.Ddc2(Index).Demodulator().MaxAgcGain(); }
            set { Radio.Ddc2(Index).Demodulator().MaxAgcGain(value); }
        }

        public SignalLevel Signal
        {
            get { return Radio.Ddc2(Index).SignalLevel(); }
        }

        public double Gain
        {
            get { return Radio.Ddc2(Index).Demodulator().Gain(); }
            set { Radio.Ddc2(Index).Demodulator().Gain(value); }
        }

        public double AudioGain
        {
            get { return Radio.Ddc2(Index).Demodulator().AudioGain(); }
            set { Radio.Ddc2(Index).Demodulator().AudioGain(value); }
        }

        public bool AudioFilterActive
        {
            get { return Radio.Ddc2(Index).Demodulator().AudioFilterState(); }
            set { Radio.Ddc2(Index).Demodulator().AudioFilterState(value); }
        }

        public AudioFilter AudioFilter
        {
            get { return Radio.Ddc2(Index).Demodulator().AudioFilter(); }
            set { Radio.Ddc2(Index).Demodulator().AudioFilter(value); }
        }

        public void Start()
        {
            Radio.Ddc2(Index).Start(4096);
            Radio.Ddc2(Index).Demodulator().StartAudio(512);
        }

        public void Stop()
        {
            Radio.Ddc2(Index).Stop();
            Radio.Ddc2(Index).Demodulator().StopAudio();
        }

        public Ddc2Model(G31DdcRadio radio, uint index)
        {
            Radio = radio;
            Index = index;

            _ddc2Provider = new G3XDdcDdc2StreamProvider(Radio.Ddc2(Index));
            _audioProvider = new G3XDdcAudioProvider(Radio.Ddc2(Index)) { UseFilteredDate = true };
            _recordProvider = new G3XDdcAudioProvider(Radio.Ddc2(Index)) { UseFilteredDate = true };

            FftAnalyzerDdc2 = new LiveFftAnalyzer(_ddc2Provider, RadioModel.DDC2_FFT, false) { ForceInstantFft = false };
        }

        public void Mute()
        {
            _audioProvider.Mute();
        }

        public void Unmute(bool left = true, bool right = true)
        {
            _audioProvider.Unmute(left, right);
        }

        public void PlayStart()
        {
            AudioPlayer = new LiveAudioPlayer(_audioProvider);
            new Task(() => AudioPlayer.Play()).Start();
        }

        public void PlayStop()
        {
            if (AudioPlayer == null)
                return;

            AudioPlayer.Stop();
            AudioPlayer.Dispose();
            AudioPlayer = null;
        }

        public void RecordStart()
        {
            AudioRecorder = new LiveAudioRecorder(_recordProvider);
            var file = File.Open("audio.wav", FileMode.Create);

            new Task(() => AudioRecorder.Record(file)).Start();
        }

        public void RecordStop()
        {
            if (AudioRecorder == null)
                return;

            AudioRecorder.Stop();
            AudioRecorder.Dispose();
        }
    }
}
