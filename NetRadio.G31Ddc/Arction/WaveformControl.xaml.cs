using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NetRadio.G31Ddc.Arction
{    
    /// <summary>
    /// Interaction logic for WaveformControl.xaml
    /// </summary>
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
        private double _precision = 1.0;
        private WaveformMonitor _waveform;

        private CursorLinePack _bandwidth;
        private CursorLinePack _notch;
        private double _start;
        private double _end;
        private double _stopFrequency = -12000.0;                       
        private double _startFrequency = 12000.0;
                       

        #endregion

        #region .ctor
        public WaveformControl()
        {
            InitializeComponent();

            SizeChanged += (sender, args) => _waveform.SetBounds((int)GridWaveformMonitor.ActualWidth, (int)GridWaveformMonitor.ActualHeight);

            PreInitialize();
            ReInitialize();
        }

        protected void OnBandwidthChanged(RangePair range, double value)
        {
            /*
            if (_lastBandwidthRange != null && _lastBandwidthRange.Start == range.Start && _lastBandwidthRange.End == range.End && _lastBandwidthShift == value)
                return;
            */

            _lastBandwidthRange = range;
            _lastBandwidthShift = value;

            if (BandwidthChanged != null)
            {
                // BandwidthChanged(new RangePair { End = TransformFromChartSamples(range.End), Start = TransformFromChartSamples(range.Start) }, TransformFromChartSamples(value));
                BandwidthChanged(new RangePair { End = range.End, Start = range.Start }, value);
            }

        }

        protected void OnNotchChanged(RangePair range, double value)
        {
            /*
            if (_lastNotchRange != null && _lastNotchRange.Start == range.Start && _lastNotchRange.End == range.End && _lastNotchShift == value)
                return;
            */

            _lastNotchRange = range;
            _lastNotchShift = value;

            if (NotchChanged != null)
                NotchChanged(new RangePair { End = TransformFromChartSamples(range.End), Start = TransformFromChartSamples(range.Start) }, TransformFromChartSamples(value));
        }

        public void PreInitialize(double startFrequency = -12000.0, double stopFrequency = 12000.0, double firstSampleTimeStamp = -12000.0, double precision = 1.0)
        {
            _precision = precision;
            // _firstSampleTimeStamp = firstSampleTimeStamp;
            _startFrequency = startFrequency;
            _stopFrequency = stopFrequency;

            _waveform = new WaveformMonitor(Colors.DarkOrange);
            _waveform.PreInitialize(startFrequency, stopFrequency, startFrequency, precision);
        }

        public void ReInitialize(int samplingRate = WaveformDefinitions.DEFAULT_SAMPLES_COUNT)
        {
            _empty = new double[samplingRate];

            _samplingRate = samplingRate;

            _waveform.Initialize(samplingRate, string.Empty);
            _waveform.SetRange(-100, 0);

            InitBandwidth();
            InitNotch();

            // AddHorizontalConstantLines();
            // AddVerticalConstantLines();

            GridWaveformMonitor.Children.Add(_waveform.ContainerChart);
        }


        private void InitNotch()
        {
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
        }

        private void AddHorizontalConstantLines()
        {
            _waveform.AddHorizontalConstantLine(-100, Colors.Orange);
            //_waveform.AddHorizontalConstantLine(1000, Colors.YellowGreen);
        }

        [Obsolete("This should remove old lines", true)]
        private void AddVerticalConstantLines()
        {
            _waveform.AddVerticalConstantLine(CenterFrequency, Colors.Orange);
            _waveform.AddVerticalConstantLine(CenterFrequency + 5.0 / 12.0 * Bandwidth, Colors.YellowGreen);
            _waveform.AddVerticalConstantLine(CenterFrequency - 5.0 / 12.0 * Bandwidth, Colors.YellowGreen);
            _waveform.AddVerticalConstantLine(CenterFrequency + 10.0 / 12.0 * Bandwidth, Colors.OrangeRed);
            _waveform.AddVerticalConstantLine(CenterFrequency - 10.0 / 12.0 * Bandwidth, Colors.OrangeRed);
        }

        private void InitBandwidth()
        {
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
                BandwidthMaximum = _stopFrequency,
                BandwidthMinimum = _startFrequency,
                ValueMaximum = _stopFrequency,
                ValueMinimum = _startFrequency,
                //BandwidthMaximum = TransformToChartSamples(_startFrequency),
                //BandwidthMinimum = TransformToChartSamples(_stopFrequncy),
                //ValueMaximum = TransformToChartSamples(_startFrequency),
                //ValueMinimum = TransformToChartSamples(_stopFrequncy),
            };

            _bandwidth = _waveform.AddPairCursors(config0);
            _bandwidth.ValueChanged += OnBandwidthChanged;
            //_bandwidth.SetValue(TransformToChartSamples(-7500), TransformToChartSamples(7500));
            _bandwidth.SetValue(CenterFrequency, CenterFrequency);
        }
        #endregion

        public void Update(IFrequencyBins bins)
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

        public void Update(double[] samples)
        {
            try
            {
                Dispatcher.Invoke(new Action(() => _waveform.FeedData(samples)));
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
            _waveform.Zoom(1 / 2.0d);
        }

        private void ButtonZoomOut_OnClick(object sender, RoutedEventArgs e)
        {
            _waveform.Zoom(2.0d);
        }

        public void SetBandwidth(double start, double end)
        {
            _start = start;
            _end = end;
            // _bandwidth.SetValue(TransformToChartSamples(start), TransformToChartSamples(end));
            _bandwidth.SetValue(_start, _end);
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
            var unit = _samplingRate / 24000.0;
            return value * unit;
        }

        private double TransformFromChartSamples(double value)
        {
            var unit = 24000.0 / _samplingRate;
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

        public RangePair GetBandwidth()
        {
            return new RangePair(_start, _end);
        }


        public void SetSampleFrequencyRange(double start, double stop)
        {
            _waveform.SetSampleFrequencyRange(start, stop);
        }

        public void SetSpectrumFrequencyRange(double start, double stop)
        {
            _waveform.SetSpectrumFrequencyRange(start, stop);
        }

        public double CenterFrequency
        {
            get { return (_startFrequency + _stopFrequency) / 2; }
        }

        public double Bandwidth
        {
            get { return (_stopFrequency - _startFrequency) / 2; }
        }

        public void SetFrequencyIntervals(double startFrequency, double stopFrequency, double precision)
        {
            _precision = precision;
            // _firstSampleTimeStamp = firstSampleTimeStamp;
            _startFrequency = startFrequency;
            _stopFrequency = stopFrequency;

            _waveform.PreInitialize(startFrequency, stopFrequency, startFrequency, precision);
            _waveform.SetFrequencyIntervals();
        }

        /*
        public void SetVisibleFrequencyIntervals(double startFrequency, double stopFrequency)
        {
            _startFrequency = startFrequency;
            _stopFrequency = stopFrequency;

            _waveform.SetSpectrumFrequencyRange(_startFrequency, _stopFrequency);
        }
         */ 
    }
}
