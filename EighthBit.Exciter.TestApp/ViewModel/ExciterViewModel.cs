using EighthBit.Exciter.TestApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace EighthBit.Exciter.TestApp.ViewModel
{
    class ExciterViewModel : ViewModelBase, IDisposable
    {
        private ExciterModel _exciterModel;

        public ICommand CommandExciterInitialize { get; private set; }
        public ICommand CommandExciterUninitialize { get; private set; }
        public ICommand CommandSelfTest { get; private set; }

        public PowerSupplyViewModel PowerSupplyView { get; private set; }
        public PowerTray1ViewModel PowerTray1View { get; private set; }
        public PowerTray2ViewModel PowerTray2View { get; private set; }
        public WarningsViewModel WarningsView { get; private set; }
        public ErrorsViewModel ErrorsView { get; private set; }
        public LightweightStatusViewModel LightweightStatusView { get; private set; }
        public SelfTestViewModel SelfTestView { get; private set; }

        public ExciterViewModel()
        {
            InitializeExciter();

            PowerSupplyView = new PowerSupplyViewModel(_exciterModel);
            PowerTray1View = new PowerTray1ViewModel(_exciterModel);
            PowerTray2View = new PowerTray2ViewModel(_exciterModel);
            WarningsView = new WarningsViewModel(_exciterModel);
            ErrorsView = new ErrorsViewModel(_exciterModel);
            LightweightStatusView = new LightweightStatusViewModel(_exciterModel);
            SelfTestView = new SelfTestViewModel(_exciterModel);

            CommandExciterInitialize = new DelegateCommand(() =>
            {
                if (_exciterModel != null)
                    return;
            });

            CommandExciterUninitialize = new DelegateCommand(Dispose);

            CommandSelfTest = new DelegateCommand(() => _exciterModel.SelfTest());

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimerTick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            if (_exciterModel == null)
                return;

            PowerSupplyView.Update();
            PowerTray1View.Update();
            PowerTray2View.Update();
            WarningsView.Update();
            ErrorsView.Update();
            LightweightStatusView.Update();
            SelfTestView.Update();
        }

        private void InitializeExciter()
        {
            logger.Debug("Initializing exciter ...");
            try
            {
                _exciterModel = ExciterModel.Create();
            }
            catch (Exception ex)
            {
                logger.Error("Initializing exciter failed ...", ex);
                _exciterModel = ExciterModel.Demo();
            }
        }

        public void Dispose()
        {
            if (_exciterModel != null)
            {
                _exciterModel.Dispose();
                _exciterModel = null;
            }
        }
    }
}
