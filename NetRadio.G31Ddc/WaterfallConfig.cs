namespace NetRadio.G31Ddc.WaterfallSample
{
    public class WaterfallConfig
    {
        public int MinIntensity { get; set; }
        public int MaxIntensity { get; set; }

        public int MinFrequency { get; set; }
        public int MaxFrequency { get; set; }

        public int Points { get; set; }
        public  int DownSample { get; set; }
        public int History { get; set; }
        public int RefreshRate {get; set; }

        public bool Validate()
        {
            if (Points%DownSample != 0)
                return false;
            if (1000%RefreshRate != 0)
                return false;

            return true;
        }
    }
}
