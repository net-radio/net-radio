using NetRadio.Devices.G313;

namespace NetRadio.G313.Utilties.G313
{
    public class MemorySlot
    {
        public string Name { get; set; }
        public uint Frequency { get; set; }
        public uint IfBandwidth { get; set; }
        public int IfShift { get; set; }

        public G313Demodulator.DemodulatorMode Mode { get; set; }
        public int Squelch { get; set; }
    }
}
