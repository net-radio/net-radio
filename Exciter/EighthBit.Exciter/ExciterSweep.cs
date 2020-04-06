using System.Threading;

namespace EighthBit.Exciter
{
    public class ExciterSweep : ExciterSubSystem, IExciter
    {
        private uint frequencyStart_;
        private uint frequencyStop_;
        private uint step_;
        private ExciterModulation modulation_;
        private byte time_;
        
        public ExciterSweep(IExciter exciter)
            : base(exciter.ExciterApi)
        {
            controller_ = exciter.Controller;
        }

        public uint FrequencyStart
        {
            get { return frequencyStart_; }
            set
            {
                if (frequencyStart_ != value)
                {
                    frequencyStart_ = value;
                    if (Active && ImmediateApply)
                        SweepApi.SweepDomain(frequencyStart_, frequencyStop_);
                }
            }
        }

        public uint FrequencyStop
        {
            get { return frequencyStop_; }
            set
            {
                if (frequencyStop_ != value)
                {
                    frequencyStop_ = value;
                    if (Active && ImmediateApply)
                        SweepApi.SweepDomain(frequencyStart_, frequencyStop_);
                }
            }
        }

        public uint Step
        {
            get { return step_; }
            set
            {
                if (step_ != value)
                {
                    step_ = value;
                    if (Active && ImmediateApply)
                        SweepApi.Sweep(step_, modulation_, time_);
                }
            }
        }

        public ExciterModulation Modulation
        {
            get { return modulation_; }
            set
            {
                if (modulation_ != value)
                {
                    modulation_ = value;
                    if (Active && ImmediateApply)
                        SweepApi.Sweep(step_, modulation_, time_);
                }
            }
        }

        public byte Time
        {
            get { return time_; }
            set
            {
                if (time_ != value)
                {
                    time_ = value;
                    if (Active && ImmediateApply)
                        SweepApi.Sweep(step_, modulation_, time_);
                }
            }
        }        

        public override IExciter Activate()
        {
            SweepApi.Sweep(step_, modulation_, time_);
            Thread.Sleep(400);
            SweepApi.SweepDomain(frequencyStart_, frequencyStop_);
            return this;
        }

        public override IExciterController Controller
        {
            get { return controller_; }
        }

        public new IExciterApi ExciterApi
        {
            get { return base.ExciterApi; }
        }

        protected IExciterSweepApi SweepApi
        {
            get { return ExciterApi as IExciterSweepApi; }
        }

        public override ExciterOperationMode Mode
        {
            get { return ExciterOperationMode.Sweep; }
        }

        public override ushort Power
        {
            get
            {
                return Controller.Power;
            }
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
