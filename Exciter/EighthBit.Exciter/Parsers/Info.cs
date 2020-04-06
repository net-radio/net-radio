using EighthBit.Exciter.Can;

namespace EighthBit.Exciter.Parsers
{
    public interface Info : IParser
    {
        byte RemoteSelfTest1 { get; }
        byte RemoteSelfTest2 { get; }
        byte TempWarning { get; }
        byte OverReflectWarning { get; }
        byte VswrWarning { get; }
        byte OverCurrentWarning { get; }
        byte FuseWarning { get; }
        byte GlobalWarning { get; }
        bool Initialize { get; }
        bool Supply { get; }
        bool PreDrive1 { get; }
        bool PreDrive2 { get; }
        bool Module1 { get; }
        bool Module2 { get; }
        bool Module3 { get; }
        bool Module4 { get; }
        bool Module5 { get; }
        bool Module6 { get; }
        bool Module7 { get; }
        bool Module8 { get; }
        bool OutputProbe { get; }
        ushort Power { get; }
        ushort Vswr { get; }
        bool PowerOn { get; }
        bool Error { get; }
        bool Warning { get; }
    }
}
