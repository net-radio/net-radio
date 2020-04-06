using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arction.WPF.LightningChartUltimate;
using Arction.WPF.LightningChartUltimate.Axes;
using Arction.WPF.LightningChartUltimate.SeriesXY;
using Arction.WPF.LightningChartUltimate.Views.ViewXY;
using System.Windows.Media;
using System.Windows;

namespace NetRadio.G31Ddc.Arction
{
    public delegate void ValueChangedEventHandler(double newValue);
    public class WaveformMonitor
    {
        #region Constants
        #endregion

        #region Fields
        private double _precision = 1.0;
        private double _startFrequency;
        private double _stopFrequency;
        private double _firstSampleTimeStamp;

        private readonly AxisX _xAxis;
        private readonly List<CursorLinePack> _packs = new List<CursorLinePack>();

        public event ValueChangedEventHandler ValueChanged;
        #endregion

        #region Properties
        public LightningChartUltimate ContainerChart { get; set; }
        public double SampleFrequency { get; private set; }
        #endregion

        #region .ctor
        public WaveformMonitor() : this(Colors.DarkOrange) { }

        public WaveformMonitor(Color lineColor, string title = "")
        {
            SampleFrequency = 0.0;

            ContainerChart = new LightningChartUltimate
            {
                Name = "Chart1",
                ChartName = "Chart1",
                Title = { Text = title },
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            ContainerChart.BeginUpdate();

            ContainerChart.ViewXY.AxisLayout.AutoAdjustMargins = true;
            ContainerChart.ViewXY.DropOldSeriesData = false;

            _xAxis = ContainerChart.ViewXY.XAxes[0];
            _xAxis.ValueType = AxisValueType.Number;
            _xAxis.SweepingGap = 2;
            _xAxis.ScrollMode = XAxisScrollMode.None;
            _xAxis.Title.Visible = false;
            _xAxis.Title.Text = "Range";
            _xAxis.Title.VerticalAlign = XAxisTitleAlignmentVertical.Top;
            _xAxis.Title.HorizontalAlign = XAxisTitleAlignmentHorizontal.Right;
            _xAxis.LabelsPosition = Alignment.Near;
            _xAxis.LabelsFont = new WPFFont("Tahoma", 9.0, "9", true, false);
            _xAxis.MajorDivTickStyle.Visible = true;
            _xAxis.MinorDivTickStyle.Visible = true;
            _xAxis.MajorGrid.Visible = true;
            _xAxis.MinorGrid.Visible = true;
            _xAxis.LabelsVisible = true;
            _xAxis.SteppingInterval = 1;
            _xAxis.MouseScaling = false;
            _xAxis.MouseScrolling = false;
            _xAxis.MouseInteraction = false;
            _xAxis.AxisThickness = 1;

            var axisY = ContainerChart.ViewXY.YAxes[0];
            axisY.SetRange(0, 30000);
            axisY.Title.Visible = false;
            axisY.LabelsFont = new WPFFont("Tahoma", 9.0, "9", true, false);

            ContainerChart.ViewXY.GraphBackground.GradientDirection = 270;
            ContainerChart.ViewXY.GraphBackground.GradientFill = GradientFill.Cylindrical;

            var color = ContainerChart.ViewXY.GraphBackground.Color;
            ContainerChart.ViewXY.GraphBackground.Color = Color.FromArgb(150, color.R, color.G, color.B);

            ContainerChart.Title.Font = new WPFFont("Tahoma", 11.0, "11", true, false);

            ContainerChart.Title.Align = ChartTitleAlignment.TopCenter;
            ContainerChart.Title.Offset.SetValues(0, 25);
            ContainerChart.Title.Color = lineColor;

            ContainerChart.ViewXY.Margins = new Thickness(70, 10, 15, 10);
            ContainerChart.ViewXY.ZoomPanOptions.ZoomRectLine.Color = Colors.Lime;

            ContainerChart.ChartBackground.Color = ChartTools.CalcGradient(lineColor, Colors.Black, 65);
            ContainerChart.ChartBackground.GradientDirection = 0;
            ContainerChart.ChartBackground.GradientFill = GradientFill.Cylindrical;

            ContainerChart.ViewXY.LegendBox.Visible = false;

            var series = new SampleDataSeries(ContainerChart.ViewXY, _xAxis, axisY)
            {
                LineStyle =
                {
                    Width = 1f,
                    Color = lineColor,
                    AntiAliasing = LineAntialias.None
                },
                MouseInteraction = false
            };
            ContainerChart.ViewXY.SampleDataSeries.Add(series);

            ContainerChart.EndUpdate();
        }

        public void AddHorizontalConstantLine(double value, Color color)
        {
            var yAxis = ContainerChart.ViewXY.YAxes[0];
            var xAxis = ContainerChart.ViewXY.XAxes[0];

            var constantLine = new ConstantLine(ContainerChart.ViewXY, xAxis, yAxis)
            {
                Title = { Visible = false },
                AssignXAxisIndex = 0,
                LineStyle = { Color = Color.FromArgb(200, color.R, color.G, color.B) },
                Behind = true,
            };

            constantLine.LineStyle.Width = 1;
            constantLine.MouseInteraction = false;
            constantLine.Value = value;
            ContainerChart.ViewXY.ConstantLines.Add(constantLine);
        }

        public void AddVerticalConstantLine(double value, Color color)
        {
            var line = new LineSeriesCursor(ContainerChart.ViewXY, ContainerChart.ViewXY.XAxes[0])
            {
                ValueAtXAxis = 1,
                LineStyle = { Width = 1 }
            };
            line.LineStyle.Color = color; Color.FromArgb(200, color.R, color.G, color.B);
            line.FullHeight = false;
            line.SnapToPoints = false;
            line.MouseInteraction = false;
            line.ValueAtXAxis = value;
            line.Style = CursorStyle.PointTracking;
            line.TrackPoint.Color1 = Colors.Transparent;
            line.TrackPoint.Color2 = Colors.Transparent;
            line.TrackPoint.Shape = Shape.Circle;

            ContainerChart.ViewXY.LineSeriesCursors.Add(line);
        }

        public void PreInitialize(double startFrequency, double stopFrequency, double firstSampleTimeStamp, double precision = 1.0)
        {
            _precision = precision;
            _firstSampleTimeStamp = firstSampleTimeStamp;
            _startFrequency = startFrequency;
            _stopFrequency = stopFrequency;
        }

        public void Initialize(double samplingFrequency, string title, XAxisScrollMode scrollMode = XAxisScrollMode.None)
        {
            SampleFrequency = samplingFrequency;
            ContainerChart.BeginUpdate();

            ContainerChart.Title.Text = title;
            ContainerChart.ViewXY.SampleDataSeries[0].Clear();
            ContainerChart.ViewXY.SampleDataSeries[0].SamplingFrequency = 1.0 / _precision;
            ContainerChart.ViewXY.ZoomPanOptions.MouseWheelZooming = MouseWheelZooming.Off;
            ContainerChart.ViewXY.ZoomPanOptions.LeftMouseButtonAction = MouseButtonAction.None;
            ContainerChart.ViewXY.ZoomPanOptions.RightMouseButtonAction = MouseButtonAction.None;

            _xAxis.Minimum = _startFrequency;
            _xAxis.Maximum = _stopFrequency;

            _xAxis.ScrollMode = scrollMode;
            // _xAxis.ScrollMode = XAxisScrollMode.None;

            _xAxis.ScrollPosition = _xAxis.Minimum;
            _xAxis.Position = _xAxis.Minimum;
            _xAxis.Title.Text = string.Format("{0} s", SampleFrequency.ToString("0.000"));

            ContainerChart.ViewXY.SampleDataSeries[0].FirstSampleTimeStamp = _xAxis.Minimum ;
            ContainerChart.ViewXY.DropOldSeriesData = true;

            var samples = new double[(int)SampleFrequency];
            FeedData(samples);

            ContainerChart.EndUpdate();
        }
        #endregion

        protected virtual void OnValueChanged(double newvalue)
        {
            var handler = ValueChanged;
            if (handler != null) handler(newvalue);
        }

        public CursorLinePack AddPairCursors(CursorLinePackConfig config)
        {
            var pack = new CursorLinePack(config);
            pack.AddToChart(ContainerChart);

            _packs.Add(pack);

            return pack;
        }

        public void SetRange(double minimum, double maximum)
        {
            ContainerChart.ViewXY.YAxes[0].SetRange(minimum, maximum);
        }

        public void FitView()
        {
            bool scaleChanged;
            ContainerChart.ViewXY.YAxes[0].Fit(0.0, out scaleChanged, true, false);
        }

        public void Dispose()
        {
            if (ContainerChart == null) return;
            ContainerChart.Dispose();
            ContainerChart = null;
        }

        public void Stop()
        {
            ContainerChart.BeginUpdate();
            ContainerChart.EndUpdate();
        }

        public void SetBounds(int width, int height)
        {
            ContainerChart.Width = width;
            ContainerChart.Height = height;
        }

        public void FeedData(double[] samples)
        {
            if (samples == null) return;

            ContainerChart.BeginUpdate();

            var half = SampleFrequency / 2;
            var skip = half - half * _zoomFactor;
            var take = SampleFrequency * _zoomFactor;

            var selected = samples.Skip((int)skip).Take((int)take).ToArray();
            ContainerChart.ViewXY.SampleDataSeries[0].SamplesDouble = selected;
            ContainerChart.EndUpdate();
        }

        private double _zoomFactor = 1.0;
        internal void Zoom(double factor)
        {
            if (_zoomFactor == 1.0 && factor > 1.0)
                return;
            if (_zoomFactor == 0.0625 && factor < 1.0)
                return;

            _zoomFactor *= factor;
            ContainerChart.BeginUpdate();

            _xAxis.SetRange(_xAxis.Minimum * factor, _xAxis.Maximum * factor);
            _xAxis.ScrollPosition = _xAxis.Minimum;
            _xAxis.Position = _xAxis.Minimum;

            //var defaultMin = -SampleFrequency/2;
            ContainerChart.ViewXY.SampleDataSeries[0].FirstSampleTimeStamp = _xAxis.Minimum;
            ContainerChart.EndUpdate();
        }

        public void SetSampleFrequencyRange(double start, double stop)
        {
            ContainerChart.ViewXY.SampleDataSeries[0].SamplingFrequency = SampleFrequency / (stop - start);
            ContainerChart.ViewXY.SampleDataSeries[0].FirstSampleTimeStamp = _xAxis.Minimum;
        }

        public void SetSpectrumFrequencyRange(double start, double stop)
        {
            _startFrequency = start;
            _stopFrequency = stop;
            _xAxis.Minimum = _startFrequency;
            _xAxis.Maximum = _stopFrequency;
        }

        internal void SetFrequencyIntervals()
        {
            _xAxis.Minimum = _startFrequency;
            _xAxis.Maximum = _stopFrequency;
            ContainerChart.ViewXY.SampleDataSeries[0].SamplingFrequency = 1.0 / _precision;
            ContainerChart.ViewXY.SampleDataSeries[0].FirstSampleTimeStamp = _startFrequency;
        }
    }
}
