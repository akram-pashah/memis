using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data.Risk
{
    [Table("RiskMatrix")]
    public class RiskMatrix
    {
        [Key]
        public int Id { get; set; }
        public int RiskConsequenceId { get; set; }
        public int RiskLikelihoodId { get; set; }
        public string? RiskValue { get; set; }
        public int RiskRank { get; set; }
        public string? EvaluationCriteria { get; set; }
    }

    public enum RiskConsequence
    {
        [Display(Name = "Insignificant (1)")]
        Insignificant = 1,
        [Display(Name = "Minor (2)")]
        Minor = 2,
        [Display(Name = "Moderate (3)")]
        Moderate = 3,
        [Display(Name = "Major (4)")]
        Major = 4,
        [Display(Name = "Catastrophic (5)")]
        Catastrophic = 5
    }

    public enum RiskLikelihood
    {
        [Display(Name = "Rare (1)")]
        Rare = 1,
        [Display(Name = "Unlikely (2)")]
        Unlikely = 2,
        [Display(Name = "Possible (3)")]
        Possible = 3,
        [Display(Name = "Likely (4)")]
        Likely = 4,
        [Display(Name = "AlmostCertain (5)")]
        AlmostCertain = 5
    }

    
}
