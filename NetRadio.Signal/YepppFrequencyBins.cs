using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NAudio.Dsp;
using NetRadio.Signal.Utilities;

namespace NetRadio.Signal
{
    public class YepppFrequencyBins:FastFrequencyBins
    {
        //private readonly double[] _rArr;
        private readonly double[] _iArr;
        private readonly double _ln10Value;


        public YepppFrequencyBins(BinParameters parameters, int fftLength,bool real)
            :base(parameters,fftLength,real)
        {
            if(parameters.BinsPerPoint>1)
                throw new NotSupportedException();

            //_rArr=new double[fftLength/2];
            _iArr=new double[fftLength/2];
            _ln10Value = 1/Math.Log(10, Math.E);

        }

        public override void Update(FftEventArgs args)
        {
            FillBins(args.Result);
        }

        private unsafe void FillBins(Complex[] fftResults)
        {
            var watch = new Stopwatch();
            watch.Start();

            fixed (double* ptrBinsIntensity = &BinsIntensity[0])
            fixed (double* ptrIArr = &_iArr[0])
            {
                var length = BinsCount;

                for (var n = 0; n < length; n += 1)
                {
                    BinsIntensity[n] = fftResults[n].X;
                    _iArr[n] = fftResults[n].Y;
                }

                Yeppp.Core.Multiply_IV64fV64f_IV64f(ptrBinsIntensity, ptrBinsIntensity,BinsIntensity.Length);
                Yeppp.Core.Multiply_IV64fV64f_IV64f(ptrIArr, ptrIArr, _iArr.Length);
                Yeppp.Core.Add_IV64fV64f_IV64f(ptrBinsIntensity, ptrIArr, BinsIntensity.Length);

                for (var n = 0; n < length; n += 1)
                {
                    var f = BinsIntensity[n]; //HACK: register call
                    BinsIntensity[n] = FastMath.Sqrt((float) f);
                }

                Yeppp.Math.Log_V64f_V64f(ptrBinsIntensity, ptrBinsIntensity,BinsIntensity.Length);
                Yeppp.Core.Multiply_IV64fS64f_IV64f(ptrBinsIntensity, _ln10Value, BinsIntensity.Length);
                Yeppp.Core.Multiply_IV64fS64f_IV64f(ptrBinsIntensity, A, BinsIntensity.Length);
                Yeppp.Core.Subtract_IV64fS64f_IV64f(ptrBinsIntensity, B, BinsIntensity.Length);

                if (!RealMode)
                {
                    var swapSize = fftResults.Length/2;
                    for (var i = 0; i < swapSize; i++)
                    {
                        var tmp = BinsIntensity[i];
                        BinsIntensity[i] = BinsIntensity[swapSize + i];
                        BinsIntensity[swapSize + i] = tmp;
                    }
                }

                watch.Stop();
                var res = watch.ElapsedMilliseconds;
                Console.WriteLine("update bin:{0}", res);
            }
        }
    }
}
