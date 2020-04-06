using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EighthBit.Exciter.TestApp.Model;

namespace EighthBit.Exciter.TestApp.ViewModel
{
    class PowerTray1ViewModel : MonitoringBaseViewModel
    {
        public PowerTray1ViewModel(ExciterModel exciterModel) 
            : base(exciterModel)
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
