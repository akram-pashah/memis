using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace MEMIS.Models
{
    public class RegionDto
    {
        [Key]
        public Guid intRegion { get; set; }
        [Display(Name = "Region Code")]
        [Required(ErrorMessage = "Region Code cannot be left blank!")]
        public string regionCode { get; set; }
        [Display(Name = "Region Name")]
        [Required(ErrorMessage = "Region Name cannot be left blank!")]
        public string regionName { get; set; }
        [Display(Name = "Head of Region")]
        [Required(ErrorMessage = "Select Head of Region before proceeding!")]
        public string intHead { get; set; }
        [Display(Name = "Regional Coordinator")]
        [Required(ErrorMessage = "Select Regional Coordinator before proceeding!")]
        public string regCoordinator { get; set; }
    }
}
