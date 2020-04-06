using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Arction.WPF.LightningChartUltimate.SeriesXY;
using Color = System.Drawing.Color;
using NetRadio.Devices.G313.Signal;

namespace NetRadio.G313.Chart
{
    public interface ISweeperAnalyzer
    {
        event WaveformPlayerSelectedValueChangedEventHandler SelectedValueChanged;
        void Update(SweepedArgs args);
        void ZoomIn();
        void ZoomOut();
        void Clear();
        void ReInitialize(double from,double to ,int points);
        void VisibleMin(bool state);
        void VisibleCurrent(bool state);
        void VisibleMax(bool state);
        bool VisibleMin();
        bool VisibleCurrent();
        bool VisibleMax();
    }

    public partial class FixedChannelChart : ISweeperAnalyzer
    {
        public event WaveformPlayerSelectedValueChangedEventHandler SelectedValueChanged;

        #region Fields
        private readonly WaveformPlayer _player;
        private readonly Random _random = new Random(100);
        #endregion

        private SampleDataSeries _min;
        private SampleDataSeries _current;
        private SampleDataSeries _max;

        #region .ctor
        public FixedChannelChart()
        {
            InitializeComponent();

            _player = new WaveformPlayer(-120, 0);
            ContentControlChart.Content = _player.ContainerChart;

            var color0 = Colors.Brown;
            var config0 = new WaveformPlayerConfig
            {
                Color = Color.FromArgb(100, color0.R, color0.G, color0.B),
                Maximum = 0,
                Minimum = -120,
            };
            _max=_player.AddNewWaveformPlayer(config0);

            var color1 = Colors.DarkOrange;
            var config1 = new WaveformPlayerConfig
            {
                Color = Color.FromArgb(100, color1.R, color1.G, color1.B),
                Maximum = 0,
                Minimum = -120,
            };
            _current=_player.AddNewWaveformPlayer(config1);

            var color2 = Colors.OrangeRed;
            var config2 = new WaveformPlayerConfig
            {
                Color = Color.FromArgb(100, color2.R, color2.G, color2.B),
                Maximum = 0,
                Minimum = -120,
            };
            _min=_player.AddNewWaveformPlayer(config2);

            _player.Initialize(0.0d, 1.0d, 500);

            _player.SelectedValueChanged += OnSelectedValueChanged;

            SizeChanged += (sender, args) => _player.SetBounds((int)RootLayout.ActualWidth, (int)RootLayout.ActualHeight - 10);
        }

        protected void OnSelectedValueChanged(double x, List<KeyValuePair<SampleDataSeries, double>> values)
        {
            if (SelectedValueChanged != null)
                SelectedValueChanged(x, values);
        }

        #endregion

        public void Update(SweepedArgs args)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    var data = args.ToArray();
                    _player.FeedNewDataToChart(data, args.Data.Count);
                }));
            }

            catch (TaskCanceledException)
            {
                //Application shutdown
            }
        }

        public void ZoomIn()
        {
            _player.Zoom(1 / 2.0d);
        }

        public void ZoomOut()
        {
            _player.Zoom(2.0d);      
        }

        public void Clear()
        {
            _player.ClearData();
        }

        public void ReInitialize(double from,double to ,int points)
        {
            _player.Initialize(from, to, points);
        }

        public void VisibleMin(bool state)
        {
            _min.Visible = state;
        }

        public void VisibleCurrent(bool state)
        {
            _current.Visible = state;
        }

        public void VisibleMax(bool state)
        {
            _max.Visible = state;
        }

        public bool VisibleMin()
        {
            return _min.Visible;
        }

        public bool VisibleCurrent()
        {
            return _current.Visible;
        }

        public bool VisibleMax()
        {
            return _max.Visible;
        }

        private void ButtonZoomIn_OnClick(object sender, RoutedEventArgs e)
        {
            ZoomIn();
        }

        private void ButtonZoomOut_OnClick(object sender, RoutedEventArgs e)
        {
            ZoomOut();
        }

        private void ButtonClear_OnClick(object sender, RoutedEventArgs e)
        {
            Clear();
        }
    }
}
