using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MEMIS.Data
{
  public class ActivityAssessRegion
  {
    [Key]
    public int intRegionAssess { get; set; }

    [Display(Name = "Output Indicator")]
    public virtual int? intAssess { get; set; }
    [ForeignKey("intAssess")]
    public virtual ActivityAssess? ActivityAssessFk { get; set; }
    [Display(Name = "Region")]
    public virtual Guid? intRegion { get; set; }
    [ForeignKey("intRegion")]
    public virtual Region? Region { get; set; }
    public double? budgetAmount { get; set; }
    [Display(Name = "Quarter")]
    public int? Quarter { get; set; }
    [Display(Name = "Quarterly Target")]
    public double? QTarget { get; set; }
    [Display(Name = "Quarterly Budget")]
    public double? QBudget { get; set; }
    [Display(Name = "Appral Status")]
    public int? ApprStatus { get; set; } = 0;
  }
}
