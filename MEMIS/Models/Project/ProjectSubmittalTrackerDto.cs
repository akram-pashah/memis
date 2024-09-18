using MEMIS.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MEMIS.Models.Project
{
  public class ProjectSubmittalTrackerDto
  {
    public int Id { get; set; }
    [Display(Name = "Submittal Description")]
    [Required]
    public string SubmittalDescription { get; set; }
    [Display(Name = "Task Owner")]
    [Required]
    public string TaskOwner { get; set; }
    [Display(Name = "Task Description")]
    [Required]
    public string TaskDescription { get; set; }
    [Display(Name = "Date Action Taken")]
    [Required]
    public DateOnly? ActionDate { get; set; } = null;
    [Display(Name = "Date Submittal Paid/Communicated (if applicable)")]
    public DateOnly? PaidDate { get; set; } = null;
    [Display(Name = "Expected Days for Submittal to be Paid/Communicated")]

    public int? ExpectedDays { get; set; } = 0;
    [Display(Name = "Variance (days)")]

    public int? VarianceDays { get; set; } = 0;
    [Display(Name = "Status")]
    [Required]
    public string Status { get; set; }
    [Display(Name = "Amount Expected")]

    public long? ExpectedAmount { get; set; } = 0;
    [Display(Name = "Amount Paid")]

    public long? AmountPaid { get; set; } = 0;
    public virtual int? ProjectInitiationId { get; set; } 
  }
}
