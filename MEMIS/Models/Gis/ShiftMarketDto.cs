using MEMIS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Models
{ 
  public class ShiftMarketDto
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
     
    public string? PersonName { get; set; }
    public string? Contact { get; set; }
    public string? Qualifications { get; set; }

    [Display(Name = "Category of  Premises")]
    public int? CategoryOfpremises { get; set; } = 0;  
      
    [Display(Name = "Regulatory action taken (no of arrests made)")]
    public string? RegulatoryAction { get; set; }  
    [Display(Name = "Consignments Impounded and Drug Categories(Vet, Human, Herbal, Medical Device)")]
    public string? Consignment { get; set; }
  }
}
