using NetRadio.Devices.G3XDdc;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetRadio.G31Ddc.Model
{
    [Table("Search")]
    public class Search : IObservable
    {
        public int ID { get; set; }
        public long StartFrequency { get; set; }
        public long StopFrequency { get; set; }
        public long Bandwiddth { get; set; }
        public double SignalLevel {get; set;}
        public long Resolution { get; set; }
        public int DelayTime { get; set; }
        public DemodulatorMode Mode {get;set;}
        public int Duration { get; set; }
        public int ReactivationTime { get; set; }
        public int AttackTime { get; set; }
        public string Callsign { get; set; }
        public string Description { get; set; }

        public int SectionID { get; set; }
        public virtual Section Parent { get; set; }
    }
}
