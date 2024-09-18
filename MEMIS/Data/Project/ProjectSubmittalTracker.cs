using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data.Project
{
  [Table("ProjectSubmittalTracker")]
  public class ProjectSubmittalTracker
  {
    [Key]
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
    public DateTime ActionDate { get; set; }
    [Display(Name = "Date Submittal Paid/Communicated (if applicable)")] 
    public DateTime? PaidDate { get; set; }
    [Display(Name = "Expected Days for Submittal to be Paid/Communicated")]

    public int? ExpectedDays { get; set; }
    [Display(Name = "Variance (days)")]

    public int? VarianceDays { get; set; }
    [Display(Name = "Status")]
    [Required]
    public string Status { get; set; }
    [Display(Name = "Amount Expected")]

    public long? ExpectedAmount { get; set; }
    [Display(Name = "Amount Paid")]

    public long? AmountPaid { get; set; }
    public virtual int? ProjectInitiationId { get; set; }
    [ForeignKey("ProjectInitiationId")]
    public virtual ProjectInitiation? ProjectInitiationFk { get; set; }

  }
}
