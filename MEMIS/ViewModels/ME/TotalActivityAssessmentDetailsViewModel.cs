
namespace MEMIS.ViewModels.ME
{
  public class TotalActivityAssessmentDetailsViewModel
  {
    public int TotalActivitiesFullyImplemented { get; set; }
    public int TotalSDTsAchieved { get; set; }
    public int TotalKPIsAchieved { get; set; }
    public double DepartmentPerformance { get; set; }
    public double OverallPerformance { get; set; }
    public List<string> StrategicInterventions { get; set; } = [];
    public List<ChartDataSeries> YearlyStrategicInterventionTrend = new();
    public List<string> FocusAreas { get; set; } = [];
    public List<double> FocusAreasPercentages { get; set; } = [];
    public List<string> Years = GetYearsSince2016();
    public List<ChartDataSeries> YearlyStrategicPlanTrend = new();
    public List<ChartDataSeries> YearlyFocusAreaTrend = new();

    private static List<string> GetYearsSince2016()
    {
      List<string> allYears = [];
      for(int year = 2016; year <= DateTime.Now.Year + 1; year++)
      {
        allYears.Add(year.ToString());
      }

      return allYears;
    }
  }
  public class ChartDataSeries
  {
    public string name { get; set; }
    public List<double> data { get; set; } = [];
  }
}
