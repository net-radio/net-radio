using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter
{
    public interface IExciterBarrageApi : IExciterApi
    {
        IExciterBarrageApi Barrage(uint frequency, ushort bandwidth);
    }
}
