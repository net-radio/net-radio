using NetRadio.Devices.G3XDdc;
using NetRadio.G31Ddc.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.Model
{
    [Table("Scan")]
    public class Scan : IObservable
    {
        public int ID { get; set; }
        public long Frequency { get; set; }
        public DemodulatorMode Demodulator { get; set; }
        public int Bandwidth { get; set; }
        public double SignalLevel { get; set; }
        public int DelayTime { get; set; }
        public ScanMode ScanType { get; set; }
        public string Callsign { get; set; }
        public string Description { get; set; }
        public bool IsSquelch { get; set; }
        public double SquelchLevel { get; set; }
        public double SquelchNoise { get; set; }
        public double SquelchVoice { get; set; }
        public long Bandwiddth { get; set; }
        public int Resolution { get; set; }
        public int Duration { get; set; }
        public int ReactivationTime { get; set; }
        public int AttackTime { get; set; }
        public int SectionID { get; set; }
        public virtual Section Parent { get; set; }
    }
}
