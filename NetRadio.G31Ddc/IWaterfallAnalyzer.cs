namespace NetRadio.G31Ddc.WaterfallSample
{
    public interface IWaterfallAnalyzer
    {
        void Begin(WaterfallConfig config);//16384,16,60,2
        void End();
        void Update(IFrequencyBins bins);
    }
}
