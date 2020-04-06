using EighthBit.Exciter.Configuration.Comb;
using EighthBit.Exciter.Origin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter
{
    public interface IExciterComb : IExciter
    {
        double PeakToAvaragePowerRatio { get; set; }
        bool AmpiltudeOver { get; set; }
        void PrepareSignals(SignalSpecification[] signals);
        void Apply(ExciterModulation modulation, ushort exists, ushort power);
    }
}
