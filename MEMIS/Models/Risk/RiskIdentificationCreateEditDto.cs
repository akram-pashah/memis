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
    [Display(Name = "Identified Date")]
    public DateTime IdentifiedDate { get; set; }
    [Required]
    [Display(Name = "Strategic Objective")]
    public int? StrategicObjective { get; set; }
    [Required]
    [Display(Name = "FocusArea")]
    public int? FocusArea { get; set; }
    [Display(Name = "Activity")]
    public int Activity { get; set; }
    [Display(Name = "Risk Category")]
    public int intCategory { get; set; }
    [Required]
    [Display(Name = "Risk Title")]
    [MaxLength(300,ErrorMessage ="Risk Title should not exceed 300 characters")]
    public string RiskTitle { get; set; }
    [Required]
    public string RiskDescription { get; set; }
    //[Required]
    //public List<Event> Events { get; set; }
    //[Required]
    //public List<RiskSource> RiskSource { get; set; }
    [Required]
    public List<RiskCause> RiskCause { get; set; }
    [Required]
    public List<RiskConsequenceDetails> RiskConsequence { get; set; }
    [Required]
    public List<RiskExistMitigation> RiskExistMitigation { get; set; }
    [Required]
    public List<RiskAdditionalMitigation> RiskAdditionalMitigation { get; set; }
    [Required]
    public List<RiskWeakness> RiskWeakness { get; set; }
    [Required]
    public List<RiskOpportunity> RiskOpportunity { get; set; }

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
    public Guid? intDept { get; set; }
    public string? Supporting_Owners { get; set; }

    public bool IsVerified { get; set; }
    public int ApprStatus { get; set; } = 0;
  }
}
