using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using NetRadio.Devices.G313;
using NetRadio.G313.Model;
using NetRadio.G313.ViewModel;

namespace NetRadio.G313.PanelViewModel
{
    class BlockScanViewModel:ViewModelBase
    {
        private const string OFFLINE = "Offline";

        private readonly RadioModel _model;
        private readonly RadioViewModel _mainView;

        private uint _from;
        private uint _to;
        private int _squelch;
        private bool _scanState;

        private string _scanInfo;

        public bool ScanState
        {
            get { return _scanState; }
            set
            {
                _scanState = value;
                if (!_scanState)
                    ScanInfo = OFFLINE;

                OnPropertyChanged("ScanState");
            }
        }

        private ICollection<uint> FrequncyCollection()
        {
            var list = new List<uint>();
            for (var i = FromValue; i <= ToValue; i += 500)
                list.Add(i);
            return list;
        } 
        public string ScanInfo
        {
            get { return _scanInfo; }
            set
            {
                _scanInfo = value;
                OnPropertyChanged("ScanInfo");
            }
        }
        public uint FromValue
        {
            get{return _from;}
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

        public int SquelchValue
        {
            get { return _squelch; }
            set
            {
                _squelch = _model.Limits.Squelch(value);
                OnPropertyChanged("SquelchValue");
            }
        }

        public ICommand CommandScan { get; private set; }

        public BlockScanViewModel(RadioModel model,RadioViewModel view)
        {
            _model = model;
            _mainView = view;

            _model.Radio.BlockScanner().FrequencyScanned += BlockScanViewModel_FrequencyScanned;
            _model.Radio.BlockScanner().ScanFinished += BlockScanViewModel_ScanFinished;

            FromValue =(uint) _model.Radio.CachedInfo.MinFrequency;
            ToValue = (uint) _model.Radio.CachedInfo.MaxFrequency;
            SquelchValue = -85;
            ScanInfo = OFFLINE;

            CommandScan = new DelegateCommand(() => TaskUtility.Run(() =>
            {
                if (ScanState)
                {
                    _model.PauseStreams();
                    _model.Radio.BlockScanner().Start(FrequncyCollection());
                }
                else
                    _model.Radio.BlockScanner().Stop();
            }));
        }

        private void BlockScanViewModel_ScanFinished(object sender, EventArgs e)
        {
            Thread.Sleep(1000); //warmup

            ScanState = false;

            _mainView.FrequencyValue = _mainView.FrequencyValue;

            TaskUtility.Run(() =>
            {
                _model.FftAnalyzer.Stop(); //free buffer
                _model.FftAnalyzer.Start();
            });

            TaskUtility.Run(() =>
            {
                if (_mainView.MuteState) return;
                _model.LiveAudio.Stop(); //free buffer
                _model.LiveAudio.Play();
            });
        }

        private void BlockScanViewModel_FrequencyScanned(object sender, BlockScannerArgs e)
        {
            ScanInfo = string.Format("{0} KHz, {1} dBm", e.Frequency/1000, e.StrengthDbm);
            _mainView.SpectrumAnalyzer.Clear();

            if (e.StrengthDbm <= SquelchValue) return;
            _model.Radio.BlockScanner().Stop();
            _mainView.FrequencyValue = e.Frequency;
        }
    }
}
