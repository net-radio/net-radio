using NetRadio.Devices.G3XDdc;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetRadio.G31Ddc.Model
{
    [Table("Skip")]
    public class Skip
    {
        public int ID { get; set; }

        public DateTime CreateDate { get; set; }
        public long Frequency { get; set; }
        public long Bandwidth { get; set; }
        public bool SquelchState { get; set; }
        public DemodulatorMode Mode { get; set; }
        public double SquelchLevel { get; set; }
        public double SquelchNoise { get; set; }
        public double SquelchVoice { get; set; }
        public bool FilterState { get; set; }
        public long FilterLow { get; set; }
        public long FilterHigh { get; set; }
        public long FilterDeEm { get; set; }
        public string Callsign { get; set; }
        public string Description { get; set; }
        public int SectionID { get; set; }
        public virtual Section Parent { get; set; }
    }
}
