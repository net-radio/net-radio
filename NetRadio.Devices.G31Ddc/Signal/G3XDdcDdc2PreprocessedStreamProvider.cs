namespace NetRadio.Devices.G3XDdc.Signal
{
    public class G3XDdcDdc2PreprocessedStreamProvider:G3XDdcDdc2StreamProvider
    {
        public override void Start()
        {
            Ddc2.PreprocessedDataRecieved += Ddc2_DataRecieved;
        }

        public override void Stop()
        {
            Ddc2.PreprocessedDataRecieved -= Ddc2_DataRecieved;
        }

        public G3XDdcDdc2PreprocessedStreamProvider(Ddc2 ddc2, bool seperateIq = false)
            :base(ddc2,seperateIq)
        {
            
        }
    }
}
