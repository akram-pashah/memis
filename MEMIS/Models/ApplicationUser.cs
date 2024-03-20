using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MEMIS.Models
{
    public class ApplicationUser:IdentityUser
    { 
        public Guid intDept { get; set; }
        public  Guid intDir { get; set; } 
        public Guid? intRegion { get; set; }
    }
}
