namespace NetRadio.Devices.G3XDdc
{
    public class DrmServiceInfo
    {
        public DrmContentType ContentType { get; private set; }
        public string DynamicLabel { get; private set; }
        public string Country { get; private set; }
        public string Language { get; private set; }
        public string ProgramType { get; private set; }
        public double AudioBitrate { get; private set; }
        public double TextMessageBitrate { get; private set; }
        public double MultimediaBitrate { get; private set; }
        public double DataBitrate { get; private set; }

        internal DrmServiceInfo(NativeDefinitions.ServiceInfo native)
        {
            ContentType = (DrmContentType)native.Content;
            DynamicLabel = native.DynamicLabel;
            Country = native.Country;
            Language = native.Language;
            ProgramType = native.ProgramType;
            AudioBitrate = native.AudioBitrate;
            TextMessageBitrate = native.TextMsgBitrate;
            MultimediaBitrate = native.MultimediaBitrate;
            DataBitrate = native.DataBitrate;
        }
    }
}
