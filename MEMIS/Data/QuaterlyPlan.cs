using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
  public class QuaterlyPlan
  {
    [Key]
    public long Id { get; set; }
    public string? Quarter {  get; set; }
    public long? QTarget { get; set; }
    public long? QBudget { get; set; }

    public long? QActual { get; set; }

    public long? QAmtSpent { get; set; }

    public string? QJustification { get; set; }

    [ForeignKey("ActivityAssess")]
    public int? ActivityAccessId { get; set; }
    public ActivityAssess? ActivityAssess { get; set; }
    [ForeignKey("QpDeptPlanID")]
    public int? DeptPlanId { get; set; }
    public DeptPlan? DeptPlan { get; set; }

  }
}
