namespace EighthBit.Exciter.Parsers
{
    public interface PowerSupply : IParser
    {
        byte RectifierTemp { get; }
        byte InverterTemp { get; }
        byte Iout { get; }
        byte Vout { get; }
        byte Iin { get; }
        byte Vin { get; }
        bool SupplyOn { get; }
        bool MajorError { get; }
        bool MinorError { get; }
        bool Charge { get; }
        bool Main { get; }
        bool PsOn { get; }
        bool FuseNot { get; }
        bool PsFaultNot { get; }
        bool UnderVoltageNot { get; }
        bool BankCharge { get; }
        bool BankEmpty { get; }
        bool OverVolL { get; }
        bool MocNotL { get; }
        bool HwErrL { get; }
        bool PsOcL { get; }
        bool PgL { get; }
        bool Vg9Not { get; }
        bool Vg19Not { get; }
        bool Vg19NegativeNot { get; }
        bool OverTemp { get; }
        bool FanOut { get; }
        bool ExtResetNot { get; }
        bool TempValid { get; }
        bool Reserve { get; }
    }
}
