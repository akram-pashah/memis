using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Models
{
    public class ProjectInitiationDto
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        [Display(Name = "Project Type")]

        public int  Type { get; set; }
        [Display(Name = "Project Section")]

        public string Section { get; set; }
        public string Program { get; set; }
        [Display(Name = "Sub Program")]

        public string SubProgram { get; set; }
        public string Desc { get; set; }
        [Display(Name = "Project Manager")]

        public string Manager { get; set; }

        [Display(Name = "Sart Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public string Members { get; set; }
        public string BudgetCode { get; set; }
        public decimal Cost { get; set; }
        [ForeignKey("Department")]
        public Guid DepartmentId { get; set; }
    }
}
