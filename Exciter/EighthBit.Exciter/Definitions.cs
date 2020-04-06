using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter
{
    public enum ExciterNoise : byte
    {
        Off = 0,
        R1 = 1,
        R3 = 3,
        R6 = 6,
        R13 = 13,
        R25 = 25
    }

    public enum ExciterModulation : byte
    {
        Cw = 65,
        Am = 66,
        Fm = 67,
        Usb = 68,
        Lsb = 69,
        Dsb = 70,
        Isb = 71
    }

    public enum ExciterOperationMode : byte
    {
        Spot = 0,
        MultiSpot = 1,
        Sweep = 2,
        Barrage = 3,
        Comb= 4,
        Hopping = 5,
    }

    public enum AccessMode : byte
    {
        Local = 0,
        Remote = 1
    }

    public enum ExciterCanBaudRate : byte
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
    
    public enum ExciterOutputLine : byte
    {
        Line1,
        Line2,
        Mic,
        Mmc,
        Tone,
        Noise
    }

    public enum Rfcus
    {
        Rfcu1,
        Rfcu2
    }
}
