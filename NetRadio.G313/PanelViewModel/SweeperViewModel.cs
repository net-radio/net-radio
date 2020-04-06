using System;
using System.Collections.Generic;
using System.Windows.Input;
using Arction.WPF.LightningChartUltimate.SeriesXY;
using NetRadio.Devices.G313;
using NetRadio.Devices.G313.Signal;
using NetRadio.G313.Model;
using NetRadio.G313.ViewModel;

namespace NetRadio.G313.PanelViewModel
{
    class SweeperViewModel:ViewModelBase
    {
        private readonly RadioModel _model;
        private readonly RadioViewModel _mainView;
        private G313Sweeper _sweeper;

        private uint _from;
        private uint _to;
        private double _precision;
        private bool _sweepState;

        private double _frequencyValue;
        private double _minValue;
        private double _maxValue;
        private double _currentValue;

        public double FrequencyValue
        {
            get { return _frequencyValue; }
            set
            {
                _frequencyValue = value;
                OnPropertyChanged("FrequencyValue");
            }
        }

        public double MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                OnPropertyChanged("MinValue");
            }
        }

        public double MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                OnPropertyChanged("MaxValue");
            }
        }

        public double CurrentValue
        {
            get { return _currentValue; }
            set
            {
                _currentValue = value;
                OnPropertyChanged("CurrentValue");
            }
        }

        public bool SweepState
        {
            get { return _sweepState; }
            set
            {
                _sweepState = value;
                OnPropertyChanged("SweepState");
            }
        }

        public uint FromValue
        {
            get { return _from; }
            set
            {
                _from = _model.Limits.Frequency(value);
                OnPropertyChanged("FromValue");
            }
        }

        public uint ToValue
        {
            get { return _to; }
            set
            {
                _to = _model.Limits.Frequency(value);
                OnPropertyChanged("ToValue");
            }
        }

        public double PrecisionValue
        {
            get { return _precision; }
            set
            {
                _precision = value > 10000 ? 10000 : value < 31.25 ? 31.25 : value;
                OnPropertyChanged("PrecisionValue");
            }
        }

        public bool MaxState
        {
            get { return _mainView.SweeperAnalyzer.VisibleMax(); }
            set
            {
                _mainView.SweeperAnalyzer.VisibleMax(value);
                OnPropertyChanged("MaxState");
            }
        }
        public bool CurrentState
        {
            get { return _mainView.SweeperAnalyzer.VisibleCurrent(); }
            set
            {
                _mainView.SweeperAnalyzer.VisibleCurrent(value);
                OnPropertyChanged("CurrentState");
            }
        }
        public bool MinState
        {
            get { return _mainView.SweeperAnalyzer.VisibleMin(); }
            set
            {
                _mainView.SweeperAnalyzer.VisibleMin(value);
                OnPropertyChanged("MinState");
            }
        }
        public ICommand CommandSweep { get; private set;}

        public SweeperViewModel(RadioModel model, RadioViewModel view)
        {
            _model = model;
            _mainView = view;

            _mainView.SweeperAnalyzer.SelectedValueChanged += SweeperAnalyzer_SelectedValueChanged;

            CommandSweep = new DelegateCommand(StartSweep);

            FromValue =(uint) _model.Radio.CachedInfo.MinFrequency;
            ToValue =(uint) _model.Radio.CachedInfo.MaxFrequency;
            PrecisionValue = 31.25;
        }

        void SweeperAnalyzer_SelectedValueChanged(double x, List<KeyValuePair<SampleDataSeries, double>> values)
        {
            FrequencyValue = x;
            MaxValue = values[0].Value;
            CurrentValue = values[1].Value;
            MinValue = values[2].Value;
        }

        private void StartSweep()
        {
            if (_sweeper!=null)
            {
                _sweeper.Stop();
                return;
            }

            SweepState = true;

            _sweeper =
                _model.Radio.Sweeper(
                    _model.Radio.SweepParametersDefault(FromValue, ToValue)
                        .SetPrecision(PrecisionValue)
                        .SetIfGain(_model.Radio.IfGain()));


            var samples=_sweeper.SampleCount();

            if (samples < 1000)
            {
                _sweeper = null;
                SweepState = false;
                return;
            }

            _mainView.SweeperAnalyzer.Clear();
            _mainView.SweeperAnalyzer.ReInitialize(FromValue/1000000.0, ToValue/1000000.0, samples);
            //_mainView.SweeperAnalyzer.SelectedValueChanged += SweeperAnalyzer_SelectedValueChanged;

            _sweeper.FrequencySweeped += (s, e) =>
            {
                _mainView.SpectrumAnalyzer.Clear();
                _mainView.SweeperAnalyzer.Update(e);          
            };
            _sweeper.SweepFinished += (s, e) =>
            {
                SweepState = false;

                TaskUtility.Run(() =>
                {
                    _model.FftAnalyzer.Stop(); //free buffer
                    _model.FftAnalyzer.Start();

                    if (_mainView.MuteState) return;
                    _model.LiveAudio.Stop(); //free buffer
                    _model.LiveAudio.Play();
                });

                _sweeper = null;
                GC.Collect();
            };

            _model.PauseStreams();
            _mainView.SpectrumAnalyzer.Clear();

            _sweeper.LoopAsync();
        }
    }
}
