using System.ComponentModel.DataAnnotations;

namespace MEMIS.Models
{
    public class StrategicInterventionDto
    {
        [Key]
        public int intIntervention { get; set; }
        [Required]
        [Display(Name = "Code")]
        [MaxLength(10)]
        public string InterventionCode { get; set; }
        [Required]
        [Display(Name = "Strategic Intervention")]
        [MaxLength(1000)]
        public string InterventionName { get; set; }
        public  int? intObjective { get; set; }
    }
}
