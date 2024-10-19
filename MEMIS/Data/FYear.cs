using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
    [Table("M_FYEAR")]
    public class FYear
    {
        [Key]
        public int intyear { get; set; }
        public string yearcode {  get; set; }

        public int KPITarget {  get; set; }
    }
}
