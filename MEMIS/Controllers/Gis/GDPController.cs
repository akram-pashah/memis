using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;
using cloudscribe.Pagination.Models;
using MEMIS.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using MEMIS.Migrations;
using System.Runtime.Intrinsics.Arm;
using GDP = MEMIS.Data.GDP;

namespace MEMIS.Controllers
{
  public class GDPController : Controller
  {
    private readonly Data.AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public GDPController(Data.AppDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    public async Task<IActionResult> Index(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      var appDbContext = _context.GDP.Include(s => s.District).Include(r => r.Region)
          .Skip(offset)
          .Take(pageSize);
      var result = new PagedResult<GDP>
      {
        Data = await appDbContext.AsNoTracking().ToListAsync(),
        TotalItems = _context.GDP.Count(),
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
      var appDbContext = _context.GDP.Include(s => s.District).Skip(offset).Take(pageSize);
      //var appDbContext = _context.PreinspectionsPharma.Include(s => s.District).Skip(offset).Take(pageSize);
      var result = new PagedResult<GDP>
      {
        Data = await appDbContext.AsNoTracking().ToListAsync(),
        TotalItems = _context.GDP.Count(),
        PageNumber = pageNumber,
        PageSize = pageSize
      };
      ViewBag.Users = _userManager;
      return View(result);
    }

    public async Task<IActionResult> Details(int? id)
    {
      if (id == null || _context.GDP == null)
      {
        return NotFound();
      }

      var enforcement = await _context.GDP
          .Include(s => s.District)
          .FirstOrDefaultAsync(m => m.Id == id);
      if (enforcement == null)
      {
        return NotFound();
      }
      ViewBag.Users = _userManager;

      return View(enforcement);
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
      ViewData["FacilityType"] = ListHelper.FacilityType();
      ViewData["CertificationStatus"] = ListHelper.CertificationStatus();
      ViewData["GDPRecommendation"] = ListHelper.GDPRecommendation();



      ViewBag.Users = _userManager;

      return View(new GDPDto { InspectionDate = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GDPDto gdp)
    {
      if (ModelState.IsValid)
      {
        GDP dataobject = new()
        {
          InspectionDate = gdp.InspectionDate,
          intRegion = gdp.intRegion,
          DistrictId = gdp.DistrictId,
          FacilityName = gdp.FacilityName,
          FacilityStatus = gdp.FacilityStatus,
          FacilityPersonType = gdp.FacilityPersonType,
          PersonName = gdp.PersonName,
          Contact = gdp.Contact,
          Qualifications = gdp.Qualifications,
          CategoryOfpremises = gdp.CategoryOfpremises,
          LicenseStatus = gdp.LicenseStatus,
          CategoryStatus = gdp.CategoryStatus,
          FacilityType = gdp.FacilityType,
          certStatus = gdp.certStatus,
          RecommendedforGDP = gdp.RecommendedforGDP,
          LicenseNo = gdp.LicenseNo,
          InspectorName = gdp.InspectorName,
          Latitude = gdp.Latitude,
          Longitude = gdp.Longitude,
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
      ViewData["FacilityType"] = ListHelper.FacilityType();
      ViewData["CertificationStatus"] = ListHelper.CertificationStatus();
      ViewData["GDPRecommendation"] = ListHelper.GDPRecommendation();
      return View(gdp);
    }

    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.GDP == null)
      {
        return NotFound();
      }

      var gdp = await _context.GDP.FindAsync(id);
      if (gdp == null)
      {
        return NotFound();
      }
      GDPDto gppdto = new GDPDto
      {
        InspectionDate = gdp.InspectionDate,
        GPS = gdp.GPS,
        FacilityName = gdp.FacilityName,
        FacilityStatus = gdp.FacilityStatus,
        FacilityPersonType = gdp.FacilityPersonType,
        PersonName = gdp.PersonName,
        Contact = gdp.Contact,
        Qualifications = gdp.Qualifications,
        CategoryOfpremises = gdp.CategoryOfpremises,
        LicenseStatus = gdp.LicenseStatus,
        CategoryStatus = gdp.CategoryStatus,
        FacilityType = gdp.FacilityType,
        certStatus = gdp.certStatus,
        RecommendedforGDP = gdp.RecommendedforGDP,
        intRegion = gdp.intRegion,
        DistrictId = gdp.DistrictId,
        Id = gdp.Id,
        InspectorId = gdp.InspectorId,
        LicenseNo = gdp.LicenseNo,
        InspectorName = gdp.InspectorName,
        Latitude = gdp.Latitude,
        Longitude = gdp.Longitude,
      };
      ViewData["intRegion"] = new SelectList(_context.Region, "intRegion", "regionName");
      ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
      ViewData["FacilityStatus"] = ListHelper.FacilityStatus();
      ViewData["PersonFoundatFacility"] = ListHelper.PersonFoundatFacility();
      ViewData["Category"] = ListHelper.CategoryofPremises();
      ViewData["LicenseStatus"] = ListHelper.LicenseStatus();
      ViewData["CategoryofDrugs"] = ListHelper.CategoryofDrugs();
      ViewData["FacilityType"] = ListHelper.FacilityType();
      ViewData["CertificationStatus"] = ListHelper.CertificationStatus();
      ViewData["GDPRecommendation"] = ListHelper.GDPRecommendation();
      return View(gppdto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(GDPDto objectdto)
    {
      if (ModelState.IsValid)
      {
        try
        {
          GDP dataobject = new()
          {
            Id = objectdto.Id,
            InspectionDate = objectdto.InspectionDate, 
            FacilityName = objectdto.FacilityName,
            FacilityStatus = objectdto.FacilityStatus,
            FacilityPersonType = objectdto.FacilityPersonType,
            PersonName = objectdto.PersonName,
            Contact = objectdto.Contact,
            Qualifications = objectdto.Qualifications,
            CategoryOfpremises = objectdto.CategoryOfpremises,
            LicenseStatus = objectdto.LicenseStatus,
            CategoryStatus = objectdto.CategoryStatus,
            FacilityType = objectdto.FacilityType,
            certStatus = objectdto.certStatus,
            RecommendedforGDP = objectdto.RecommendedforGDP,
            intRegion = objectdto.intRegion,
            DistrictId = objectdto.DistrictId,
            //InspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            InspectorId = objectdto.InspectorId,
            LicenseNo = objectdto.LicenseNo,
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
      ViewData["intRegion"] = new SelectList(_context.Region, "intRegion", "regionName");
      ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
      ViewData["FacilityStatus"] = ListHelper.FacilityStatus();
      ViewData["PersonFoundatFacility"] = ListHelper.PersonFoundatFacility();
      ViewData["Category"] = ListHelper.CategoryofPremises();
      ViewData["LicenseStatus"] = ListHelper.LicenseStatus();
      ViewData["CategoryofDrugs"] = ListHelper.CategoryofDrugs();
      ViewData["FacilityType"] = ListHelper.FacilityType();
      ViewData["CertificationStatus"] = ListHelper.CertificationStatus();
      ViewData["GDPRecommendation"] = ListHelper.GDPRecommendation();
      return View(objectdto);
    }

    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.GDP == null)
      {
        return NotFound();
      }

      var dataobject = await _context.GDP.Include(x => x.District)
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
      if (_context.GDP == null)
      {
        return Problem("Entity set 'AppDbContext.GDP'  is null.");
      }
      var dataobject = await _context.GDP.FindAsync(id);
      if (dataobject != null)
      {
        _context.GDP.Remove(dataobject);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool SDTMasterExists(int id)
    {
      return (_context.GDP?.Any(e => e.Id == id)).GetValueOrDefault();
    }


  }
}
