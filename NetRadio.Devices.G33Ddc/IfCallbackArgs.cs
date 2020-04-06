namespace NetRadio.Devices.G3XDdc
{
    public class IfCallbackArgs:CallbackArgs
    {
        public ushort MaxAdcAmplitude { get; private set; }
        public uint AdcSamplingRate { get; private set; }
        public byte[] Data { get; private set; }

        internal IfCallbackArgs(ushort maxAdcAmplitude, uint adcSamplingRate, byte[] data)
        {
            MaxAdcAmplitude = maxAdcAmplitude;
            AdcSamplingRate = adcSamplingRate;
            Data = data;

            SamplingRate = 100000000;//100Mhz
            BitCount = 16; //16bit- little endian
        }
    }
}
