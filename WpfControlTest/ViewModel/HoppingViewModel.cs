using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfControlTest.ViewModel
{
    public class HoppingViewModel : ViewModelBase
    {
        public ICommand CommandStart { get; set; }
        public ICommand CommandStop { get; set; }
        public ICommand AddItem { get; private set; }

        private uint _frequencyStart = 10000;
        private uint _frequencyStop = 20000;

        public uint FrequencyStart
        {
            get
            {
                Logger.Info("FrequencyStart : " + _frequencyStart);
                return _frequencyStart;
            }
            set
            {
                Logger.Info("Setting FrequencyStart : " + value);
                _frequencyStart = value;
                NotifyPropertyChanged("FrequencyStart");
            }
        }

        public uint FrequencyStop
        {
            get { return _frequencyStop; }
            set
            {
                _frequencyStop = value;
                NotifyPropertyChanged("FrequencyStop");
            }
        }

        public HoppingViewModel()
        {
            AddItem = new DelegateCommand(() => Logger.Info("AddItem"));
            CommandStart = new DelegateCommand(() => Logger.Info("Hopping Started"));
            CommandStop = new DelegateCommand(() => Logger.Info("Hopping Stopped"));
        }
    }
}
