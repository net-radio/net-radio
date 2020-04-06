using EighthBit.Exciter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace NetRadio.G31Ddc.ViewModel.Exciter
{
    public abstract class ExciterOperationViewModel : ViewModelBase
    {
        private readonly IExciter _exciter;

        protected ExciterViewModel controllerViewModel_;
        public abstract IExciterController Controller { get; }

        public ICommand CommandApply { get; set; }
        public abstract void Apply();

        public IEnumerable<ExciterModulation> ExciterModulationList
        {
            get { return Enum.GetValues(typeof(ExciterModulation)).Cast<ExciterModulation>(); }
        }

        public ExciterOperationViewModel(IExciter exciter)
        {
            _exciter = exciter;
            CommandApply = new DelegateCommand(() => Apply());
        }

        public IExciter Exciter
        {
            get { return _exciter; }
        }

        internal ExciterViewModel ControllerViewModel
        {
            get { return controllerViewModel_; }
        }
    }
}
