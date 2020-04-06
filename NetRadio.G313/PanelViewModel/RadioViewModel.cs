using System;
using System.Windows.Input;
using NetRadio.Devices;
using NetRadio.Devices.G313;
using NetRadio.Devices.G313.Signal;
using NetRadio.G313.Chart;
using NetRadio.G313.Model;
using NetRadio.G313.ViewModel;
using NetRadio.Signal;

namespace NetRadio.G313.PanelViewModel
{
    class RadioViewModel:ViewModelBase
    {
        private readonly RadioModel _model;
        private int _updateMeterStep;
        private bool _squelched;
        public MeterViewModel MeterView { get; private set; }
        public RecordingViewModel RecordingView { get; private set; }
        public BlockScanViewModel BlockScanView { get; private set; }
        public SweeperViewModel SweepView { get; private set; }
        public MemoryViewModel MemoryView { get; private set; }
        public ISpectrumAnalyzer SpectrumAnalyzer { get; private set; }
        public ISweeperAnalyzer SweeperAnalyzer { get; private set; }
        
        private bool _notchState;
        private uint _notchBandwidth;
        private int _notchFrequency;

        private uint _blankerThreshold;
        private bool _blankerState;

        private uint _frequencyStep;
        private int _squelchLevel;
        private bool _autoStep;

        private bool _muteState;

        private bool _videoFilter;
        private int _videoPoints;

        private Agc _softwareAgc;

        private bool _afc;

        private uint _volume;

        public uint IfBandwidthValue
        {
            get
            {
                var value = _model.Radio.Demodulator().IfBandwidth();
                var shift = _model.Radio.Demodulator().IfShift();

                var halfBand = value/2;
                SpectrumAnalyzer.SetBandwidth(shift-halfBand,shift+halfBand);

                return value;
            }
            set
            {
                var bandwidth = _model.Limits.IfBandwidth(value);
                var shift = _model.Radio.Demodulator().IfShift();

                _model.Radio.Demodulator().IfBandwidth(bandwidth);

                var halfBand = bandwidth / 2;
                SpectrumAnalyzer.SetBandwidth(shift - halfBand, shift + halfBand);

                OnPropertyChanged("IfBandwidthValue");
            }
        }

        public int IfShiftValue
        {
            get
            {
                var value = _model.Radio.Demodulator().IfBandwidth();
                var shift = _model.Radio.Demodulator().IfShift();

                var halfBand = value / 2;
                SpectrumAnalyzer.SetBandwidth(shift - halfBand, shift + halfBand);

                return shift;
            }
            set
            {
                var bandwidth = _model.Radio.Demodulator().IfBandwidth();
                var shift = _model.Limits.IfShift(value);

                _model.Radio.Demodulator().IfShift(shift);

                var halfBand = bandwidth / 2;
                SpectrumAnalyzer.SetBandwidth(shift - halfBand, shift + halfBand);

                OnPropertyChanged("IfShiftValue");
            }
        }

        public bool AfcState
        {
            get { return _afc; }
            set
            {
                _afc = value;
                OnPropertyChanged("AfcState");
            }
        }

        public Agc SoftwareAgcState
        {
            get { return _softwareAgc; }
            set
            {
                _softwareAgc = value;
                switch (value)
                {
                    case Agc.Off:
                        _model.Radio.Demodulator().DisableSofwareAgc();
                        break;
                    case Agc.Slow:
                        _model.Radio.Demodulator().SoftwareAgc(_model.Settings.SlowSagc);
                        break;
                    case Agc.Medium:
                        _model.Radio.Demodulator().SoftwareAgc(_model.Settings.MediumSagc);
                        break;
                    case Agc.Fast:
                        _model.Radio.Demodulator().SoftwareAgc(_model.Settings.FastSagc);
                        break;
                }
                OnPropertyChanged("SoftwareAgcState");
            }
        }

        public uint AudioBandwidthValue
        {
            get { return _model.Radio.Demodulator().AudioBandwidth(); }
            set
            {
                _model.Radio.Demodulator().AudioBandwidth(value);
                OnPropertyChanged("AudioBandwidthValue");
            }
        }

        public uint AudioGainValue
        {
            get { return _model.Radio.Demodulator().AudioGain(); }
            set
            {
                _model.Radio.Demodulator().AudioGain(value);
                OnPropertyChanged("AudioGainValue");
            }
        }

