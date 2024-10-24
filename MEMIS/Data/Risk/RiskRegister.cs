using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data.Risk
{
  [Table("RiskRegister")]
  public class RiskRegister
  {
    [Key]
    public int RiskRefID { get; set; }
    public string? RiskCode { get; set; }
    public DateTime IdentifiedDate { get; set; }
    public virtual int? StrategicObjective { get; set; }

    [ForeignKey("StrategicObjective")]
    public virtual StrategicObjective? StrategicPlanFk { get; set; }
    public virtual int? FocusArea { get; set; }
    [ForeignKey("FocusArea")]
    public virtual FocusArea? FocusAreaFk { get; set; }
    public virtual int Activity { get; set; }
    [ForeignKey("Activity")]
    public virtual Activity? ActivityFk { get; set; }
    public string RiskDescription { get; set; }
    public virtual int intCategory { get; set; }
    [ForeignKey("intCategory")]
    public virtual RiskCategory RiskCategoryFk { get; set; }
    public string? RiskOwner { get; set; }
    public int RiskConsequenceId { get; set; }
    public int RiskLikelihoodId { get; set; }
    public int RiskRatingId { get; set; }
    public string? RiskRatingCategory { get; set; }
    public string? RiskRatingColor { get; set; }
    public int RiskScore { get; set; }
    public string? RiskRank { get; set; }
    public string? EvalCriteria { get; set; }
    public string? ExistingMitigation { get; set; }
    public string? Weakness { get; set; }
    public string? Additional_Mitigation { get; set; }
    [Display(Name = "Opportunity")]
    public string? Opportunity { get; set; }
    [Display(Name = "Primary Owner")]
    public virtual Guid? intDept { get; set; }
    [ForeignKey("intDept")]
    public virtual Department? DepartmentFk { get; set; }
    public string? Supporting_Owners { get; set; }
    public virtual int? RiskId { get; set; }
    [ForeignKey("RiskId")]
    public virtual RiskIdentification? RiskIdentificationFk { get; set; }
    //[Display (Name ="Activity / Additional mitigation strategies")]
    //public string? AdditionalMitigation { get;set; }
    //[Display(Name = "Resources Required for effective risk management")]
    //public string? ResourcesRequired { get; set; }
    //[Display(Name = "By when (expected date/period of implementation)")]
    //public DateTime? ExpectedDate { get; set; }


    [Display(Name = "Review/Implementation date")]
    public DateTime? ReviewDate { get; set; }
    public int? ApprStatus { get; set; } = 0;
    public int? riskTolerence { get; set; }
    public string? riskTolerenceJustification { get; set; }
    [Display(Name = "Action Undertaken toMitigate Risk")]
    public string? ActionTaken { get; set; }
    [Display(Name = "Date (Actual Date/Period of Implementation)")]
    public DateTime? ActualDate { get; set; }
    public string? ActualBy { get; set; }
    public int? RiskResidualConsequenceId { get; set; }
    public int? RiskResidualLikelihoodId { get; set; }
    public int? RiskResidualScore { get; set; }
    public string? RiskResidualRank { get; set; }
    public long ActivityBudget { get; set; }
    public string? ControlEffectiveness { get; set; }
    public int? Effectiveness { get; set; }
    public string? Recommendation { get; set; }
    [Display(Name = "Incident Impact")]
    public double? IncidentImpact { get; set; }
    [Display(Name = "Financial Impact")]
    public double? FinancialImpact { get; set; }
    [Display(Name = "Operation / Governance Impact")]
    public double? OperationGovernanceImpact { get; set; }
    public virtual ICollection<RiskTreatmentPlan> RiskTreatmentPlans { get; set; } = [];
  }
}
