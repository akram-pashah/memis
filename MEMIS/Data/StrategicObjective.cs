using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
    public class StrategicObjective
    {
        [Key]
        public int intObjective {  get; set; }
        [Required]
        [Display(Name ="Code")]
        [MaxLength(10)]
        public string ObjectiveCode { get; set; }
        [Required]
        [Display(Name ="Strategic Objective")]
        [MaxLength (1000)]
        public string ObjectiveName { get; set;}
        [Display(Name = "Focus Area")]
        public virtual int? intFocus { get; set; }
        [ForeignKey("intFocus")]
        public virtual FocusArea? FocusArea { get; set; }
    }
}
