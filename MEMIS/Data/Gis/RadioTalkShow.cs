using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Data
{
  [Table("RadioTalkShow")]
  public class RadioTalkShow
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
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    
    [Display(Name = "District")]
    public virtual int DistrictId { get; set; }
    [ForeignKey("DistrictId")]
    public virtual District District { get; set; }
    [Display(Name = "Name of Radio Company")]
    public string? FacilityName { get; set; }
    [Display(Name = "Topic of Discussion")]
    public string? Topic { get; set; }
  }
}
