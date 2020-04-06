using System;
using System.Windows.Forms;
using System.Windows.Input;
using NetRadio.Devices.G313;
using NetRadio.G313.Model;
using NetRadio.G313.ViewModel;
using NetRadio.Signal.Utilities;

namespace NetRadio.G313.PanelViewModel
{
    class SettingsViewModel:ViewModelBase
    {
        private RadioModel _model;

        public string RecordingPath
        {
            get { return _model.Settings.RecordingPath; }
            set
            {
                _model.Settings.RecordingPath = value;
                OnPropertyChanged("RecordingPath");
            }
        }

        public bool RecordMp3
        {
            get { return _model.Settings.RecordMp3; }
            set
            {
                _model.Settings.RecordMp3 = value;
                OnPropertyChanged("RecordMp3");
            }
        }

        public bool RecordWav
        {
            get { return _model.Settings.RecordWav; }
            set
            {
                _model.Settings.RecordWav = value;
                OnPropertyChanged("RecordWav");
            }
        }

        public uint FastSagcAttack
        {
            get { return (uint)_model.Settings.FastSagc.AttackTime; }
            set
            {
                _model.Settings.FastSagc.AttackTime = value;
                OnPropertyChanged("FastSagcAttack");
            }
        }
        public uint FastSagcDecay
        {
            get { return (uint)_model.Settings.FastSagc.DecayTime; }
            set
            {
                _model.Settings.FastSagc.DecayTime = value;
                OnPropertyChanged("FastSagcDecay");
            }
        }
        public int FastSagcReferenceLevel
        {
            get { return (int)_model.Settings.FastSagc.ReferenceLevel; }
            set
            {
                _model.Settings.FastSagc.ReferenceLevel = value;
                OnPropertyChanged("FastSagcReferenceLevel");
            }
        }
        public uint MediumSagcAttack
        {
            get { return (uint)_model.Settings.MediumSagc.AttackTime; }
            set
            {
                _model.Settings.MediumSagc.AttackTime = value;
                OnPropertyChanged("MediumSagcAttack");
            }
        }
        public uint MediumSagcDecay
        {
            get { return (uint)_model.Settings.MediumSagc.DecayTime; }
            set
            {
                _model.Settings.MediumSagc.DecayTime = value;
                OnPropertyChanged("MediumSagcDecay");
            }
        }
        public int MediumSagcReferenceLevel
        {
            get { return (int)_model.Settings.MediumSagc.ReferenceLevel; }
            set
            {
                _model.Settings.MediumSagc.ReferenceLevel = value;
                OnPropertyChanged("MediumSagcReferenceLevel");
            }
        }

        public uint SlowSagcAttack
        {
            get { return (uint)_model.Settings.SlowSagc.AttackTime; }
            set
            {
                _model.Settings.SlowSagc.AttackTime = value;
                OnPropertyChanged("SlowSagcAttack");
            }
        }
        public uint SlowSagcDecay
        {
            get { return (uint)_model.Settings.SlowSagc.DecayTime; }
            set
            {
                _model.Settings.SlowSagc.DecayTime = value;
                OnPropertyChanged("SlowSagcDecay");
            }
        }
        public int SlowSagcReferenceLevel
        {
            get { return (int)_model.Settings.SlowSagc.ReferenceLevel; }
            set
            {
                _model.Settings.SlowSagc.ReferenceLevel = value;
                OnPropertyChanged("SlowSagcReferenceLevel");
            }
        }

        public uint StartFrequency
        {
            get { return _model.Settings.StartFrequency; }
            set
            {
                _model.Settings.StartFrequency = _model.Limits.Frequency(value);
                OnPropertyChanged("StartFrequency");
            }
        }

        public G313Demodulator.IsbAudioChannels AudioChannel
        {
            get { return _model.Radio.Demodulator().IsbAudioChannel(); }
            set
            {
                _model.Radio.Demodulator().IsbAudioChannel(value);
                OnPropertyChanged("AudioChannel");
            }
        }

        public ICommand CommandAudioPanel { get; private set; }
        public ICommand CommandSaveAndClose { get; private set; }
        public ICommand CommandOpenDialog { get; private set; }
        public ICommand CommandOpenAbout { get; private set; }
        public ICommand CommandOpenDebug { get; private set; }

        public Action WindowCloseAction { get; set; }

        public SettingsViewModel(RadioModel radioModel)
        {
            _model = radioModel;

            CommandOpenDialog=new DelegateCommand(() =>
            {
                var dialog = new FolderBrowserDialog();
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK || result == DialogResult.Yes)
                    RecordingPath = dialog.SelectedPath;
            });

            CommandOpenAbout = new DelegateCommand(() =>
            {
                var about = new AboutWindow(_model);
                about.ShowDialog();
            });

            CommandOpenDebug = new DelegateCommand(() =>
            {
                var debugWindow = new DebugWindow(_model);
                debugWindow.ShowDialog();
            });

            CommandAudioPanel=new DelegateCommand(OsHelpers.OpenPlaybackConfig);

            CommandSaveAndClose = new DelegateCommand(() =>
            {
                _model.Settings.Save();
                WindowCloseAction();
            });
        }
    }
}
