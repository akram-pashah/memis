using System.ComponentModel.DataAnnotations;

namespace MEMIS.Models
{
    public class ProgramImplementationPlanDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Activity")]
        [MaxLength(1000)]
        public string Activity { get; set; }
        [Required]
        [Display(Name = "Output")]
        [MaxLength(1000)]
        public string Output { get; set; }
        [Required]
        [Display(Name = "Responsible Party")]
        [MaxLength(1000)]
        public string ResponsibleParty { get; set; }
        [Required]
        [Display(Name = "Date of Planned Action")]
        public DateTime DateofAction { get; set; }
        [Required]
        [Display(Name = "Baseline")]
        public long Baseline { get; set; }
        [Display(Name = "Target FY 1")]
        public long? FY1 { get; set; } = 0;
        [Display(Name = "Target FY 2")]
        public long? FY2 { get; set; } = 0;
        [Display(Name = "Target FY 3")]
        public long? FY3 { get; set; } = 0;
        [Display(Name = "Target FY 4")]
        public long? FY4 { get; set; } = 0;
        [Display(Name = "Target FY 5")]
        public long? FY5 { get; set; } = 0;
    }
}
