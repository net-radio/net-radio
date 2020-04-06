using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetRadio.Signal;

namespace NetRadio.Devices.G313.Signal
{
    /// <summary>
    /// Represents software based sweeper for WinRadio G313 radio.
    /// </summary>
    public class G313Sweeper
    {
        private readonly G313Radio _radio;
        private uint _oldFreqeuncy;
        private Agc _oldAgc;
        private int _oldIfGain;
        private bool _stop;

        private readonly SweepParameters _sweepParameters;
        private readonly ManualResetEvent _fftLock = new ManualResetEvent(false);

        private double _lastFrequency;

        private readonly Dictionary<double, double> _current = new Dictionary<double, double>();
        private readonly Dictionary<double, double> _min = new Dictionary<double, double>();
        private readonly Dictionary<double, double> _max = new Dictionary<double, double>();

        /// <summary>
        /// Occures when a frequency band is sweeped.
        /// </summary>
        public event EventHandler<SweepedArgs> FrequencySweeped;
        /// <summary>
        /// Occures when sweeping is finished or stopped.
        /// </summary>
        public event EventHandler<EventArgs> SweepFinished;
        /// <summary>
        /// Occures when a sweeping round is finished.
        /// </summary>
        public event EventHandler<EventArgs> SweepDone;

        /// <summary>
        /// Raises <see cref="FrequencySweeped"/>
        /// </summary>
        /// <param name="data">Sweep data</param>
        /// <param name="frequency">Sweeped frequency band.</param>
        /// <param name="precision">Sweep precision (Hz).</param>
        protected void OnFrequencySweeped(ICollection<SweepedFrequency> data, uint frequency, double precision)
        {
            if (FrequencySweeped != null)
                FrequencySweeped(this, new SweepedArgs(frequency, precision, data));
        }

