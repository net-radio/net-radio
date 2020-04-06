using NetRadio.G313.ViewModel;
using NetRadio.Signal;

namespace NetRadio.G313.PanelViewModel
{
    class MeterViewModel:ViewModelBase
    {
        private double _peakFrequency;
        private double _frequencyError;
        private double _powerDbm;
        private double _powerUVolts;
        private RfMath.SUnits _sMeter;
        private double _powerMicroWatts;

        public double PowerWattsValue
        {
            get { return _powerMicroWatts; }
            set
            {
                _powerMicroWatts = value;
                OnPropertyChanged("PowerWattsValue");
            }
        }

        public RfMath.SUnits SMeterValue
        {
            get { return _sMeter; }
            set
            {
                _sMeter = value;
                OnPropertyChanged("SMeterValue");
            }
        }
        public double PeakFrequencyValue
        {
            get { return _peakFrequency; }
            set
            {
                _peakFrequency = value;
                OnPropertyChanged("PeakFrequencyValue");
            }
        }
        public double FrequencyErrorValue
        {
            get { return _frequencyError; }
            set
            {
                _frequencyError = value;
                OnPropertyChanged("FrequencyErrorValue");
            }
        }

        public double PowerDbmValue
        {
            get { return _powerDbm; }
            set
            {
                _powerDbm = value;
                OnPropertyChanged("PowerDbmValue");
            }
        }

        public double PowerUVoltsValue
        {
            get { return _powerUVolts; }
            set
            {
                _powerUVolts = value;
                OnPropertyChanged("PowerUVoltsValue");
            }
        }
    }
}
