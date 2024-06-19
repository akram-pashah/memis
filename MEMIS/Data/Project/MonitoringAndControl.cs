using Syncfusion.Blazor.Gantt.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data.Project
{
  [Table("MonitoringAndControl")]
  public class MonitoringAndControl
  {
    [Key]
    public int Id { get; set; }

    [Display(Name = "Task Name")]
    public string? TaskName { get; set; }
    public long? Duration { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set;}
    public string? ImplementationStatus { get; set; }
    public DateOnly? CompletedDate { get; set; }
    public string? Status { get; set; }
    public string? OverDue {  get; set; }
    public DateOnly? LateBy { get; set; }
    public virtual int ProjectInitiationId { get; set; }

    [ForeignKey("ProjectInitiationId")]
    public virtual ProjectInitiation? ProjectInitiationFk { get; set; }
  }
}
