using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Arction.WPF.LightningChartUltimate;
using Arction.WPF.LightningChartUltimate.SeriesXY;
using Arction.WPF.LightningChartUltimate.Views.ViewXY;

namespace NetRadio.G313.Chart
{
    public delegate void WaveformPlayerSelectedValueChangedEventHandler(double x, List<KeyValuePair<SampleDataSeries, double>> values);
    public class WaveformPlayer
    {
        #region Constants
        private const ulong SCROLL_BAR_MAX = 100;
        private const ulong SCROLL_BAR_AVAILABLE_MAX = 91;
        private const double TOLERANCE = 0.0001d;
        #endregion

        #region Fields
        private int _points;
        private int _currentIndex;
        private LineSeriesCursor _cursor;
        private double _end;
        private double _start;
        private ScrollBar _scrollBar;
        private double _zoomFactor = 1.0;

        public event WaveformPlayerSelectedValueChangedEventHandler SelectedValueChanged;
        #endregion

        #region Properties
        public LightningChartUltimate ContainerChart { get; set; }
        #endregion

        #region .ctor
        public WaveformPlayer(double yMinimum = 0, double yMaximum = 1000)
        {
            CreateChart(Colors.Tomato, yMinimum, yMaximum);
        }
        #endregion

        protected virtual void OnSelectedValueChanged(double x, List<KeyValuePair<SampleDataSeries, double>> values)
        {
            var handler = SelectedValueChanged;
            if (handler != null) handler(x, values);
        }

        private void CreateChart(Color lineColor, double yMinimum, double yMaximum)
        {
            if (ContainerChart != null)
            {
                ContainerChart.Dispose();
                ContainerChart = null;
            }

            ContainerChart = new LightningChartUltimate
            {
                Name = "Chart2",
                ChartName = "Chart2",
                Title = { Text = string.Empty },
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,                
            };

            ContainerChart.MouseMove += (sender, args) =>
            {
                var position = args.GetPosition(ContainerChart);
                double doubleIndex;
                ContainerChart.ViewXY.XAxes[0].CoordToValue((int)position.X, out doubleIndex, true);

                var list = new List<KeyValuePair<SampleDataSeries, double>>();

                foreach (var series in ContainerChart.ViewXY.SampleDataSeries)
                {
                    var intIndex = doubleIndex * series.SamplingFrequency;
                    if (intIndex > _points || intIndex < 0 || series.SamplesDouble.Length-1<intIndex) return;

                    var value = series.SamplesDouble[(int)intIndex];

                    list.Add(new KeyValuePair<SampleDataSeries, double>(series, value));
                }

                OnSelectedValueChanged(doubleIndex, list);
            };

            var chartView = ContainerChart.ViewXY;            
            chartView.AxisLayout.AutoAdjustMargins = false;
            chartView.AxisLayout.AutoAdjustAxisGap = 0;

            var xAxis = chartView.XAxes[0];
            chartView.AxisLayout.YAxesLayout = YAxesLayout.Layered;
            chartView.AxisLayout.StackedYAxesGap = 1;

            xAxis.ScrollPosition = 0;
            xAxis.ScrollMode = XAxisScrollMode.None;
            xAxis.SweepingGap = 1;
            xAxis.Title.Visible = true;
            xAxis.Title.Text = string.Empty;

            chartView.LegendBox.Visible = false;
            chartView.DropOldSeriesData = false;
            chartView.ZoomPanOptions.RectangleZoomDirectionLayered = RectangleZoomDirectionLayered.Horizontal;
            chartView.ZoomPanOptions.MouseWheelZooming = MouseWheelZooming.Off;
            chartView.ZoomPanOptions.RightMouseButtonAction = MouseButtonAction.None;
            chartView.ZoomPanOptions.ShiftEnabled = false;
            chartView.ZoomPanOptions.CtrlEnabled = false;
            chartView.ZoomPanOptions.RightToLeftZoomAction = RightToLeftZoomAction.Off;
            chartView.ZoomPanOptions.LeftMouseButtonAction = MouseButtonAction.None;
            chartView.ZoomPanOptions.PanDirection = PanDirection.Horizontal;

            // Initialize left cursor
            _cursor = new LineSeriesCursor(ContainerChart.ViewXY, ContainerChart.ViewXY.XAxes[0]) { ValueAtXAxis = 1, LineStyle = { Width = 3 } };
            _cursor.LineStyle.Color = Color.FromArgb(70, lineColor.R, lineColor.G, lineColor.B);
            _cursor.SnapToPoints = true;
            _cursor.Style = CursorStyle.VerticalNoTracking;
            _cursor.TrackPoint.Color1 = Colors.Yellow;
            _cursor.TrackPoint.Color2 = Colors.Transparent;
            _cursor.TrackPoint.Shape = Shape.Circle;
            _cursor.MoveByMouse = false;
            _cursor.IndicateTrackingYRange = true;
            ContainerChart.ViewXY.LineSeriesCursors.Add(_cursor);

            // After test layerd and stacked
            ContainerChart.ViewXY.YAxes[0].SetRange(yMinimum, yMaximum);
            ContainerChart.ViewXY.YAxes[0].Title.Text = string.Empty;

            _scrollBar = new ScrollBar { Offset = new PointIntXY(0, 30) };
            xAxis.LabelsPosition = Alignment.Near;

            _scrollBar.Scroll += (sender, type, value, newValue) =>
            {
                if (value == newValue) return;

                var initialLength = _end - _start;
                var currentLength = xAxis.Maximum - xAxis.Minimum;
                if (Math.Abs(initialLength - currentLength) < TOLERANCE) return;

                ContainerChart.BeginUpdate();

                var ratio = 1 - (currentLength / initialLength);
                var step = ((double)newValue - value) / SCROLL_BAR_AVAILABLE_MAX;
                var offset = ratio * step;

                xAxis.SetRange(xAxis.Minimum + offset, xAxis.Maximum + offset);
                ContainerChart.EndUpdate();
            };

            ContainerChart.ScrollBars.Add(_scrollBar);
        }

