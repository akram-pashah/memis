using MEMIS.Data.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data.Risk
{
  public class QuarterlyRiskAction
  {
    [Key]
    public long Id { get; set; }
    public int Quarter { get; set; }
    [ForeignKey("RiskTreatmentPlan")]
    public int? TreatmentPlanId { get; set; }
    public RiskTreatmentPlan? RiskTreatmentPlan { get; set; }
    [ForeignKey("ImplementationStatus")]
    [Display(Name = "Implementation Status")]
    public int ImpStatusId { get; set; } = 1;
    public virtual ImplementationStatus? ImplementationStatus { get; set; }
    [Display(Name = "No. of Incidents")]
    public double? NoOfIncedents { get; set; }
    [Display(Name = "Risk Description")]
    public string? RiskDescription { get; set; }
    [Display(Name = "Incident Value/Financial Loss")]
    public double? IncidentValue { get; set; }
    public List<Incident> Incidents { get; set; }

  }
}
