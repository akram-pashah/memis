using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
  public class AnnualImplemtationPlan
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "Sub Program")]

    [MaxLength(1000)]
    public string? subProgram { get; set; }

    [Display(Name = "Focus Area")]
    public virtual int? intFocus { get; set; }
    [ForeignKey("intFocus")]
    public virtual FocusArea? FocusAreaFk { get; set; }
    [Display(Name = "Strategic Objective")]
    public virtual int? intObjective { get; set; }
    [ForeignKey("intObjective")]
    public virtual StrategicObjective? StrategicObjectiveFk { get; set; }
    [Display(Name = "Strategic Intervention")]
    public virtual int? intIntervention { get; set; }
    [ForeignKey("intIntervention")]
    public virtual StrategicIntervention? StrategicInterventionFk { get; set; }
    [Display(Name = "Strategic Action")]
    public virtual int? intAction { get; set; }
    [ForeignKey("intAction")]
    public virtual StrategicAction? StrategicActionFk { get; set; }
    [Display(Name = "Activity")]
    public virtual int? intActivity { get; set; }
    [ForeignKey("intActivity")]
    public virtual Activity? ActivityFk { get; set; }
    [Display(Name = "Unit Cost")]
    public double? unitCost { get; set; }
    [Display(Name = "Baseline")]
    public long? baseline { get; set; }
    [Display(Name = "Year")]
    public int? Year { get; set; }

    [Display(Name = "Annual Target")]
    public long? annualTarget { get; set; }
    [Display(Name = "Means of Verification")]
    public string meansofVerification { get; set; }
    [Display(Name = "Output Indicator")]
    public string? outputIndicator { get; set; }
    [Display(Name = "Assumptions/Risk")]
    public string? Risk { get; set; }
    [Display(Name = "Responsible Party")]
    public virtual Guid? intDept { get; set; }
    [ForeignKey("intDept")]
    public virtual Department? DepartmentFk { get; set; }
    public int? regStatus { get; set; } = 0;
  }
}
