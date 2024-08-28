using System.ComponentModel.DataAnnotations;

namespace MEMIS.Data.Risk
{
  public class RiskCategory
  {
    [Key]
    public int intCategory { get; set; }
    [Required]
    [Display(Name = "Code")]
    [MaxLength(10)]
    public string CategoryCode { get; set; }
    [Required]
    [Display(Name = "Risk Category")]
    [MaxLength(200)]
    public string CategoryName { get; set; }
    public virtual ICollection<RiskIdentification> RiskIdentifications { get; set; }
  }
}
