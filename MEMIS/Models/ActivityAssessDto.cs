using MEMIS.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MEMIS.Models
{
  public class ActivityAssessDto
  {
    [Key]
    public int intAssess { get; set; }

    [Display(Name = "Strategic Intervention")]
    public virtual int? intIntervention { get; set; }
    [ForeignKey("intIntervention")]
    public virtual StrategicIntervention? StrategicIntervention { get; set; }
    [Display(Name = "Strategic Action")]
    public virtual int? intAction { get; set; }
    [ForeignKey("intAction")]
    public virtual StrategicAction? StrategicAction { get; set; }
    [Display(Name = "Activity")]
    public virtual int? intActivity { get; set; }
    [ForeignKey("intActivity")]
    public virtual Activity? ActivityFk { get; set; }
    [Display(Name = "Output Indicators")]
    public string outputIndicator { get; set; }
    [Display(Name = "Baseline")]
    public double? baseline { get; set; }
    [Display(Name = "Budget Code")]
    public double? budgetCode { get; set; }
    [Display(Name = "Comparative Target")]
    public double? comparativeTarget { get; set; }
    [Display(Name = "Justification")]
    public string? justification { get; set; }
    [Display(Name = "Budget Amount")]
    public double? budgetAmount { get; set; }
    [Display(Name = "Quarter")]
    public int? Quarter { get; set; }
    [Display(Name = "Quarterly Target")]
    public double? QTarget { get; set; }
    [Display(Name = "Quarterly Budget")]
    public double? QBudget { get; set; }
    [Display(Name = "Identified Risks")]
    public string? IdentifiedRisks { get; set; }
    [Display(Name = "Appral Status")]
    public int? ApprStatus { get; set; } = 0;
    [Display(Name = "Department")]
    public Guid? intDept { get; set; }
    public List<QuaterlyPlan> QuaterlyPlans { get; set; } 
  }
}