        public uint CwToneValue
        {
            get { return _model.Radio.Demodulator().CwTone(); }
            set
            {
                _model.Radio.Demodulator().CwTone(value);
                OnPropertyChanged("CwToneValue");
            }
        }
        public bool VideoFilter
        {
            get { return _videoFilter; }
            set
            {
                _videoFilter = value;
                if (_videoFilter)
                {
                    var points = _model.Radio.UsableDownSamples(_model.FftAnalyzer.FftLength, VideoPoints);
                    SpectrumAnalyzer.ReInitialize(points);
                }
                else
                    SpectrumAnalyzer.ReInitialize();

                OnPropertyChanged("VideoFilter");
            }
        }

        public int VideoPoints
        {
            get { return _videoPoints; }
            set
            {
                _videoPoints = value;
                _videoPoints = _videoPoints < 10 ? 10 : _videoPoints > 10000 ? 10000 : _videoPoints;
                if (VideoFilter)
                {
                    var points = _model.Radio.UsableDownSamples(_model.FftAnalyzer.FftLength, _videoPoints);
                    SpectrumAnalyzer.ReInitialize(points);
                }
                OnPropertyChanged("VideoPoints");
            }
        }
        public bool AutoStep
        {
            get { return _autoStep; }
            set
            {
                _autoStep = value;
                OnPropertyChanged("AutoStep");
            }
        }

        public bool Squelched
        {
            get
            {
                _model.Radio.Demodulator().Volume(!_squelched ? _volume : 0);
                return _squelched;
            }
            set
            {
                _squelched = value;

                _model.Radio.Demodulator().Volume(!_squelched ? _volume : 0);

                OnPropertyChanged("Squelched");
            }
        }
        public uint VolumeValue
        {
            get
            {
                return _volume;/*_model.Radio.Demodulator().Volume();*/ }
            set
            {
                _volume = _model.Limits.Volume(value);
                Squelched = Squelched; //to set volume
                //_model.Radio.Demodulator().Volume(_volume);
                OnPropertyChanged("VolumeValue");
            }
        }
        public bool MuteState
        {
            get { return _muteState; }
            set
            {
                _muteState = value;
                if (_muteState)
                    TaskUtility.Run(() => _model.LiveAudio.Stop());
                else
                    TaskUtility.Run(() => _model.LiveAudio.Play());

                UpdateSquelch();

                OnPropertyChanged("MuteState");
            }
        }

        public int SquelchValue
        {
            get { return _squelchLevel; }
            set
            {
                _squelchLevel = _model.Limits.Squelch(value);
                OnPropertyChanged("SquelchValue");
            }
        }

        public uint AfSquelchValue
        {
            get { return _model.Radio.Demodulator().AfSquelchLevel(); }
            set
            {
                var afSquelch = _model.Limits.AfSquelch(value);
                _model.Radio.Demodulator().AfSquelchLevel(afSquelch);
                OnPropertyChanged("AfSquelchValue");
            }
        }

        public long MinFrequency
        {
            get { return (long)_model.Radio.CachedInfo.MinFrequency; }
        }

        public long MaxFrequency
        {
            get { return (long)_model.Radio.CachedInfo.MaxFrequency; }
        }

        public uint FrequencyStep
        {
            get { return _frequencyStep; }
            set
            {
                _frequencyStep= value;
                OnPropertyChanged("FrequencyStep");
            }
        }

        public uint FrequencyValue
        {
            get { return _model.Radio.Frequency(); }
            set
            {
                var freq = _model.Limits.Frequency(value);
                _model.Radio.Frequency(freq);
                OnPropertyChanged("FrequencyValue");
            }
        }

        public bool BlankerState
        {
            get { return _blankerState; }
            set
            {
                _model.Radio.Demodulator()
                    .NoiseBlanker(new NoiseBlanker {Active = value, Threshold = _blankerThreshold});
                _blankerState = value;
                OnPropertyChanged("BlankerState");
            }
        }
        public uint BlankerThreshold
        {
            get { return _blankerThreshold; }
            set
            {
                _blankerThreshold = value;
                OnPropertyChanged("BlankerThreshold");
            }
        }

        public uint NotchBandwidth
        {
            get
            {
                SetNotchBands();
                return _notchBandwidth;
            }
            set
            {
                _notchBandwidth = _model.Limits.NotchBandwidth(value);
                ApplyNotch(_notchState);
                OnPropertyChanged("NotchBandwidth");
            }
        }

        private void SetNotchBands()
        {
            SpectrumAnalyzer.Notch(_notchState);

            var halfBand = _notchBandwidth/2;
            SpectrumAnalyzer.SetNotch(_notchFrequency - halfBand, _notchFrequency + halfBand);
        }

