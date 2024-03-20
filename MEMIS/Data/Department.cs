using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MEMIS.Models;
namespace MEMIS.Data
{
    public class Department
    {
        [Key]
        public Guid intDept { get; set; }
        [Required(ErrorMessage = "Department Code cannot be left blank!")]
        [Display(Name = "Department Code")]
        public string deptCode { get; set; }
        [Required(ErrorMessage = "Department Name cannot be left blank!")]
        [Display(Name = "Department Name")]
        public string deptName { get; set; } 

        public Guid intDir { get; set; }
        [ForeignKey("intDir")]
        public virtual Directorate DirectorateFk { get; set; }

        public string intHod { get; set; }
        public ApplicationUser Hod { get; set; }

    }
}
