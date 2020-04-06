using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using NetRadio.G31Ddc.WaterfallSample.Shaders;

namespace NetRadio.G31Ddc.WaterfallSample
{
    /// <summary>
    /// Interaction logic for Waterfall.xaml
    /// </summary>
    public partial class Waterfall : IWaterfallAnalyzer
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr memmove(IntPtr dst, IntPtr src, uint count);

        private readonly Label[] _timeLabels;
        private readonly Label[] _frequencyLabels;

        private WriteableBitmap _buffer;

        private Stopwatch _updateTimer;
        private int _downSample;
        private Transform _transform;
        private int _updateThreshold;
        private int _width;
        private int _height;
        private uint _blockSize;
        private Int32Rect _invalidateArea;
        private double _frequencyDomain;

        private bool _initialized;

        private double _windowStart;
        private double _windowStop;
        private double _window;

        public Brush MarkerColor
        {
            get { return (Brush)GetValue(MarkerColorProperty); }
            set { SetValue(MarkerColorProperty, value); }
        }

        public static readonly DependencyProperty MarkerColorProperty = DependencyProperty.Register(
            "MarkerColor", typeof (Brush), typeof (Waterfall), new PropertyMetadata(Brushes.Red));

        public Waterfall()
        {
            InitializeComponent();
            _timeLabels = new[]
            {
                T0,
                T1,
                T2,
                T3,
                T4,
                T5,
                T6,
                T7,
                T8,
                T9
            };

            _frequencyLabels = new[]
            {
                F0,
                F1,
                F2,
                F3,
                F4,
                F5
            };
        }


        public void Begin(WaterfallConfig config)
        {
            InitializeBuffer(config);
            InitializeSampler(config);

            UpdateTimeLabel(config.History);
            UpdateFrequencyLabel(1,0.5);
            UpdateLegendLabel(config);

            _initialized = true;
        }

        private void UpdateLegendLabel(WaterfallConfig config)
        {
            LegendMin.Content = config.MinIntensity;
            LegendMax.Content = config.MaxIntensity;
        }

        private void UpdateTimeLabel(int history)
        {
            var step = history/10; //number of time bar steps (vertical)
            for (var i = 0; i < _timeLabels.Length; i++)
                _timeLabels[i].Content = step * (i + 1);
        }

        private void UpdateFrequencyLabel(double zoom, double position)
        {
            //NOTE: this code is based on the XAML transformation used

            var scaleHorizon = _frequencyDomain*position;
            var domain = _frequencyDomain/zoom;
            _windowStart = scaleHorizon - domain*position;
            _windowStop = scaleHorizon + domain*(1.0 - position);
            _window = _windowStop - _windowStart;

            var step = _window/6; //as number of parts in frequency scale (Horizontal)
            for (var i = 0; i < _frequencyLabels.Length; i++)
                _frequencyLabels[i].Content = Math.Round((step*(i + 1) + _windowStart)/1000000, 1);
        }

        private void InitializeSampler(WaterfallConfig config)
        {
            _frequencyDomain = config.MaxFrequency - config.MinFrequency;
            _windowStop = config.MaxFrequency;
            _windowStart = config.MinFrequency;
            _updateTimer = new Stopwatch();
            _updateThreshold = 1000/config.RefreshRate;
            _downSample = config.DownSample;
            _transform = new Transform(config.MinIntensity, config.MaxIntensity, 0, 255);
            _updateTimer.Start();
        }

        private void InitializeBuffer(WaterfallConfig config)
        {
            _width = config.Points/config.DownSample;
            _height = config.History*config.RefreshRate;
            _invalidateArea = new Int32Rect(0, 0, _width, _height);
            _blockSize = (uint) ((_height*_width) - _width);
            _buffer = new WriteableBitmap(_width, _height, 300, 300, PixelFormats.Gray8, BitmapPalettes.Gray256);
            Spectrum.Source = _buffer;
        }

        public void End()
        {
            _initialized = false;
        }

        public void Update(IFrequencyBins bins)
        {
            if (!_initialized)
                return;

            if (_updateTimer.ElapsedMilliseconds < _updateThreshold)
                return;

            _updateTimer.Restart();

            var backBuffer = IntPtr.Zero;

            Dispatcher.Invoke(() =>
            {
                _buffer.Lock();
                backBuffer = _buffer.BackBuffer;
            });

            memmove(backBuffer + _width, backBuffer, _blockSize);
            unsafe
            {
                Draw(bins, (byte*) backBuffer.ToPointer());
            }

            Dispatcher.Invoke(() =>
            {
                _buffer.AddDirtyRect(_invalidateArea);
                _buffer.Unlock();
            });
        }

        private unsafe void Draw(IFrequencyBins bins, byte* ptr)
        {
            var intensities = bins.Intensities();

            var counter = 0;
            var sum = 0.0;
            foreach (var s in intensities)
            {
                if (counter < _downSample)
                {
                    sum += s;
                    counter++;
                    continue;
                }
                sum /= _downSample;
                sum = _transform.Apply(sum);
                *ptr = (byte) sum;

                sum = s;
                ptr++;
                counter = 1;
            }

            sum /= counter;
            sum = _transform.Apply(sum);
            *ptr = (byte)sum;
        }

        private void ScaleChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Spectrum == null)
                return;

            if (Math.Abs(Zoom.Value - 1F) < 0.01)
            {
                Position.Value = 0.5;
                Position.Visibility = Visibility.Hidden;
            }
            else
                Position.Visibility = Visibility.Visible;
            

            UpdateFrequencyLabel(Zoom.Value, Position.Value);
            Spectrum.RenderTransformOrigin = new Point(Position.Value, Position.Value);
        }

        private void MarkerEnter(object sender, MouseEventArgs e)
        {
            Marker.Y1 = 1;
            Marker.Y2 = MarkerPlane.RenderSize.Height - 1;
            Marker.Visibility = Visibility.Visible;
            Frequency.Visibility = Visibility.Visible;
        }

        private void MarkerSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Marker.Y1 = 1;
            Marker.Y2 = MarkerPlane.RenderSize.Height - 1;
 
        }

        private void MarkerExit(object sender, MouseEventArgs e)
        {
            Marker.Visibility = Visibility.Hidden;
            Frequency.Visibility = Visibility.Hidden;
        }

        private void MarkerMove(object sender, MouseEventArgs e)
        {
            Marker.X1 = e.GetPosition(MarkerPlane).X;
            Marker.X2 = Marker.X1;

            var ratio = Marker.X1/MarkerPlane.RenderSize.Width;
            var freq = ratio*_window + _windowStart;
            freq /= 1000; //as khz;
            freq = Math.Round(freq, 3);
            Frequency.Content = string.Format("[ {0} KHz]", freq);
        }

        private void EffectChanged(object sender, SelectionChangedEventArgs e)
        {
            Effect effect = null;
            var item = (ComboBoxItem)EffectCombo.SelectedItem;

            switch (item.Content.ToString())
            {
                case "Gray":
                    break;
                case "Orange":
                    effect=new OrangeEffect();
                    break;
                case "DeepBlue":
                    effect=new DeepBlueEffect();
                    break;
                case "DeepRed":
                    effect = new DeepRedEffect();
                    break;
                case "Rainbow":
                    effect = new RainbowEffect();
                    break;
                default:
                    throw new NotSupportedException();
            }

            if (Spectrum == null || Legend == null) return;

            Spectrum.Effect = effect;
            Legend.Effect = effect;
        }
    }
}
