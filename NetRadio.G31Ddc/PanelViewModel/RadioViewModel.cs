using NetRadio.Devices;
using NetRadio.Devices.Exceptions;
using NetRadio.Devices.G3XDdc;
using NetRadio.G31Ddc.Arction;
using NetRadio.G31Ddc.Operations;
using NetRadio.G31Ddc.ViewModel;
using NetRadio.G31Ddc.WaterfallSample;
using NetRadio.Signal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace NetRadio.G31Ddc.PanelViewModel
{
    public class RadioViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private readonly RadioModel _model;
        // private readonly uint _channel = 0;
        private readonly IEnumerable<DdcInfo> _availableDdc1;
        private DdcInfo _activeDdc1;

        private bool _autoStep;
        private uint _frequencyStep;
        private bool _afc;
        private bool _muteState;
        private float _volume = 0.4f;
        private float _audioPlayerVolume;

        // Video 
        private bool _videoFilter;
        private int _videoPoints;

        /// <summary>
        /// Software Automatic Gain Control
        /// </summary>
        private Agc _softwareAgc;
        #endregion

        #region Properties
        public MeterViewModel MeterViewDemodulator { get; private set; }
        public MeterViewModel MeterViewDdc1 { get; private set; }
        public SquelchViewModel SquelchView { get; private set; }
        public IfSpectrumViewModel IfSpectrumView { get; set; }
        public Ddc1SpectrumViewModel Ddc1SpectrumView { get; set; }
        public Ddc2SpectrumViewModel Ddc2SpectrumView { get; set; }
        public Ddc2ViewModel Ddc2View { get; set; }
        public ISpectrumAnalyzer SpectrumAnalyzerIf { get; private set; }
        public ISpectrumAnalyzer SpectrumAnalyzerDdc1 { get; private set; }
        public ISpectrumAnalyzer SpectrumAnalyzerDdc2 { get; private set; }
        public IWaterfallAnalyzer Waterfall { get; private set; }
        #endregion

        #region Commands
        public ICommand CommandTuneToPeak { get; private set; }
        public ICommand CommandPeakToCenter { get; private set; }
        public ICommand CommandNextStep { get; private set; }
        public ICommand CommandPreviousStep { get; private set; }
        public ICommand CommandNextTenStep { get; private set; }
        public ICommand CommandPreviousTenStep { get; private set; }
        public ICommand CommandOpenSetup { get; private set; }
        public ICommand CommandStartup { get; private set; }
        #endregion

        //private WaveformControl WaveformIf;
        //private WaveformControl WaveformDdc1;
        //private WaveformControl WaveformDdc2;

        #region Costructor
        private RadioViewModel()
        {
            logger.Debug("RadioViewModel Ctor");

            try
            {
                _model = new RadioModel();
                _model.Initialize();

                InitializeReceiver1();

                MeterViewDemodulator = new MeterViewModel(_model, 0);
                MeterViewDdc1 = new MeterViewModel(_model, 0);
                SquelchView = new SquelchViewModel(_model);

                /* UNCOMMENT THIS BLOCK
                SpectrumAnalyzerIf = vo.SpectrumAnalyzerIf;
                SpectrumAnalyzerDdc1 = vo.SpectrumAnalyzerDdc1;
                SpectrumAnalyzerDdc2 = vo.SpectrumAnalyzerDdc2;
                Waterfall = vo.WaterfallAnalyzer;
                */

                IfSpectrumView = new IfSpectrumViewModel(this, SpectrumAnalyzerIf);
                Ddc1SpectrumView = new Ddc1SpectrumViewModel(this, SpectrumAnalyzerDdc1);
                Ddc2SpectrumView = new Ddc2SpectrumViewModel(this, SpectrumAnalyzerDdc2);

                Ddc2View = new Ddc2ViewModel();

                InitializeReceiver();

                CommandTuneToPeak = new DelegateCommand(() => Ddc2RelativeFrequency = (int)(MeterViewDdc1.PeakFrequency * 1000000 - Ddc1Frequency));
                CommandPeakToCenter = new DelegateCommand(() => AbsoluteFrequency = (uint)Ddc2AbsoluteFrequency);

                CommandNextStep = new DelegateCommand(() => AbsoluteFrequency += FrequencyStep);
                CommandNextTenStep = new DelegateCommand(() => AbsoluteFrequency += (FrequencyStep * 10));
                CommandPreviousStep = new DelegateCommand(() => AbsoluteFrequency -= FrequencyStep);
                CommandPreviousTenStep = new DelegateCommand(() => AbsoluteFrequency -= (FrequencyStep * 10));

                CommandOpenSetup = new DelegateCommand(() =>
                {
                    var settings = new SettingsWindow(_model);
                    settings.ShowDialog();

                    // ReInitDataContext();
                });

                CommandStartup = new RelayCommand((object obj) =>
                {
                    lock (_startupLock)
                    {
                        G31DdcMainWnd wnd = obj as G31DdcMainWnd;
                        if (SpectrumAnalyzerIf == null)
                        {
                            SpectrumAnalyzerIf = wnd.WaveformIf;
                            IfSpectrumView = new IfSpectrumViewModel(this, SpectrumAnalyzerIf);

                            SpectrumAnalyzerDdc1 = wnd.WaveformDdc1;
                            Ddc1SpectrumView = new Ddc1SpectrumViewModel(this, SpectrumAnalyzerDdc1);

                            SpectrumAnalyzerDdc2 = wnd.WaveformDdc2;
                            Ddc2SpectrumView = new Ddc2SpectrumViewModel(this, SpectrumAnalyzerDdc2);

                            Waterfall = wnd.Waterfall;
                        }
                    }
                });

                // VideoPoints = _model.FftAnalyzerIf.FftLength;

                // VideoPoints = 1000;
                FrequencyStep = 500;
                MuteState = false;

                LiveAudioPlayer audioPlayer = _model.Ddc2[0].AudioPlayer;
                if (audioPlayer != null)
                {
                    _audioPlayerVolume = audioPlayer.VolumeLevel;
                    Volume = _audioPlayerVolume;
                }

                _availableDdc1 = _model.AvailableDdc1.ToList<DdcInfo>();
                ActiveDdc1 = _model.AvailableDdc1[_model.AvailableDdc1.Length - 1];
            }
            catch (OperationFailedException ex)
            {
                logger.Error("Failed to initializa radio.", ex);
                MessageBox.Show("Failed to initializa radio. Start radio in demo mode.");
            }
        }
        #endregion

        private void InitializeReceiver1()
        {
            _model.Ddc2[0].FftAnalyzerDdc2.Start();

            _model.Ddc2[0].AbsoluteFrequency = 585000;

            _model.Ddc2[0].Gain = 80;
            _model.Ddc2[1].Gain = 80;
            _model.Ddc2[2].Gain = 80;

            _model.Ddc2[0].AgcActive = true;

            _model.Ddc2[0].AudioFilter = new AudioFilter
            {
                CutOffHigh = 4000,
                CutOffLow = 50,
                DeEmphasis = 0
            };
        }

        private void InitializeReceiver()
        {
            _model.FftAnalyzerIf.FftCalculated += FftAnalyzerIf_FftCalculated;
            _model.FftAnalyzerDdc1.FftCalculated += FftAnalyzerDdc1_FftCalculated;

            _model.Ddc2[0].FftAnalyzerDdc2.FftCalculated += FftAnalyzerDdc2_FftCalculated;
            // _model.Ddc2[1].FftAnalyzerDdc2.FftCalculated += FftAnalyzerDdc2_FftCalculated;
            // _model.Ddc2[2].FftAnalyzerDdc2.FftCalculated += FftAnalyzerDdc2_FftCalculated;

        }

        private void FftAnalyzerIf_FftCalculated(object sender, FftEventArgs e)
        {
            var bin = new FrequencyBins(e,
            new BinParameters
            {
                MinimumIntensityDb = _model.Settings.IfParameter.MinimumIntensityDb,
                SamplingRate = 100000000,
                BinsPerPoint = IfSpectrumView.SelectedResolution.Resolution,
                CalibrationAmplitude = _model.Settings.IfParameter.CalibrationAmplitude,
                CalibrationOffset = _model.Settings.IfParameter.CalibrationOffset
            });

            if (SpectrumAnalyzerIf != null)
                SpectrumAnalyzerIf.Update(bin);

            if (Waterfall != null)
                Waterfall.Update(bin);

            /*
            if (!VideoFilter)
                bin = new FrequencyBins(e, _model.Radio.BinParametersDefault());
            else
                bin = new FrequencyBins(e, _model.Radio.BinParametersVideoFilter(_model.FftAnalyzerIf.FftLength, VideoPoints));
            */

            /*
            FrequencyBins bin = new FrequencyBins(e, new BinParameters
            {
                BinsPerPoint = 4,
                CalibrationAmplitude = 1.1, //1.5,//1.0 in sweeper is better
                CalibrationOffset = 20,//8,
                MinimumIntensityDb = -150,
                Relative = true,
                SamplingRate = 100000000
            });
            */

            /*
            if (_model.Settings.AsyncAnalyze)
                TaskUtility.Run(() => Analyze(e, bin));
            else
                Analyze(e, bin);

            UpdateSquelch();

            if (AutoStep)
                FrequencyStep = (uint)Math.Abs(MeterView.FrequencyErrorValue * 500000);

            if (AfcState)
            {
                if (Math.Abs(MeterView.FrequencyErrorValue * 1000000) > _model.Settings.AfcThreshold) //1000 as default
                    FrequencyValue = (uint)(MeterView.PeakFrequencyValue * 1000000);
            }
            RecordingView.Tick();
            */
        }

        [Obsolete("Use AbsoluteFrequency instead", true)]
        public uint FrequencyValue
        {
            get { return _model.Ddc2[0].AbsoluteFrequency; }
            set
            {
                var frequency = _model.Limits.Frequency(value);
                _model.Ddc2[0].AbsoluteFrequency = frequency;
                NotifyPropertyChanged("FrequencyValue");
            }
        }

        public DemodulatorMode ModeState
        {
            get
            {
                if (_model.Ddc2[0].DemodulatorMode == DemodulatorMode.Lsb
                    && _model.Ddc2[1].DemodulatorMode == DemodulatorMode.Usb)
                    return DemodulatorMode.Dsb;
                else
                    return _model.Ddc2[0].DemodulatorMode;
            }
            set
            {
                switch (value)
                {
                    case DemodulatorMode.Dsb:
                        // _model.Ddc2[1].Start();
                        _model.Ddc2[0].DemodulatorMode = DemodulatorMode.Lsb;
                        _model.Ddc2[1].DemodulatorMode = DemodulatorMode.Usb;
                        break;
                    case DemodulatorMode.Isb:
                        break;
                    default:
                        // _model.Ddc2[1].Stop();
                        _model.Ddc2[0].DemodulatorMode = value;
                        _model.Ddc2[1].DemodulatorMode = value;
                        break;
                }

                if (MuteState)
                    Mute();
                else
                    Unmute();


                NotifyPropertyChanged("ModeState");
            }
        }

        public RadioModel Radio
        {
            get { return _model; }
        }



        // ddc1Bandwidth.Items.AddRange(_model.AvailableDdc1);
        // _model.AvailableDdc1.ToList().ForEach(item => ddc1Bandwidth.Items.Add(item));
        // http://stackoverflow.com/questions/561166/binding-wpf-combobox-to-a-custom-list
        public CollectionView AvailableDdc1
        {
            get { return new CollectionView(_availableDdc1); }
        }

        public DdcInfo ActiveDdc1
        {
            get { return _activeDdc1; }
            set
            {
                if (_activeDdc1 == value)
                    return;

                _activeDdc1 = value;
                int index = Array.IndexOf<DdcInfo>(_model.AvailableDdc1, _activeDdc1);
                Ddc1BandwidthChanged(index);
                if (IfSpectrumView != null)
                    IfSpectrumView.Bandwidth = _activeDdc1.SampleRate;

                NotifyPropertyChanged("ActiveDdc1");
                UpdateSpectrum();
            }
        }

        private void UpdateSpectrum()
        {
            if (IfSpectrumView != null)
                IfSpectrumView.Update();
            if (Ddc1SpectrumView != null)
                Ddc1SpectrumView.Update();
            if (Ddc2SpectrumView != null)
                Ddc2SpectrumView.Update();
        }

        private void Ddc1BandwidthChanged(int index)
        {
            var playing = true; // _model.Ddc2[_channel].AudioPlayer != null;

            _model.Ddc2[0].PlayStop();
            _model.Ddc2[1].PlayStop();

            _model.Ddc2[0].RecordStop();

            _model.Stop();
            _model.Radio.Ddc1().Ddc((uint)index);
            _model.Start();

            if (playing)
            {
                _model.Ddc2[0].PlayStart();
                _model.Ddc2[1].PlayStart();
            }

            NotifyPropertyChanged("Ddc1BandwidthValue");
        }

        private void FftAnalyzerDdc1_FftCalculated(object sender, FftEventArgs e)
        {
            /*
            var bin = new FrequencyBins(e,
                new BinParameters
                {
                    Relative = true,
                    CarrierFrequency = Ddc1Frequency,
                    MinimumIntensityDb = -120,
                    SamplingRate = Ddc1Info.SampleRate,
                    BinsPerPoint = Ddc1SpectrumView.SelectedResolution.Resolution
                }, false);
            */

            Wait = true;

            var bins = new FastFrequencyBins(e,
                new BinParameters
                {
                    Relative = true,
                    CarrierFrequency = Ddc1Frequency,
                    MinimumIntensityDb = _model.Settings.Ddc1Parameter.MinimumIntensityDb,
                    SamplingRate = Ddc1Info.SampleRate,
                    BinsPerPoint = 1,
                    CalibrationAmplitude = _model.Settings.Ddc1Parameter.CalibrationAmplitude,
                    CalibrationOffset = _model.Settings.Ddc1Parameter.CalibrationOffset
                }, false);

            // Morse decoder

            if (SearchEventHandler != null)
                SearchEventHandler(this, new SearchEventArgs(bins));

            Wait = false;

            var intensities = bins.Intensities().ToList<double>().Where((x, i) => i % Ddc1SpectrumView.SelectedResolution.Resolution == 0);

            if (SpectrumAnalyzerDdc1 != null)
                SpectrumAnalyzerDdc1.Update(intensities.ToArray());

            MeterViewDdc1.Update(bins);

            /*
            FrequencyBins bin = new FrequencyBins(e, new BinParameters
            {
                Relative = true,
                CarrierFrequency = _model.Ddc1Frequency,
                MinimumIntensityDb = -150,
                SamplingRate = _model.Radio.Ddc1().DdcArgs().Info.SampleRate,
                BinsPerPoint = 4,
                CalibrationAmplitude = 1.1, //1.5,//1.0 in sweeper is better
                CalibrationOffset = 20,//8,
            });
            SpectrumAnalyzerDdc1.Update(bin);

            RecordingView.Tick();
            */
        }

        //////////////// Search

        public event EventHandler<SearchEventArgs> SearchEventHandler;

        public bool AutoStep
        {
            get { return _autoStep; }
            set
            {
                _autoStep = value;
                NotifyPropertyChanged("AutoStep");
            }
        }

        public uint FrequencyStep
        {
            get { return _frequencyStep; }
            set
            {
                _frequencyStep = value;
                NotifyPropertyChanged("FrequencyStep");
            }
        }

        public bool AfcState
        {
            get { return _afc; }
            set
            {
                _afc = value;
                NotifyPropertyChanged("AfcState");
            }
        }

        public bool MuteState
        {
            get { return _muteState; }
            set
            {
                _muteState = value;
                if (value)
                    Mute();
                else
                    Unmute();

                // UpdateVolume();
                NotifyPropertyChanged("MuteState");

                // var playing = _model.Ddc2[0].AudioPlayer != null;

                /*
                if (_muteState)
                    TaskUtility.Run(() => _model.LiveAudio.Stop());
                else
                    TaskUtility.Run(() => _model.LiveAudio.Play());
                */
            }
        }

        private void Unmute()
        {
            switch (ModeState)
            {
                case DemodulatorMode.Dsb:
                    _model.Ddc2[0].Unmute(true, false);
                    _model.Ddc2[1].Unmute(false, true);
                    break;

                default:
                    _model.Ddc2[0].Unmute();
                    _model.Ddc2[1].Mute();
                    break;
            }
        }

        private void Mute()
        {
            _model.Ddc2[0].Mute();
            _model.Ddc2[1].Mute();
        }

        public float Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
                UpdateVolume();
                NotifyPropertyChanged("Volume");
            }
        }

        private void UpdateVolume()
        {
            LiveAudioPlayer audioPlayer = _model.Ddc2[0].AudioPlayer;

            if (MuteState)
                return;

            float volume = !SquelchView.Squelched ? _volume : 0f;

            if (audioPlayer != null && _audioPlayerVolume != volume)
            {
                _audioPlayerVolume = volume;
                audioPlayer.VolumeLevel = volume;
            }
        }

        private void FftAnalyzerDdc2_FftCalculated(object sender, FftEventArgs e)
        {
            var bin = new FrequencyBins(e,
                new BinParameters
                {
                    MinimumIntensityDb = _model.Settings.Ddc2Parameter.MinimumIntensityDb,
                    SamplingRate = _model.Radio.Ddc2(0).DdcArgs().Info.SampleRate,
                    // CarrierFrequency = (uint)Ddc2AbsoluteFrequency,
                    BinsPerPoint = Ddc2SpectrumView.SelectedResolution.Resolution,
                    CalibrationAmplitude = _model.Settings.Ddc2Parameter.CalibrationAmplitude,
                    CalibrationOffset = _model.Settings.Ddc2Parameter.CalibrationOffset
                }, false);

            if (SpectrumAnalyzerDdc2 != null)
                SpectrumAnalyzerDdc2.Update(bin);

            /*
            FrequencyBins bin = new FrequencyBins(e, new BinParameters
            {
                MinimumIntensityDb = -150,
                SamplingRate = _model.Radio.Ddc2(_index).DdcArgs().Info.SampleRate,
                Relative = true,
                CarrierFrequency = _model.Ddc1Frequency,
                BinsPerPoint = 4,
                CalibrationAmplitude = 1.1, //1.5,//1.0 in sweeper is better
                CalibrationOffset = 20,//8,
            });
            */

            // MeterViewDemodulator.Update(e, bin);
            MeterViewDemodulator.Update(bin);

            SquelchView.UpdateSquelch(MeterViewDemodulator.RmsDbm);
            UpdateVolume();

            if (AutoStep)
                FrequencyStep = (uint)Math.Abs(MeterViewDemodulator.FrequencyErrorValue * 500000);

            if (AfcState)
            {
                if (Math.Abs(MeterViewDemodulator.FrequencyErrorValue * 1000000) > _model.Settings.AfcThreshold) //1000 as default
                    DemodulatorFrequency = (int)(MeterViewDemodulator.PeakFrequency * 1000000);
            }

            /* 
            RecordingView.Tick();
            */
        }

        /*
        public bool VideoFilter
        {
            get { return _videoFilter; }
            set
            {
                _videoFilter = value;
                if (_videoFilter)
                {
                    var points = _model.Radio.UsableDownSamples(_model.FftAnalyzerIf.FftLength, VideoPoints);
                    SpectrumAnalyzerIf.ReInitialize(points);
                }
                else
                {
                    SpectrumAnalyzerIf.ReInitialize();
                }

                NotifyPropertyChanged("VideoFilter");
            }
        }
        */

        /*
        public int VideoPoints
        {
            get { return _videoPoints; }
            set
            {
                _videoPoints = value;
                _videoPoints = _videoPoints < 10 ? 10 : _videoPoints > 10000 ? 10000 : _videoPoints;
                if (VideoFilter)
                {                    
                    var points = _model.Radio.UsableDownSamples(_model.FftAnalyzerIf.FftLength, _videoPoints);
                    SpectrumAnalyzerIf.ReInitialize(points);
                }
                NotifyPropertyChanged("VideoPoints");
            }
        }
        */

        /// <summary>
        /// Changing of DDC1 frequency causes change of absolute frequency of the DDC2 and demodulator in each channel.
        /// </summary>
        public uint Ddc1Frequency
        {
            get { return Radio.Ddc1Frequency; }
            set
            {
                Radio.Ddc1Frequency = value;
                NotifyPropertyChanged("Ddc1Frequency");
                NotifyPropertyChanged("Ddc2RelativeFrequency");
                NotifyPropertyChanged("Ddc2AbsoluteFrequency");
                NotifyPropertyChanged("DemodulatorFrequency");
                NotifyPropertyChanged("AbsoluteFrequency");

                UpdateSpectrum();
            }
        }

        /// <summary>
        /// Changing of DDC2 relative frequency changes absolute frequency of the DDC2 and demodulator in the specified channel.
        /// </summary>
        public int Ddc2RelativeFrequency
        {
            get { return _model.Ddc2[0].Ddc2Frequency; }
            set
            {
                if (value < int.MinValue || value > int.MaxValue)
                    return;

                _model.Ddc2[0].Ddc2Frequency = value;

                NotifyPropertyChanged("Ddc2RelativeFrequency");
                NotifyPropertyChanged("Ddc2AbsoluteFrequency");
                NotifyPropertyChanged("AbsoluteFrequency");

                UpdateSpectrum();
            }
        }

        public long Ddc2AbsoluteFrequency
        {
            get { return Ddc1Frequency + Ddc2RelativeFrequency; }
        }

        public uint AbsoluteFrequency
        {
            get { return _model.Ddc2[0].AbsoluteFrequency; }
            set
            {
                var frequency = _model.Limits.Frequency(value);
                _model.Ddc2[0].AbsoluteFrequency = frequency;
                _model.Ddc2[1].AbsoluteFrequency = frequency;

                long bandwidth = _model.Radio.Ddc1().DdcArgs().Info.Bandwidth;
                long sampleRate = _model.Radio.Ddc1().DdcArgs().Info.SampleRate;
                long ddc1Frequency = _model.Ddc1Frequency;

                NotifyPropertyChanged("Ddc1Frequency");
                NotifyPropertyChanged("Ddc2RelativeFrequency");
                NotifyPropertyChanged("Ddc2AbsoluteFrequency");
                NotifyPropertyChanged("AbsoluteFrequency");

                UpdateSpectrum();
            }
        }

        public uint DemodulatorBandwidth
        {
            get { return _model.Ddc2[0].DemodulatorBandwidth; }
            set
            {
                _model.Ddc2[0].DemodulatorBandwidth = value;
                NotifyPropertyChanged("DemodulatorBandwidth");
                if (Ddc1SpectrumView != null)
                    Ddc1SpectrumView.Update();
                if (Ddc2SpectrumView != null)
                    Ddc2SpectrumView.Update();
            }
        }

        public int DemodulatorFrequency
        {
            get { return _model.Ddc2[0].DemodulatorFrequency; }
            set
            {
                _model.Ddc2[0].DemodulatorFrequency = value;
                NotifyPropertyChanged("DemodulatorFrequency");
                NotifyPropertyChanged("AbsoluteFrequency");
                if (Ddc2SpectrumView != null)
                    Ddc2SpectrumView.Update();
            }
        }

        public DdcInfo Ddc2Info
        {
            get
            {
                uint index;
                DdcInfo info = _model.Radio.Ddc2Info(out index);
                return info;
            }
        }

        public DdcInfo Ddc1Info
        {
            get { return _model.Radio.Ddc1().DdcArgs().Info; }
        }

        public bool Wait { get; set; }

        #region Ddc2 Audio Filter & Audio Gain
        public bool AudioFilterState
        {
            get { return _model.Ddc2[0].AudioFilterActive && _model.Ddc2[1].AudioFilterActive; }
            set
            {
                _model.Ddc2[0].AudioFilterActive = value;
                _model.Ddc2[1].AudioFilterActive = value;
                NotifyPropertyChanged("AudioFilterState");
            }
        }

        public uint CutOffLow
        {
            get { return _model.Ddc2[0].AudioFilter.CutOffLow; }
            set
            {
                _model.Ddc2[0].AudioFilter = new AudioFilter() { CutOffLow = value, CutOffHigh = CutOffHigh, DeEmphasis = DeEmphasis };
                _model.Ddc2[1].AudioFilter = new AudioFilter() { CutOffLow = value, CutOffHigh = CutOffHigh, DeEmphasis = DeEmphasis };
                NotifyPropertyChanged("CutOffLow");
            }
        }

        public uint CutOffHigh
        {
            get { return _model.Ddc2[0].AudioFilter.CutOffHigh; }
            set
            {
                _model.Ddc2[0].AudioFilter = new AudioFilter() { CutOffLow = CutOffLow, CutOffHigh = value, DeEmphasis = DeEmphasis };
                _model.Ddc2[1].AudioFilter = new AudioFilter() { CutOffLow = CutOffLow, CutOffHigh = value, DeEmphasis = DeEmphasis };
                NotifyPropertyChanged("CutOffHigh");
            }
        }

        public double DeEmphasis
        {
            get { return _model.Ddc2[0].AudioFilter.DeEmphasis; }
            set
            {
                _model.Ddc2[0].AudioFilter = new AudioFilter() { CutOffLow = CutOffLow, CutOffHigh = CutOffHigh, DeEmphasis = value };
                _model.Ddc2[1].AudioFilter = new AudioFilter() { CutOffLow = CutOffLow, CutOffHigh = CutOffHigh, DeEmphasis = value };
                NotifyPropertyChanged("DeEmphasis");
            }
        }

        public double AudioGain
        {
            get { return _model.Ddc2[0].AudioGain; }
            set
            {
                _model.Ddc2[0].AudioGain = value;
                _model.Ddc2[1].AudioGain = value;
                NotifyPropertyChanged("AudioGain");
            }
        }
        #endregion

        #region Ddc2 Automatic Gain Control (AGC)
        public bool AgcState
        {
            get { return _model.Ddc2[0].AgcActive; }
            set
            {
                _model.Ddc2[0].AgcActive = value;
                _model.Ddc2[1].AgcActive = value;
                NotifyPropertyChanged("AgcState");
                NotifyPropertyChanged("AgcAttackTime");
                NotifyPropertyChanged("AgcDecayTime");
                NotifyPropertyChanged("AgcReferenceLevel");
                NotifyPropertyChanged("MaxAgcGain");
                NotifyPropertyChanged("Gain");
            }
        }

        public double AgcAttackTime
        {
            get { return _model.Ddc2[0].Agc.AttackTime; }
            set
            {
                _model.Ddc2[0].Agc = new SoftwareAgc() { AttackTime = value, DecayTime = AgcDecayTime, ReferenceLevel = AgcReferenceLevel };
                _model.Ddc2[1].Agc = new SoftwareAgc() { AttackTime = value, DecayTime = AgcDecayTime, ReferenceLevel = AgcReferenceLevel };
                NotifyPropertyChanged("AgcAttackTime");
                NotifyPropertyChanged("AgcDecayTime");
                NotifyPropertyChanged("AgcReferenceLevel");
                NotifyPropertyChanged("MaxAgcGain");
                NotifyPropertyChanged("Gain");
            }
        }

        public double AgcDecayTime
        {
            get { return _model.Ddc2[0].Agc.DecayTime; }
            set
            {
                _model.Ddc2[0].Agc = new SoftwareAgc() { AttackTime = AgcAttackTime, DecayTime = value, ReferenceLevel = AgcReferenceLevel };
                _model.Ddc2[1].Agc = new SoftwareAgc() { AttackTime = AgcAttackTime, DecayTime = value, ReferenceLevel = AgcReferenceLevel };
                NotifyPropertyChanged("AgcAttackTime");
                NotifyPropertyChanged("AgcDecayTime");
                NotifyPropertyChanged("AgcReferenceLevel");
                NotifyPropertyChanged("MaxAgcGain");
                NotifyPropertyChanged("Gain");
            }
        }

        public double AgcReferenceLevel
        {
            get { return _model.Ddc2[0].Agc.ReferenceLevel; }
            set
            {
                _model.Ddc2[0].Agc = new SoftwareAgc() { AttackTime = AgcAttackTime, DecayTime = AgcDecayTime, ReferenceLevel = value };
                _model.Ddc2[1].Agc = new SoftwareAgc() { AttackTime = AgcAttackTime, DecayTime = AgcDecayTime, ReferenceLevel = value };
                NotifyPropertyChanged("AgcAttackTime");
                NotifyPropertyChanged("AgcDecayTime");
                NotifyPropertyChanged("AgcReferenceLevel");
                NotifyPropertyChanged("MaxAgcGain");
                NotifyPropertyChanged("Gain");
            }
        }

        public double MaxAgcGain
        {
            get { return _model.Ddc2[0].MaxAgcGain; }
            set
            {
                _model.Ddc2[0].MaxAgcGain = value;
                _model.Ddc2[1].MaxAgcGain = value;
                NotifyPropertyChanged("AgcAttackTime");
                NotifyPropertyChanged("AgcDecayTime");
                NotifyPropertyChanged("AgcReferenceLevel");
                NotifyPropertyChanged("MaxAgcGain");
                NotifyPropertyChanged("Gain");
            }
        }

        public double Gain
        {
            get { return _model.Ddc2[0].Gain; }
            set
            {
                _model.Ddc2[0].Gain = value;
                _model.Ddc2[1].Gain = value;
                NotifyPropertyChanged("AgcAttackTime");
                NotifyPropertyChanged("AgcDecayTime");
                NotifyPropertyChanged("AgcReferenceLevel");
                NotifyPropertyChanged("MaxAgcGain");
                NotifyPropertyChanged("Gain");
            }
        }

        public Agc SoftwareAgcState
        {
            get { return _softwareAgc; }
            set
            {
                _softwareAgc = value;

                SoftwareAgc softwareAgc = new SoftwareAgc() { AttackTime = AgcAttackTime, DecayTime = AgcDecayTime, ReferenceLevel = AgcReferenceLevel };

                switch (_softwareAgc)
                {
                    case Agc.Off:
                        // _model.Radio.Demodulator().DisableSofwareAgc();
                        break;
                    case Agc.Slow:
                        softwareAgc = _model.Settings.SlowSagc;
                        break;
                    case Agc.Medium:
                        softwareAgc = _model.Settings.MediumSagc;
                        break;
                    case Agc.Fast:
                        softwareAgc = _model.Settings.FastSagc;
                        break;
                }
                softwareAgc.ReferenceLevel = AgcReferenceLevel;

                _model.Ddc2[0].Agc = softwareAgc;
                _model.Ddc2[1].Agc = softwareAgc;
                NotifyPropertyChanged("SoftwareAgcState");
                NotifyPropertyChanged("AgcAttackTime");
                NotifyPropertyChanged("AgcDecayTime");
                NotifyPropertyChanged("AgcReferenceLevel");
                NotifyPropertyChanged("MaxAgcGain");
                NotifyPropertyChanged("Gain");
            }
        }

        #endregion

        private static RadioViewModel _instance;
        private static object _instanceLock = new object();
        private static object _startupLock = new object();
        public static RadioViewModel GetInstance()
        {
            lock (_instanceLock)
            {
                if (_instance == null)
                    _instance = new RadioViewModel();
            }

            return _instance;
        }

        public void Dispose()
        {
            _model.Ddc2[0].FftAnalyzerDdc2.Stop();
            _model.Dispose();
        }
    }
}
