using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NAudio.Dsp;
using NetRadio.Signal;

namespace NetRadio.G313.Sample
{
    class SpectrumAnalyzer:UserControl
    {
        private Pen _pen = new Pen(Color.Orange, 1.0F);
        private Pen _penHalf = new Pen(Color.Green, 1.0F);
        private Pen _penFull = new Pen(Color.Red, 1.0F);

        public int IfBandwidth { get; set; }
        public int IfShift { get; set; }
        public int NotchFreq { get; set; }
        public int NotchBandwidth { get; set; }

        private Bitmap _backBuffer;
        private Graphics _gBuffer;
        private Graphics _g;

        private const int _binsPerPoint = 1; //6; // reduce the number of points we plot for a less jagged line?
        private double _xScale = 100;
        private int _bins;
        private List<Point> _polyline = new List<Point>();
        public SpectrumAnalyzer()
        {
            InitializeComponent();
            CalculateXScale();
            this.SizeChanged += SpectrumAnalyzer_SizeChanged;
        }

        void SpectrumAnalyzer_SizeChanged(object sender, EventArgs e)
        {
            CalculateXScale();
        }

        public void Clear()
        {
            _polyline.Clear();
        }

        public new int Width
        {
            get { return base.Width; }
            set
            {
                base.Width = value;
                CalculateXScale();
            }
        }

        private void CalculateXScale()
        {
            if (_gBuffer != null)
                _gBuffer.Dispose();
            if (_g != null)
                _g.Dispose();
            if (_backBuffer != null)
                _backBuffer.Dispose();

            this._xScale = this.Width / (double)(_bins / _binsPerPoint);
            _backBuffer = new Bitmap(this.Width, this.Height);
            _gBuffer = Graphics.FromImage(_backBuffer);
            if(this.InvokeRequired)
            this.Invoke(new Action(()=>_g = Graphics.FromHwnd(this.Handle)));
            else
            {
                _g = Graphics.FromHwnd(this.Handle);
            }
        }

        public void Update(FrequencyBins bin)
        {
            if (bin.BinsCount != _bins)
            {
                _bins = bin.BinsCount;
                CalculateXScale();
            }

            var intensities = bin.Intensities().ToArray();

            for (int n = 0; n < bin.BinsCount- _binsPerPoint; n += 1)
            {
                // averaging out bins
                double yPos = 0;
                for (int b = 0; b < _binsPerPoint; b++)
                {
                    yPos += GetYPosLog(intensities[n + b],bin.MinimumIntensity);
                }
                AddResult(n / _binsPerPoint, yPos / _binsPerPoint);
            }

            Draw(bin.SpectralWidth);
        }

        private void Draw(int spectralWidth)
        {
            _gBuffer.Clear(Color.Black);

            spectralWidth /= 1000;

            var ifShift = IfShift * spectralWidth / 1000;
            var ifbandwidth = IfBandwidth * spectralWidth / 1000;
            var bandwidthX = Width / 2 - ifbandwidth / 2 + ifShift;
            _gBuffer.FillRectangle(new SolidBrush(Color.FromArgb(128, 255, 255, 255)), bandwidthX, 0, ifbandwidth, Height);

            var notchFreq = NotchFreq * spectralWidth / 1000;
            var notchbandwidth = NotchBandwidth * spectralWidth / 1000;
            var notchX = Width / 2 - notchbandwidth / 2 + notchFreq;
            _gBuffer.FillRectangle(new SolidBrush(Color.FromArgb(128,255,100,100)), notchX, 0, notchbandwidth, Height);

            for (int i = 1; i < _polyline.Count; i++)
            {
                _gBuffer.DrawLine(_pen, _polyline[i - 1], _polyline[i]);
            }


            _gBuffer.DrawLine(_pen, new Point(Width / 2, 0), new Point(Width / 2, Height));
            _gBuffer.DrawLine(_penHalf, new Point((Width / 2) + (Width * 5 / spectralWidth), 0), new Point((Width / 2) + (Width * 5 / spectralWidth), Height));
            _gBuffer.DrawLine(_penFull, new Point((Width / 2) + (Width * 10 / spectralWidth), 0), new Point((Width / 2) + (Width * 10 / spectralWidth), Height));
            _gBuffer.DrawLine(_penHalf, new Point((Width / 2) - (Width * 5 / spectralWidth), 0), new Point((Width / 2) - (Width * 5 / spectralWidth), Height));
            _gBuffer.DrawLine(_penFull, new Point((Width / 2) - (Width * 10 / spectralWidth), 0), new Point((Width / 2) - (Width * 10 / spectralWidth), Height));


            _g.DrawImageUnscaled(_backBuffer, 0, 0);
        }

        private double GetYPosLog(Complex c)
        {
            // not entirely sure whether the multiplier should be 10 or 20 in this case.
            // going with 20 from here http://stackoverflow.com/a/10636698/7532
            double intensityDB = 20 * Math.Log10(Math.Sqrt(c.X * c.X + c.Y * c.Y));
            double minDB = -100;
            if (intensityDB < minDB) intensityDB = minDB;
            double percent = intensityDB / minDB;
            // we want 0dB to be at the top (i.e. yPos = 0)
            var calibration =1.5;

            double yDb = percent * minDB * calibration - minDB * (calibration - 1.0);
            //double yPos = percent * this.Height*calibration -this.Height*(calibration-1.0);

            double yPos = yDb/minDB * this.Height;
            return yPos;
        }

        private double GetYPosLog(double decible,double minDb)
        {
            return decible/minDb*Height;
        }
        private void AddResult(int index, double power)
        {
            Point p = new Point(CalculateXPos(index),(int)Math.Round(power,0));
            if (index >= _polyline.Count)
                _polyline.Add(p);
                _polyline[index] = p;
        }

        private int CalculateXPos(int bin)
        {
            if (bin == 0) return 0;
            var real= bin * _xScale; // Math.Log10(bin) * xScale;
            return (int)Math.Round(real,0);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SpectrumAnalyzer
            // 
            this.DoubleBuffered = true;
            this.Name = "SpectrumAnalyzer";
            this.ResumeLayout(false);

        }
    }
}
