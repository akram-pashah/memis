namespace MEMIS.ViewModels.Planning
{
  public class TotalActivitiesViewModel
  {
    public int TotalActivities { get; set; }
    public double? TotalBudget { get; set; }
    public double? TotalTarget { get; set; }
    public int PendingActivities { get; set; }
    public List<int> ActivitiesCount { get; set; }
    public List<double?> DepartmentBudgets { get; set; }
    public List<string> Departments { get; set; }
    public List<int> FocusAreaActivitiesCount { get; set; }
    public List<string> FocusAreas { get; set; }
    public List<DepartmentBudget> BudgetWithDepartment { get; set; } = new();
  }
  public class DepartmentBudget
  {
    public string Name { get; set; }
    public string Code { get; set; }
    public double? Budget { get; set; }
  }
}
