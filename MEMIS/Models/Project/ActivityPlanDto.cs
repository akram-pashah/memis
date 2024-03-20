using System.ComponentModel.DataAnnotations;

namespace MEMIS.Models
{

	public class ActivityPlanDto
	{
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
		[Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
		public decimal Cost { get; set; }

		[Required]
		public virtual int? ProjectInitiationId { get; set; }
	}
}
