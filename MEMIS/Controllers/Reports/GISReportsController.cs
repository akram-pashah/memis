using MEMIS.Data;
using MEMIS.ViewModels.GIS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MEMIS.Controllers.Reports
{
  public class GISReportsController : Controller
  {
    private readonly AppDbContext _context;
    public GISReportsController(AppDbContext context)
    {
      _context = context;
    }

    public async Task<IActionResult> Index()
    {
      var list = await _context.GPP.Include(s => s.District).Include(r => r.Region).ToListAsync();

      var facilityStatusDict = ListHelper.FacilityStatusDict();
      var personTypeDict = ListHelper.PersonFoundatFacilityDict();
      var categoryDict = ListHelper.CategoryofPremisesDict();
      var licenseStatusDict = ListHelper.LicenseStatusDict();
      var categoryStatusDict = ListHelper.CategoryofDrugsDict();
      var facilityTypeDict = ListHelper.FacilityTypeDict();
      var certStatusDict = ListHelper.CertificationStatusDict();
      var recommendedGPPDict = ListHelper.GPPRecommendationDict();

      var model = list.Select(x => new GPPViewModel
      {
        InspectionDate = x.InspectionDate.ToString("dd-MMM-yyyy"),
        InspectorName = x.InspectorName,
        GPS = x.GPS,
        Region = x.Region?.regionName,
        District = x.District?.Name,
        FacilityName = x.FacilityName,
        FacilityStatus = facilityStatusDict.GetValueOrDefault(x.FacilityStatus),
        FacilityPersonType = x.FacilityPersonType.HasValue ? personTypeDict.GetValueOrDefault(x.FacilityPersonType.Value) : null,
        PersonName = x.PersonName,
        Contact = x.Contact,
        Qualifications = x.Qualifications,
        CategoryOfpremises = x.CategoryOfpremises.HasValue ? categoryDict.GetValueOrDefault(x.CategoryOfpremises.Value) : null,
        LicenseStatus = x.LicenseStatus.HasValue ? licenseStatusDict.GetValueOrDefault(x.LicenseStatus.Value) : null,
        CategoryStatus = x.CategoryStatus.HasValue ? categoryStatusDict.GetValueOrDefault(x.CategoryStatus.Value) : null,
        FacilityType = x.FacilityType.HasValue ? facilityTypeDict.GetValueOrDefault(x.FacilityType.Value) : null,
        CertificationStatus = x.certStatus.HasValue ? certStatusDict.GetValueOrDefault(x.certStatus.Value) : null,
        RecommendedforGPP = x.RecommendedforGPP.HasValue ? recommendedGPPDict.GetValueOrDefault(x.RecommendedforGPP.Value) : null,
        InspectorId = x.InspectorId,
        Latitude = x.Latitude,
        Longitude = x.Longitude,
        LicenseNo = x.LicenseNo
      }).ToList();

      return View(model);
    }
    public async Task<IActionResult> GDPReport()
    {
      var list = await _context.GDP.Include(s => s.District).Include(r => r.Region).ToListAsync();

      var model = list.Select(x => new GDPViewModel
      {
        InspectionDate = x.InspectionDate.ToString("dd-MMM-yyyy"),
        InspectorName = x.InspectorName,
        GPS = x.GPS,
        Region = x.Region?.regionName,
        District = x.District?.Name,
        FacilityName = x.FacilityName,
        FacilityStatus = ListHelper.FacilityStatusDict().GetValueOrDefault(x.FacilityStatus),
        FacilityPersonType = x.FacilityPersonType.HasValue ? ListHelper.PersonFoundatFacilityDict().GetValueOrDefault(x.FacilityPersonType.Value) : null,
        PersonName = x.PersonName,
        Contact = x.Contact,
        Qualifications = x.Qualifications,
        CategoryOfpremises = x.CategoryOfpremises.HasValue ? ListHelper.CategoryofPremisesDict().GetValueOrDefault(x.CategoryOfpremises.Value) : null,
        LicenseStatus = x.LicenseStatus.HasValue ? ListHelper.LicenseStatusDict().GetValueOrDefault(x.LicenseStatus.Value) : null,
        CategoryStatus = x.CategoryStatus.HasValue ? ListHelper.CategoryofDrugsDict().GetValueOrDefault(x.CategoryStatus.Value) : null,
        FacilityType = x.FacilityType.HasValue ? ListHelper.FacilityTypeDict().GetValueOrDefault(x.FacilityType.Value) : null,
        CertificationStatus = x.certStatus.HasValue ? ListHelper.CertificationStatusDict().GetValueOrDefault(x.certStatus.Value) : null,
        RecommendedforGDP = x.RecommendedforGDP.HasValue ? ListHelper.GDPRecommendationDict().GetValueOrDefault(x.RecommendedforGDP.Value) : null,
        InspectorId = x.InspectorId,
        Latitude = x.Latitude,
        Longitude = x.Longitude,
        LicenseNo = x.LicenseNo
      }).ToList();

      return View(model);
    }
    public async Task<IActionResult> ComplianceSupportSupervisionReport()
    {
      var list = await _context.ComplianceSupportSupervision.Include(s => s.District).Include(r => r.Region).ToListAsync();

      var model = list.Select(x => new ComplianceSupportSupervisionViewModel
      {
        InspectionDate = x.InspectionDate.ToString("dd-MMM-yyyy"),
        InspectorName = x.InspectorName,
        InspectorId = x.InspectorId,
        Latitude = x.Latitude,
        Longitude = x.Longitude,
        Region = x.Region?.regionName,
        District = x.District?.Name,
        FacilityName = x.FacilityName,
        FacilityStatus = x.FacilityStatus.HasValue ? ListHelper.FacilityStatusDict().GetValueOrDefault(x.FacilityStatus.Value) : null,
        FacilityPersonType = x.FacilityPersonType.HasValue ? ListHelper.PersonFoundatFacilityDict().GetValueOrDefault(x.FacilityPersonType.Value) : null,
        PersonName = x.PersonName,
        Contact = x.Contact,
        Qualifications = x.Qualifications,
        CategoryOfpremises = x.CategoryOfpremises.HasValue ? ListHelper.CategoryofPremisesDict().GetValueOrDefault(x.CategoryOfpremises.Value) : null,
        OtherCategoryPremise = x.Other_CategoryPremise,
        LicenseStatus = x.LicenseStatus.HasValue ? ListHelper.LicenseStatusDict().GetValueOrDefault(x.LicenseStatus.Value) : null,
        LicenseNo = x.LicenseNo,
        UnlicensedStatus = x.Unlicensed.HasValue ? ListHelper.UnlicensedStatusDict().GetValueOrDefault(x.Unlicensed.Value) : null,
        CategoryStatus = x.CategoryStatus.HasValue ? ListHelper.CategoryofDrugsDict().GetValueOrDefault(x.CategoryStatus.Value) : null,
        PremisesCondition = x.PremisesCondition.HasValue ? ListHelper.ConditionofPremisesDict().GetValueOrDefault(x.PremisesCondition.Value) : null,
        RecordKeeping = x.RecordKeeping.HasValue ? ListHelper.RecordKeepingDict().GetValueOrDefault(x.RecordKeeping.Value) : null,
        ClassofDrugs = x.ClassofDrugs.HasValue ? ListHelper.ClassofDrugsDict().GetValueOrDefault(x.ClassofDrugs.Value) : null,
        UnRegisteredDrug = x.UnRegisteredDrug.HasValue ? ListHelper.UnregisteredDrugsDict().GetValueOrDefault(x.UnRegisteredDrug.Value) : null,
        UnRegDrugQty = x.UnRegDrugQty,
        Action = x.Action.HasValue ? ListHelper.ComplianceActionDict().GetValueOrDefault(x.Action.Value) : null
      }).ToList();

      return View(model);
    }
    public async Task<IActionResult> PostMarketSurveillanceReport()
    {
      var list = await _context.PostMarketSurveillance.Include(s => s.District).Include(r => r.Region).ToListAsync();

      var model = list.Select(x => new PostMarketSurveillanceViewModel
      {
        InspectionDate = x.InspectionDate.ToString("dd-MMM-yyyy"),
        InspectorName = x.InspectorName,
        InspectorId = x.InspectorId,
        Latitude = x.Latitude,
        Longitude = x.Longitude,
        Region = x.Region?.regionName,
        District = x.District?.Name,
        FacilityName = x.FacilityName,
        FacilityStatus = x.FacilityStatus.HasValue ? ListHelper.FacilityStatusDict().GetValueOrDefault(x.FacilityStatus.Value) : null,
        FacilityPersonType = x.FacilityPersonType.HasValue ? ListHelper.PersonFoundatFacilityDict().GetValueOrDefault(x.FacilityPersonType.Value) : null,
        PersonName = x.PersonName,
        Contact = x.Contact,
        Qualifications = x.Qualifications,
        CategoryOfPremises = x.CategoryOfpremises.HasValue ? ListHelper.CategoryofPremisesDict().GetValueOrDefault(x.CategoryOfpremises.Value) : null,
        OtherCategoryPremise = x.Other_CategoryPremise,
        LicenseStatus = x.LicenseStatus.HasValue ? ListHelper.LicenseStatusDict().GetValueOrDefault(x.LicenseStatus.Value) : null,
        LicenseNo = x.LicenseNo,
        UnlicensedStatus = x.Unlicensed.HasValue ? ListHelper.UnlicensedStatusDict().GetValueOrDefault(x.Unlicensed.Value) : null,
        PMSActivity = x.PMSActivity.HasValue ? ListHelper.PMSActivityDict().GetValueOrDefault(x.PMSActivity.Value) : null,
        SampleProductName = x.Sample_ProductName,
        SampleQuantity = x.Sample_No,
        SampleBatch = x.Sample_Batch,
        FollowupComment = x.Followup_Comment,
        ComplaintProduct = x.Complaint_Product,
        OtherActivity = x.Other_Activity
      }).ToList();

      return View(model);
    }
    public async Task<IActionResult> ShiftMarketReport()
    {
      var list = await _context.ShiftMarket.Include(s => s.District).Include(r => r.Region).ToListAsync();

      var model = list.Select(x => new ShiftMarketViewModel
      {
        InspectionDate = x.InspectionDate.ToString("dd-MMM-yyyy"),
        InspectorName = x.InspectorName,
        InspectorId = x.InspectorId,
        Latitude = x.Latitude,
        Longitude = x.Longitude,
        Region = x.Region?.regionName,
        District = x.District?.Name,
        FacilityName = x.FacilityName,
        FacilityStatus = x.FacilityStatus.HasValue ? ListHelper.FacilityStatusDict().GetValueOrDefault(x.FacilityStatus.Value) : null,
        PersonName = x.PersonName,
        Contact = x.Contact,
        Qualifications = x.Qualifications,
        CategoryOfPremises = x.CategoryOfpremises.HasValue ? ListHelper.CategoryofPremisesDict().GetValueOrDefault(x.CategoryOfpremises.Value) : null,
        RegulatoryAction = x.RegulatoryAction,
        Consignment = x.Consignment
      }).ToList();

      return View(model);
    }
    public async Task<IActionResult> RadioTalkShowsReport()
    {
      var list = await _context.RadioTalkShow.Include(s => s.District).Include(r => r.Region).ToListAsync();

      var model = list.Select(x => new RadioTalkShowViewModel
      {
        InspectionDate = x.InspectionDate.ToString("dd-MMM-yyyy"),
        InspectorName = x.InspectorName,
        Region = x.Region?.regionName,
        District = x.District?.Name,
        FacilityName = x.FacilityName,
        Topic = x.Topic,
        Latitude = x.Latitude,
        Longitude = x.Longitude
      }).ToList();

      return View(model);
    }
    public async Task<IActionResult> SensitizationMeetingsReport()
    {
      var list = await _context.SensitizationMeeting.Include(s => s.District).Include(r => r.Region).ToListAsync();

      var model = list.Select(x => new SensitizationMeetingViewModel
      {
        InspectionDate = x.InspectionDate.ToString("dd-MMM-yyyy"),
        InspectorName = x.InspectorName,
        Region = x.Region?.regionName,
        District = x.District?.Name,
        FacilityName = x.FacilityName,
        Topic = x.Topic,
        Participants = x.Participants,
        Latitude = x.Latitude,
        Longitude = x.Longitude
      }).ToList();

      return View(model);
    }
  }
}
