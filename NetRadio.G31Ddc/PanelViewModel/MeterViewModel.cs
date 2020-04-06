using NetRadio.Devices;
using NetRadio.Devices.G3XDdc;
using NetRadio.G31Ddc.ViewModel;
using NetRadio.Signal;
using System;
using NetRadio.Devices.G3XDdc.Signal;

namespace NetRadio.G31Ddc.PanelViewModel
{
    [Obsolete("Make it general, now it is for demolator purpose only", false)]
    public class MeterViewModel : ViewModelBase
    {
        private const float G31DDC_RECEIVER_INPUT_IMPEDANCE = 50f;

        private double _peakFrequency;
        private double _peakPower;

        private double _frequencyError;
        private double _powerDbm;
        private double _powerUVolts;
        private RfMath.SUnits _sMeter;
        private double _powerMicroWatts;

        private SignalLevel _signal;
        private double _peakDbm;
        private double _peakUVolts;
        private double _rmsDbm;
        private double _rmsUVolts;

        private readonly RadioModel _model;
        private readonly uint _channel;
        private int _updateMeterStep;

        public MeterViewModel(RadioModel model, uint channel)
        {
            _model = model;
            _channel = channel;
        }

        public double PowerWattsValue
        {
            get { return _powerMicroWatts; }
            set
            {
                _powerMicroWatts = value;
                NotifyPropertyChanged("PowerWattsValue");
            }
        }

        public RfMath.SUnits SMeterValue
        {
            get { return _sMeter; }
            set
            {
                _sMeter = value;
                NotifyPropertyChanged("SMeterValue");
            }
        }

        public double PeakFrequency
        {
            get { return _peakFrequency; }
            set
            {
                _peakFrequency = value;
                NotifyPropertyChanged("PeakFrequency");
            }
        }

        public double PeakPower
        {
            get { return _peakPower; }
            set
            {
                _peakPower = value;
                NotifyPropertyChanged("PeakPower");
            }
        }

        public double FrequencyErrorValue
        {
            get { return _frequencyError; }
            set
            {
                _frequencyError = value;
                NotifyPropertyChanged("FrequencyErrorValue");
            }
        }

        public double PowerDbmValue
        {
            get { return _powerDbm; }
            set
            {
                _powerDbm = value;
                NotifyPropertyChanged("PowerDbmValue");
            }
        }

        public double PowerUVoltsValue
        {
            get { return _powerUVolts; }
            set
            {
                _powerUVolts = value;
                NotifyPropertyChanged("PowerUVoltsValue");
            }
        }

        public SignalLevel Signal
        {
            get { return _signal; }
            set
            {
                _signal = value;
                _peakUVolts = _signal.Peak * 1000000;
                _peakDbm = RfMath.MicroVoltsToDbm(_peakUVolts);
                _rmsUVolts = _signal.Rms * 1000000;
                _rmsDbm = RfMath.MicroVoltsToDbm(_rmsUVolts);
                NotifyPropertyChanged("Signal");
                NotifyPropertyChanged("PeakDbm");
                NotifyPropertyChanged("PeakUVolts");
                NotifyPropertyChanged("RmsDbm");
                NotifyPropertyChanged("RmsUVolts");
            }
        }

        public double PeakDbm
        {
            get { return _peakDbm; }
        }

        public string PeakUVolts
        {
            get { return _peakUVolts.ToString("N3"); }
        }

        /// <summary>
        /// RmsReferenceLevel is for considering LNA effect on gain 
        /// </summary>
        public double RmsDbm
        {
            get { return _rmsDbm - _model.Settings.RmsReferenceLevel; }
        }

        public double RmsUVolts
        {
            get { return _rmsUVolts; }
        }


        [Obsolete("Remove e from parameters", false)]
        public void Update(FftEventArgs e, FrequencyBins bin)
        {
            if (_model.Settings.AsyncAnalyze)
                TaskUtility.Run(() => Analyze(e, bin));
            else
                Analyze(e, bin);
        }

        public void Update(FrequencyBins bin)
        {
            UpdateMeterView(bin);
        }

        public void Update(FastFrequencyBins bin)
        {
            UpdateMeterView(bin);
        }

        private void Analyze(FftEventArgs e, FrequencyBins bin)
        {
            var detailedBin = _model.Settings.DetailedAnalyze
                ? new FrequencyBins(e, _model.Radio.BinParametersDetailed(_channel))
                : bin;

            UpdateMeterView(detailedBin);
        }

        private void UpdateMeterView(FrequencyBins detailedBin)
        {
            _updateMeterStep++;
            _updateMeterStep %= _model.Settings.MeterUpdateSpeed; //5;
            if (_updateMeterStep != 0)
                return;

            Signal = _model.Ddc2[_channel].Signal;

            PeakFrequency = detailedBin.MaxIntensityAt() / 1000000.0;
            PeakPower = detailedBin.MaxIntensity();

            FrequencyErrorValue = detailedBin.MaxIntensityAt() / 1000000;
            PowerDbmValue = PeakDbm;
            PowerUVoltsValue = RfMath.DbmToMicroVolts(PeakDbm);
            PowerWattsValue = RfMath.DbmToMicroWatts(PeakDbm);
            SMeterValue = RfMath.DbmToSUnit(PeakDbm);
        }

        private void UpdateMeterView(FastFrequencyBins detailedBin)
        {
            _updateMeterStep++;
            _updateMeterStep %= _model.Settings.MeterUpdateSpeed; //5;
            if (_updateMeterStep != 0)
                return;

            Signal = _model.Ddc2[_channel].Signal;

            PeakFrequency = detailedBin.FrequencyAt(detailedBin.MaxIntensityAt()) / 1000000.0;
            PeakPower = detailedBin.MaxIntensity();

            FrequencyErrorValue = detailedBin.MaxIntensityAt() / 1000000;
            PowerDbmValue = PeakDbm;
            PowerUVoltsValue = RfMath.DbmToMicroVolts(PeakDbm);
            PowerWattsValue = RfMath.DbmToMicroWatts(PeakDbm);
            SMeterValue = RfMath.DbmToSUnit(PeakDbm);
        }
    }
}
