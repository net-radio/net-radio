using EighthBit.Exciter.Can;

namespace EighthBit.Exciter.Parsers.ExciterParser1Kw
{
    public class Rfcu1KwImpl : Rfcu, IParser
    {        

        public const uint ID_RFCU1_TEMP = 53;
        public const uint ID_RFCU1_STATUS = 54;
        public const uint ID_RFCU1_ANALOG0_7 = 55;
        public const uint ID_RFCU1_ANALOG8_15 = 56;
        public const uint ID_RFCU1_ANALOG16_23 = 57;

        public const uint ID_RFCU2_TEMP = 67;
        public const uint ID_RFCU2_STATUS = 68;
        public const uint ID_RFCU2_ANALOG0_7 = 69;
        public const uint ID_RFCU2_ANALOG8_15 = 70;
        public const uint ID_RFCU2_ANALOG16_23 = 71;

        private byte[] _status=new byte[8];

        public byte Module1Temp { get; private set; }
        public byte Module2Temp { get; private set; }
        public byte Module3Temp { get; private set; }
        public byte Module4Temp { get; private set; }
        public byte PreDrive1Temp { get; private set; }
        public byte CombinerTemp { get; private set; }


        public bool PowerOn
        {
            get { return (_status[0] & 1) > 0; }
        }

        public bool VrMod1NotL
        {
            get { return (_status[0] & 2) > 0; }
        }

        public bool VrMod2NotL
        {
            get { return (_status[0] & 4) > 0; }
        }

        public bool VrMod3NotL
        {
            get { return (_status[0] & 8) > 0; }
        }

        public bool VrMod4NotL
        {
            get { return (_status[0] & 16) > 0; }
        }

        public bool VReflectNotLatched
        {
            get { return (_status[0] & 32) > 0; }
        }

        public bool RfFaultNot
        {
            get { return (_status[0] & 64) > 0; }
        }

        public bool ExtResetNot
        {
            get { return (_status[0] & 128) > 0; }
        }

        public bool FuseNot
        {
            get { return (_status[1] & 1) > 0; }
        }

        public bool FusePredNot
        {
            get { return (_status[1] & 2) > 0; }
        }

        public bool FuseMod1Not
        {
            get { return (_status[1] & 4) > 0; }
        }

        public bool FuseMod2Not
        {
            get { return (_status[1] & 8) > 0; }
        }

        public bool FuseMod3Not
        {
            get { return (_status[1] & 16) > 0; }
        }

        public bool FuseMod4Not
        {
            get { return (_status[1] & 32) > 0; }
        }

        public bool FanOut
        {
            get { return (_status[1] & 64) > 0; }
        }

        public bool Mod1FaultNot
        {
            get { return (_status[1] & 128) > 0; }
        }

        public bool Mod2FaultNot
        {
            get { return (_status[2] & 1) > 0; }
        }

        public bool Mod3FaultNot
        {
            get { return (_status[2] & 2) > 0; }
        }

        public bool Mod4FaultNot
        {
            get { return (_status[2] & 4) > 0; }
        }

        public bool PredOn
        {
            get { return (_status[2] & 8) > 0; }
        }

        public bool Mod1On
        {
            get { return (_status[2] & 16) > 0; }
        }

        public bool Mod2On
        {
            get { return (_status[2] & 32) > 0; }
        }

        public bool Mod3On
        {
            get { return (_status[2] & 64) > 0; }
        }

        public bool Mod4On
        {
            get { return (_status[2] & 128) > 0; }
        }

        public bool MocPrdNotLatched
        {
            get { return (_status[3] & 1) > 0; }
        }

        public bool Moc1NotLatched
        {
            get { return (_status[3] & 2) > 0; }
        }

        public bool Moc2NotLatched
        {
            get { return (_status[3] & 4) > 0; }
        }

        public bool Moc3NotLatched
        {
            get { return (_status[3] & 8) > 0; }
        }

        public bool Moc4NotLatched
        {
            get { return (_status[3] & 16) > 0; }
        }

        public bool OverTemp
        {
            get { return (_status[3] & 32) > 0; }
        }

        public bool VswrEq
        {
            get { return (_status[3] & 64) > 0; }
        }

        public bool TempValid
        {
            get { return (_status[3] & 128) > 0; }
        }


        public byte A0Reserve
        {
            get; private set;
        }

        public byte A1VforMod2
        {
            get;
            private set;
        }

        public byte A2VrefMod2
        {
            get;
            private set;
        }

        public byte A3RfSamMod2
        {
            get;
            private set;
        }

        public byte A4CurMod2
        {
            get;
            private set;
        }

        public byte A5VrefEq
        {
            get;
            private set;
        }

        public byte A6RfSamMod3
        {
            get;
            private set;
        }

        public byte A7VforMod4
        {
            get;
            private set;
        }

