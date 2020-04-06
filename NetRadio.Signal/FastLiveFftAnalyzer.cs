using System;
using System.Runtime.InteropServices;
using System.Threading;
using FFTWSharp;
using NAudio.Dsp;
using NAudio.Wave;
using NetRadio.Devices;

namespace NetRadio.Signal
{
    /// <summary>
    /// Real-Time FFT analyzer based on underlying stream provider.
    /// </summary>
    public class FastLiveFftAnalyzer : IDisposable
    {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private uint _pushCount;
        private uint _pullCounter;
        private readonly object _bufferLock = new object();
        private FloatArgs _floatArgs;
        private Thread _fftThread;
        private bool _stop;

        /// <summary>
        /// Gets or Sets whether the analyzer should be executed in current thread or not.
        /// </summary>
        public bool SameContext { get; set; }

        private readonly IFloatProvider _source;

        private readonly float[] _in;
        private readonly float[] _out;

        /// <summary>
        /// Occures when FFT calculation is finished.
        /// </summary>
        public event EventHandler<FastFftEventArgs> FftCalculated;

        private readonly FastFftEventArgs _args=new FastFftEventArgs();
        private GCHandle _handleIn;
        private GCHandle _handleOut;
        private readonly IntPtr _handlePlan;

        /// <summary>
        /// Gets specified wave format
        /// </summary>
        public WaveFormat WaveFormat { get; private set; }

        /// <summary>
        /// Gets length of FFT.
        /// </summary>
        public int FftLength { get; private set; }

        /// <summary>
        /// Creates Real-Time FFT analyzer.
        /// </summary>
        /// <param name="source">Specified audio provider.</param>
        /// <param name="bufferSize">Fft input/ output buffer size should be bigger than input data, possibly equal to fftlength</param>
        /// <param name="fftLength">Length of FFT.</param>
        /// <param name="real">True if input is real</param>
        public FastLiveFftAnalyzer(IFloatProvider source, int bufferSize = 1000000, int fftLength = 16384,
            bool real = true)
        {
            if (!IsPowerOfTwo(fftLength))
                throw new ArgumentException("FFT Length must be a power of two");

            _source = source;
            _source.FloatChunkRecieved += FloatChunkRecieved;
            WaveFormat = new WaveFormat(_source.SamplingRate(), _source.Bits(), _source.ChannelCount());


            FftLength = fftLength;

            _in = new float[bufferSize]; //DEFAULT: max buffer is 1M Real/ 512k Complex sample
            _out = new float[bufferSize]; //DEFAULT: max buffer is 1M Real/ 512k Complex sample
            _handleIn = GCHandle.Alloc(_in, GCHandleType.Pinned);
            _handleOut = GCHandle.Alloc(_out, GCHandleType.Pinned);
            _args.Update(_out, fftLength*2);

            _handlePlan = real
                ? fftwf.dft_r2c_1d(fftLength, _handleIn.AddrOfPinnedObject(),
                    _handleOut.AddrOfPinnedObject(),
                    fftw_flags.Patient)
                : fftwf.dft_1d(fftLength, _handleIn.AddrOfPinnedObject(), _handleOut.AddrOfPinnedObject(),
                    fftw_direction.Forward, fftw_flags.Patient);
        }

        private void Analyze()
        {
            FloatArgs floatArgs;

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
                    _pullCounter++;
                }

                if (floatArgs != null)
                    AnalyzeFloat(floatArgs);
            }
        }

        private void FloatChunkRecieved(object sender, FloatArgs e)
        {
            if (SameContext)
                AnalyzeFloat(e);
            else
                lock (_bufferLock)
                {
                    _pushCount++;
                    _floatArgs = e;
                }
        }

        private void AnalyzeFloat(FloatArgs e)
        {
            _in.Initialize();
            _out.Initialize();

            Buffer.BlockCopy(e.RawData,0,_in,0,e.DataLength);

            fftwf.execute(_handlePlan);

            if (FftCalculated != null)
                FftCalculated(this, _args);
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
        }

        /// <summary>
        /// Starts FFT analyzer
        /// </summary>
        public void Start()
        {
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
            fftw.destroy_plan(_handlePlan);
            _handleIn.Free();
            _handleOut.Free();

            Stop();
        }
    }

    /// <summary>
    /// Fast FFT event argument
    /// </summary>
    public class FastFftEventArgs : FftEventArgs
    {
        private Complex[] _complexResult;

        /// <summary>
        /// Gets FFT float result
        /// </summary>
        public float[] FloatResult { get; private set; }

        /// <summary>
        /// Gets Available FFT float result
        /// </summary>
        public int DataLength { get; private set; }

        internal void Update(float[] floatResult, int dataLength)
        {
            FloatResult = floatResult;
            DataLength = dataLength;
        }

        protected internal override Complex[] Result
        {
            get
            {
                if (_complexResult == null)
                    _complexResult = new Complex[DataLength/2];

                var j = 0;
                for (var i = 0; i < DataLength; i += 2)
                {
                    _complexResult[j].X = FloatResult[i];
                    _complexResult[j].Y = FloatResult[i + 1];
                    j++;
                }

                return _complexResult;
            }

            protected set
            {
                throw new NotSupportedException();
            }
        }
    }
}