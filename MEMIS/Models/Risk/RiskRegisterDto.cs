﻿using MEMIS.Data.Risk;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Models.Risk
{
    public class RiskRegisterDto
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
        [Display(Name = "Activity / Additional mitigation strategies")]
        public string? AdditionalMitigation { get; set; }
        [Display(Name = "Opportunity")]
        public string? Opportunity { get; set; }
        [Display(Name = "Review/Implementation date")]
        public DateTime? ReviewDate { get; set; }
        public int? ApprStatus { get; set; } = 0;
        [Display(Name = "Risk Tolerence/Apetite")]
        public int? riskTolerence { get; set; }
        [Display(Name = "Justification")]
        public string? riskTolerenceJustification { get; set; }

    }
}
