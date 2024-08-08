using MEMIS.Data;
using System.ComponentModel.DataAnnotations;

namespace MEMIS.Models
{
  public class ConsolidateActivityAssessment
  {
    public string strategicObjective { get; set; }
    [Display(Name = "Strategic Intervention")]
    public string strategicIntervention { get; set; }
    [Display(Name = "Strategic Action")]
    public string StrategicAction { get; set; }
    [Display(Name = "Activity / Initiative")]
    public string activity { get; set; }
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
    [Display(Name = "Annual Achievement")]
    public double? AnnualAchievement { get; set; }
    [Display(Name = "Total Amount Spent")]
    public double? TotAmtSpent { get; set; }
    [Display(Name = "Responsible Party")]
    public virtual Guid? intDept { get; set; }
    [Display(Name = "Region")]
    public virtual Guid? intRegion { get; set; }

    [Display(Name = "Justification")]
    public string? AnnualJustification { get; set; }
    public virtual List<QuaterlyPlan> QuaterlyPlans { get; set; }
  }
}
