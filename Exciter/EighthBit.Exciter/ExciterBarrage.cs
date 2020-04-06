using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter
{
    public class ExciterBarrage : ExciterSubSystem, IExciter
    {
        private uint _frequency;
        private ushort _bandwidth;

        internal ExciterBarrage(IExciter ctrl)
            : base(ctrl.ExciterApi)
        {
            controller_ = ctrl.Controller;
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
                        BarrageApi.Barrage(_frequency, _bandwidth);
                }
            }
        }

        public ushort Bandwidth
        {
            get { return _bandwidth; }
            set
            {
                if (_bandwidth != value)
                {
                    _bandwidth = value;
                    if (Active && ImmediateApply)
                        BarrageApi.Barrage(_frequency, _bandwidth);
                }
            }
        }

        protected IExciterBarrageApi BarrageApi
        {
            get { return ExciterApi as IExciterBarrageApi; }
        }

        public override IExciterController Controller
        {
            get { return controller_; }
        }

        public override IExciter Activate()
        {
            base.Activate();
            BarrageApi.Barrage(_frequency, _bandwidth);
            return this;
        }

        public override ExciterOperationMode Mode
        {
            get { return ExciterOperationMode.Barrage; }
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
