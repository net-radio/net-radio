using EighthBit.Exciter.Origin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter
{
    class ExciterHopping : ExciterSubSystem, IExciterHopping
    {
        public ExciterHopping(IExciter exciter)
            : base(exciter.ExciterApi)
        {
            controller_ = exciter.Controller;
        }

        public override ushort Power
        {
            get { return Controller.Power; }
            set
            {
                Controller.Power = value;
            }
        }

        public override IExciterController Controller
        {
            get { return controller_; }
        }

        public override ExciterOperationMode Mode
        {
            get { return ExciterOperationMode.Hopping; }
        }

        public override ushort PowerMinimum
        {
            get { return Controller.PowerMinimum; }
        }

        public override ushort PowerMaximum
        {
            get { return Controller.PowerMaximum; }
        }

        protected IExciterHoppingApi HoppingApi
        {
            get { return ExciterApi as IExciterHoppingApi; }
        }

        public override IExciter Activate()
        {
            // logger.Info(string.Format("Apply modulation & power on hopping exciter. modulation: {0} exists: {1} power: {2}", modulation, exists, power));
            var exciter = base.Activate();
            HoppingApi.Apply(FrequencyStart, FrequencyStop, Modulation);
            return exciter;
        }

        private float _gain = 0.0f;
        private uint _frequencyStart = 8000000;
        private uint _frequencyStop = 12000000;
        private float _thresholdMinimum = -90.0f;
        private float _thresholdMaximum = -40.0f;
        private ExciterModulation _modulation = ExciterModulation.Am;

        public float Gain
        {
            get { return _gain; }
            set
            {
                _gain = value;
            }
        }

        public uint FrequencyStart
        {
            get { return _frequencyStart; }
            set
            {
                _frequencyStart = value;
            }
        }

        public uint FrequencyStop
        {
            get { return _frequencyStop; }
            set
            {
                _frequencyStop = value;
            }
        }

        public float ThresholdMinimum
        {
            get { return _thresholdMinimum; }
            set
            {
                _thresholdMinimum = value;
            }
        }

        public float ThresholdMaximum
        {
            get { return _thresholdMaximum; }
            set
            {
                _thresholdMaximum = value;
            }
        }

        public ExciterModulation Modulation
        {
            get { return _modulation; }
            set
            {
                _modulation = value;
            }
        }
    }
}

