using MEMIS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Models
{ 
  public class PostMarketSurveillanceDto
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "Inspection Date")]
    public DateTime InspectionDate { get; set; }
    [Display(Name = "Inspector Name")]
    public string? InspectorName { get; set; }
    public string? InspectorId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    [Display(Name = "Region")]
    public virtual Guid intRegion { get; set; }
    [Display(Name = "District")]

    public virtual int DistrictId { get; set; } 

    [Display(Name = "Name of Facility")]
    public string? FacilityName { get; set; }
    [Display(Name = "Facility Status")]

    public int? FacilityStatus { get; set; } = 0;
    [Display(Name = "Person Found at Facility")]

    public int? FacilityPersonType { get; set; } = 0;
    public string? PersonName { get; set; }
    public string? Contact { get; set; }
    public string? Qualifications { get; set; }

    [Display(Name = "Category of  Premises")]
    public int? CategoryOfpremises { get; set; } = 0;
    [Display(Name = "State the type of facility")]
    public string? Other_CategoryPremise { get; set; }
    [Display(Name = "License Status")]
    public int? LicenseStatus { get; set; } = 0;
    [Display(Name = "License No.")]
    public string? LicenseNo { get; set; }
    [Display(Name = "Previously Licensed or Illegal Outlet")]
    public int? Unlicensed { get; set; } = 0; 
    [Display(Name = "PMS Activity Carried Out")]

    public int? PMSActivity { get; set; } = 0; 
      
    [Display(Name = "Name of Product")]
    public string? Sample_ProductName { get; set; }
    [Display(Name = "Quantity ")]
    public int? Sample_No { get; set; } = 0;
    [Display(Name = "Batch Number ")]

    public string? Sample_Batch { get; set; } 
    [Display(Name = "Comment on Over all Follow up  ")]
    public string? Followup_Comment { get; set; }
    [Display(Name = "State any post market complaint noted")]
    public string? Complaint_Product { get; set; }
    [Display(Name = "Specify Activity ")]
    public string? Other_Activity { get; set; }
  }
}
