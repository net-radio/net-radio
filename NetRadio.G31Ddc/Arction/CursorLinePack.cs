using System.Windows.Media;
using Arction.WPF.LightningChartUltimate;
using Arction.WPF.LightningChartUltimate.SeriesXY;
using Arction.WPF.LightningChartUltimate.Views.ViewXY;

namespace NetRadio.G31Ddc.Arction
{
    public delegate void CursorLinePackValueChangedEventHandler(RangePair range, double value);

    public class RangePair
    {
        #region Properties
        public double Start { get; set; }
        public double End { get; set; }
        #endregion

        #region .ctor
        public RangePair() { }

        public RangePair(double minimum, double maximum)
        {
            End = maximum;
            Start = minimum;
        }
        #endregion
    }

    public class CursorLinePackConfig
    {
        public Color Color { get; set; }
        public double ValueMaximum { get; set; }
        public double ValueMinimum { get; set; }
        public double BandwidthMaximum { get; set; }
        public double BandwidthMinimum { get; set; }

        public float LineWidth { get; set; }

        public Color BandColor { get; set; }
    }

    public class CursorLinePack
    {
        #region field
        bool _isChanging;
        public event CursorLinePackValueChangedEventHandler ValueChanged;
        #endregion

        #region Properties
        public LightningChartUltimate Chart { get; set; }
        public CursorLinePackConfig Config { get; set; }
        public LineSeriesCursor Left { get; set; }
        public LineSeriesCursor Right { get; set; }
        public LineSeriesCursor Middle { get; set; }
        public Band BandBar { get; set; }
        public double Value { get; set; }
        public RangePair Range { get; set; }

        public bool Visible
        {
            get { return Left.Visible && Right.Visible && BandBar.Visible && Middle.Visible; }
            set
            {
                Left.Visible = Right.Visible = Middle.Visible = BandBar.Visible = value;
            }
        }

        #endregion

        #region .ctor
        public CursorLinePack(CursorLinePackConfig config)
        {
            Config = config;
        }
        #endregion

        protected virtual void OnValueChanged(RangePair range, double value)
        {
            var handler = ValueChanged;
            if (handler != null) handler(range, value);
        }

