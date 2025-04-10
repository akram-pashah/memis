using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
  public class Outcome
  {
    [Key]
    public int intOutcome { get; set; }
    [Required]
    [Display(Name = "Code")]
    [MaxLength(10)]
    public string OutcomeCode  { get; set; }
    [Required]
    [Display(Name = "Outcome")]
    [MaxLength(1000)]
    public string OutcomeName { get; set; }
    [Display(Name = "Strategic Objective")]
    public virtual int? intObjective { get; set; }
    [ForeignKey("intObjective")]
    public virtual StrategicObjective? StrategicObjective { get; set; }

  }
}