        /// <summary>
        /// Raises <see cref="SweepFinished"/>
        /// </summary>
        protected void OnSweepFinished()
        {
            if (SweepFinished != null)
                SweepFinished(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="SweepDone"/>
        /// </summary>
        protected void OnSweepDone()
        {
            if (SweepDone != null)
                SweepDone(this, EventArgs.Empty);
        }

        /// <summary>
        /// Creates a WinRadio G313 sweeper context.
        /// </summary>
        /// <param name="radio">Radio context.</param>
        /// <param name="parameters">Sweepe parameters</param>
        public G313Sweeper(G313Radio radio, SweepParameters parameters)
        {
            _radio = radio;
            _sweepParameters = parameters;
        }

        /// <summary>
        /// Starts sweeping.
        /// </summary>
        /// <returns>Returns <see cref="G313Sweeper"/> sweeper context.</returns>
        public G313Sweeper Sweep()
        {
            SweepOnBand();
            OnSweepFinished();
            return this;
        }

        private void SweepOnBand()
        {
            BackupOldParameters();
            AutoConfigure();

            Execute();

            _lastFrequency = 0;
            _fftLock.Reset();

            RetrieveOldParameters();

            OnSweepDone();
        }

        /// <summary>
        /// Starts sweeping.
        /// </summary>
        /// <returns>Returns sweeper task.</returns>
        public async Task SweepAsync()
        {
            _stop = false;
            await TaskUtility.Run(() => Sweep());
        }

        /// <summary>
        /// Starts infinite sweeping loop.
        /// </summary>
        /// <returns>Returns sweeper task.</returns>
        public async Task LoopAsync()
        {
            _stop = false;
            while (!_stop)
            {
                //Reset();
                await TaskUtility.Run(SweepOnBand);
            }
            OnSweepFinished();
        }

        /// <summary>
        /// Stops sweeping operation.
        /// </summary>
        /// <returns>Returns <see cref="G313Sweeper"/> sweeper context.</returns>
        public G313Sweeper Stop()
        {
            _stop = true;
            return this;
        }

        /// <summary>
        /// Resets Sweeper state.
        /// </summary>
        /// <returns>Returns <see cref="G313Sweeper"/> sweeper context.</returns>
        public G313Sweeper Reset()
        {
            _lastFrequency = 0;
            _fftLock.Reset();

            _current.Clear();
            _min.Clear();
            _max.Clear();

            return this;
        }

        private void Execute()
        {
            var fftLength = FftLength();
            var garbage = 0;

            var fft = new LiveFftAnalyzer(new G313IfProvider(_radio), fftLength);
            fft.FftCalculated += (s, e) =>
            {
                garbage++;
                if (garbage < _sweepParameters.FftWarmup)
                    return;

                fft.Stop();
                fft.Reset();
                SweepBins(e);
                garbage = 0;
                _fftLock.Set();
            };

            var start = _sweepParameters.From + _sweepParameters.SweepSpan;
            var end = _sweepParameters.To - _sweepParameters.SweepSpan;

            for (var i = start; i < end; i += _sweepParameters.SweepSpan)
            {
                if (_stop)
                {
                    //OnSweepFinished();
                    fft.Stop();
                    return;
                }

                _fftLock.Reset();
                _radio.Frequency((uint) i);
                fft.Start();
                _fftLock.WaitOne();
            }

            //OnSweepFinished();
        }

        private void RetrieveOldParameters()
        {
            _radio.Frequency(_oldFreqeuncy)
                .Agc(_oldAgc)
                .IfGain(_oldIfGain);
            //.Demodulator()
            //.SoftwareAgc(_oldSagc);
        }

        private void SweepBins(FftEventArgs e)
        {
            //mark frequencies based on current IF2 in [-10Khz, 10KHz] domain
            var bin = new FrequencyBins(e, /*_radio.BinParametersDefault()*/ _radio.BinParametersWideSpectrum());
            var frequency = _radio.Frequency();
            var minFrequency = frequency - _sweepParameters.SweepSpan;
            var maxFrequency = frequency + _sweepParameters.SweepSpan;

            var sweeped = new List<SweepedFrequency>();
            var frequnecies = bin.Frequencies();
            foreach (var f in frequnecies)
                if (f > _lastFrequency && f >= minFrequency && f <= maxFrequency)
                {
                    _lastFrequency = f;
                    var data = SweepFrequency(bin, f);
                    sweeped.Add(data);
                }

            OnFrequencySweeped(sweeped, frequency, _sweepParameters.Precision);
        }

        private SweepedFrequency SweepFrequency(FrequencyBins bin, double f)
        {
            var current = bin.Intensity(f);
            var min = current;
            var max = current;

            if (!_current.ContainsKey(f))
                _current.Add(f, current);
            else
                _current[f] = current;

            if (!_min.ContainsKey(f))
                _min.Add(f, current);
            else
            {
                min = Math.Min(_min[f], current);
                _min[f] = min;
            }

            if (!_max.ContainsKey(f))
                _max.Add(f, current);
            else
            {
                max = Math.Max(_max[f], current);
                _max[f] = max;
            }

            var data = new SweepedFrequency(f, min, max, current);
            return data;
        }

        private void AutoConfigure()
        {
            if (!_sweepParameters.AutoConfigure)
                return;
            _radio.Agc(Agc.Off).IfGain(_sweepParameters.IfGain).Demodulator().DisableSofwareAgc();
            Thread.Sleep(100);
        }

        private void BackupOldParameters()
        {
            _oldFreqeuncy = _radio.Frequency();
            _oldAgc = _radio.Agc();
            _oldIfGain = _radio.IfGain();
            //_oldSagc = _radio.Demodulator().SoftwareAgc();

        }

        private int FftLength()
        {
            var samplingRate = _radio.Demodulator().IfSamplingRate();

            var bins = samplingRate/_sweepParameters.Precision;
            var fftPower = Math.Log(bins, 2);
            var round = Math.Round(fftPower, 0);
            return (int) Math.Pow(2, round);
        }

        /// <summary>
        /// determines number of sweepe samples.
        /// </summary>
        /// <returns>Returns sweepe sample count.</returns>
        public int SampleCount()
        {
            var fft = FftLength();
            var usableFft = fft/2;
            var sampling = _radio.Demodulator().IfSamplingRate();
            var usableSampling = sampling/2;
            var band = usableSampling/2;

            var start = _sweepParameters.From + _sweepParameters.SweepSpan;
            var end = _sweepParameters.To - _sweepParameters.SweepSpan;

            var steps = end - start;
            steps /= _sweepParameters.SweepSpan;

            var unitPoint = usableFft/(double)band;
            var sweepPointsperStep = (int)Math.Round(unitPoint*_sweepParameters.SweepSpan);

            var middlePoints = Math.Ceiling(sweepPointsperStep / 2.0);
            var points = (steps-1)*middlePoints + sweepPointsperStep;
            return (int)points;
        }
    }
}
