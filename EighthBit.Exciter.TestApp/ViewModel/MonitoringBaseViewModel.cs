using EighthBit.Exciter.TestApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighthBit.Exciter.TestApp.ViewModel
{
    abstract class MonitoringBaseViewModel : ViewModelBase
    {
        private readonly ExciterModel exciterModel_;

        public MonitoringBaseViewModel(ExciterModel exciterModel)
        {
            exciterModel_ = exciterModel;
        }

        public ExciterModel Exciter
        {
            get { return exciterModel_; }
        }

        internal abstract void Update();    
    }
}