        public void AddToChart(LightningChartUltimate chart)
        {
            Chart = chart;

            var xAxis = chart.ViewXY.XAxes[0];
            var yAxis = chart.ViewXY.YAxes[0];
            var color = Config.Color;

            // Initialize left cursor
            Left = new LineSeriesCursor(Chart.ViewXY, xAxis) { ValueAtXAxis = 1, LineStyle = { Width = Config.LineWidth } };
            Left.LineStyle.Color = Color.FromArgb(180, color.R, color.G, color.B);
            Left.FullHeight = false;
            Left.SnapToPoints = false;
            Left.Style = CursorStyle.PointTracking;
            Left.TrackPoint.Color1 = Colors.Transparent;
            Left.TrackPoint.Color2 = Colors.Transparent;
            Left.TrackPoint.Shape = Shape.Circle;
            Chart.ViewXY.LineSeriesCursors.Add(Left);

            // Initialize right cursor
            Right = new LineSeriesCursor(Chart.ViewXY, xAxis) { ValueAtXAxis = 1, LineStyle = { Width = Config.LineWidth } };
            Right.LineStyle.Color = Color.FromArgb(180, color.R, color.G, color.B);
            Right.FullHeight = false;
            Right.SnapToPoints = false;
            Right.Style = CursorStyle.PointTracking;
            Right.TrackPoint.Color1 = Colors.Transparent;
            Right.TrackPoint.Color2 = Colors.Transparent;
            Right.TrackPoint.Shape = Shape.Circle;
            Chart.ViewXY.LineSeriesCursors.Add(Right);

            // Initialize middle cursor
            Middle = new LineSeriesCursor(Chart.ViewXY, xAxis) { ValueAtXAxis = 1, LineStyle = { Width = Config.LineWidth } };
            Middle.LineStyle.Color = Color.FromArgb(180, color.R, color.G, color.B);
            Middle.FullHeight = false;
            Middle.SnapToPoints = false;
            Middle.Style = CursorStyle.PointTracking;
            Middle.TrackPoint.Color1 = Colors.Transparent;
            Middle.TrackPoint.Color2 = Colors.Transparent;
            Middle.TrackPoint.Shape = Shape.Circle;
            Chart.ViewXY.LineSeriesCursors.Add(Middle);

            //Edit bands           
            BandBar = new Band(Chart.ViewXY, xAxis, yAxis)
            {
               Title = { Text = string.Empty },
               Behind = true,
               Binding = AxisBinding.XAxis
            };
            var bandColor = Config.BandColor;
            BandBar.Fill.Color = Color.FromArgb(bandColor.A, bandColor.R, bandColor.G, bandColor.B);
            BandBar.Fill.GradientColor = ChartTools.CalcGradient(BandBar.Fill.Color, Color.FromArgb(150, 0, 0, 0), 50);
            BandBar.Fill.GradientDirection = 0;

            BandBar.Title.Visible = true;
            BandBar.Title.Color = Colors.White;
            BandBar.MouseResize = false;
            Chart.ViewXY.Bands.Add(BandBar);

            BandBar.MovedByMouse += (sender, args) =>
            {
                if (_isChanging) return;
                _isChanging = true;

                var middle = (BandBar.ValueBegin + BandBar.ValueEnd) / 2;
                var diff = middle - Value;

                var left = Left.ValueAtXAxis + diff;
                var right = Right.ValueAtXAxis + diff;

                SetByValue(right, left, middle);

                // resize bandwidth
                //var middleDiff = Middle.ValueAtXAxis - BandBar.ValueBegin;
                //if (middleDiff >= 0)
                //{
                //    Range.Start = -middleDiff;
                //    Range.End = middleDiff;
                //}
                //else
                //{
                //    Range.Start = 0;
                //    Range.End = 0;
                //}

                //if (middleDiff > Config.BandwidthMaximum)
                //{
                //    Range.Start = -Config.BandwidthMaximum;
                //    Range.End = Config.BandwidthMaximum;
                //}

                //var right = Middle.ValueAtXAxis + Range.End;
                //var left = Middle.ValueAtXAxis + Range.Start;
                //var middle = Middle.ValueAtXAxis;

                //SetByValue(right, left, middle);

                _isChanging = false;
            };

            Right.PositionChanged += (LineSeriesCursor sender, double value, ref bool rendering) =>
            {
                if (_isChanging) return;
                _isChanging = true;

                var middleDiff = Right.ValueAtXAxis - Middle.ValueAtXAxis;
                if (middleDiff >= 0)
                {
                    Range.Start = -middleDiff;
                    Range.End = middleDiff;
                }
                else
                {
                    Range.Start = 0;
                    Range.End = 0;
                }

                if (middleDiff > Config.BandwidthMaximum)
                {
                    Range.Start = -Config.BandwidthMaximum;
                    Range.End = Config.BandwidthMaximum;
                }

                var right = Middle.ValueAtXAxis + Range.End;
                var left = Middle.ValueAtXAxis + Range.Start;
                var middle = Middle.ValueAtXAxis;

                SetByValue(right, left, middle);

                _isChanging = false;
            };

            Left.PositionChanged += (LineSeriesCursor sender, double value, ref bool rendering) =>
            {
                if (_isChanging) return;
                _isChanging = true;

                var middleDiff = Middle.ValueAtXAxis - Left.ValueAtXAxis;
                if (middleDiff >= 0)
                {
                    Range.Start = -middleDiff;
                    Range.End = middleDiff;
                }
                else
                {
                    Range.Start = 0;
                    Range.End = 0;
                }

                if (middleDiff > Config.BandwidthMaximum)
                {
                    Range.Start = -Config.BandwidthMaximum;
                    Range.End = Config.BandwidthMaximum;
                }

                var right = Middle.ValueAtXAxis + Range.End;
                var left = Middle.ValueAtXAxis + Range.Start;
                var middle = Middle.ValueAtXAxis;

                SetByValue(right, left, middle);

                _isChanging = false;
            };

            Middle.PositionChanged += (LineSeriesCursor sender, double value, ref bool rendering) =>
            {
                if (_isChanging) return;
                _isChanging = true;

                var diff = value - Value;

                var left = Left.ValueAtXAxis + diff;
                var right = Right.ValueAtXAxis + diff;

                SetByValue(right, left, value);

                _isChanging = false;
            };
        }

        internal void SetValue(double start, double end)
        {
            _isChanging = true;

            var middle = (end - start) / 2;
            Range = new RangePair(-middle, middle);

            SetByValue(end, start, (start + end) / 2);

            _isChanging = false;
        }

        private void SetByValue(double right, double left, double middle)
        {
            /*
            if (right >= Config.ValueMaximum)
            {
                Right.ValueAtXAxis = Config.ValueMaximum;
                Value = Middle.ValueAtXAxis = Config.ValueMaximum + Range.Start;
                Left.ValueAtXAxis = Value + Range.Start;
            }
            else if (left <= Config.ValueMinimum)
            {
                Left.ValueAtXAxis = Config.ValueMinimum;
                Value = Middle.ValueAtXAxis = Config.ValueMinimum + Range.End;
                Right.ValueAtXAxis = Value + Range.End;
            }
            
            else
            {
                Right.ValueAtXAxis = right;
                Left.ValueAtXAxis = left;

                Value = Middle.ValueAtXAxis = middle;
            }
            */
            Right.ValueAtXAxis = right;
            Left.ValueAtXAxis = left;

            Value = Middle.ValueAtXAxis = middle;

            BandBar.ValueBegin = Left.ValueAtXAxis;
            BandBar.ValueEnd = Right.ValueAtXAxis;

            OnValueChanged(Range, Value);
        }        
    }
}
