using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
  public class ProgramImplementationPlan
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "Strategic Objective")]
    public virtual int? intObjective { get; set; }
    [ForeignKey("intObjective")]
    public virtual StrategicObjective? StrategicObjectiveFK { get; set; }
    [Display(Name = "Strategic Intervention")]
    public virtual int? intIntervention { get; set; }
    [ForeignKey("intIntervention")]
    public virtual StrategicIntervention? StrategicInterventionFK { get; set; }
    [Display(Name = "Strategic Action")]
    public virtual int? intAction { get; set; }
    [ForeignKey("intAction")]
    public virtual StrategicAction? StrategicActionFK { get; set; }

    [Display(Name = "Activity")]
    public virtual int? intActivity { get; set; }
    [ForeignKey("intActivity")]
    public virtual Activity? ActivityFK { get; set; }
    [Display(Name = "Unit Cost")]
    public double? unitCost { get; set; }
    [Required]
    [Display(Name = "Output Indicator")]
    [MaxLength(1000)]
    public string Output { get; set; } 
    [Display(Name = "Output Target")]
    [MaxLength(1000)]
    public string? OutputTarget { get; set; } 
    [Display(Name = "Target FY 1")]
    public string? FY1 { get; set; }
    [Display(Name = "Target FY 2")]
    public string? FY2 { get; set; }
    [Display(Name = "Target FY 3")]
    public string? FY3 { get; set; }
    [Display(Name = "Target FY 4")]
    public string? FY4 { get; set; }
    [Display(Name = "Target FY 5")]
    public string? FY5 { get; set; } 
    [Display(Name = "Means of Verification")]
    [MaxLength(1000)]
    public string? MeansofVerification { get; set; }
    [Required]
    [Display(Name = "Responsible Party")]
    [MaxLength(1000)]
    public string ResponsibleParty { get; set; }



  }
}
