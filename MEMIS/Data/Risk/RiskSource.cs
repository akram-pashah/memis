using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MEMIS.Data.Risk
{
  public class RiskSource
  {
    [Key]
    public int RiskSourceId { get; set; }
    public string Description { get; set; }

    [ForeignKey("RiskIdentification")]
    public int RiskId { get; set; }
    public RiskIdentification RiskIdentification { get; set; }
  }
}
