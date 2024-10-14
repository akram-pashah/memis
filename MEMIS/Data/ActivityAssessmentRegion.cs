using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
  public class ActivityAssessmentRegion
  {
    [Key]
    public int intRegionAssess { get; set; }

    [Display(Name = "Output Indicator")]
    public virtual int intAssess { get; set; }
    [ForeignKey("intAssess")]
    public virtual ActivityAssess? ActivityAssessFk { get; set; }
    public virtual int intAssessment { get; set; }
    [ForeignKey("intAssessment")]
    public virtual ActivityAssessment? ActivityAssessmentFk { get; set; }
    [Display(Name = "Region")]
    public virtual Guid? intRegion { get; set; }
    [ForeignKey("intRegion")]
    public virtual Region? Region { get; set; }
    public double? budgetAmount { get; set; }
    [Display(Name = "Quarter")]
    public int? Quarter { get; set; }
    [Display(Name = "Quarterly Target")]
    public double? QTarget { get; set; }
    [Display(Name = "Achievement")]
    public double? QAchievement{ get; set; }
    [Display(Name = "Quarterly Budget")]
    public double? QBudget { get; set; }
    [Display(Name = "Amount Spent")]
    public double? QAmtSpent { get; set; }
    [Display(Name = "Regional Justification")]
    public double? QRegJustification { get; set; }
    [Display(Name = "Unit Cost")]
    public double? unitCost { get; set; }
    [Display(Name = "Appral Status")]
    public int? ApprStatus { get; set; } = 0;
    [Display(Name = "Financial Year")]
    public int Fyear = DateTime.Now.Year;
    public virtual ICollection<QuaterlyPlan> QuaterlyPlans { get; set; }
  }
}
