using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.ViewModel.Exciter.Monitoring
{
    public class PowerTrayViewModel : MonitoringBaseViewModel
    {
        public PowerTrayViewModel(ExciterViewModel exciterViewModel)
            : base(exciterViewModel.ModelExciter)
        {
        }

        public bool PreDrive
        {
            get { return Exciter.Tray1Status.PredOn; }
        }

        public bool Module1
        {
            get { return Exciter.Tray1Status.Mod1On; }
        }

        public bool Module2
        {
            get { return Exciter.Tray1Status.Mod2On; }
        }

        public bool Module3
        {
            get { return Exciter.Tray1Status.Mod3On; }
        }

        public bool Module4
        {
            get { return Exciter.Tray1Status.Mod4On; }
        }

        internal override void Update()
        {
            NotifyPropertyChanged("PreDrive");
            NotifyPropertyChanged("Module1");
            NotifyPropertyChanged("Module2");
            NotifyPropertyChanged("Module3");
            NotifyPropertyChanged("Module4");
        }
    }
}
