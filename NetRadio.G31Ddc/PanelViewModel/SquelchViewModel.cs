using NetRadio.G31Ddc.ViewModel;

namespace NetRadio.G31Ddc.PanelViewModel
{
    public class SquelchViewModel : ViewModelBase
    {
        private bool _enable;
        private double _level;
        private int _noise;
        private int _voice;

        private readonly RadioModel _model;

        public SquelchViewModel(RadioModel model)
        {
            _model = model;
        }

        public bool IsEnable
        {
            get
            {
                return _enable;
            }
            set
            {
                _enable = value;
                NotifyPropertyChanged("IsEnable");
            }
        }

        /// <summary>
        /// SquelchReferenceLevel is LNA effect on signal level
        /// </summary>
        public double Level
        {
            get { return _level - _model.Settings.SquelchReferenceLevel; }
            set
            {
                _level = value + _model.Settings.SquelchReferenceLevel;
                NotifyPropertyChanged("Level");
            }
        }

        public int Noise
        {
            get { return _noise; }
            set
            {
                _noise = value;
                NotifyPropertyChanged("Noise");
            }
        }

        public int Voice
        {
            get { return _voice; }
            set
            {
                _voice = value;
                NotifyPropertyChanged("Voice");
            }
        }

        private bool _squelched;

        public bool Squelched
        {
            get { return _squelched; }
            set { _squelched = value; }
        }

        public void UpdateSquelch(double signalLevel)
        {
            _squelched = _enable && _level > signalLevel;
        }
    }
}
