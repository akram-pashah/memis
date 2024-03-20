using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
    [Table("ProjectRiskIdentification")]
    public class ProjectRiskIdentification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Risk { get; set; }
       
        [Display(Name = "Rank")]
        [Required]
        public int Rank { get; set; }

        [Required]
        public virtual int? ProjectInitiationId { get; set; }
        [ForeignKey("ProjectInitiationId")]
        public virtual ProjectInitiation? ProjectInitiationFk { get; set; }
    }
}
