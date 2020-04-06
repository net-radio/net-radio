using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.Model
{
    [Table("ScanResult")]
    public class ScanResult
    {
        public int ID { get; set; }
        
        public long Frequency { get; set; }

        public int SectionID { get; set; }
        
        public virtual Section Parent { get; set; }

        public int HitCount { get; set; }

        public double HitDuration { get; set; }

        public int Stage { get; set; }

        public DateTime HitStartTime { get; set; }

        public DateTime HitEndTime { get; set; }
    }
}
