
using MEMIS.Models;
using System.ComponentModel.DataAnnotations;

namespace MEMIS.Data
{
  public class Region
  {
    [Key]
    public Guid intRegion { get; set; }
    [Display(Name = "Region Code")]
    [Required(ErrorMessage = "Region Code cannot be left blank!")]
    public string regionCode { get; set; }
    [Display(Name = "Region Name")]
    [Required(ErrorMessage = "Region Name cannot be left blank!")]
    public string regionName { get; set; }
    public string intHead { get; set; }
    public ApplicationUser Head { get; set; }
    public string regCoordinator { get; set; }
    public ApplicationUser Coordinator { get; set; }
  }
}
