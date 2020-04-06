using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter
{
    public interface IExciterCombApi
    {
        IExciterCombApi SendCommand(byte[] command, uint param);
        IExciterCombApi StartDataTransfer(byte numberOfPackets);
        IExciterCombApi Frequency(byte id, uint frequency, double phase);
        IExciterCombApi Comb(ExciterModulation modulation, ushort frequencies, ushort amplitude);
    }
}
