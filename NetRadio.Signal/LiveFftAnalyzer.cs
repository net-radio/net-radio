using System;
using System.Threading;
using NAudio.Dsp;
using NAudio.Wave;
using NetRadio.Devices;
using NetRadio.Signal.Utilities;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using FFTWSharp;

namespace NetRadio.Signal
{
    /// <summary>
    /// Real-Time FFT analyzer based on underlying stream provider.
    /// </summary>
    public class LiveFftAnalyzer : IDisposable
    {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private uint _pushCount;
        private uint _pullCounter;
        private object _bufferLock = new object();
        private FloatArgs _floatArgs;
        private ChunkArgs _rawArgs;
        private Thread _fftThread;
        private bool _stop;

        private readonly FloatConverter _converter;
        private readonly int _channels;
        private readonly int _step;
        private bool _real;

        private readonly FftEventArgs _fftArgs;
        private readonly Complex[] _fftBuffer;
        private readonly int _fftLength;
        private readonly int _m;
        private readonly IAudioProvider _source;
        private int _count;
        private int _fftPos;
        private float _maxValue;
        private float _minValue;
        private int _notificationCount = 1;

        public bool ForceInstantFft { get; set; }

        /// <summary>
        /// Occures when maximum intensity is calculated.
        /// </summary>
        public event EventHandler<MaxSampleEventArgs> MaximumCalculated;

        /// <summary>
        /// Occures when FFT calculation is finished.
        /// </summary>
        public event EventHandler<FftEventArgs> FftCalculated;

        /// <summary>
        /// Gets specified wave format
        /// </summary>
        public WaveFormat WaveFormat { get; private set; }

        /// <summary>
        /// Sets maximum intensity notifation rate
        /// </summary>
        /// <param name="count">Notification count.</param>
        /// <returns>Return <see cref="FastLiveFftAnalyzer"/> FFT analyzer.</returns>
        public LiveFftAnalyzer NotifyPeakEvery(int count)
        {
            _notificationCount = count;
            return this;
        }

        /// <summary>
        /// Gets maximum intensity notifation rate
        /// </summary>
        /// <returns>Returns notification rate.</returns>
        public int NotifyPeakEvery()
        {
            return _notificationCount;
        }

        /// <summary>
        /// Gets length of FFT.
        /// </summary>
        public int FftLength { get; private set; }

        /// <summary>
        /// Creates Real-Time FFT analyzer.
        /// </summary>
        /// <param name="source">Specified audio provider.</param>
        /// <param name="fftLength">Length of FFT.</param>
        public LiveFftAnalyzer(IAudioProvider source, int fftLength = 16384, bool real = true)
        {
            _real = real;
            FftLength = fftLength;

            _channels = source.ChannelCount();
            if (!IsPowerOfTwo(fftLength))
                throw new ArgumentException("FFT Length must be a power of two");

            _m = (int)Math.Log(fftLength, 2.0);
            _fftLength = fftLength;

            FftPlan(_fftLength);

            _fftBuffer = new Complex[fftLength];
            _fftArgs = new FftEventArgs(_fftBuffer);

            _source = source;
            _source.DataChunkRecieved += DataChunkRecieved;

            if (_source is IFloatProvider)
                (_source as IFloatProvider).FloatChunkRecieved += FloatChunkRecieved;

            WaveFormat = new WaveFormat(_source.SamplingRate(), _source.Bits(), _source.ChannelCount());
            _converter = new FloatConverter(_source.SamplingRate(), _source.Bits(), _source.ChannelCount());
            _step = _channels * _converter.Step();

        }



        //pointers to unmanaged arrays
        IntPtr pin, pout;

        //managed arrays
        float[] fin, fout;
        double[] din, dout;

        //handles to managed arrays, keeps them pinned in memory
        GCHandle hin, hout, hdin, hdout;

        //pointers to the FFTW plan objects
        // IntPtr fplan1, fplan2, fplan3, fplan4, fplan5;
        IntPtr fplan5;

        // and an example of the managed interface
        fftw_plan mplan;
        fftw_complexarray min, mout;

        // Initializes FFTW and all arrays
        // n: Logical size of the transform


