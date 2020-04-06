using EighthBit.Exciter.Can;

namespace EighthBit.Exciter.Parsers
{
    public interface Rfcu : IParser
    {
        byte Module1Temp { get; }
        byte Module2Temp { get; }
        byte Module3Temp { get; }
        byte Module4Temp { get; }
        byte PreDrive1Temp { get; }
        byte CombinerTemp { get; }
        bool PowerOn { get; }
        bool VrMod1NotL { get; }
        bool VrMod2NotL { get; }
        bool VrMod3NotL { get; }
        bool VrMod4NotL { get; }
        bool VReflectNotLatched { get; }
        bool RfFaultNot { get; }
        bool ExtResetNot { get; }
        bool FuseNot { get; }
        bool FusePredNot { get; }
        bool FuseMod1Not { get; }
        bool FuseMod2Not { get; }
        bool FuseMod3Not { get; }
        bool FuseMod4Not { get; }
        bool FanOut { get; }
        bool Mod1FaultNot { get; }
        bool Mod2FaultNot { get; }
        bool Mod3FaultNot { get; }
        bool Mod4FaultNot { get; }
        bool PredOn { get; }
        bool Mod1On { get; }
        bool Mod2On { get; }
        bool Mod3On { get; }
        bool Mod4On { get; }
        bool MocPrdNotLatched { get; }
        bool Moc1NotLatched { get; }
        bool Moc2NotLatched { get; }
        bool Moc3NotLatched { get; }
        bool Moc4NotLatched { get; }
        bool OverTemp { get; }
        bool VswrEq { get; }
        bool TempValid { get; }
        byte A0Reserve { get; }
        byte A1VforMod2 { get; }
        byte A2VrefMod2 { get; }
        byte A3RfSamMod2 { get; }
        byte A4CurMod2 { get; }
        byte A5VrefEq { get; }
        byte A6RfSamMod3 { get; }
        byte A7VforMod4 { get; }
        byte A8VrefMod1 { get; }
        byte A9VforEq { get; }
        byte A10Reserve { get; }
        byte A11CurMod4 { get; }
        byte A12CurPrD { get; }
        byte A13VforMod1 { get; }
        byte A14VrefMod4 { get; }
        byte A15RfSamMod1 { get; }
        byte A16Reserve { get; }
        byte A17RfSamPrD { get; }
        byte A18Reserve { get; }
        byte A19VforMod3 { get; }
        byte A20VrefMod3 { get; }
        byte A21RfSameMod4 { get; }
        byte A22CurMod3 { get; }
        byte A23CurMod1 { get; }
    }
}
