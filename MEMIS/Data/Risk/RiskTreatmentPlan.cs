using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MEMIS.Data.Risk
{
  [Table("RiskTreatmentPlan")]
  public class RiskTreatmentPlan
  {
    [Key]
    public int TreatmentPlanId { get; set; }

    [ForeignKey("RiskRegister")]
    public int? RiskRefID { get; set; }
    public virtual RiskRegister? RiskRegister { get; set; }

    [StringLength(500)]
    public string TreatmentAction { get; set; }

    [StringLength(500)]
    public string IndicatorDescription { get; set; }

    public long Baseline { get; set; }
    public long CumulativeTarget { get; set; }

    [Required]
    public string FrequencyOfReporting { get; set; }  // Monthly, Quarterly, Annually
  }
}
