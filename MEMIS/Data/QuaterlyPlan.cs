using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
  public class QuaterlyPlan
  {
    [Key]
    public long Id { get; set; }
    public string? Quarter { get; set; }
    public double? QTarget { get; set; }
    public double? QBudget { get; set; }
    public double? QActual { get; set; }
    public double? QAmtSpent { get; set; }
    public string? QJustification { get; set; }

    [ForeignKey("ActivityAssess")]
    public int? ActivityAccessId { get; set; }
    public ActivityAssess? ActivityAssess { get; set; }

    [ForeignKey("DeptPlan")]
    public int? DeptPlanId { get; set; }
    public DeptPlan? DeptPlan { get; set; }

    [ForeignKey("ActivityAssessment")]
    public int? ActivityAssessmentId { get; set; }
    public ActivityAssessment? ActivityAssessment { get; set; }
  }
}