using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter
{
    public interface IExciterSweepApi
    {
        IExciterSweepApi Sweep(uint step, ExciterModulation modulation, byte time);
        IExciterSweepApi SweepDomain(uint startFrequency, uint stopFrequency);
    }
}
