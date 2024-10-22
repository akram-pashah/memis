using MEMIS.Data;

namespace MEMIS.Models.Report
{
  public class ConsolidatedWorkPlanReport
  {
    public string? StrategicIntervention { get; set; }
    public List<CWP_StrategicAction> StrategicActions = new List<CWP_StrategicAction>();
  }
  public class CWP_StrategicAction
  {
    public string? StrategicAction { get; set; }
    public List<ActivityAssess> ActivityAssesses = new List<ActivityAssess>();
  }
}
