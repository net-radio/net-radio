using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using NetRadio.Signal;
using System.Windows.Threading;

namespace NetRadio.G313.Chart
{
    public interface ISpectrumAnalyzer
    {
        event CursorLinePackValueChangedEventHandler BandwidthChanged;
        event CursorLinePackValueChangedEventHandler NotchChanged;
        void Update(FrequencyBins bins);
        void ZoomIn();
        void ZoomOut();
        void SetBandwidth(double start, double end);
        void ReInitialize(int samplingRate = 2048);
        void SetNotch(double start, double end);
        void Notch(bool enable);

        void Clear();
    }

    public partial class WaveformControl : ISpectrumAnalyzer
    {
        public event CursorLinePackValueChangedEventHandler BandwidthChanged;
        public event CursorLinePackValueChangedEventHandler NotchChanged;

        private RangePair _lastBandwidthRange;
        private double _lastBandwidthShift;

        private RangePair _lastNotchRange;
        private double _lastNotchShift;

        private double[] _empty;

        #region Fields

        private int _samplingRate;
        private WaveformMonitor _waveform;

        private CursorLinePack _bandwidth;
        private CursorLinePack _notch;

        #endregion

        #region .ctor
        public WaveformControl()
        {
            InitializeComponent();

            SizeChanged += (sender, args) => _waveform.SetBounds((int)GridWaveformMonitor.ActualWidth, (int)GridWaveformMonitor.ActualHeight);

            ReInitialize();
        }

        protected void OnBandwidthChanged(RangePair range, double value)
        {
            if (_lastBandwidthRange != null && _lastBandwidthRange.Start == range.Start && _lastBandwidthRange.End == range.End && _lastBandwidthShift==value)
                return;

            _lastBandwidthRange = range;
            _lastBandwidthShift = value;

            if (BandwidthChanged != null)
                BandwidthChanged(new RangePair { End = TransformFromChartSamples(range.End), Start = TransformFromChartSamples(range.Start) }, TransformFromChartSamples(value));
        }

        protected void OnNotchChanged(RangePair range, double value)
        {
            if (_lastNotchRange != null && _lastNotchRange.Start == range.Start && _lastNotchRange.End == range.End && _lastNotchShift == value)
                return;

            _lastNotchRange = range;
            _lastNotchShift = value;

            if (NotchChanged != null)
                NotchChanged(new RangePair { End = TransformFromChartSamples(range.End), Start = TransformFromChartSamples(range.Start) }, TransformFromChartSamples(value));
        }

        public void ReInitialize(int samplingRate=2048 )
        {
            _empty=new double[samplingRate];

            _samplingRate = samplingRate;
            _waveform = new WaveformMonitor(Colors.DarkOrange);
            _waveform.Initialize(samplingRate, string.Empty);
            _waveform.SetRange(-120, 0);

            var color0 = Colors.OrangeRed;
            var config0 = new CursorLinePackConfig
            {
                LineWidth = 1.0f,
                BandColor = new Color
                {
                    A = 20,
                    R = color0.R,
                    G = color0.G,
                    B = color0.B,
                },
                Color = color0,
                BandwidthMaximum = TransformToChartSamples(7500),
                BandwidthMinimum = TransformToChartSamples(-7500),
                ValueMaximum = TransformToChartSamples(12000),
                ValueMinimum = TransformToChartSamples(-12000),
            };

            _bandwidth = _waveform.AddPairCursors(config0);
            _bandwidth.ValueChanged += OnBandwidthChanged; 
            _bandwidth.SetValue(TransformToChartSamples(-7500), TransformToChartSamples(7500));

            var color1 = Colors.White;
            var config1 = new CursorLinePackConfig
            {
                LineWidth = 1.0f,
                BandColor = new Color
                {
                    A = 128,
                    R = color1.R,
                    G = color1.G,
                    B = color1.B,
                },
                Color = color1,
                BandwidthMaximum = TransformToChartSamples(1500),
                BandwidthMinimum = TransformToChartSamples(-1500),
                ValueMaximum = TransformToChartSamples(12000),
                ValueMinimum = TransformToChartSamples(-12000),
            };
            _notch = _waveform.AddPairCursors(config1);
            _notch.ValueChanged += OnNotchChanged;
            _notch.SetValue(TransformToChartSamples(-1500), TransformToChartSamples(1500));        
            
            _waveform.AddHorizontalConstantLine(-100, Colors.Orange);
            //_waveform.AddHorizontalConstantLine(1000, Colors.YellowGreen);
            _waveform.AddVerticalConstantLine(0, Colors.Orange);
            _waveform.AddVerticalConstantLine(TransformToChartSamples(5000), Colors.YellowGreen);
            _waveform.AddVerticalConstantLine(TransformToChartSamples(-5000), Colors.YellowGreen);
            _waveform.AddVerticalConstantLine(TransformToChartSamples(10000), Colors.OrangeRed);
            _waveform.AddVerticalConstantLine(TransformToChartSamples(-10000), Colors.OrangeRed);

            GridWaveformMonitor.Children.Add(_waveform.ContainerChart);
        }
        #endregion

        public void Update(FrequencyBins bins)
        {
            var data = bins.Intensities().ToArray();
            try
            {
                Dispatcher.Invoke(new Action(() => _waveform.FeedData(data)));
            }
            catch (TaskCanceledException)
            {
                //closing app
            }
        }

        public void ZoomIn()
        {
            _waveform.Zoom(1 / 2.0d);
        }

        public void ZoomOut()
        {
            _waveform.Zoom(2.0d);
        }
        private void ButtonZoomIn_OnClick(object sender, RoutedEventArgs e)
        {
            _waveform.Zoom(1/2.0d);
        }

        private void ButtonZoomOut_OnClick(object sender, RoutedEventArgs e)
        {
            _waveform.Zoom(2.0d);
        }

        public void SetBandwidth(double start, double end)
        {
            _bandwidth.SetValue(TransformToChartSamples(start), TransformToChartSamples(end));
        }
        public void SetNotch(double start, double end)
        {
            _notch.SetValue(TransformToChartSamples(start), TransformToChartSamples(end));
        }

        public void Notch(bool enable)
        {
            _notch.Visible = enable;
        }

        private double TransformToChartSamples(double value)
        {
            var unit = _samplingRate/24000.0;
            return value*unit;
        }

        private double TransformFromChartSamples(double value)
        {
            var unit = 24000.0/_samplingRate;
            return value * unit;
        }


        public void Clear()
        {
            try
            {
                Dispatcher.Invoke(new Action(() => _waveform.FeedData(_empty)));
            }
            catch (TaskCanceledException)
            {
                //closing app
            }
        }
    }
}
