using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
  public class StrategicIntervention
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
    [Display(Name = "Strategic Objective")]
    public virtual int? intObjective { get; set; }
    [ForeignKey("intObjective")]
    public virtual StrategicObjective? StrategicObjective { get; set; }
    public virtual ICollection<StrategicAction> StrategicActions { get; set; }

  }
}
