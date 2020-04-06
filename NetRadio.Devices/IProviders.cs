namespace NetRadio.Devices
{
    public interface IProviders<T>where T:Radio<T>
    {
        IRadioInfoProvider<T> InfoProvider();
        IRadioProvider<T> RadioProvider();
    }
}
