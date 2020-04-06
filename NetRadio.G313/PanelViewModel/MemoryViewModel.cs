using System.Windows.Input;
using NetRadio.Devices.G313;
using NetRadio.G313.Model;
using NetRadio.G313.Utilties.G313;
using NetRadio.G313.ViewModel;

namespace NetRadio.G313.PanelViewModel
{
    class MemoryViewModel : ViewModelBase
    {
        private readonly RadioModel _model;
        private readonly RadioViewModel _mainView;

        private MemorySlot _selected;

        public MemorySlot SelectedMemory
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged("SelectedMemory");
                OnPropertyChanged("SelectedName");
            }
        }

        public string SelectedName
        {
            get { return _selected.Name; }
            set
            {
                _selected.Name = value;
                OnPropertyChanged("SelectedName");
            }
        }

        public uint SelectedFrequency
        {
            get { return _selected.Frequency; }
            set
            {
                _selected.Frequency = _model.Limits.Frequency(value);
                OnPropertyChanged("SelectedFrequency");
            }
        }

        public uint SelectedIfBandwidth
        {
            get { return _selected.IfBandwidth; }
            set
            {
                _selected.IfBandwidth = _model.Limits.IfBandwidth(value);
                OnPropertyChanged("SelectedIfBandwidth");
            }
        }

        public int SelectedIfShift
        {
            get { return _selected.IfShift; }
            set
            {
                _selected.IfShift = _model.Limits.IfShift(value);
                OnPropertyChanged("SelectedIfShift");
            }
        }

        public int SelectedSquelch
        {
            get { return _selected.Squelch; }
            set
            {
                _selected.Squelch = _model.Limits.Squelch(value);
                OnPropertyChanged("SelectedSquelch");
            }
        }

        public G313Demodulator.DemodulatorMode SelectedMode
        {
            get { return _selected.Mode; }
            set
            {
                _selected.Mode = value;
                OnPropertyChanged("SelectedMode");
            }
        }


        public ICommand CommandSave { get; private set; }
        public ICommand CommandLoad { get; private set; }
        public ICommand CommandLast { get; private set; }
        public ICommand CommandFirst { get; private set; }
        public ICommand CommandNext { get; private set; }
        public ICommand CommandPrevious { get; private set; }
        public ICommand CommandDelete { get; private set; }
        public ICommand CommandDeleteAll { get; private set; }

        public MemoryViewModel(RadioModel model, RadioViewModel view)
        {
            _mainView = view;
            _model = model;

            SelectFirst();

            CommandSave = new DelegateCommand(() =>
            {
                SelectedMemory = new MemorySlot();

                SelectedName = string.Format("{0} {1}", _model.Radio.Demodulator().Mode(), (_model.Radio.Frequency() / 1000).ToString("#.00"));
                SelectedFrequency = _model.Radio.Frequency();
                SelectedIfBandwidth = _model.Radio.Demodulator().IfBandwidth();
                SelectedIfShift = _model.Radio.Demodulator().IfShift();
                SelectedSquelch = _mainView.SquelchValue;
                SelectedMode = _model.Radio.Demodulator().Mode();

                _model.Memory.Add(SelectedMemory);
                _model.Memory.Save();
            });

            CommandLoad = new DelegateCommand(() =>
            {
                if (_selected.Frequency == default(uint))
                    return;

                _mainView.ModeState = _selected.Mode;
                _mainView.FrequencyValue = _selected.Frequency;
                _mainView.IfBandwidthValue = _selected.IfBandwidth;
                _mainView.IfShiftValue = _selected.IfShift;
                _mainView.SquelchValue = _selected.Squelch;

            });

            CommandNext = new DelegateCommand(() =>
            {
                if (_model.Memory.Count == 0)
                    return;

                var index = _model.Memory.IndexOf(SelectedMemory);
                if (index == -1)
                    return;

                index++;

                index = index >= _model.Memory.Count - 1 ? _model.Memory.Count - 1 : index;

                SelectedMemory = _model.Memory[index];
                //CommandLoad.Execute(null);
            });

            CommandPrevious = new DelegateCommand(() =>
            {
                if (_model.Memory.Count == 0)
                    return;

                var index = _model.Memory.IndexOf(SelectedMemory);
                if (index == -1)
                    return;

                index--;

                index = index <= 0 ? 0 : index;

                SelectedMemory = _model.Memory[index];
                //CommandLoad.Execute(null);
            });

            CommandLast = new DelegateCommand(() =>
            {
                if (_model.Memory.Count == 0)
                    return;

                SelectedMemory = _model.Memory[_model.Memory.Count - 1];
                //CommandLoad.Execute(null);
            });

            CommandFirst = new DelegateCommand(() =>
            {
                if (_model.Memory.Count == 0)
                    return;

                SelectedMemory = _model.Memory[0];
                //CommandLoad.Execute(null);
            });

            CommandDelete = new DelegateCommand(() =>
            {
                _model.Memory.Remove(SelectedMemory);
                _model.Memory.Save();
                SelectFirst();
            });

            CommandDeleteAll = new DelegateCommand(() =>
            {
                _model.Memory.Clear();
                _model.Memory.Save();
                SelectFirst();
            });
        }

        private void SelectFirst()
        {
            SelectedMemory = _model.Memory.Count == 0 ? new MemorySlot() : _model.Memory[0];
        }
    }
}
