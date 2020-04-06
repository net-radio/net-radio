using System;
using System.Diagnostics;
using System.IO;
using NetRadio.G313.Model;
using NetRadio.G313.ViewModel;

namespace NetRadio.G313.PanelViewModel
{
    class RecordingViewModel:ViewModelBase
    {
        private const string EXTENSION_WAV = ".wav";
        private const string EXTENSION_MP3 = ".mp3";
        private const string EXTENSION_IF = ".if";

        private readonly RadioModel _model;
        private readonly Stopwatch _watch;

        private bool _recordState;
        private bool _recordAudio;
        private bool _recordIf;

        private string _elapsed;

        public string ElapsedValue
        {
            get { return _elapsed; }
            set
            {
                _elapsed = value;
                OnPropertyChanged("ElapsedValue");
            }
        }

        public bool RecordAudioState
        {
            get { return _recordAudio; }
            set
            {
                _recordAudio = value;
                OnPropertyChanged("RecordAudioState");
            }
        }

        public bool RecordIfState
        {
            get { return _recordIf; }
            set
            {
                _recordIf = value;
                OnPropertyChanged("RecordIfState");
            }
        }

        public bool RecordState
        {
            get { return _recordState; }
            set
            {
                _recordState = value;

                if (_recordState)
                    Record();
                else
                    Stop();

                OnPropertyChanged("RecordState");
            }
        }

        public void Tick()
        {
            var time = _watch.Elapsed;
            ElapsedValue = string.Format("{0}:{1}:{2}",time.Hours,time.Minutes,time.Seconds);
        }

        private void Stop()
        {
            _model.RecordAudio.Stop();
            _model.RecordIf.Stop();
            _model.RecordMp3Audio.Stop();
            _model.RecordMp3If.Stop();

            _watch.Stop();
            _watch.Reset();
            ElapsedValue = "00:00:00";
        }

        private void Record()
        {
            SetRecordingFiles();
            _watch.Reset();

            RecordWav();
            RecordMp3();

            if ((RecordAudioState || RecordIfState) && (_model.Settings.RecordWav || _model.Settings.RecordMp3))
                _watch.Start();
            else
                RecordState = false;
        }

        private void RecordWav()
        {
            if (!_model.Settings.RecordWav)
                return;

            if (RecordAudioState)
                _model.RecordAudio.Record(File.Open(_model.AudioFile, FileMode.Create));

            if (RecordIfState)
                _model.RecordIf.Record(File.Open(_model.IfFile, FileMode.Create));
        }
        private void RecordMp3()
        {
            if (!_model.Settings.RecordMp3)
                return;

            if (RecordAudioState)
                _model.RecordMp3Audio.Record(File.Open(_model.AudioMp3File, FileMode.Create));

            if (RecordIfState)
                _model.RecordMp3If.Record(File.Open(_model.IfMp3File, FileMode.Create));
        }

        private void SetRecordingFiles()
        {
            var path = _model.Settings.RecordingPath;
            var time = DateTime.Now;
            var date = string.Format("nr-{0}-{1}-{2}--{3}-{4}-{5}",time.Year,time.Month,time.Day,time.Hour,time.Minute,time.Second);
            _model.IfFile = Path.Combine(path, date + EXTENSION_IF + EXTENSION_WAV);
            _model.AudioFile = Path.Combine(path, date + EXTENSION_WAV);
            _model.IfMp3File = Path.Combine(path, date + EXTENSION_IF + EXTENSION_MP3);
            _model.AudioMp3File = Path.Combine(path, date + EXTENSION_MP3);
        }

        public RecordingViewModel(RadioModel model)
        {
            _model = model;
            _watch = new Stopwatch();

            RecordAudioState = true;
        }
    }
}
