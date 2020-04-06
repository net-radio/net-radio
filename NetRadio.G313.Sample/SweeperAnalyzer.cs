using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetRadio.Devices.G313.Signal;

namespace NetRadio.G313.Sample
{
    public partial class SweeperAnalyzer : UserControl
    {
        private Pen _pen = new Pen(Color.Orange, 1.0F);
        private Pen _penHalf = new Pen(Color.Green, 1.0F);
        private Pen _penFull = new Pen(Color.Red, 1.0F);

        public SweeperAnalyzer()
        {
            InitializeComponent();
        }

        public void Update(ICollection<SweepedFrequency> freqs)
        {
            var freqsArr = freqs.ToArray();
            var x = Width/(double)freqsArr.Length;
            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {
                g.Clear(Color.Black);
                var lastPoint = new Point();
                var lastMin = new Point();
                var lastMax = new Point();
                for (int i = 0; i < freqsArr.Length; i++)
                {
                    var point = new Point((int)(i*x),(int) (freqsArr[i].Current/-100*Height));
                    g.DrawLine(_pen,lastPoint,point);
                    lastPoint = point;

                    //var min = new Point((int)(i * x), (int)(freqsArr[i].Min / -100 * Height));
                    //g.DrawLine(_penFull, lastMin, min);
                    //lastMin = point;

                    //var max = new Point((int)(i * x), (int)(freqsArr[i].Max / -100 * Height));
                    //g.DrawLine(_penFull, lastMax, max);
                    //lastMax = point;
                }
            }


        }
        private double GetYPosLog(double decible)
        {
            return decible / 100 * Height;
        }
    }
}
