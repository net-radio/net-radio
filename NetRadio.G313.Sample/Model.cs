using System;
using System.Diagnostics;
using System.Linq;
using NetRadio.Devices;
using NetRadio.Devices.G313;
using NetRadio.Devices.G313.Signal;
using NetRadio.Signal;

namespace NetRadio.G313.Sample
{
    class Model:IDisposable
    {
        public  BasicRadioInfo[] AvailableRadio { get; private set; } 
        public  G313Radio Radio { get; private set; }
        public  string AudioFile { get; set; }
        public  string IfFile { get; set; }
        public string AudioMp3File { get; set; }
        public string IfMp3File { get; set; }

        public bool VideoFilter { get; set; }
        public int VideoPoints { get; set; }

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
            Radio=info.Open<G313Radio>();

            IfFile = "If.wav";
            AudioFile = "audio.wav";

            IfMp3File = "If.mp3";
            AudioMp3File = "audio.mp3";

            var scanner=Radio.BlockScanner();
            scanner.FrequencyScanned += (s, ee) => Debug.Print("frequency:{0}, power:{1}, raw:{2}", ee.Frequency, (ee.Strength * 1000 / 255 - 1300) / 10, ee.Strength);
            scanner.ScanFinished += (s, ee) => Debug.Print("finished.");
        }

        public void InitializeStreams()
        {
            LiveAudio = new LiveAudioPlayer(new G313AudioProvider(Radio),fftAnalysis:false);
            LiveIf = new LiveAudioPlayer(new G313IfProvider(Radio),fftAnalysis:false);

            RecordAudio = new LiveAudioRecorder(new G313AudioProvider(Radio));
            RecordIf = new LiveAudioRecorder(new G313IfProvider(Radio));

            RecordMp3Audio = new LiveMp3Recorder(new G313AudioProvider(Radio));
            RecordMp3If = new LiveMp3Recorder(new G313IfProvider(Radio));

            FftAnalyzer=new LiveFftAnalyzer(new G313IfProvider(Radio));
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
        public Model()
        {
            AvailableRadio = Radio<G313Radio>.Find().ListAll().ToArray();
        }
    }
}
