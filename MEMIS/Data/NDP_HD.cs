using System.ComponentModel.DataAnnotations;

namespace MEMIS.Data
{
    public class NDP_HD
    {
        [Key]
        public int ID { get; set; }
        [Display(Name ="Name")]
        public string ndpname {  get; set; }
        [Display(Name = "Status")]
        public int? active { get; set; } = 0;
        [Display(Name = "From Year")]
        public int fromyear { get; set; }
        [Display(Name = "To Year")]
        public int toyear { get; set; }
    }
}
