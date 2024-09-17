using MEMIS.ViewModels.ME;

namespace MEMIS.ViewModels.RiskManagement
{
  public class RiskDashboardViewModel
  {
    public int TotalRiskInRiskRegister { get; set; }
    public int TotalActions { get; set; }
    public int TotalActionsImplemented { get; set; }
    public int TotalActionsNotImplemented { get; set; }
    public int TotalRisksReduced { get; set; }
    public int TotalRisksIncreased { get; set; }
    public List<double> ImplementedCounts { get; set; } = [];
    public List<double> CorporateImplementedCounts { get; set; } = [];
    public List<double> CategoryRisks { get; set; } = [];
    public List<double> CurrentYearCategoryRisks { get; set; } = [];
    public List<string> Categories = [];
    public List<ChartDataSeries> CategoryWiseRiskMovementTrend = new();
    public List<string> Colors = new();

    public double DepartmentPerformance { get; set; }
    public List<string> StrategicInterventions { get; set; } = [];
    public List<ChartDataSeries> YearlyStrategicInterventionTrend = new();
    public List<string> FocusAreas { get; set; } = [];
    public List<double> FocusAreasPercentages { get; set; } = [];
    public List<string> Years = GetYearsSince2016();
    public List<ChartDataSeries> YearlyStrategicPlanTrend = new();
    public List<ChartDataSeries> RisksFocusAreaTrend = new();

    private static List<string> GetYearsSince2016()
    {
      List<string> allYears = [];
      for (int year = 2016; year <= DateTime.Now.Year + 1; year++)
      {
        allYears.Add(year.ToString());
      }

      return allYears;
    }
  }
}
