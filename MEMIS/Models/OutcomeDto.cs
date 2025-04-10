using System.ComponentModel.DataAnnotations;

namespace MEMIS.Models
{
  public class OutcomeDto
  {
    [Key]
    public int intOutcome { get; set; }
    [Required]
    [Display(Name = "Code")]
    [MaxLength(10)]
    public string OutcomeCode { get; set; }
    [Required]
    [Display(Name = "Outcome")]
    [MaxLength(1000)]
    public string OutcomeName { get; set; }
    [Display(Name = "Strategic Objective")]
    public int? intObjective { get; set; }
  }
}
