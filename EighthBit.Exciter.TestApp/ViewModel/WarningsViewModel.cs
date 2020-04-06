using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EighthBit.Exciter.TestApp.Model;

namespace EighthBit.Exciter.TestApp.ViewModel
{
    class WarningsViewModel : MonitoringBaseViewModel
    {
        public WarningsViewModel(ExciterModel exciterModel) : base(exciterModel)
        {
        }

        public bool UnderVoltage
        {
            get { return Exciter.PowerSupplyStatus.UnderVoltageNot; }
        }

        public bool VswrModule
        {
            get { return Exciter.Warnings.VswrWarning != 0; }
        }

        public bool OverCurrentModule
        {
            get { return Exciter.Warnings.OverCurrentWarning != 0; }
        }

        public bool OverReflectModule
        {
            get { return Exciter.Warnings.OverReflectWarning != 0; }
        }

        public bool FuseModule
        {
            get { return Exciter.Warnings.FuseWarning != 0; }
        }

        public bool OverTemp
        {
            get { return Exciter.Warnings.TempWarning != 0; }
        }

        internal override void Update()
        {
            NotifyPropertyChanged("UnderVoltage");
            NotifyPropertyChanged("VswrModule");
            NotifyPropertyChanged("OverCurrentModule");
            NotifyPropertyChanged("OverReflectModule");
            NotifyPropertyChanged("FuseModule");
            NotifyPropertyChanged("OverTemp");
        }
    }
}
