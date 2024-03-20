using MEMIS.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MEMIS.Models.Project
{
	public class StakeholderDto
	{
		[Key]
		public int Id { get; set; }
		[Display(Name = "Stakeholder Name")]
		[Required]
		public string StakeholderName { get; set; }
		[Display(Name = "Contact Person – Name")]
		[Required]
		public string ContactPersonName { get; set; }
		[Display(Name = "Contact Person – Email")]
		[Required]
		public string ContactPersonEmail { get; set; }
		[Display(Name = "Contact Person – Phone")]
		[Required]
		public string ContactPersonPhone { get; set; }
		[Display(Name = "Contact Person – Address")]
		[Required]
		public string ContactPersonAddress { get; set; }
		[Display(Name = "Contact Person – Website")]
		[Required]
		public string ContactPersonWebsite { get; set; }
		[Display(Name = "Impact")]
		[Required]
		public int Impact { get; set; }
		[Display(Name = "Influence")]
		[Required]
		public int Influence { get; set; }
		[Display(Name = "What is important to the stakeholder?")]
		[Required]
		public string StakeHolderImportant { get; set; }
		[Display(Name = "How could the stakeholder contribute to the project?")]
		[Required]
		public string StakeholderContribution { get; set; }
		[Display(Name = "How could the stakeholder block the project?")]
		[Required]
		public string Stakeholderblock { get; set; }
		[Display(Name = "Strategy for engaging the stakeholder")]
		[Required]
		public string StakeholderStrategy { get; set; }
		[Required]
		public virtual int? ProjectInitiationId { get; set; } 

	}
}
