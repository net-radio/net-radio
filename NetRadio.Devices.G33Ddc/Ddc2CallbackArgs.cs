namespace NetRadio.Devices.G3XDdc
{
    public class Ddc2CallbackArgs:CallbackArgs
    {
        public uint Channel { get; private set; }
        public float[] Data { get; private set; }

        internal Ddc2CallbackArgs(uint bitCount,uint samplingRate,uint channel, float[] data)
        {
            Channel = channel;
            Data = data;
            BitCount = bitCount;
            SamplingRate = samplingRate;
        }
    }
}
