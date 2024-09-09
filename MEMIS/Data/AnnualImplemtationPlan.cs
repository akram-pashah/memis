using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
  public class AnnualImplemtationPlan
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "Sub Program")]

    [Required(ErrorMessage = "Sub Program cannot be left blank!")]
    [MaxLength(1000)]
    public string subProgram { get; set; }

    [Required]
    [Display(Name = "Focus Area")]
    public virtual int? intFocus { get; set; }
    [ForeignKey("intFocus")]
    public virtual FocusArea? FocusAreaFk { get; set; }
    [Required]
    [Display(Name = "Strategic Objective")]
    public virtual int? intObjective { get; set; }
    [ForeignKey("intObjective")]
    public virtual StrategicObjective? StrategicObjectiveFk { get; set; }
    [Required]
    [Display(Name = "Strategic Intervention")]
    public virtual int? intIntervention { get; set; }
    [ForeignKey("intIntervention")]
    public virtual StrategicIntervention? StrategicInterventionFk { get; set; }
    [Required]
    [Display(Name = "Strategic Action")]
    public virtual int? intAction { get; set; }
    [ForeignKey("intAction")]
    public virtual StrategicAction? StrategicActionFk { get; set; }
    [Required]
    [Display(Name = "Activity")]
    public virtual int? intActivity { get; set; }
    [ForeignKey("intActivity")]
    public virtual Activity? ActivityFk { get; set; }
    [Required]
    [Display(Name = "Baseline")]
    public long? baseline { get; set; }
    [Required]
    [Display(Name = "Year")]
    public int? Year { get; set; }

    [Required]
    [Display(Name = "Annual Target")]
    public string annualTarget { get; set; }
    [Required]
    [Display(Name = "Means of Verification")]
    public string meansofVerification { get; set; }
    [Display(Name = "Output Indicator")]
    public string outputIndicator { get; set; }
    [Display(Name = "Assumptions/Risk")]
    public string? Risk { get; set; }
    [Required]
    [Display(Name = "Responsible Party")]
    public virtual Guid? intDept { get; set; }
    [ForeignKey("intDept")]
    public virtual Department? DepartmentFk { get; set; }
    public int? regStatus { get; set; } = 0;
  }
}
