using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
namespace MEMIS.Models
{
    public class LoginAuth
    {

        [Required(ErrorMessage = "Username cannot be left blank!")]
        [Display(Name = "Username")]
        public string userName { get; set; } = "";
        [Required(ErrorMessage = "Password cannot be left blank!")]
        [Display(Name = "Password")]
        public string password { get; set; } = "";
        [Display(Name = "Financial Year")]
        public int? fyear { get; set; } = 2024;
    }
}
