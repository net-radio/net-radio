namespace EighthBit.Exciter
{
    public interface IExciterMultiSpotApi
    {
        IExciterMultiSpotApi MultiSpot(byte pointsCount, ExciterModulation modulation);
        IExciterMultiSpotApi MultiSpotPoint(byte index, uint frequency, byte time);
    }
}
