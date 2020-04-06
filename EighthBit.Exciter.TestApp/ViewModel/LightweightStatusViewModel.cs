using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EighthBit.Exciter.TestApp.Model;

namespace EighthBit.Exciter.TestApp.ViewModel
{
    class LightweightStatusViewModel : MonitoringBaseViewModel
    {
        public LightweightStatusViewModel(ExciterModel exciterModel)
            : base(exciterModel)
        {
        }

        public uint Power
        {
            get { return Exciter.ExciterInfo.Power; }
        }

        public uint Vswr
        {
            get { return Exciter.ExciterInfo.Vswr; }
        }

        public bool PowerOn
        {
            get { return Exciter.ExciterInfo.PowerOn; }
        }

        public bool Error
        {
            get { return Exciter.ExciterInfo.Error; }
        }

        public bool Warning
        {
            get { return Exciter.ExciterInfo.Warning; }
        }

        internal override void Update()
        {
            NotifyPropertyChanged("Power");
            NotifyPropertyChanged("Vswr");
            NotifyPropertyChanged("PowerOn");
            NotifyPropertyChanged("Error");
            NotifyPropertyChanged("Warning");
        }
    }
}
