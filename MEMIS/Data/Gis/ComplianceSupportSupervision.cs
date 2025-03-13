using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Data
{
  [Table("ComplianceSupportSupervision")]
  public class ComplianceSupportSupervision
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "Inspection Date")]
    public DateTime InspectionDate { get; set; }
    [Display(Name = "Inspector Name")]
    public string? InspectorName { get; set; }
    
    public virtual Guid intRegion { get; set; }
    [ForeignKey("intRegion")]
    public virtual Region Region { get; set; }
    [Display(Name = "District")]
    public virtual int DistrictId { get; set; }
    [ForeignKey("DistrictId")]
    public virtual District District { get; set; }
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

    [Display(Name = "License Status")]
    public int? LicenseStatus { get; set; } = 0;
    [Display(Name = "PMS Activity Carried Out")]
    public int? PMSActivity { get; set; } = 0;

    [Display(Name = "Category of Product Sampled")]
    public int? CategoryStatus { get; set; } = 0;

    [Display(Name = "Condition of Facility")]
    public int? PremisesCondition { get; set; } = 0; 
    public string? InspectorId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    [Display(Name = "License No.")]

    public string? LicenseNo { get; set; }
    [Display(Name = "Name of Product Sampled")]
    public string? Sample_ProductName { get; set; }
    [Display(Name = "Number of Samples Collected ")]
    public int? Sample_No { get; set; } = 0;
    [Display(Name = "Batch Number of Samples Collected ")]

    public string? Sample_Batch { get; set; }
    [Display(Name = "Product being Followed Up ")]
    public string? Followup_Product  { get; set; }
    [Display(Name = "Comment on Over all Follow up  ")]
    public string? Followup_Comment { get; set; }
    [Display(Name = "Product Complaint Investigated ")]
    public string? Complaint_Product { get; set; }
    [Display(Name = "Specify Activity ")]
    public string? Other_Activity { get; set; }
  }
}
