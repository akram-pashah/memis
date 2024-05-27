using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MEMIS.Data.Risk
{
  public class RiskDetail
  {
    [Key]
    public int RiskDetailId { get; set; }

    [ForeignKey("RiskIdentification")]
    public int RiskId { get; set; }
    public RiskIdentification RiskIdentification { get; set; }
    public string Event { get; set; }
    public string RiskSource { get; set; }
    public string RiskCause { get; set; }
    public string RiskConsequence { get; set; }
  }
}