        public int NotchFrequency
        {
            get
            {
                SetNotchBands();
                return _notchFrequency;
            }
            set
            {
                _notchFrequency = value;
                ApplyNotch(_notchState);
                OnPropertyChanged("NotchFrequency");
            }
        }
        public bool NotchState
        {
            get
            {
                SetNotchBands();
                return _notchState;
            }
            set
            {                
                _notchState = value;
                ApplyNotch(value);
                OnPropertyChanged("NotchState");
            }
        }

        private void ApplyNotch(bool value)
        {
            _model.Radio.Demodulator()
                .NotchFilter(new NotchFilter
                {
                    Active = value,
                    Bandwidth = _notchBandwidth,
                    Frequency = _notchFrequency
                });

            SetNotchBands();
        }

        public G313Demodulator.DemodulatorMode ModeState
        {
            get { return _model.Radio.Demodulator().Mode(); }
            set
            {
                _model.Radio.Demodulator().Mode(value);
                OnPropertyChanged("ModeState");
            }
        }

        public int IfGainValue
        {
            get
            {
                return _model.Radio.IfGain();
            }
            set
            {
                var ifGain = _model.Limits.IfGain(value);
                _model.Radio.IfGain(ifGain);
                OnPropertyChanged("IfGainValue");
            }
        }

        public bool AttenuatorState
        {
            get { return _model.Radio.Attenuator(); }
            set
            {
                _model.Radio.Attenuator(value);
                OnPropertyChanged("AttenuatorState");
            }
        }

        public Agc AgcState
        {
            get { return _model.Radio.Agc(); }
            set
            {
                _model.Radio.Agc(value);
                OnPropertyChanged("AgcState");
            }
        }

        public ICommand CommandDefeatIfGain { get; private set; }
        public ICommand CommandDefeatSquelch { get; private set; }
        public ICommand CommandDefeatAfSquelch { get; private set; }
        public ICommand CommandTuneToPeak { get; private set; }
        public ICommand CommandNextStep { get; private set;}
        public ICommand CommandPreviousStep { get; private set; }
        public ICommand CommandNextTenStep { get; private set; }
        public ICommand CommandPreviousTenStep { get; private set; }
        public ICommand CommandSpectrumZoomIn { get; private set; }
        public ICommand CommandSpectrumZoomOut { get; private set; }
        public ICommand CommandSweeperZoomIn { get; private set; }
        public ICommand CommandSweeperZoomOut { get; private set; }
        public ICommand CommandOpenSetup { get; private set; }
        public ICommand CommandIfShiftReset { get; private set; }
        public Action ReInitDataContext { get; set; }

        public RadioViewModel(RadioModel model,ISpectrumAnalyzer spectrumAnalyzer,ISweeperAnalyzer sweeperAnalyzer)
        {
            SpectrumAnalyzer = spectrumAnalyzer;
            SweeperAnalyzer = sweeperAnalyzer;

            SpectrumAnalyzer.BandwidthChanged += (range, value) =>
            {
                var bandwidth = range.End - range.Start;
                //var halfBand = bandwidth/2;
                //var shift = range.Start + halfBand;

                IfBandwidthValue = (uint)bandwidth;
                IfShiftValue = (int)value;
            };

            SpectrumAnalyzer.NotchChanged += (range, value) =>
            {
                var bandwidth = range.End - range.Start;

                NotchBandwidth = (uint)bandwidth;
                NotchFrequency = (int)value;
            };

            _model = model;
            _model.FftAnalyzer.FftCalculated += FftAnalyzer_FftCalculated;
            MeterView = new MeterViewModel();
            RecordingView=new RecordingViewModel(_model);
            BlockScanView=new BlockScanViewModel(_model,this);
            SweepView=new SweeperViewModel(_model,this);
            MemoryView = new MemoryViewModel(_model, this);

            CommandDefeatSquelch=new DelegateCommand(()=> SquelchValue=G313RadioLimits.SQUELCH_MIN);
            CommandDefeatIfGain = new DelegateCommand(() => IfGainValue=G313RadioLimits.IFGAIN_MIN);
            CommandDefeatAfSquelch=new DelegateCommand(()=>AfSquelchValue=G313RadioLimits.AF_SQUELCH_MIN);
            CommandTuneToPeak = new DelegateCommand(() => FrequencyValue = (uint) (MeterView.PeakFrequencyValue*1000000));

            CommandNextStep=new DelegateCommand(()=> FrequencyValue+=FrequencyStep);
            CommandNextTenStep = new DelegateCommand(() => FrequencyValue += (FrequencyStep*10));
            CommandPreviousStep = new DelegateCommand(() => FrequencyValue -= FrequencyStep);
            CommandPreviousTenStep = new DelegateCommand(() => FrequencyValue -= (FrequencyStep * 10));

            CommandSpectrumZoomIn=new DelegateCommand(()=>SpectrumAnalyzer.ZoomIn());
            CommandSpectrumZoomOut=new DelegateCommand(()=>SpectrumAnalyzer.ZoomOut());
            CommandSweeperZoomIn = new DelegateCommand(() => SweeperAnalyzer.ZoomIn());
            CommandSweeperZoomOut = new DelegateCommand(() => SweeperAnalyzer.ZoomOut());

            CommandIfShiftReset=new DelegateCommand(()=>IfShiftValue=0);

            CommandOpenSetup=new DelegateCommand(() =>
            {
                var settings = new SettingsWindow(_model);
                settings.ShowDialog();

                ReInitDataContext();
            });

            NotchBandwidth = _model.Radio.Demodulator().NotchFilter().Bandwidth;
            NotchFrequency = _model.Radio.Demodulator().NotchFilter().Frequency;
            NotchState = _model.Radio.Demodulator().NotchFilter().Active;

            BlankerThreshold =(uint) _model.Radio.Demodulator().NoiseBlanker().Threshold;
            BlankerState = _model.Radio.Demodulator().NoiseBlanker().Active;

            SquelchValue = G313RadioLimits.SQUELCH_MIN;

            VideoPoints = 1000;

            FrequencyStep = 500;

            SoftwareAgcState=Agc.Off;

            MuteState = true; //start as muted

            FrequencyValue = _model.Settings.StartFrequency;

            VolumeValue = _model.Radio.Demodulator().Volume();
        }

