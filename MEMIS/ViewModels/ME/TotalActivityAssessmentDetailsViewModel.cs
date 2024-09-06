namespace MEMIS.ViewModels.ME
{
  public class TotalActivityAssessmentDetailsViewModel
  {
    public int TotalActivitiesFullyImplemented { get; set; }
    public int TotalSDTsAchieved { get; set; }
    public int TotalKPIsAchieved { get; set; }
    public double DepartmentPerformance { get; set; }
    public List<string> StrategicInterventions { get; set; } = [];
    public List<ChartDataSeries> YearlyStrategicInterventionTrend = new();
  }
  public class ChartDataSeries
  {
    public string name { get; set; }
    public List<int> data { get; set; } = [];
  }
}
