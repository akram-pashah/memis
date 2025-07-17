using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
  [Table("SensitizationMeeting")]
  public class SensitizationMeeting
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "Inspection Date")]
    public DateTime InspectionDate { get; set; }
    [Display(Name = "Inspector Name")]
    public string? InspectorName { get; set; }

    public virtual Guid intRegion { get; set; }
    [ForeignKey("intRegion")]
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public virtual Region Region { get; set; }
    [Display(Name = "District")]
    public virtual int DistrictId { get; set; }
    [ForeignKey("DistrictId")]
    public virtual District District { get; set; }
    [Display(Name = "Venue/Location ")]
    public string? FacilityName { get; set; }
    [Display(Name = "Topic of Discussion")]
    public string? Topic { get; set; }
    [Display(Name = "Number of Participants")]
    public int? Participants { get; set; } = 0;
  }
}