        private void FftPlan(int _fftLength)
        {
            System.Console.WriteLine("Starting test with _fftLength = " + _fftLength + " complex numbers");

            // create two unmanaged arrays, properly aligned
            pin = fftwf.malloc(_fftLength * 8);
            pout = fftwf.malloc(_fftLength * 8);

            // create two managed arrays, possibly misalinged
            // _fftLength*2 because we are dealing with complex numbers
            fin = new float[_fftLength * 2];
            fout = new float[_fftLength * 2];
            // and two more for double FFTW
            din = new double[_fftLength * 2];
            dout = new double[_fftLength * 2];

            // get handles and pin arrays so the GC doesn't move them
            hin = GCHandle.Alloc(fin, GCHandleType.Pinned);
            hout = GCHandle.Alloc(fout, GCHandleType.Pinned);
            hdin = GCHandle.Alloc(din, GCHandleType.Pinned);
            hdout = GCHandle.Alloc(dout, GCHandleType.Pinned);

            // create a few test transforms
            /*
            fplan1 = fftwf.dft_1d(_fftLength, pin, pout, fftw_direction.Forward, fftw_flags.Estimate);
            fplan2 = fftwf.dft_1d(_fftLength, hin.AddrOfPinnedObject(), hout.AddrOfPinnedObject(),
                fftw_direction.Forward, fftw_flags.Estimate);
            fplan3 = fftwf.dft_1d(_fftLength, hout.AddrOfPinnedObject(), pin,
                fftw_direction.Backward, fftw_flags.Measure);
            // end with transforming back to original array
            fplan4 = fftwf.dft_1d(_fftLength, hout.AddrOfPinnedObject(), hin.AddrOfPinnedObject(),
                fftw_direction.Backward, fftw_flags.Estimate);
            */
            // and check a quick one with doubles, just to be sure
            fplan5 = fftw.dft_1d(_fftLength, hdin.AddrOfPinnedObject(), hdout.AddrOfPinnedObject(),
                fftw_direction.Forward, fftw_flags.Measure);

            // create a managed plan as well
            min = new fftw_complexarray(din);
            mout = new fftw_complexarray(dout);
            mplan = fftw_plan.dft_1d(_fftLength, min, mout, fftw_direction.Forward, fftw_flags.Estimate);

            // fill our arrays with an arbitrary complex sawtooth-like signal
            for (int i = 0; i < _fftLength * 2; i++)
                fin[i] = i % 50;
            for (int i = 0; i < _fftLength * 2; i++)
                fout[i] = i % 50;
            for (int i = 0; i < _fftLength * 2; i++)
                din[i] = i % 50;

            // copy managed arrays to unmanaged arrays
            Marshal.Copy(fin, 0, pin, _fftLength * 2);
            Marshal.Copy(fout, 0, pout, _fftLength * 2);
        }

        private void DataChunkRecieved(object sender, ChunkArgs e)
        {
            logger.Debug(string.Format(" DATA CHUNK RECIEVED - {7,7} : Source: {0,-55} Real: {1,-5} Step: {2} Bits: {3} Sample rate: {4,-10} Data length: {5,-7} Fft Length: {6,-6}", sender.ToString(), _real, _step, _converter.BitRate, e.SamplingRate, e.RawData.Length, _fftLength, _pushCount + 1));
            lock (_bufferLock)
            {
                _pushCount++;
                _rawArgs = e;
            }
        }

        private void FloatChunkRecieved(object sender, FloatArgs e)
        {
            logger.Debug(string.Format("FLOAT CHUNK RECIEVED - {7,7} : Source: {0,-55} Real: {1} Step: {2} Bits: {3} Sample rate: {4,-10} Data length: {5,-7} Fft Length: {6,-6}", sender.ToString(), _real, _step, _converter.BitRate, e.SamplingRate, e.RawData.Length, _fftLength, _pushCount + 1));
            lock (_bufferLock)
            {
                _pushCount++;
                _floatArgs = e;
            }
        }

        private void Analyze()
        {
            FloatArgs floatArgs;
            ChunkArgs rawArgs;

            while (!_stop)
            {
                lock (_bufferLock)
                {
                    if (_pullCounter == _pushCount)
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    floatArgs = _floatArgs;
                    rawArgs = _rawArgs;

                    while (_pullCounter < _pushCount)
                        _pullCounter++;

                }

                if (floatArgs != null)
                    AnalyzeFloat(floatArgs);

                if (rawArgs != null)
                    AnalyzeRaw(rawArgs);
            }
        }

