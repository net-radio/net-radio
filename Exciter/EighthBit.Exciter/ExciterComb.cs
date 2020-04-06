using EighthBit.Exciter.Origin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter
{
    class ExciterComb : ExciterSubSystem, IExciterComb
    {
        private double _peakToAvaragePowerRatio;
        private ushort power_;

        public ExciterComb(IExciter exciter)
            : base(exciter.ExciterApi)
        {
            controller_ = exciter.Controller;
        }

        public override IExciterController Controller
        {
            get { return controller_; }
        }

        public override ExciterOperationMode Mode
        {
            get { return ExciterOperationMode.Comb; }
        }

        public override ushort Power
        {
            get { return power_; }
            set
            {
                power_ = value;

                /*
                var refMax = CombView.RefMax;

                if (combPower > refMax)
                    combPower = refMax;
                */

                SetPower(power_);
            }
        }

        private void SetPower(ushort power_)
        {            
        }

        public override ushort PowerMinimum
        {
            get { return 0; }
        }

        public override ushort PowerMaximum
        {
            get { return 26; }
        }

        public double PeakToAvaragePowerRatio
        {
            get { return _peakToAvaragePowerRatio; }
            set
            {
                _peakToAvaragePowerRatio = value;
            }
        }

        public bool AmpiltudeOver
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void PrepareSignals(SignalSpecification[] signals)
        {
            for (byte i = 0; i < signals.Length; i++)
            {
                if (signals[i].Enabled)
                {
                    logger.Info(string.Format("Apply frequency on comb exciter ID: {0} Frequency: {1} Phase: {2}", i+1, signals[i].Frequency, signals[i].Phase));
                    CombApi.Frequency((byte)(i + 1), signals[i].Frequency, signals[i].Phase);
                }
            }
        }

        protected IExciterCombApi CombApi
        {
            get { return ExciterApi as IExciterCombApi; }
        }

        public void Apply(ExciterModulation modulation, ushort exists, ushort power)
        {
            power = 16384;
            logger.Info(string.Format("Apply modulation & power on comb exciter. modulation: {0} exists: {1} power: {2}", modulation, exists, power));
            CombApi.Comb(modulation, exists, power);
        }        
    }
}
