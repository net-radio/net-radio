using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EighthBit.Exciter
{
    public class ExciterMultiSpot : ExciterSubSystem, IExciter
    {
        internal ExciterMultiSpot(IExciter exciter)
            : base(exciter.ExciterApi)
        {
            controller_ = exciter.Controller;
        }

        private readonly uint[] _frequency = new uint[16];
        private readonly byte[] _time = new byte[16];

        private ExciterModulation _modulation;
        private byte _spotCounts;

        protected IExciterMultiSpotApi ExciterMultiSpotApi 
        {
            get { return base.ExciterApi as IExciterMultiSpotApi; }
        }

        public ExciterModulation ModulationActive
        {
            get { return _modulation; }
            set
            {
                _modulation = value;
                ExciterMultiSpotApi.MultiSpot(_spotCounts, _modulation);
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

        public byte SpotCountsActive
        {
            get { return _spotCounts; }
            set
            {
                _spotCounts = value;
                ExciterMultiSpotApi.MultiSpot(_spotCounts, _modulation);
            }
        }

        public byte SpotCounts
        {
            get { return _spotCounts; }
            set
            {
                _spotCounts = value;
            }
        }

        public void DetermineSpotpoints()
        {
            SpotCounts = (byte)_time.Count(t => t > 0);
        }

        public Tuple<uint, byte> this[byte index, bool active = true]
        {
            get { return new Tuple<uint, byte>(_frequency[index], _time[index]); }
            set
            {
                _frequency[index] = value.Item1;
                _time[index] = value.Item2;
                if (active)
                    ExciterMultiSpotApi.MultiSpotPoint(index, _frequency[index], _time[index]);
            }
        }


        public override IExciter Activate()
        {
            Task.Factory.StartNew(() =>
            {
                ApplySpotPoints();
                ExciterMultiSpotApi.MultiSpot(_spotCounts, _modulation);
            });
            return this;
        }

        public void ApplySpotPoints()
        {
            for (byte i = 0; i < 16; i++)
            {
                ExciterMultiSpotApi.MultiSpotPoint(i, _frequency[i], _time[i]);
                Thread.Sleep(400);
            }
        }

        public override IExciterController Controller
        {
            get { return controller_; }
        }

        public new IExciterApi ExciterApi
        {
            get { return base.ExciterApi; }
        }

        protected IExciterMultiSpotApi MultiSpotApi
        {
            get { return ExciterApi as IExciterMultiSpotApi; }
        }

        public override ExciterOperationMode Mode
        {
            get { return ExciterOperationMode.MultiSpot; }
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
