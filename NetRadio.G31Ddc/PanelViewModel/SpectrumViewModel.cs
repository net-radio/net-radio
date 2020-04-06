using NetRadio.G31Ddc.Arction;
using NetRadio.G31Ddc.Core.Extension;
using NetRadio.G31Ddc.ViewModel;
using System;
using System.Collections.ObjectModel;

namespace NetRadio.G31Ddc.PanelViewModel
{
    public abstract class SpectrumViewModel : ViewModelBase
    {
        private readonly RadioViewModel _radioViewModel;
        public ISpectrumAnalyzer SpectrumAnalyzer { get; private set; }

        private bool _isNotchEnabled = false;
        protected bool _updating = false;

        public RadioViewModel RadioView
        {
            get { return _radioViewModel; }
        }

        public bool IsNotchEnabled
        {
            get { return _isNotchEnabled; }
            set { _isNotchEnabled = value; }
        }

        public abstract ResolutionBandwidth SelectedResolution { get; set; }
        public ObservableCollection<ResolutionBandwidth> Resolutions { get; set; }

        public abstract uint Bandwidth { get; set; }
        public abstract long Shift { get; set; }

        //public abstract double SpectrumStartFrequency { get; }
        //public abstract double SpectrumStopFrequency { get; }

        public abstract double StartFrequency { get; }
        public abstract double StopFrequency { get; }
        public abstract double Precision { get; }

        public SpectrumViewModel(RadioViewModel radioViewModel, ISpectrumAnalyzer spectrumAnalyzer)
        {
            _radioViewModel = radioViewModel;
            SpectrumAnalyzer = spectrumAnalyzer;

            Resolutions = new ObservableCollection<ResolutionBandwidth>();
            for (uint i = 0; i < 8; i++)
                Resolutions.Add(new ResolutionBandwidth() { Resolution = Extensions.Pow(2, i) });

            if (SpectrumAnalyzer != null)
            {
                SpectrumAnalyzer.BandwidthChanged += (range, value) =>
                {
                    if (_updating)
                        return;

                    // var shift = TransformFromChartSamples(value);
                    // var bandwidth = TransformFromChartSamples(range.End) - TransformFromChartSamples(range.Start);

                    var shift = value;
                    var bandwidth = range.End - range.Start;

                    if ((shift >= long.MinValue && shift <= long.MaxValue)
                        && (bandwidth >= uint.MinValue && bandwidth <= uint.MaxValue))
                    {
                        Shift = (long)shift;
                        Bandwidth = (uint)bandwidth;

                        Update();
                    }
                };

                SpectrumAnalyzer.NotchChanged += (range, value) =>
                {
                    var bandwidth = range.End - range.Start;
                };
            }
        }

        [Obsolete("Remove line [if (App.Current == null) return;]", false)]
        public void Update()
        {
            if (App.Current == null)
                return;

            if (SpectrumAnalyzer == null)
                return;

            // Dispatcher.Invoke(new Action(() =>
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                _updating = true;
                // double start = TransformToChartSamples(Shift - Bandwidth / 2);
                // double end = TransformToChartSamples(Shift + Bandwidth / 2);
                double start = Shift - Bandwidth / 2;
                double end = Shift + Bandwidth / 2;
                SpectrumAnalyzer.SetBandwidth(start, end);

                SpectrumAnalyzer.SetFrequencyIntervals(StartFrequency, StopFrequency, Precision);
                // SpectrumAnalyzer.SetVisibleFrequencyIntervals();
                _updating = false;
            });
        }

        /*
        protected double TransformToChartSamples(double value)
        {            
            return ((value - SpectrumStartFrequency) * 24000.0) / (SpectrumStopFrequency - SpectrumStartFrequency) - 12000.0;
        }

        protected double TransformFromChartSamples(double value)
        {
            return ((value + 12000.0) * (SpectrumStopFrequency - SpectrumStartFrequency) / 24000.0) + SpectrumStartFrequency;            
        }
        */
    }
}
