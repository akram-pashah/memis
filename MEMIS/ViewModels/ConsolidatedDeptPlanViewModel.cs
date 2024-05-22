using MEMIS.Data;

namespace MEMIS.ViewModels
{
  public class ConsolidatedDeptPlanViewModel
  {
    public Department Department { get; set; }
    public List<ActivityAssess> AllocatedActivityAssesses { get; set; } = new List<ActivityAssess>();
    public List<ActivityAssess> ActivityAssesses { get; set; } = new List<ActivityAssess>();
    public double? BudgetCost { get; set; }
  }
}
