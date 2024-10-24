using MEMIS.Data.Risk;
using System.ComponentModel.DataAnnotations;

namespace MEMIS.Models.Risk
{
  public class RiskResidualDto
  {

    public int RiskRefID { get; set; }
    [Required]
    public DateTime IdentifiedDate { get; set; }
    [Required]
    public int? StrategicObjective { get; set; }
    [Required]
    public int? FocusArea { get; set; }
    public int Activity { get; set; }
    [Required]
    public string RiskDescription { get; set; }
    [Required]
    public string Events { get; set; }
    [Required]
    public string RiskSource { get; set; }
    [Required]
    public string RiskCause { get; set; }
    [Required]
    public string RiskConsequence { get; set; }

    [Required]
    [Range(1, 5, ErrorMessage = "Please select correct value")]
    public int RiskConsequenceId { get; set; }
    [Required]
    [Range(1, 5, ErrorMessage = "Please select correct value")]
    public int RiskLikelihoodId { get; set; }
    [Required]
    public int RiskScore { get; set; }
    [Required]
    public string? RiskRank { get; set; }
    [Required]
    public string? EvalCriteria { get; set; }
    public virtual int? RiskId { get; set; }
    [Display(Name = "Additional Mitigation Measure /Activities")]
    public string? AdditionalMitigation { get; set; }
    [Display(Name = "Resources Required for effective risk management")]
    public string? ResourcesRequired { get; set; }
    [Display(Name = "By when (expected date/period of implementation)")]
    public DateTime? ExpectedDate { get; set; }
    public int? ApprStatus { get; set; } = 0;
    [Display(Name = "Risk Tolerence/Apetite")]
    public int? riskTolerence { get; set; }
    [Display(Name = "Justification")]
    public string? riskTolerenceJustification { get; set; }
    [Display(Name = "Action Undertaken toMitigate Risk")]
    public string? ActionTaken { get; set; }
    [Display(Name = "Control Effectiveness")]
    public string? ControlEffectiveness { get; set; }
    public int? Effectiveness { get; set; }
    public string? Recommendation { get; set; }
    [Display(Name = "Date (Actual Date/Period of Implementation)")]
    public DateTime? ActualDate { get; set; }
    public string? ActualBy { get; set; }
    [Required]
    [Range(1, 5, ErrorMessage = "Please select correct value")]
    public int? RiskResidualConsequenceId { get; set; }
    [Required]
    [Range(1, 5, ErrorMessage = "Please select correct value")]
    public int? RiskResidualLikelihoodId { get; set; }
    [Required]
    public int? RiskResidualScore { get; set; }
    [Required]
    public string? RiskResidualRank { get; set; }
    public double? IncidentImpact { get; set; }
    public double? FinancialImpact { get; set; }
    public double? OperationGovernanceImpact { get; set; }
    public double? ActivityBudget { get; set; }
    public int RiskRatingId { get; set; }
    public string? RiskRatingCategory { get; set; }
    public string? RiskRatingColor { get; set; }
    public virtual ICollection<RiskTreatmentPlan> RiskTreatmentPlans { get; set; } = [];
  }
}
