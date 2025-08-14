using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data.Risk
{
  public class RiskOpportunity
  {
    [Key]
    public int OpportunityId { get; set; }
    public string Description { get; set; }

    [ForeignKey("RiskIdentification")]
    public int RiskId { get; set; }
    public RiskIdentification? RiskIdentification { get; set; } 
  }
}
