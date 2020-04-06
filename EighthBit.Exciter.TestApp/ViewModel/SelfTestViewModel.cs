using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EighthBit.Exciter.TestApp.Model;

namespace EighthBit.Exciter.TestApp.ViewModel
{
    class SelfTestViewModel : MonitoringBaseViewModel
    {
        public SelfTestViewModel(ExciterModel exciterModel)
            : base(exciterModel)
        {
        }

        public bool Initialize
        {
            get { return Exciter.Warnings.Initialize; }
        }

        public bool Supply
        {
            get { return Exciter.Warnings.Supply; }
        }

        public bool OutputProbe
        {
            get { return Exciter.Warnings.OutputProbe; }
        }

        public bool PreDrive1
        {
            get { return Exciter.Warnings.PreDrive1; }
        }

        public bool Module1
        {
            get { return Exciter.Warnings.Module1; }
        }

        public bool Module2
        {
            get { return Exciter.Warnings.Module2; }
        }

        public bool Module3
        {
            get { return Exciter.Warnings.Module3; }
        }

        public bool Module4
        {
            get { return Exciter.Warnings.Module4; }
        }

        public bool PreDrive2
        {
            get { return Exciter.Warnings.PreDrive2; }
        }

        public bool Module5
        {
            get { return Exciter.Warnings.Module5; }
        }

        public bool Module6
        {
            get { return Exciter.Warnings.Module6; }
        }

        public bool Module7
        {
            get { return Exciter.Warnings.Module7; }
        }

        public bool Module8
        {
            get { return Exciter.Warnings.Module8; }
        }

        internal override void Update()
        {
            NotifyPropertyChanged("Initialize");
            NotifyPropertyChanged("PreDrive1");
            NotifyPropertyChanged("PreDrive2");
            NotifyPropertyChanged("OutputProbe");
            NotifyPropertyChanged("Supply");
            NotifyPropertyChanged("Module1");
            NotifyPropertyChanged("Module2");
            NotifyPropertyChanged("Module3");
            NotifyPropertyChanged("Module4");
            NotifyPropertyChanged("Module5");
            NotifyPropertyChanged("Module6");
            NotifyPropertyChanged("Module7");
            NotifyPropertyChanged("Module8");
        }
    }
}
