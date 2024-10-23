using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MEMIS.Data.Risk
{
  public class Incident
  {
    [Key]
    public int IncidentId { get; set; }
    [Display(Name = "No. of Incidents")]
    public double? NoOfIncedents { get; set; }
    public string? Description { get; set; }
    public DateTime? DateOccured { get; set; }
    public double? FinancialLoss { get; set; }

    [ForeignKey("QuarterlyRiskAction")]
    public long? QuarterlyRiskActionId { get; set; }
    public QuarterlyRiskAction? QuarterlyRiskAction { get; set; }
  }
}