        private void FftAnalyzer_FftCalculated(object sender, FftEventArgs e)
        {
            FrequencyBins bin;
           // _spectrumAnalyzer.Clear();
            if (!VideoFilter)
                bin = new FrequencyBins(e, _model.Radio.BinParametersDefault());
            else
                bin = new FrequencyBins(e,
                    _model.Radio.BinParametersVideoFilter(_model.FftAnalyzer.FftLength, VideoPoints));

            SpectrumAnalyzer.Update(bin);

            if (_model.Settings.AsyncAnalyze)
                TaskUtility.Run(() => Analyze(e, bin));
            else
                Analyze(e, bin);

            UpdateSquelch();

            if (AutoStep)
                FrequencyStep = (uint) Math.Abs(MeterView.FrequencyErrorValue*500000);

            if (AfcState)
            {
                if (Math.Abs(MeterView.FrequencyErrorValue*1000000) > _model.Settings.AfcThreshold) //1000 as default
                    FrequencyValue = (uint) (MeterView.PeakFrequencyValue*1000000);
            }
            RecordingView.Tick();
        }

        private void Analyze(FftEventArgs e, FrequencyBins bin)
        {
            var detailedBin = _model.Settings.DetailedAnalyze
                ? new FrequencyBins(e, _model.Radio.BinParametersDetailed())
                : bin;

            UpdateMeterView(detailedBin);
        }

        private void UpdateSquelch()
        {
            if (MuteState)
                return;

            if (SquelchValue > _model.Radio.Demodulator().SignalStrength() && !_squelched)
            {
                Squelched = true;
                //TaskUtility.Run(() => _model.LiveAudio.Stop());
            }
            else if (SquelchValue < _model.Radio.Demodulator().SignalStrength() && _squelched)
            {
                Squelched = false;
                //TaskUtility.Run(() => _model.LiveAudio.Play());
            }
        }

        private void UpdateMeterView(FrequencyBins detailedBin)
        {
            _updateMeterStep++;
            _updateMeterStep %= _model.Settings.MeterUpdateSpeed; //5;
            if (_updateMeterStep != 0)
                return;

            MeterView.PeakFrequencyValue = (detailedBin.MaxIntensityAt() / 1000000);
            MeterView.FrequencyErrorValue = (detailedBin.MaxIntensityAt() - FrequencyValue) / 1000000;
            MeterView.PowerDbmValue = _model.Radio.Demodulator().SignalStrength();
            MeterView.PowerUVoltsValue = RfMath.DbmToMicroVolts(_model.Radio.Demodulator().SignalStrength());
            MeterView.PowerWattsValue = RfMath.DbmToMicroWatts(_model.Radio.Demodulator().SignalStrength());
            MeterView.SMeterValue = RfMath.DbmToSUnit(_model.Radio.Demodulator().SignalStrength());
        }
    }
}
