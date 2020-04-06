using NetRadio.Devices.G3XDdc;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetRadio.G31Ddc.Model
{
    [Serializable()]
    [Table("SearchResult")]
    public class SearchResult
    {
        public int ID { get; set; }
        public long Frequency { get; set; }
        public long Bandwiddth { get; set; }
        public double SignalLevel { get; set; }
        public long Resolution { get; set; }
        public int DelayTime { get; set; }
        public DemodulatorMode Mode { get; set; }
        public int Duration { get; set; }
        public int ReactivationTime { get; set; }
        public int AttackTime { get; set; }
        public string Callsign { get; set; }
        public string Description { get; set; }
        public int HitCount { get; set; }
        public double HitDuration { get; set; }
        public int SectionID { get; set; }
        public virtual Section Parent { get; set; }
        public int Stage { get; set; }

        // public DateTime StartTime { get; set; }

        // public DateTime EndTime { get; set; }

        public DateTime HitStartTime { get; set; }

        public DateTime HitEndTime { get; set; }
    }
}
