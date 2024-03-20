using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Models
{ 
    public class EnforcementDto
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Inspection Date")]
        public DateTime InspectionDate { get; set; }
        [Display(Name = "Inspector Name")]

        public string? InspectorName { get; set; }
        public string GPS { get; set; }
        [Display(Name = "Region")]
        public virtual Guid intRegion { get; set; } 
        public virtual int DistrictId { get; set; } 
        public string FacilityName { get; set; }
        [Display(Name = "Facility Status")]

        public int FacilityStatus { get; set; }
        [Display(Name = "Person Found at Facility")]

        public int FacilityPersonType { get; set; } 
        public string PersonName { get; set; }
        public string Contact { get; set; }
        public string Qualifications { get; set; }
        [Display(Name = "Category of  Premises")]
        public int CategoryOfpremises { get; set; }

        [Display(Name = "License Status")]
        public int LicenseStatus { get; set; }

        [Display(Name = "Category Status")]
        public int CategoryStatus { get; set; }
         
        [Display(Name = "Enforcement Action Taken")]
        public int EnfAction { get; set; }
        [Display(Name = "Comments")]
        public string? Comments { get; set; }
        public string? InspectorId { get; set; } 
    }
}
