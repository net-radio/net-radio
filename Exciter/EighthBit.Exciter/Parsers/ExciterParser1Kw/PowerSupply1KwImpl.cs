namespace EighthBit.Exciter.Parsers.ExciterParser1Kw
{
    public class PowerSupply1KwImpl : PowerSupply, IParser
    {
        public const uint ID_SUPPLY_TEMP = 50;
        public const uint ID_SUPPLY_STATUS = 51;
        public const uint ID_SUPPLY_ANALOGS = 52;

        private byte[] _status=new byte[4];

        public byte RectifierTemp { get; private set; }
        public byte InverterTemp { get; private set; }
        public byte Iout { get; private set; }
        public byte Vout { get; private set; }
        public byte Iin { get; private set; }
        public byte Vin { get; private set; }

        public bool SupplyOn
        {
            get { return (_status[0] & 1) > 0; }
        }

        public bool MajorError
        {
            get { return (_status[0] & 2) > 0; }
        }

        public bool MinorError
        {
            get { return (_status[0] & 4) > 0; }
        }

        public bool Charge
        {
            get { return (_status[0] & 8) > 0; }
        }

        public bool Main
        {
            get { return (_status[0] & 16) > 0; }
        }

        public bool PsOn
        {
            get { return (_status[0] & 32) > 0; }
        }

        public bool FuseNot
        {
            get { return (_status[0] & 64) > 0; }
        }

        public bool PsFaultNot
        {
            get { return (_status[0] & 128) > 0; }
        }


        public bool UnderVoltageNot
        {
            get { return (_status[1] & 1) > 0; }
        }

        public bool BankCharge
        {
            get { return (_status[1] & 2) > 0; }
        }

        public bool BankEmpty
        {
            get { return (_status[1] & 4) > 0; }
        }

        public bool OverVolL
        {
            get { return (_status[1] & 8) > 0; }
        }

        public bool MocNotL
        {
            get { return (_status[1] & 16) > 0; }
        }

        public bool HwErrL
        {
            get { return (_status[1] & 32) > 0; }
        }

        public bool PsOcL
        {
            get { return (_status[1] & 64) > 0; }
        }

        public bool PgL
        {
            get { return (_status[1] & 128) > 0; }
        }

        public bool Vg9Not
        {
            get { return (_status[2] & 1) > 0; }
        }

        public bool Vg19Not
        {
            get { return (_status[2] & 2) > 0; }
        }

        public bool Vg19NegativeNot
        {
            get { return (_status[2] & 4) > 0; }
        }

        public bool OverTemp
        {
            get { return (_status[2] & 8) > 0; }
        }

        public bool FanOut
        {
            get { return (_status[2] & 16) > 0; }
        }

        public bool ExtResetNot
        {
            get { return (_status[2] & 32) > 0; }
        }

        public bool TempValid
        {
            get { return (_status[2] & 64) > 0; }
        }

        public bool Reserve
        {
            get { return (_status[2] & 128) > 0; }
        }

        public void Update(Can.CanFrame frame)
        {
            switch (frame.Id)
            {
                case ID_SUPPLY_TEMP:
                    RectifierTemp = frame[0];
                    InverterTemp = frame[1];
                    break;
                case ID_SUPPLY_STATUS:
                    _status = frame.Data();
                    break;
                case ID_SUPPLY_ANALOGS:
                    Iout = frame[0];
                    Vout = frame[1];
                    Iin = frame[2];
                    Vin = frame[3];
                    break;
            }
        }


        internal PowerSupply1KwImpl(IDispatcher dispatcher)
        {
            dispatcher.Register(ID_SUPPLY_ANALOGS,this);
            dispatcher.Register(ID_SUPPLY_STATUS,this);
            dispatcher.Register(ID_SUPPLY_TEMP,this);
        }
    }
}
