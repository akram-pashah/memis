namespace MEMIS.ViewModels.GIS
{
  public class ShiftMarketViewModel
  {
    public string InspectionDate { get; set; }
    public string? InspectorName { get; set; }
    public string? InspectorId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public string? Region { get; set; }
    public string? District { get; set; }
    public string? FacilityName { get; set; }
    public string? FacilityStatus { get; set; }
    public string? PersonName { get; set; }
    public string? Contact { get; set; }
    public string? Qualifications { get; set; }
    public string? CategoryOfPremises { get; set; }

    public string? RegulatoryAction { get; set; }
    public string? Consignment { get; set; }
  }

}
