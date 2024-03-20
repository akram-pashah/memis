using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Models
{
	public class ProjectOthersTabDto
	{
		public int Id { get; set; }

		[Required]
		public string Resourses { get; set; }

		[Display(Name = "Attachment")]
		public string Attachment { get; set; }

		[Required]
		public virtual int? ProjectInitiationId { get; set; }
	}
}
