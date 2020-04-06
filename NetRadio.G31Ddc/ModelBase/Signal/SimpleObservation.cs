using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.ModelBase.Signal
{
    class SimpleObservation
    {
        public int Stage { get; set; }
        private int hitCount_;
        private double frequency_;
        private TimeSpan duration_;
        private double hitTime_;
        private DateTime startTime_;
        private DateTime endTime_;
        private SimpleObservation next_;

        public double Frequency
        {
            get { return frequency_; }
            set { frequency_ = value; }
        }

        public int HitCount
        {
            get { return hitCount_; }
            set { hitCount_ = value; }
        }

        public TimeSpan Duration
        {
            get { return duration_; }
            set { duration_ = value; }
        }

        public double HitTime
        {
            get { return hitTime_; }
            set { hitTime_ = value; }
        }

        public DateTime StartTime
        {
            get { return startTime_; }
            set { startTime_ = value; }
        }

        public DateTime EndTime
        {
            get { return endTime_; }
            set { endTime_ = value; }
        }

        public SimpleObservation Next
        {
            get { return next_; }
            set { next_ = value; }
        }
    }
}
