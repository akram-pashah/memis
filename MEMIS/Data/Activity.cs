using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
    public class Activity
    {
        [Key]
        public int intActivity {  get; set; }
        [Required]
        [MaxLength(10)]
        [Display(Name ="Code")]
        public string activityCode { get; set; }
        [Required]
        [MaxLength(1000)]
        [Display(Name ="Activity")]
        public string activityName { get; set; }
        public virtual int? intAction { get; set; }
        [ForeignKey("intAction")]
        public virtual StrategicAction? StrategicAction { get; set; }

    }
}
