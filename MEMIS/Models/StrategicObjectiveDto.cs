using System.ComponentModel.DataAnnotations;

namespace MEMIS.Models
{
  public class StrategicObjectiveDto
  {
    [Key]
    public int intObjective { get; set; }
    [Required]
    [Display(Name = "Code")]
    [MaxLength(10)]
    public string ObjectiveCode { get; set; }
    [Required]
    [Display(Name = "Strategic Objective")]
    [MaxLength(1000)]
    public string ObjectiveName { get; set; }
    [Display(Name = "Focus Area")]
    public virtual int intFocus { get; set; }
  }
}
