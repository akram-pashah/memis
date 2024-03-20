
using MEMIS.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
    public class Region
    {
        [Key] 
        public Guid intRegion {  get; set; }
        [Display(Name ="Region Code")]
        [Required(ErrorMessage = "Region Code cannot be left blank!")]
        public string regionCode { get; set;}
        [Display(Name ="Region Name")]
        [Required(ErrorMessage ="Region Name cannot be left blank!")]
        public string regionName { get; set;} 
        public string intHead { get; set; }
        public ApplicationUser IntHead { get; set; }
        public string regCoordinator { get; set; }
        public ApplicationUser RegCoordinator { get; set; }
    }
}