        public byte A8VrefMod1
        {
            get;
            private set;
        }

        public byte A9VforEq
        {
            get;
            private set;
        }

        public byte A10Reserve
        {
            get;
            private set;
        }

        public byte A11CurMod4
        {
            get;
            private set;
        }

        public byte A12CurPrD
        {
            get;
            private set;
        }

        public byte A13VforMod1
        {
            get;
            private set;
        }

        public byte A14VrefMod4
        {
            get;
            private set;
        }

        public byte A15RfSamMod1
        {
            get;
            private set;
        }

        public byte A16Reserve
        {
            get;
            private set;
        }

        public byte A17RfSamPrD
        {
            get;
            private set;
        }

        public byte A18Reserve
        {
            get;
            private set;
        }

        public byte A19VforMod3
        {
            get;
            private set;
        }

        public byte A20VrefMod3
        {
            get;
            private set;
        }

        public byte A21RfSameMod4
        {
            get;
            private set;
        }

        public byte A22CurMod3
        {
            get;
            private set;
        }

        public byte A23CurMod1
        {
            get;
            private set;
        }

        public void Update(CanFrame frame)
        {
            switch (frame.Id)
            {
                case ID_RFCU1_TEMP:
                    RfcuTemp(frame);
                    break;
                case ID_RFCU2_TEMP:
                    RfcuTemp(frame);
                    break;

                case ID_RFCU1_STATUS:
                    RfcuStatus(frame);
                    break;
                case ID_RFCU2_STATUS:
                    RfcuStatus(frame);
                    break;

                case ID_RFCU1_ANALOG0_7:
                    RfcuAnalog0_7(frame);
                    break;
                case ID_RFCU2_ANALOG0_7:
                    RfcuAnalog0_7(frame);
                    break;

                case ID_RFCU1_ANALOG8_15:
                    RfcuAnalog8_15(frame);
                    break;
                case ID_RFCU2_ANALOG8_15:
                    RfcuAnalog8_15(frame);
                    break;

                case ID_RFCU1_ANALOG16_23:
                    RfcuAnalog16_23(frame);
                    break;
                case ID_RFCU2_ANALOG16_23:
                    RfcuAnalog16_23(frame);
                    break;
            }
        }

        private void RfcuAnalog16_23(CanFrame frame)
        {
            A16Reserve = frame[0];
            A17RfSamPrD = frame[1];
            A18Reserve = frame[2];
            A19VforMod3 = frame[3];
            A20VrefMod3 = frame[4];
            A21RfSameMod4 = frame[5];
            A22CurMod3 = frame[6];
            A23CurMod1 = frame[7];
        }

        private void RfcuAnalog8_15(CanFrame frame)
        {
            A8VrefMod1 = frame[0];
            A9VforEq = frame[1];
            A10Reserve = frame[2];
            A11CurMod4 = frame[3];
            A12CurPrD = frame[4];
            A13VforMod1 = frame[5];
            A14VrefMod4 = frame[6];
            A15RfSamMod1 = frame[7];
        }

        private void RfcuAnalog0_7(CanFrame frame)
        {
            A0Reserve = frame[0];
            A1VforMod2 = frame[1];
            A2VrefMod2 = frame[2];
            A3RfSamMod2 = frame[3];
            A4CurMod2 = frame[4];
            A5VrefEq = frame[5];
            A6RfSamMod3 = frame[6];
            A7VforMod4 = frame[7];
        }

        private void RfcuStatus(CanFrame frame)
        {
            _status = frame.Data();
        }

        private void RfcuTemp(CanFrame frame)
        {
            Module1Temp = frame[0];
            Module2Temp = frame[1];
            Module3Temp = frame[2];
            Module4Temp = frame[3];
            PreDrive1Temp = frame[4];
            CombinerTemp = frame[5];
        }

        internal Rfcu1KwImpl(IDispatcher dispatcher, Rfcus index)
        {
            if (index == Rfcus.Rfcu1)
            {
                dispatcher.Register(ID_RFCU1_ANALOG0_7,this);
                dispatcher.Register(ID_RFCU1_ANALOG16_23,this);
                dispatcher.Register(ID_RFCU1_ANALOG8_15,this);
                dispatcher.Register(ID_RFCU1_STATUS,this);
                dispatcher.Register(ID_RFCU1_TEMP,this);

                return;
            }
            if (index == Rfcus.Rfcu2)
            {
                dispatcher.Register(ID_RFCU2_ANALOG0_7, this);
                dispatcher.Register(ID_RFCU2_ANALOG16_23, this);
                dispatcher.Register(ID_RFCU2_ANALOG8_15, this);
                dispatcher.Register(ID_RFCU2_STATUS, this);
                dispatcher.Register(ID_RFCU2_TEMP, this);
            }
        }
    }
}
