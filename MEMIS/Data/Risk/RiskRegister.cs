using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data.Risk
{
    [Table("RiskRegister")]
    public class RiskRegister
    {
        [Key]
        public int RiskRefID { get; set; }
        public DateTime IdentifiedDate { get; set; }
        public virtual int? StrategicObjective { get; set; }

        [ForeignKey("StrategicObjective")]
        public virtual StrategicObjective StrategicPlanFk { get; set; }
        public virtual int? FocusArea { get; set; }
        [ForeignKey("FocusArea")]
        public virtual FocusArea FocusAreaFk { get; set; }
        public virtual int Activity { get; set; }
        [ForeignKey("Activity")]
        public virtual Activity ActivityFk { get; set; } 
        public string RiskDescription { get; set; }
        public string Events { get; set; }
        public string RiskSource { get; set; }
        public string RiskCause { get; set; }
        public string RiskConsequence { get; set; }
        public string RiskOwner { get; set; }
        public int RiskConsequenceId { get; set; }
        public int RiskLikelihoodId { get; set; }
        public int RiskScore { get; set; }
        public string? RiskRank { get; set; }
        public string? EvalCriteria { get; set; }
        public virtual int? RiskId { get; set; }
        [ForeignKey("RiskId")]
        public virtual DeptPlan RiskIdentificationFk{ get; set; }
        [Display (Name ="Activity / Additional mitigation strategies")]
        public string? AdditionalMitigation { get;set; }
        [Display(Name = "Resources Required for effective risk management")]
        public string? ResourcesRequired { get; set; }
        [Display(Name = "By when (expected date/period of implementation)")]
        public DateTime? ExpectedDate { get; set; }
        [Display(Name = "Opportunity")] 
        public string? Opportunity { get; set; }
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
    }
}
