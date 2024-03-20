using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
    [Table("ProjectPayment")]
    public class ProjectPayment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Amount")]
		[Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
		public decimal Amount { get; set; }
       
        [Display(Name = "Due Date")]
        [Required]
        public DateTime DueDate { get; set; }

        [Display(Name = "Activity")]
        [Required]
        public String Activity { get; set; }

        [Required]
        public virtual int? ProjectInitiationId { get; set; }
        [ForeignKey("ProjectInitiationId")]
        public virtual ProjectInitiation? ProjectInitiationFk { get; set; }
    }
}
