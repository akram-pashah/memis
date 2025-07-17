namespace MEMIS.ViewModels.GIS
{
  public class RadioTalkShowViewModel
  {
    public string InspectionDate { get; set; }
    public string? InspectorName { get; set; }
    public string? Region { get; set; }
    public string? District { get; set; }
    public string? FacilityName { get; set; } // Radio Company
    public string? Topic { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
  }

}
