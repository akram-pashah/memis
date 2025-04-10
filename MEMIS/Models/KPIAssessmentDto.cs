using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MEMIS.Data;
using Syncfusion.Blazor.Navigations;
namespace MEMIS.Models
{
  public class KPIAssessmentDto:IValidatableObject
  {
    [Key]
    public int Id { get; set; }
    public virtual int? KPIMasterId { get; set; }
    [ForeignKey("Id")]
    public virtual KPIMaster? KPIMasterFk { get; set; }

    public DateTime AssessmentDate { get; set; }

    [Display(Name = "Performance indicator")]
    public string? PerformanceIndicator { get; set; }
    [Display(Name = "Frequency of Reporting")]
    public int FrequencyofReporting { get; set; }
    [Display(Name = "Numerator (Description)")]
    public string? IndicatorFormulae { get; set; }
    [Display(Name = "Denominator (Description)")]
    public string? IndicatorDefinition { get; set; }
    [Display(Name = "Financial Year")]
    [Required]
    public int FY { get; set; }
    [Display(Name = "Target")]
    public double? Target { get; set; }
    [Display(Name = "Numerator")]
    [Required]
    public double? Numerator { get; set; }
    [Display(Name = "Denominator")]
    [Required]
    public double? Denominator { get; set; }
    [Display(Name = "Performance")]
    public double? Rate { get; set; }
    [Display(Name = "Rating")]
    public string? Achieved { get; set; }
    [Display(Name = "Approval Status")]
    public int? ApprovalStatus { get; set; } = 0;
    [Display(Name = "Justification")]
    public string? Justification { get; set; }
    [Display(Name = "Responsible Party")]
    public virtual Guid? intDept { get; set; }
    public bool ShouldValidateJustification()
    {
      return (Rate > Target);
    } 

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (ShouldValidateJustification() && string.IsNullOrWhiteSpace(Justification))
      {
        yield return new ValidationResult("Justification is required when Performance is greater than Target.", new[] { nameof(Justification) });
      }
      
    }
  }
}
