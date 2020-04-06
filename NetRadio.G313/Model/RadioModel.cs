using System;
using System.Linq;
using NetRadio.Devices;
using NetRadio.Devices.G313;
using NetRadio.Devices.G313.Signal;
using NetRadio.G313.Utilties.G313;
using NetRadio.Signal;

namespace NetRadio.G313.Model
{
    class RadioModel : IDisposable
    {
        public Memorizer Memory { get; private set; }
        public DebugModel Debug { get;private set; }
        public SettingsModel Settings { get;private set; }
        public G313RadioLimits Limits { get; private set; }
        public RadioInfo[] AvailableRadio { get; private set; }
        public G313Radio Radio { get; private set; }
        public string AudioFile { get; set; }
        public string IfFile { get; set; }
        public string AudioMp3File { get; set; }
        public string IfMp3File { get; set; }

        public LiveAudioPlayer LiveAudio { get; set; }
        public LiveAudioPlayer LiveIf { get; set; }
        public LiveAudioRecorder RecordAudio { get; set; }
        public LiveAudioRecorder RecordIf { get; set; }
        public LiveMp3Recorder RecordMp3Audio { get; set; }
        public LiveMp3Recorder RecordMp3If { get; set; }

        public LiveFftAnalyzer FftAnalyzer { get; set; }
        public void Initialize(int index)
        {
            if (AvailableRadio == null || AvailableRadio.Length == 0)
                return;

            var info = AvailableRadio[index];
            Radio = info.Open<G313Radio>();
            Limits=new G313RadioLimits(Radio);
            Debug = new DebugModel(Radio);

            IfFile = "If.wav";
            AudioFile = "audio.wav";

            IfMp3File = "If.mp3";
            AudioMp3File = "audio.mp3";

            //var scanner = Radio.BlockScanner();
            //scanner.FrequencyScanned += (s, ee) => Debug.Print("frequency:{0}, power:{1}, raw:{2}", ee.Frequency, (ee.Strength * 1000 / 255 - 1300) / 10, ee.Strength);
            //scanner.ScanFinished += (s, ee) => Debug.Print("finished.");
        }

        public void InitializeStreams()
        {
            LiveAudio = new LiveAudioPlayer(new G313AudioProvider(Radio), fftAnalysis: false, preBuffer:Settings.AudioBuffer);
            LiveIf = new LiveAudioPlayer(new G313IfProvider(Radio), fftAnalysis: false, preBuffer: Settings.AudioBuffer);

            RecordAudio = new LiveAudioRecorder(new G313AudioProvider(Radio));
            RecordIf = new LiveAudioRecorder(new G313IfProvider(Radio));

            RecordMp3Audio = new LiveMp3Recorder(new G313AudioProvider(Radio));
            RecordMp3If = new LiveMp3Recorder(new G313IfProvider(Radio));

            FftAnalyzer = new LiveFftAnalyzer(new G313IfProvider(Radio));
        }

        public void Dispose()
        {
            if (Radio == null)
                return;

            StopStreams();
            Radio.Dispose();
        }

        public void StopStreams()
        {
            LiveAudio.Dispose();
            LiveIf.Dispose();

            RecordAudio.Dispose();
            RecordIf.Dispose();

            RecordMp3Audio.Dispose();
            RecordMp3If.Dispose();

            FftAnalyzer.Dispose();
        }

        public void PauseStreams()
        {
            LiveAudio.Stop();
            LiveIf.Stop();

            RecordAudio.Stop();
            RecordIf.Stop();

            RecordMp3Audio.Stop();
            RecordMp3If.Stop();

            FftAnalyzer.Stop();
        }

        public void ResumeStreams()
        {
            LiveAudio.Play();
            //LiveIf.Play();

            FftAnalyzer.Start();
    
        }

        public RadioModel()
        {
            AvailableRadio = Radio<G313Radio>.Find<G313RadioInfoProvider>().List().ToArray();
            Limits = new G313RadioLimits(null);

            Settings = SettingsModel.LoadOrCreate();
            Settings.Initialize();
            Memory = Memorizer.LoadOrCreate();

            Debug=new DebugModel(null);
        }

        public bool VideoFilter { get; set; }

        public int VideoPoints { get; set; }
    }
}
