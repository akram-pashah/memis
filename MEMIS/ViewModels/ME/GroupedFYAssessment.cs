using MEMIS.Data;

namespace MEMIS.ViewModels.ME
{
  public class FinancialYearData
  {
    public int FinancialYear { get; set; }
    public bool IsCurrentYear { get; set; }
    public List<GroupedFYAssessment> GroupedFYAssessment { get; set; }
  }
  public class GroupedFYAssessment
  {
    public string FocusArea { get; set; }
    public int ImplementationStatusId { get; set; }
    public string ImplementationStatusName { get; set; }
    public List<ActivityAssessment> Assessments { get; set; }
  }
}
