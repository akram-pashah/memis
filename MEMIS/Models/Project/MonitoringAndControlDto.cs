namespace MEMIS.Models.Project
{
  public class MonitoringAndControlDto
  {
    public int Id { get; set; }
    public string TaskName { get; set; }

    public long Duration { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string ImplementationStatus { get; set; }

    public DateOnly CompletedDate { get; set; }
    public string Status { get; set; }

    public int ProjectInitiationId { get; set; }

  }
}
