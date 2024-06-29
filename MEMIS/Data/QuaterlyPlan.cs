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

    [ForeignKey("ActivityAssess")]
    public int ActivityAccessId { get; set; }
    public ActivityAssess? ActivityAssess { get; set; }

  }
}
