using NetRadio.G31Ddc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetRadio.G31Ddc.PanelViewModel
{
    class SettingsViewModel : ViewModelBase
    {
        private readonly RadioModel _model;

        public SettingsViewModel(RadioModel model)
        {
            this._model = model;

            CommandSaveAndClose = new DelegateCommand(() =>
            {
                _model.Settings.Save();
                WindowCloseAction();
            });
        }

        public Action WindowCloseAction { get; set; }

        public ICommand CommandSaveAndClose { get; private set; }

        public string RecordingPath
        {
            get { return _model.Settings.RecordingPath; }
            set
            {
                _model.Settings.RecordingPath = value;
                NotifyPropertyChanged("RecordingPath");
            }
        }

        public double IfMinimumIntensityDb
        {
            get { return _model.Settings.IfParameter.MinimumIntensityDb; }
            set
            {
                _model.Settings.IfParameter.MinimumIntensityDb = value;
                NotifyPropertyChanged("IfMinimumIntensityDb");
            }
        }

        public double IfCalibrationAmplitude
        {
            get { return _model.Settings.IfParameter.CalibrationAmplitude; }
            set
            {
                _model.Settings.IfParameter.CalibrationAmplitude = value;
                NotifyPropertyChanged("IfCalibrationAmplitude");
            }
        }

        public int IfCalibrationOffset
        {
            get { return _model.Settings.IfParameter.CalibrationOffset; }
            set
            {
                _model.Settings.IfParameter.CalibrationOffset = value;
                NotifyPropertyChanged("IfCalibrationOffset");
            }
        }

        public double Ddc1MinimumIntensityDb
        {
            get { return _model.Settings.Ddc1Parameter.MinimumIntensityDb; }
            set
            {
                _model.Settings.Ddc1Parameter.MinimumIntensityDb = value;
                NotifyPropertyChanged("Ddc1MinimumIntensityDb");
            }
        }

        public double Ddc1CalibrationAmplitude
        {
            get { return _model.Settings.Ddc1Parameter.CalibrationAmplitude; }
            set
            {
                _model.Settings.Ddc1Parameter.CalibrationAmplitude = value;
                NotifyPropertyChanged("Ddc1CalibrationAmplitude");
            }
        }

        public int Ddc1CalibrationOffset
        {
            get { return _model.Settings.Ddc1Parameter.CalibrationOffset; }
            set
            {
                _model.Settings.Ddc1Parameter.CalibrationOffset = value;
                NotifyPropertyChanged("Ddc1CalibrationOffset");
            }
        }

        public double Ddc2MinimumIntensityDb
        {
            get { return _model.Settings.Ddc2Parameter.MinimumIntensityDb; }
            set
            {
                _model.Settings.Ddc2Parameter.MinimumIntensityDb = value;
                NotifyPropertyChanged("Ddc2MinimumIntensityDb");
            }
        }

        public double Ddc2CalibrationAmplitude
        {
            get { return _model.Settings.Ddc2Parameter.CalibrationAmplitude; }
            set
            {
                _model.Settings.Ddc2Parameter.CalibrationAmplitude = value;
                NotifyPropertyChanged("Ddc2CalibrationAmplitude");
            }
        }

        public int Ddc2CalibrationOffset
        {
            get { return _model.Settings.Ddc2Parameter.CalibrationOffset; }
            set
            {
                _model.Settings.Ddc2Parameter.CalibrationOffset = value;
                NotifyPropertyChanged("Ddc2CalibrationOffset");
            }
        }

        public double FastSagcAttack
        {
            get { return _model.Settings.FastSagc.AttackTime; }
            set
            {
                _model.Settings.FastSagc.AttackTime = value;
                NotifyPropertyChanged("FastSagcAttack");
            }
        }

        public double FastSagcDecay
        {
            get { return _model.Settings.FastSagc.DecayTime; }
            set
            {
                _model.Settings.FastSagc.DecayTime = value;
                NotifyPropertyChanged("FastSagcDecay");
            }
        }

        public double FastSagcReferenceLevel
        {
            get { return _model.Settings.FastSagc.ReferenceLevel; }
            set
            {
                _model.Settings.FastSagc.ReferenceLevel = value;
                NotifyPropertyChanged("FastSagcReferenceLevel");
            }
        }

        public double MediumSagcAttack
        {
            get { return _model.Settings.MediumSagc.AttackTime; }
            set
            {
                _model.Settings.MediumSagc.AttackTime = value;
                NotifyPropertyChanged("MediumSagcAttack");
            }
        }

        public double MediumSagcDecay
        {
            get { return _model.Settings.MediumSagc.DecayTime; }
            set
            {
                _model.Settings.MediumSagc.DecayTime = value;
                NotifyPropertyChanged("MediumSagcDecay");
            }
        }

        public double MediumSagcReferenceLevel
        {
            get { return _model.Settings.MediumSagc.ReferenceLevel; }
            set
            {
                _model.Settings.MediumSagc.ReferenceLevel = value;
                NotifyPropertyChanged("MediumSagcReferenceLevel");
            }
        }

        public double SlowSagcAttack
        {
            get { return _model.Settings.SlowSagc.AttackTime; }
            set
            {
                _model.Settings.SlowSagc.AttackTime = value;
                NotifyPropertyChanged("SlowSagcAttack");
            }
        }

        public double SlowSagcDecay
        {
            get { return _model.Settings.SlowSagc.DecayTime; }
            set
            {
                _model.Settings.SlowSagc.DecayTime = value;
                NotifyPropertyChanged("SlowSagcDecay");
            }
        }

        public double SlowSagcReferenceLevel
        {
            get { return _model.Settings.SlowSagc.ReferenceLevel; }
            set
            {
                _model.Settings.SlowSagc.ReferenceLevel = value;
                NotifyPropertyChanged("SlowSagcReferenceLevel");
            }
        }

        /// <summary>
        /// For considering LNA effect on squelch level
        /// </summary>
        public double SquelchReferenceLevel
        {
            get { return _model.Settings.SquelchReferenceLevel; }
            set
            {
                _model.Settings.SquelchReferenceLevel = value;
                NotifyPropertyChanged("SquelchReferenceLevel");
            }
        }

        /// <summary>
        /// For considering LNA effect on Rms level
        /// </summary>
        public double RmsReferenceLevel
        {
            get { return _model.Settings.RmsReferenceLevel; }
            set
            {
                _model.Settings.RmsReferenceLevel = value;
                NotifyPropertyChanged("RmsReferenceLevel");
            }
        }
    }
}
