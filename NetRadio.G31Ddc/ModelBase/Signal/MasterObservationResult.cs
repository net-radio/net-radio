using NetRadio.G31Ddc.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.ModelBase.Signal
{
    class MasterObservationResult
    {
        private List<ConsecutiveObservation> consecutiveObservations_ = new List<ConsecutiveObservation>();

        public MasterObservationResult()
        {
        }

        public List<ConsecutiveObservation> ConsecutiveObservations
        {
            get { return consecutiveObservations_; }
            set { consecutiveObservations_ = value; }
        }

        internal void NewObserve(SimpleObservation simpleObservation)
        {
            double frequency = simpleObservation.Frequency;

            var lastResult = consecutiveObservations_.FindLast(x => x.Frequency == frequency);

            if (lastResult == null)
                Add(simpleObservation);
            else
            {
                SimpleObservation lastDetail = lastResult.ObservationDetailSeries.Last();
                if (simpleObservation.Stage - lastDetail.Stage > 3)
                    Add(simpleObservation);
                else
                    Update(lastResult, simpleObservation);
            }
        }

        private void Update(ConsecutiveObservation consecutiveObservation, SimpleObservation simpleObservation)
        {
            consecutiveObservation.HitCount += simpleObservation.HitCount;
            consecutiveObservation.HitTime += simpleObservation.HitTime;
            consecutiveObservation.EndTime = simpleObservation.EndTime;
            consecutiveObservation.ObservationDetailSeries.Add(simpleObservation);
        }

        private void Add(SimpleObservation simpleObservation)
        {
            ConsecutiveObservation observationResult = new ConsecutiveObservation();
            observationResult.Frequency = simpleObservation.Frequency;
            observationResult.Stage = simpleObservation.Stage;
            observationResult.StartTime = simpleObservation.StartTime;
            observationResult.EndTime = simpleObservation.EndTime;
            observationResult.HitCount = simpleObservation.HitCount;
            observationResult.HitTime = simpleObservation.HitTime;
            observationResult.ObservationDetailSeries.Add(simpleObservation);
            consecutiveObservations_.Add(observationResult);
        }
    }
}
