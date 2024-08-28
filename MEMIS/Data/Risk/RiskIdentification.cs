using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data.Risk
{
  [Table("RiskIdentification")]
  public class RiskIdentification
  {
    [Key]
    public int RiskId { get; set; }
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
    public virtual int intCategory { get; set; }
    [ForeignKey("intCategory")]
    public virtual RiskCategory RiskCategoryFk { get; set; }
    public string RiskDescription { get; set; }
    public string RiskOwner { get; set; }
    [Display(Name = "Risk Consquence ")]
    public int RiskConsequenceId { get; set; }
    public int RiskLikelihoodId { get; set; }
    public int RiskScore { get; set; }
    public string? RiskRank { get; set; }
    public string? EvalCriteria { get; set; }
    public string? ExistingMitigation { get; set; }
    public string? Weakness { get; set; }
    public string? Additional_Mitigation { get; set; }
    public bool IsVerified { get; set; }
    public int ApprStatus { get; set; } = 0;
    public ICollection<RiskDetail> RiskDetails { get; set; }
    public ICollection<Event> Events { get; set; }
    public ICollection<RiskSource> RiskSources { get; set; }
    public ICollection<RiskCause> RiskCauses { get; set; }
    public ICollection<RiskConsequenceDetails> RiskConsequenceDetails { get; set; }
    public RiskIdentification()
    {
      RiskDetails = new HashSet<RiskDetail>();
      Events = new HashSet<Event>();
      RiskSources = new HashSet<RiskSource>();
      RiskCauses = new HashSet<RiskCause>();
      RiskConsequenceDetails = new HashSet<RiskConsequenceDetails>();
    }
  }
  public class Causes
  {

  }

}
