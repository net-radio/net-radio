using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EighthBit.Exciter.Can;
using EighthBit.Nmg.ManagedCan;
using EighthBit.Nmg.ManagedDemoCan;

namespace EighthBit.Exciter.TestApp.Model
{
    class ExciterModel : ExciterManager
    {
        private static ExciterModel _exciter;

        public ExciterModel(ICanControl control) 
            : base(control)
        {
        }

        public static ExciterModel Create()
        {
            if (_exciter == null)
            {
                logger.Error("Exciter not found");
                logger.Warn("Initialize Exciter Can in demo mode with Baud Rate 50Kbs");
                var can = new NmgManagedCanControl();
                can.Initialize(ExciterCanBaudRate.C50Kbs);
                _exciter = new ExciterModel(can);
            }

            return _exciter;
        }

        internal static ExciterModel Demo()
        {
            if (_exciter == null)
            {
                _exciter = new ExciterModel(new NmgManagedDemoCanControl());
            }
            return _exciter;
        }
    }
}
