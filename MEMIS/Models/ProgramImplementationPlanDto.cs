using System.ComponentModel.DataAnnotations;

namespace MEMIS.Models
{
  public class ProgramImplementationPlanDto
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "Strategic Objective")]
    public virtual int? intObjective { get; set; }
    [Display(Name = "Strategic Intervention")]
    public virtual int? intIntervention { get; set; }
    [Display(Name = "Strategic Action")]
    public virtual int? intAction { get; set; }
    [Display(Name = "Activity")]
    public virtual int? intActivity { get; set; }
    [Required]
    [Display(Name = "Output Indicator")]
    [MaxLength(1000)]
    public string Output { get; set; }
    [Display(Name = "Output Target")]
    [MaxLength(1000)]
    public string? OutputTarget { get; set; }
    
   
    [Display(Name = "Target FY 1")]
    public long? FY1 { get; set; } = 0;
    [Display(Name = "Target FY 2")]
    public long? FY2 { get; set; } = 0;
    [Display(Name = "Target FY 3")]
    public long? FY3 { get; set; } = 0;
    [Display(Name = "Target FY 4")]
    public long? FY4 { get; set; } = 0;
    [Display(Name = "Target FY 5")]
    public long? FY5 { get; set; } = 0;
    [Display(Name = "Means of Verification")]
    [MaxLength(1000)]
    public string? MeansofVerification { get; set; }
    [Required]
    [Display(Name = "Responsible Party")]
    [MaxLength(1000)]
    public string ResponsibleParty { get; set; }
  }
}
