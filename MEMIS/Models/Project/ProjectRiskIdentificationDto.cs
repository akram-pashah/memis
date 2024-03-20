using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Models
{
	public class ProjectRiskIdentificationDto
	{
		public int Id { get; set; }

		[Required]
		public string Risk { get; set; }

		[Display(Name = "Rank")]
		[Required]
		public int Rank { get; set; }

		[Required]
		public virtual int? ProjectInitiationId { get; set; }
	}
}
