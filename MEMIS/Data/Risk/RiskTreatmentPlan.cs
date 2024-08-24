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
    public string? DataCollectionInstrumentMethods { get; set; }
    public string? MeansOfVerification { get; set; }
    public string? ResponsiblePersons { get; set; }
    public virtual ICollection<QuarterlyRiskAction>? QuarterlyRiskActions { get; set; }
  }
}
