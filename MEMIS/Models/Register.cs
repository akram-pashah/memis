using MEMIS.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Models
{
    public class Register
    {
        [Key]
        public int IntUser { get; set; }
        [Required(ErrorMessage = "Username cannot be left blank!")]
        [Display(Name = "Username:")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Password cannot be left blank!")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$", ErrorMessage = "Password should contain at least 8-character length with one capital letter, small letter and number")]
        [Display(Name = "Password:")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Confirm Password cannot be left blank!")]
        [Display(Name = "ConfirmPassword:")]
        [Compare(nameof(Password),ErrorMessage ="Password and Confirm Passwor do not match!")]
        public String ConfirmPassword { get; set; } 
        public int UserStatus { get; set; } = 0;

        [Display(Name ="Department")]
        [Required(ErrorMessage = "Department cannot be left blank!")]
        public Guid intDept { get;set; }
        [Display(Name = "Directorate")]
        [Required(ErrorMessage = "Directorate cannot be left blank!")]
        public Guid intDir { get; set; }
        [Display(Name = "Region")]
        public Guid? intRegion { get; set; }

        [Required(ErrorMessage = "Email cannot be left blank!")]
        [Display(Name = "Email:")]
        [RegularExpression (@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage ="Invalid Email format")]
        public string Email { get; set; } = "";
        public string Address { get; set; } = "";

    }
}
