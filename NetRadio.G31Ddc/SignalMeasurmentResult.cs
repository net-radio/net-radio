using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.G31Ddc
{
    public class SignalMeasurmentResult
    {
        private readonly Dictionary<double, int> _frequencyHits = new Dictionary<double, int>();
        private readonly Dictionary<double, double> _frequencyitPeriod = new Dictionary<double, double>();

        public TimeSpan TestDuration { get; set; }
        public int SampleCount { get; set; }
        public double HitDuration { get; set; }
        public double EstimatedHopDuration { get; set; }

        public IEnumerable<double> HitFrequencies()
        {
            return _frequencyHits.Keys;
        }

        public int FrequncyHitCount(double frequency)
        {
            return _frequencyHits[frequency];
        }

        public double FrequencyHitDuration(double frequency)
        {
            return _frequencyitPeriod[frequency];
        }

        internal SignalMeasurmentResult(TimeSpan testDuration, Dictionary<double, int> frequencyHits,
            Dictionary<double, double> frequencyHitPeriod)
        {
            _frequencyHits = frequencyHits;
            _frequencyitPeriod = frequencyHitPeriod;

            TestDuration = testDuration;
            SampleCount = frequencyHits.Values.Sum();
            HitDuration = frequencyHitPeriod.Values.Sum();
            EstimatedHopDuration = HitDuration / SampleCount;
        }
    }
}
