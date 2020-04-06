using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfControlTest.ViewModel
{
    class MainViewModel:ViewModelBase
    {
        protected static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private uint _frequency = 2000000;

        public uint Frequency
        {
            get
            { return _frequency; }
            set
            {
                _frequency = value;
                NotifyPropertyChanged("Frequency");
            }
        }

        public MainViewModel()
        {
            HoppingView = new HoppingViewModel();
        }

        public HoppingViewModel HoppingView { get; set; }       

    }
}
