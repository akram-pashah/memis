namespace MEMIS.ViewModels.GIS
{
  public class SensitizationMeetingViewModel
  {
    public string InspectionDate { get; set; }
    public string? InspectorName { get; set; }
    public string? Region { get; set; }
    public string? District { get; set; }
    public string? FacilityName { get; set; }
    public string? Topic { get; set; }
    public int? Participants { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
  }

}