        private void AnalyzeFloat(FloatArgs e)
        {
            if (_real)
                for (var n = 0; n < e.DataLength; n += _channels)
                {
                    var f = e.RawData[n];
                    Add(f, 0);
                }
            else // NetRadio.Devices.G3XDdc.Signal.G3XDdcDdc2StreamProvider
                for (var n = 0; n < e.DataLength; n += _channels * 2)
                {
                    var r = e.RawData[n];
                    var i = e.RawData[n + 1];
                    Add(r, i);
                }

            if (ForceInstantFft)
                CalculateFft();
        }

        private void AnalyzeRaw(ChunkArgs e)
        {
            // logger.Debug(string.Format("ANALYZE RAW Real: {0} Step: {1} Bits: {2} Sample rate: {3} Data length: {4} Fft Length: {5}", _real, _step, _converter.BitRate, e.SamplingRate, e.RawData.Length, _fftLength));

            if (_real)
            {
                // NetRadio.Devices.G3XDdc.Signal.G3XDdcIfProvider
                for (var n = 0; n < e.RawData.Length; n += _step)
                {
                    var f = _converter.Convert(e.RawData, n);
                    Add(f, 0);
                }
            }
            else
            {
                // NetRadio.Devices.G3XDdc.Signal.G3XDdcDdc1StreamProvider
                for (var n = 0; n < e.RawData.Length; n += _step * 2)
                {
                    var r = _converter.Convert(e.RawData, n);
                    var i = _converter.Convert(e.RawData, n + _step);
                    Add(r, i);
                }
            }

            if (ForceInstantFft)
                CalculateFft();
        }

        private static bool IsPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }

        /// <summary>
        /// Resets Real-Time FFT analyzer.
        /// </summary>
        public void Reset()
        {
            _count = 0;
            _maxValue = _minValue = 0;
        }

        private void Add(float r, float i)
        {
            _count++;

            if (FftCalculated != null)
            {
                _fftBuffer[_fftPos].X = (float)(r * FastFourierTransform.HammingWindow(_fftPos, _fftLength));
                _fftBuffer[_fftPos].Y = (float)(i * FastFourierTransform.HammingWindow(_fftPos, _fftLength));
                _fftPos++;
                if (_fftPos >= _fftBuffer.Length)
                    CalculateFft();

            }

            _maxValue = Math.Max(_maxValue, r);
            _minValue = Math.Min(_minValue, r);

            if (_count < _notificationCount || _notificationCount <= 0) return;
            if (MaximumCalculated != null)
            {
                MaximumCalculated(this, new MaxSampleEventArgs(_minValue, _maxValue));
            }
            Reset();
        }

        private void CalculateFft()
        {
            var oldPos = _fftPos;

            _fftPos = 0;

            for (int i = 0; i < _fftBuffer.Length; i++)
            {
                din[i * 2] = _fftBuffer[i].X;
                din[i * 2 + 1] = _fftBuffer[i].Y;
            }

            fftwf.execute(fplan5);

            for (int i = 0; i < _fftBuffer.Length; i++)
            {
                _fftBuffer[i].X = Convert.ToSingle(dout[i * 2]);
                _fftBuffer[i].Y = Convert.ToSingle(dout[i * 2 + 1]);
            }

            // FastFourierTransform.FFT(true, _m, _fftBuffer);

            FftCalculated(this, _fftArgs);

            for (int i = 0; i < oldPos; i++)
                _fftBuffer[i] = new Complex();
        }

        /// <summary>
        /// Starts FFT analyzer
        /// </summary>
        public void Start()
        {
            _stop = false;

            _fftThread = new Thread(Analyze) { Name = "fft thread" };
            _fftThread.Start();

            _source.Start();
        }

        /// <summary>
        /// Stops FFT analyzer
        /// </summary>
        public void Stop()
        {
            _stop = true;

            _source.Stop();
            Reset();
        }

        public void Dispose()
        {
            // it is essential that you call these after finishing
            // that's why they're in the destructor. See Also: RAII
            fftwf.free(pin);
            fftwf.free(pout);
            //fftwf.destroy_plan(fplan1);
            //fftwf.destroy_plan(fplan2);
            //fftwf.destroy_plan(fplan3);
            //fftwf.destroy_plan(fplan4);
            fftwf.destroy_plan(fplan5);
            hin.Free();
            hout.Free();

            Stop();
        }
    }
}