using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Data
{
    [Table("PreinspectionManufacturingUnit")]
    public class PreinspectionNewManUnit
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Inspection Date")]
        public DateTime InspectionDate { get; set; }
        public string Applicant { get; set; }
        public string Business { get; set; }
        public string Road { get; set; }
        public string Zone { get; set; }
        public string Village { get; set; }
        public string Country { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string GPS { get; set; }
        [Display(Name = "Product Classification")]
        public int ProductClassification { get; set; }

        [Display(Name = "Category of  Premises")]
        public int CategoryOfpremises { get; set; }
        public string? comments { get; set; }    
		[Display(Name = "District")]
		public virtual int DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }
        [Display(Name ="Inspector Name")]
        public string? InspectorId { get; set; }
    }
}
