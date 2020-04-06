namespace NetRadio.Devices.G3XDdc
{
    public class G31DdcRadioInfo:BasicRadioInfo
    {
        public uint ChannelCount { get; private set; }
        public uint DdcTypeCount { get; private set; }
        public G3XInterfaceType InterfaceType { get; private set; }

        internal G31DdcRadioInfo(NativeDefinitions.G31DDC_DEVICE_INFO nativeInfo)
        {
            Name = nativeInfo.DevicePath;
            Serial = nativeInfo.SerialNumber;
            ChannelCount = nativeInfo.ChannelCount;
            DdcTypeCount = nativeInfo.DDCTypeCount;
            InterfaceType = (G3XInterfaceType) nativeInfo.InterfaceType;
        }
    }
}
