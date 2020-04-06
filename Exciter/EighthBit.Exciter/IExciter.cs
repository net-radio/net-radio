using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter
{
    public interface IExciter
    {
        IExciterController Controller { get; }
        IExciterApi ExciterApi { get; }
        IExciter Activate();
        IExciter Shutdown();
        ExciterOperationMode Mode { get; }
        ushort Power { get; set; }
        ushort PowerMinimum { get; }
        ushort PowerMaximum { get; }
    }
}
