using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.ModelBase.Signal
{
    class ConsecutiveObservation
    {
        public ConsecutiveObservation()
        {
            ObservationDetailSeries = new List<SimpleObservation>();
        }

        public double Frequency { get; set; }
        public int Stage { get; set; }

        public ICollection<SimpleObservation> ObservationDetailSeries { get; set; }

        public int HitCount { get; set; }

        public double HitTime { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
