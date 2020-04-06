using NetRadio.G31Ddc.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.Model
{
    [Table("Section")]
    public partial class Section
    {
        public Section()
        {
            Children = new HashSet<Section>();
        }

        public Section(UserMode mode)
        {
            Mode = mode;
            Children = new HashSet<Section>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public UserMode Mode { get; set; }
        public int? ParentID { get; set; }
        public virtual Section Parent { get; set; }
        public virtual ICollection<Section> Children { get; set; }
        public virtual ICollection<Memory> MemoryCollection { get; set; }
        public virtual ICollection<Search> SearchCollection { get; set; }
        public virtual ICollection<SearchResult> SearchResultCollection { get; set; }
        public virtual ICollection<Scan> ScanCollection { get; set; }
        public virtual ICollection<ScanResult> ScanResultCollection { get; set; }
        public virtual ICollection<Skip> SkipCollection { get; set; }
    }
}
