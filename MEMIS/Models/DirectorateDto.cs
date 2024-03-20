using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MEMIS.Models
{
    public class DirectorateDto
    {

        public Guid IntDir { get; set; }
        [Required(ErrorMessage = "Directorate Code cannot be left blank!")]
        [StringLength(50)]
        [Display(Name = "Directorate Code")]
        public string DirCode { get; set; }
        [Required(ErrorMessage = "Directorate Name cannot be left blank!")]
        [StringLength(200)]
        [Display(Name = "Directorate Name")]
        public string DirName { get; set; }

        [Required(ErrorMessage = "Director cannot be left blank!")]
        public string director { get; set; } 
    }
}
