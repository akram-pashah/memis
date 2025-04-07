using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data.Risk
{
  [Table("RiskEvaluation")]
  public class RiskEvaluation
  {
    [Key]
    public long Id { get; set; }
    public int? RiskRefID { get; set; }
    [ForeignKey("RiskRefID")]
    public virtual RiskRegister? RiskRegister { get; set; }

    public int Year { get; set; } = DateTime.Now.Year;
    public int Quarter { get; set; } = DateTime.Now.Month / 4 + 1;

    [Required]
    [Display(Name = "Risk Residual Consequence")]
    public double ResidualRiskConsequence { get; set; }
    [Required]
    [Display(Name = "Risk Residual Likelihood")]
    public double ResidualRiskLikelihood { get; set; }
    [Required]
    [Display(Name = "Residual Risk Score")]
    public double ResidualRiskScore { get; set; }
    [Required]
    [Display(Name = "Risk Residual Rank")]
    public double ResidualRiskRank { get; set; }

    [Display(Name = "Evaluation Summary")]
    public string? EvaluationSummary { get; set; }

    public DateTime? EvaluationDate { get; set; } = DateTime.UtcNow;

    public string? EvaluatedBy { get; set; } // Director/RMO

    public bool IsFinalized { get; set; } // Flag to indicate if it's part of annual report
  }
}
