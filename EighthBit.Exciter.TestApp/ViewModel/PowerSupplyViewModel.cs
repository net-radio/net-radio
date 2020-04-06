using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EighthBit.Exciter.TestApp.Model;

namespace EighthBit.Exciter.TestApp.ViewModel
{
    class PowerSupplyViewModel : MonitoringBaseViewModel
    {
        public PowerSupplyViewModel(ExciterModel exciterModel) 
            : base(exciterModel)
        {
        }

        public bool Power
        {
            get { return Exciter.PowerSupplyStatus.SupplyOn; }
        }

        public bool PowerGood
        {
            get { return Exciter.PowerSupplyStatus.PgL; }
        }

        public bool PsFault
        {
            get { return !Exciter.PowerSupplyStatus.PsFaultNot; }
        }

        public bool PsOn
        {
            get { return Exciter.PowerSupplyStatus.PsOn; }
        }

        internal override void Update()
        {
            NotifyPropertyChanged("Power");
            NotifyPropertyChanged("PowerGood");
            NotifyPropertyChanged("PsFault");
            NotifyPropertyChanged("PsOn");
        }
    }
}
