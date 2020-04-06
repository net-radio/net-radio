using NetRadio.G31Ddc.Arction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRadio.G31Ddc.WaterfallSample;

namespace NetRadio.G31Ddc.View
{
    public class ViewObject
    {
        public ISpectrumAnalyzer SpectrumAnalyzerIf { get; set; }

        public WaveformControl SpectrumAnalyzerDdc1 { get; set; }

        public WaveformControl SpectrumAnalyzerDdc2 { get; set; }

        public IWaterfallAnalyzer WaterfallAnalyzer { get; set; }
    }
}
