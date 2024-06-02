using MEMIS.Data.Risk;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Models.Risk
{
    public class RiskIdentificationCreateEditDto
    {

        public int RiskId { get; set; }
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
        public List<Event> Events { get; set; } 
        [Required]
        public List<RiskSource> RiskSource { get; set; } 
        [Required]
        public List<RiskCause> RiskCause { get; set; }
        [Required]
        public List<RiskConsequenceDetails> RiskConsequence { get; set; } 

        [Required]
        [Range(1, 5, ErrorMessage = "Please select correct value")]
        [Display(Name = "Risk Consquence ")]
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
        public bool IsVerified { get; set; }
        public int ApprStatus { get; set; } = 0;
    }
}
