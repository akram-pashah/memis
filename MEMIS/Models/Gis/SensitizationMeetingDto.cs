using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Models
{
  public class SensitizationMeetingDto
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "Inspection Date")]
    public DateTime InspectionDate { get; set; }
    [Display(Name = "Inspector Name")]

    public string? InspectorName { get; set; } 
    [Display(Name = "Region")]
    public virtual Guid intRegion { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    [Display(Name = "District")]
    public virtual int DistrictId { get; set; }
    [Display(Name = "Venue/Location ")]
    public string? FacilityName { get; set; }
    [Display(Name = "Topic of Discussion")]
    public string? Topic { get; set; }
    [Display(Name = "Number of Participants")]
    public int? Participants { get; set; } = 0;
  }
}
