using System.ComponentModel.DataAnnotations;

namespace MEMIS.Models
{
    public class NDP_HDDto
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Name")]
        public string ndpname { get; set; }
        [Display(Name = "Status")]
        public int? active { get; set; } = 0;
        [Display(Name = "From Year")]
        public int fromyear { get; set; }
        [Display(Name = "To Year")]
        public int toyear { get; set; }
    }
}
