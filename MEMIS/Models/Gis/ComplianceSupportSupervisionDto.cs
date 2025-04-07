using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Models
{
  public class ComplianceSupportSupervisionDto
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "Inspection Date")]
    public DateTime InspectionDate { get; set; }
    [Display(Name = "Inspector Name")]

    public string? InspectorName { get; set; } 
    [Display(Name = "Region")]
    public virtual Guid intRegion { get; set; }
    [Display(Name = "District")]
    public virtual int DistrictId { get; set; }
    public string? FacilityName { get; set; }
    [Display(Name = "Facility Status")]

    public int? FacilityStatus { get; set; } = 0;
    [Display(Name = "Person Found at Facility")]

    public int? FacilityPersonType { get; set; } = 0;
    public string? PersonName { get; set; }
    public string? Contact { get; set; }
    public string? Qualifications { get; set; }
    [Display(Name = "Category of  Facility")]
    public int? CategoryOfpremises { get; set; } = 0;
    [Display(Name = "State the other type of facility")]

    public string? OtherTypePremise { get; set; }

    [Display(Name = "License Status")]
    public int? LicenseStatus { get; set; } = 0;
    [Display(Name = "Previously Licensed or Illegal Outlet")]
    public int? Unlicensed { get; set; } = 0;
    

    [Display(Name = "Category of Drugs")]
    public int? CategoryStatus { get; set; } = 0;
     

    [Display(Name = "Condition of Premises")]
    public int? PremisesCondition { get; set; } = 0;
    [Display(Name = "Record Keeping")]
    public int? RecordKeeping { get; set; } = 0;
    [Display(Name = "Class of Drugs")]
    public int? ClassofDrugs { get; set; } = 0;
    [Display(Name = "Un Registered Drugs")]
    public int? UnRegisteredDrug { get; set; } = 0;
    public string? InspectorId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    [Display(Name = "License No.")]

    public string? LicenseNo { get; set; }
    [Display(Name = "State the name and quantities of unregistered drug")]

    public string? UnRegDrugQty { get; set; }
    [Display(Name = "Action")]

    public int? Action { get; set; }
  }
}
