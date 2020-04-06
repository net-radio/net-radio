using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EighthBit.Exciter.TestApp.Model;

namespace EighthBit.Exciter.TestApp.ViewModel
{
    class ErrorsViewModel : MonitoringBaseViewModel
    {
        public ErrorsViewModel(ExciterModel exciterModel) 
            : base(exciterModel)
        {
        }

        public bool PowerGoodFailure
        {
            get { return !Exciter.PowerSupplyStatus.PgL; }
        }

        public bool PsFault
        {
            get { return !Exciter.PowerSupplyStatus.PsFaultNot; }
        }

        public bool RfFault
        {
            get { return !Exciter.Tray1Status.RfFaultNot; }
        }

        public bool FuseFailure
        {
            get
            {
                bool accurate = Exciter.PowerSupplyStatus.FuseNot
                    && Exciter.Tray1Status.FuseNot && Exciter.Tray1Status.FusePredNot;
                if (Exciter.Tray2Status != null)
                    accurate = accurate && Exciter.Tray2Status.FuseNot && Exciter.Tray2Status.FusePredNot;
                return !accurate;
            }
        }

        internal override void Update()
        {
            NotifyPropertyChanged("PowerGoodFailure");
            NotifyPropertyChanged("PsFault");
            NotifyPropertyChanged("RfFault");
            NotifyPropertyChanged("FuseFailure");
        }
    }
}
