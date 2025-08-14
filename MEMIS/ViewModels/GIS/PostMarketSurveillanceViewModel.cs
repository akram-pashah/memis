namespace MEMIS.ViewModels.GIS
{
  public class PostMarketSurveillanceViewModel
  {
    public string? InspectionDate { get; set; }
    public string? InspectorName { get; set; }
    public string? InspectorId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public string? Region { get; set; }
    public string? District { get; set; }
    public string? FacilityName { get; set; }
    public string? FacilityStatus { get; set; }
    public string? FacilityPersonType { get; set; }
    public string? PersonName { get; set; }
    public string? Contact { get; set; }
    public string? Qualifications { get; set; }
    public string? CategoryOfPremises { get; set; }
    public string? OtherCategoryPremise { get; set; }
    public string? LicenseStatus { get; set; }
    public string? LicenseNo { get; set; }
    public string? UnlicensedStatus { get; set; }
    public string? PMSActivity { get; set; }
    public string? SampleProductName { get; set; }
    public int? SampleQuantity { get; set; }
    public string? SampleBatch { get; set; }
    public string? FollowupComment { get; set; }
    public string? ComplaintProduct { get; set; }
    public string? OtherActivity { get; set; }
  }

}
