using System.Threading;

namespace EighthBit.Exciter
{
    public class ExciterSpot : ExciterSubSystem, IExciter
    {
        private uint _frequency;
        private ExciterModulation _modulation;

        internal ExciterSpot(IExciter exciter)
            : base(exciter.ExciterApi)
        {
            controller_ = exciter.Controller;
        }

        public uint Frequency
        {
            get { return _frequency; }
            set
            {
                if (_frequency != value)
                {
                    _frequency = value;
                    if (Active && ImmediateApply)
                        SpotApi.Spot(_frequency, _modulation);
                }
            }
        }

        public ExciterModulation Modulation
        {
            get { return _modulation; }
            set
            {
                if (_modulation != value)
                {
                    _modulation = value;
                    if (Active && ImmediateApply)
                        SpotApi.Spot(_frequency, _modulation);
                }
            }
        }

        public override IExciter Activate()
        {
            base.Activate();
            SpotApi.Spot(_frequency, _modulation);
            return this;
        }

        public override IExciterController Controller
        {
            get { return controller_; }
        }

        protected IExciterSpotApi SpotApi
        {
            get { return ExciterApi as IExciterSpotApi; }
        }

        public override ExciterOperationMode Mode
        {
            get { return ExciterOperationMode.Spot; }
        }

        public override ushort Power
        {
            get { return Controller.Power; }
            set
            {
                Controller.Power = value;
            }
        }

        public override ushort PowerMinimum
        {
            get { return Controller.PowerMinimum; }
        }

        public override ushort PowerMaximum
        {
            get { return Controller.PowerMaximum; }
        }
    }
}
