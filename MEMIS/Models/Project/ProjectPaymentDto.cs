using System.ComponentModel.DataAnnotations;

namespace MEMIS.Models
{
    public class ProjectPaymentDto
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
       
        [Display(Name = "Due Date")]
        [Required]
        public DateTime DueDate { get; set; }

        [Display(Name = "Activity")]
        [Required]
        public String Activity { get; set; }

        [Required]
        public virtual int? ProjectInitiationId { get; set; }
    }
}
