using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
  [Table("ProjectOthersTab")]
  public class ProjectOthersTab
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Resourses { get; set; }

    [Display(Name = "Attachment")]
    public byte[] Attachment { get; set; }

    public string FileName { get; set; }
    [Required]
    public virtual int? ProjectInitiationId { get; set; }
    [ForeignKey("ProjectInitiationId")]
    public virtual ProjectInitiation? ProjectInitiationFk { get; set; }
  }
}
