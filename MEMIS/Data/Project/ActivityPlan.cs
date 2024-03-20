using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
    [Table("ActivityPlan")]
    public class ActivityPlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Activity { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Responsible Person")]
        [Required]
        public string? Person { get; set; }

        [Display(Name = "Cost")]
        [Required]
        public decimal Cost { get; set; }

        [Required]
        public virtual int? ProjectInitiationId { get; set; }
        [ForeignKey("ProjectInitiationId")]
        public virtual ProjectInitiation? ProjectInitiationFk { get; set; }
    }
}
