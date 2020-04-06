using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.Arction
{
    public interface ISpectrumAnalyzer
    {
        event CursorLinePackValueChangedEventHandler BandwidthChanged;
        event CursorLinePackValueChangedEventHandler NotchChanged;
        void Update(IFrequencyBins bins);
        void Update(double[] samples);
        void ZoomIn();
        void ZoomOut();
        void SetBandwidth(double start, double end);
        RangePair GetBandwidth();
        void PreInitialize(double startFrequency = -12000.0, double stopFrequency = 12000.0, double firstSampleTimeStamp = -12000.0, double precision = 1.0);
        void ReInitialize(int samplingRate = WaveformDefinitions.DEFAULT_SAMPLES_COUNT);
        void SetNotch(double start, double end);
        void Notch(bool enable);
        void Clear();

        void SetSampleFrequencyRange(double start, double stop);
        void SetSpectrumFrequencyRange(double start, double stop);


        void SetFrequencyIntervals(double startFrequency, double stopFrequency, double precision);
        // void SetVisibleFrequencyIntervals(double startFrequency, double stopFrequency);
    }
}
