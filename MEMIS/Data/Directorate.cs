using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MEMIS.Models;
using Microsoft.AspNetCore.Identity;

namespace MEMIS.Data
{ 
    public class Directorate
    {
        [Key] 
        public Guid intDir { get; set; }
      
        public string dirCode { get; set; }
      
        public string dirName { get; set; } 
        public string director { get; set; }
        public  ApplicationUser Director { get; set; } 
    }
}
