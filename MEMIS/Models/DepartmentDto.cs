using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MEMIS.Models
{
    public class DepartmentDto
    {
        [Key]
        public Guid intDept { get; set; }
        [Required(ErrorMessage = "Department Code cannot be left blank!")]
        [Display(Name = "Department Code")]
        public string deptCode { get; set; }
        [Required(ErrorMessage = "Department Name cannot be left blank!")]
        [Display(Name = "Department Name")]
        public string deptName { get; set; }
        [Required(ErrorMessage = "Select Directorate before proceeding!")]
        [Display(Name = "Directorate")]
        public Guid intDir { get; set; }
        [Required(ErrorMessage = "Select Head of Department before proceeding!")]
        [Display(Name = "Head of Department")]
        public string intHod { get; set; }
    }
}
