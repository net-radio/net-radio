using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Nmg.ManagedCanToUsb
{
    public enum CanBaudRate : byte
    {
        C10Kbs = 1,
        C15Kbs = 2,
        C20Kbs = 3,
        C32Kbs = 4,
        C40Kbs = 5,
        C50Kbs = 6,
        C80Kbs = 7,
        C100Kbs = 8,
        C125Kbs = 9,
        C200Kbs = 10,
        C250Kbs = 11,
        C320Kbs = 12,
        C400Kbs = 13,
        C500Kbs = 14,
        C666Kbs = 15,
        C800Kbs = 16,
        C1000Kbs = 17,
    }
}