        public SampleDataSeries AddNewWaveformPlayer(WaveformPlayerConfig config)
        {
            var chartView = ContainerChart.ViewXY;

            var yAxis = chartView.YAxes.AddNew();
            // ReSharper disable once PossibleNullReferenceException
            yAxis.MouseInteraction = false;
            yAxis.Title.Visible = false;
            yAxis.Visible = false;
            yAxis.SetRange(config.Minimum, config.Maximum);

            chartView.DropOldSeriesData = false;

            var series = new SampleDataSeries(chartView, chartView.XAxes[0], yAxis);
            chartView.SampleDataSeries.Add(series);
            series.LineStyle.Width = 1;
            series.LineStyle.Color = Color.FromArgb(100, config.Color.R, config.Color.G, config.Color.B);
            series.MouseInteraction = false;
            return series;
        }

        public void Initialize(double start, double end, int pointsCount)
        {
            _start = start;
            _end = end;

            var chartView = ContainerChart.ViewXY;
            _points = pointsCount;

            _scrollBar.Minimum = 0;
            _scrollBar.Value = 0;
            _scrollBar.Maximum = SCROLL_BAR_MAX;

            ContainerChart.BeginUpdate();

            var xAxis = chartView.XAxes[0];
            xAxis.ValueType = AxisValueType.Number;
            xAxis.SetRange(start, end);
            xAxis.SteppingInterval = (end - start) / pointsCount;

            var points = new double[chartView.SampleDataSeries.Count][];
            for (var seriesIndex = 0; seriesIndex < points.Length; seriesIndex++)
            {
                points[seriesIndex] = new double[pointsCount];
            }

            foreach (var itemPoint in points)
                for (var index = 0; index < pointsCount; index++)
                    itemPoint[index] = (index % 450) * 10;

            for (var seriesIndex = 0; seriesIndex < chartView.SampleDataSeries.Count; seriesIndex++)
            {
                var series = chartView.SampleDataSeries[seriesIndex];
                series.SamplingFrequency = 1 / xAxis.SteppingInterval;
                series.FirstSampleTimeStamp = start;

                series.SamplesDouble = points[seriesIndex];
                series.InvalidateData();
            }

            ContainerChart.EndUpdate();
        }

        public void FeedNewDataToChart(double[][] data, int length)
        {
            ContainerChart.BeginUpdate();

            var chartView = ContainerChart.ViewXY;

            for (var seriesIndex = 0; seriesIndex < chartView.SampleDataSeries.Count; seriesIndex++)
            {
                var series = chartView.SampleDataSeries[seriesIndex];

                var point = data[seriesIndex];

                for (var index = 0; index < length; index++)
                {
                    if (_currentIndex + index >= _points) break;

                    series.SamplesDouble[_currentIndex + index] = point[index];
                }

                series.InvalidateData();
            }

            if (_currentIndex >= _points) _currentIndex = 0;
            _currentIndex += length;

            var step = _currentIndex * chartView.XAxes[0].SteppingInterval;
            _cursor.ValueAtXAxis = step + _start;

            ContainerChart.EndUpdate();
        }

        public void SetBounds(int width, int height)
        {
            ContainerChart.Width = width;
            ContainerChart.Height = height;
        }

        internal void Zoom(double factor)
        {
            if (_zoomFactor == 1.0 && factor > 1.0)
                return;
            if (_zoomFactor == 0.125 && factor < 1.0)
                return;

            _zoomFactor *= factor;

            var xAxis = ContainerChart.ViewXY.XAxes[0];

            var initialLength = _end - _start;
            xAxis.SetRange(xAxis.Minimum * factor, xAxis.Maximum * factor);
            var diff = _start - xAxis.Minimum;
            xAxis.SetRange(xAxis.Minimum + diff, xAxis.Maximum + diff);

            var currentLength = xAxis.Maximum - xAxis.Minimum;

            var ratio = 1 - (currentLength / initialLength);
            var step = (double)_scrollBar.Value / SCROLL_BAR_AVAILABLE_MAX;
            var offset = ratio * step;

            xAxis.SetRange(xAxis.Minimum + offset, xAxis.Maximum + offset);

            foreach (var series in ContainerChart.ViewXY.SampleDataSeries)
                series.FirstSampleTimeStamp = _start;
        }

        internal void ClearData()
        {
            ContainerChart.BeginUpdate();
            _currentIndex = 0;

            var chartView = ContainerChart.ViewXY;

            var xAxis = chartView.XAxes[0];
            xAxis.SetRange(_start, _end);
            xAxis.ValueType = AxisValueType.Number;

            var points = new double[chartView.SampleDataSeries.Count][];
            for (var seriesIndex = 0; seriesIndex < points.Length; seriesIndex++)
            {
                points[seriesIndex] = new double[_points];
            }

            foreach (var itemPoint in points)
                for (var index = 0; index < _points; index++)
                    itemPoint[index] = 0;

            for (var seriesIndex = 0; seriesIndex < chartView.SampleDataSeries.Count; seriesIndex++)
            {
                var series = chartView.SampleDataSeries[seriesIndex];
                series.SamplingFrequency = 1 / xAxis.SteppingInterval;
                series.FirstSampleTimeStamp = _start;

                series.SamplesDouble = points[seriesIndex];
                series.InvalidateData();
            }

            ContainerChart.EndUpdate();
        }
    }
}
