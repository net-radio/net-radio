using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter
{
    interface IExciterHoppingApi : IExciterApi, IDisposable
    {
        IExciterHoppingApi Apply(uint frequencyStart, uint frequencyStop, ExciterModulation modulation);
    }
}
