using NetRadio.Devices.G3XDdc;
using NetRadio.G31Ddc.Model;
using NetRadio.G31Ddc.Model.Entities;
using NetRadio.G31Ddc.PanelViewModel;
using NetRadio.Signal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.ModelBase.Signal
{
    class SignalObservation
    {
        public SignalObservation(RadioViewModel radioView)
        {
            this.radioView_ = radioView;
        }

        private double _frequency;
        private readonly RadioViewModel radioView_;
        private bool _wait = false;
        public bool _search = false;
        private Dictionary<double, SimpleObservation> _frequencies = new Dictionary<double, SimpleObservation>();
        // private Dictionary<double, double> _time = new Dictionary<double, double>();
        private DateTime _lastTime;
        private IObservable observable_;

        internal void Initialize(uint frequncy, bool agcState, double gain, DemodulatorMode demodulatorMode)
        {
            radioView_.Ddc1Frequency = frequncy;
            radioView_.AgcState = agcState;
            radioView_.Gain = gain;
            radioView_.ModeState = demodulatorMode;
        }

        public double Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }

        //public double RoundedFrequency
        //{
        //    get { return Math.Round(_frequency / 100, 1) * 100; }
        //}

        internal Dictionary<double, SimpleObservation> Observe(IObservable searchData, int time = 3000)
        {
            _frequencies = new Dictionary<double, SimpleObservation>();

            observable_ = searchData;

            radioView_.SearchEventHandler += SearchDataReceived;

            _lastTime = DateTime.Now;
            startTime_ = DateTime.Now;

            _search = true;
            Thread.Sleep(time);
            _search = false;

            while (this._wait) ;
            while (radioView_.Wait) ;

            endTime_ = DateTime.Now;

            radioView_.SearchEventHandler -= SearchDataReceived;

            return _frequencies;
        }

        public void SearchDataReceived(object sender, Operations.SearchEventArgs e)
        {
            if (!_search)
                return;

            _wait = true;

            FastFrequencyBins bin = (FastFrequencyBins)e.Bins;

            var startTime = _lastTime;

            var index = bin.MaxIntensityAt();
            var frequency = bin.FrequencyAt(index);
            var peak = bin.IntensityAt(index);

            //var frequency = bin.MaxIntensityAt();
            //var peak = bin.Intensity(frequency);

            if (peak < observable_.SignalLevel)
            {
                // _maxFrequency = null;
                _wait = false;
                return;
            }

            // _maxFrequency = Convert.ToUInt32(frequency);

            var fRounded = Math.Round(frequency / 1000, 1) * 1000;
            if (!RoundFrequencies)
                fRounded = frequency;

            var hitTime = DateTime.Now - _lastTime;
            var endTime = DateTime.Now;
            _lastTime = endTime;

            if (!_frequencies.ContainsKey(fRounded))
            {
                SimpleObservation result = new SimpleObservation();
                result.Stage = _stage;
                result.Frequency = fRounded;
                result.HitCount = 1;
                result.HitTime = hitTime.TotalMilliseconds;
                result.StartTime = startTime;
                result.EndTime = endTime;
                result.Duration = endTime - startTime;
                _frequencies.Add(fRounded, result);
            }
            else
            {
                SimpleObservation result = _frequencies[fRounded];
                result.Frequency = fRounded;
                result.HitCount++;
                result.HitTime += hitTime.TotalMilliseconds;
                result.EndTime = endTime;
                result.Duration = endTime - result.StartTime;
            }

            _wait = false;
        }


        private bool _roundFrequencies = true;

        public bool RoundFrequencies
        {
            get { return _roundFrequencies; }
            set { _roundFrequencies = value; }
        }

        private int _stage;
        public int Stage
        {
            get { return _stage; }
            set { _stage = value; }
        }

        private DateTime startTime_;

        public DateTime StartTime
        {
            get { return startTime_; }
            set { startTime_ = value; }
        }

        private DateTime endTime_;

        public DateTime EndTime
        {
            get { return endTime_; }
            set { endTime_ = value; }
        }

    }
}
