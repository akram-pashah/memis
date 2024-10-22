namespace MEMIS.ViewModels.Project
{
  public class PMDashboardViewModel
  {
    public double TotalProjects { get; set; }
    public double TotalProjectsCompleted { get; set; }
    public double TotalProjectsPending { get; set; }
    public double TotalProjectsOverdue { get; set; }
    public double TotalProjectsInProgress { get; set; }
    public List<double> ProjectsByType { get; set; } = [];
    public List<double> RisksByRank { get; set; } = [];
    public List<double> ActivitiesByStatus { get; set; } = [];
    public List<double> TasksByStatus { get; set; } = [];
  }
}
