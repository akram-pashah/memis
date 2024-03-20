using System.ComponentModel.DataAnnotations;

namespace MEMIS.Data
{
    public class FocusArea
    {
        [Key]
        public int intFocus {  get; set; }
        [Required]
        [Display(Name ="Code")]
        [MaxLength(10)]
        public string FocusAreacode { get; set; }
        [Required]
        [Display(Name ="Focus Area")]
        [MaxLength(1000)]
        public string FocusAreaName {  get; set; }
    }
}
