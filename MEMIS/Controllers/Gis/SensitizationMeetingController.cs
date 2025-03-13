using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;
using cloudscribe.Pagination.Models;
using MEMIS.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MEMIS.Controllers
{
  public class SensitizationMeetingController : Controller
  {
    private readonly Data.AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public SensitizationMeetingController(Data.AppDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    public async Task<IActionResult> Index(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      var appDbContext = _context.SensitizationMeeting.Include(s => s.District).Include(r => r.Region)
          .Skip(offset)
          .Take(pageSize);
      var result = new PagedResult<SensitizationMeeting>
      {
        Data = await appDbContext.AsNoTracking().ToListAsync(),
        TotalItems = _context.SensitizationMeeting.Count(),
        PageNumber = pageNumber,
        PageSize = pageSize
      };
      ViewBag.Users = _userManager;
      return View(result);
    }

    public async Task<IActionResult> Verify(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      var appDbContext = _context.PreinspectionsPharma.Include(s => s.District).Skip(offset).Take(pageSize).Where(e => e.ApprovalStatusInspector == 0);
      //var appDbContext = _context.PreinspectionsPharma.Include(s => s.District).Skip(offset).Take(pageSize);
      var result = new PagedResult<PreinspectionPharma>
      {
        Data = await appDbContext.AsNoTracking().ToListAsync(),
        TotalItems = _context.PreinspectionsPharma.Count(),
        PageNumber = pageNumber,
        PageSize = pageSize
      };
      ViewBag.Users = _userManager;
      return View(result);
    }

    public async Task<IActionResult> Details(int? id)
    {
      if (id == null || _context.SensitizationMeeting == null)
      {
        return NotFound();
      }

      var preinspection = await _context.SensitizationMeeting
          .Include(s => s.District)
          .FirstOrDefaultAsync(m => m.Id == id);
      if (preinspection == null)
      {
        return NotFound();
      }
      ViewBag.Users = _userManager;

      return View(preinspection);
    }

    public IActionResult Create()
    {

      ViewData["intRegion"] = new SelectList(_context.Region, "intRegion", "regionName");
      ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
      ViewData["FacilityStatus"] = ListHelper.FacilityStatus();
      ViewData["PersonFoundatFacility"] = ListHelper.PersonFoundatFacility();
      ViewData["Category"] = ListHelper.CategoryofPremises();
      ViewData["LicenseStatus"] = ListHelper.LicenseStatus();
      ViewData["CategoryofDrugs"] = ListHelper.CategoryofDrugs();
      ViewData["ConditionofPremises"] = ListHelper.ConditionofPremises();
      ViewData["RecordKeeping"] = ListHelper.RecordKeeping();
      ViewData["ClassofDrugs"] = ListHelper.ClassofDrugs();
      ViewData["UnregisteredDrugs"] = ListHelper.UnregisteredDrugs();
      ViewData["ComplianceAction"] = ListHelper.ComplianceAction();
      ViewData["CertificationStatus"] = ListHelper.CertificationStatus();
      ViewData["GDPRecommendation"] = ListHelper.GDPRecommendation();
      ViewData["PMSActivity"] = ListHelper.PMSActivity();



      ViewBag.Users = _userManager;

      return View(new SensitizationMeetingDto { InspectionDate = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SensitizationMeetingDto radiodto)
    {
      if (ModelState.IsValid)
      {
        SensitizationMeeting dataobject = new()
        {
          InspectionDate = radiodto.InspectionDate,
          intRegion = radiodto.intRegion,
          DistrictId = radiodto.DistrictId,
          FacilityName = radiodto.FacilityName,
          Topic= radiodto.Topic,
          Latitude = radiodto.Latitude,
          Longitude = radiodto.Longitude,
          InspectorName = radiodto.InspectorName,
          // InspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier), 
        };
        _context.Add(dataobject);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }

      ViewData["intRegion"] = new SelectList(_context.Region, "intRegion", "regionName");
      ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
      ViewData["FacilityStatus"] = ListHelper.FacilityStatus();
      ViewData["PersonFoundatFacility"] = ListHelper.PersonFoundatFacility();
      ViewData["Category"] = ListHelper.CategoryofPremises();
      ViewData["LicenseStatus"] = ListHelper.LicenseStatus();
      ViewData["CategoryofDrugs"] = ListHelper.CategoryofDrugs();
      ViewData["ConditionofPremises"] = ListHelper.ConditionofPremises();
      ViewData["RecordKeeping"] = ListHelper.RecordKeeping();
      ViewData["ClassofDrugs"] = ListHelper.ClassofDrugs();
      ViewData["UnregisteredDrugs"] = ListHelper.UnregisteredDrugs();
      ViewData["ComplianceAction"] = ListHelper.ComplianceAction();
      ViewData["PMSActivity"] = ListHelper.PMSActivity();

      return View(radiodto);
    }

    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.SensitizationMeeting == null)
      {
        return NotFound();
      }

      var complianceSupervision = await _context.SensitizationMeeting.FindAsync(id);
      if (complianceSupervision == null)
      {
        return NotFound();
      }
     SensitizationMeetingDto  radioTalkShowDto = new SensitizationMeetingDto()
     {
        InspectionDate = complianceSupervision.InspectionDate,
        FacilityName = complianceSupervision.FacilityName, 
        intRegion = complianceSupervision.intRegion,
        DistrictId = complianceSupervision.DistrictId,
        Id = complianceSupervision.Id,
        Latitude= complianceSupervision.Latitude,
        Longitude= complianceSupervision.Longitude,
        Topic = complianceSupervision.Topic,
        InspectorName= complianceSupervision.InspectorName, 
      };
      ViewData["intRegion"] = new SelectList(_context.Region, "intRegion", "regionName");
      ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
      ViewData["FacilityStatus"] = ListHelper.FacilityStatus();
      ViewData["Category"] = ListHelper.CategoryofPremises(); 
      ViewData["ProductClassification"] = ListHelper.ProductClassification();
      return View(radioTalkShowDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SensitizationMeetingDto objectdto)
    {
      if (ModelState.IsValid)
      {
        try
        {
          SensitizationMeeting dataobject = new()
          {
            Id = objectdto.Id,
            InspectionDate = objectdto.InspectionDate,
            FacilityName = objectdto.FacilityName, 
            intRegion = objectdto.intRegion,
            DistrictId = objectdto.DistrictId, 
            Topic = objectdto.Topic,
            InspectorName = objectdto.InspectorName,
            Latitude = objectdto.Latitude,
            Longitude = objectdto.Longitude,

          };
          _context.Update(dataobject);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!SDTMasterExists(objectdto.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(Index));
      }
      ViewData["Category"] = ListHelper.CategoryofPremises();
      ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
      ViewData["ProductClassification"] = ListHelper.ProductClassification();
      return View(objectdto);
    }

    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.SensitizationMeeting == null)
      {
        return NotFound();
      }

      var dataobject = await _context.SensitizationMeeting.Include(x => x.District)
          .FirstOrDefaultAsync(m => m.Id == id);
      if (dataobject == null)
      {
        return NotFound();
      }
      ViewBag.Users = _userManager;
      return View(dataobject);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      if (_context.SDTMasters == null)
      {
        return Problem("Entity set 'AppDbContext.Preinspection'  is null.");
      }
      var dataobject = await _context.SensitizationMeeting.FindAsync(id);
      if (dataobject != null)
      {
        _context.SensitizationMeeting.Remove(dataobject);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool SDTMasterExists(int id)
    {
      return (_context.PreinspectionsPharma?.Any(e => e.Id == id)).GetValueOrDefault();
    }


  }
}
