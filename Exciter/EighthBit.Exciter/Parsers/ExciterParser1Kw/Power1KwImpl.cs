using System.Runtime.InteropServices;

namespace EighthBit.Exciter.Parsers.ExciterParser1Kw
{
    [StructLayout(LayoutKind.Explicit)]
    public class Power1KwImpl : Power, IParser
    {
        public const uint ID_POWER_INFO = 58;

        [FieldOffset(1)]
        private byte _powerOnMsb;
        [FieldOffset(0)]
        private byte _powerOnLsb;
        [FieldOffset(0)]
        private ushort _powerOn;

        [FieldOffset(3)]
        private byte _powerOffMsb;
        [FieldOffset(2)]
        private byte _powerOffLsb;
        [FieldOffset(2)]
        private ushort _powerOff;


        public ushort PowerOn
        {
            get { return _powerOn; }
        }

        public ushort PowerOff
        {
            get { return _powerOff; }
        }

        public void Update(Can.CanFrame frame)
        {
            if (frame.Id == ID_POWER_INFO && frame[0] == 80)
            {
                _powerOnMsb = frame[1];
                _powerOnLsb = frame[2];
                return;
            }

            if (frame.Id == ID_POWER_INFO && frame[0] == 79)
            {
                _powerOffMsb = frame[1];
                _powerOffLsb = frame[2];
            }

        }

        internal Power1KwImpl(IDispatcher dispatcher)
        {
            dispatcher.Register(ID_POWER_INFO,this);
        }
    }
}
