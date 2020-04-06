using System.Runtime.InteropServices;
using EighthBit.Exciter.Can;

namespace EighthBit.Exciter.Parsers
{
    public interface Modulator : IParser
    {
        byte ASource1 { get; }
        byte NoiseBandwidth1 { get; }
        byte ASource2 { get; }
        byte NoiseBandwidth2 { get; }
        ushort Tone1 { get; }
        ushort Tone2 { get; }
        uint Frequency { get; }
        uint SweepStartFrequency { get; }
        uint SweepStopFrequency { get; }
        byte WarningFlag { get; }
        byte ErrorFlag { get; }
        byte VforEq { get; }
        byte VrefEq { get; }
        ExciterModulation Modulation { get; }
        uint StepFrequency { get; }
    }
}
