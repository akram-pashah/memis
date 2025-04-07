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
  public class ShiftMarketController : Controller
  {
    private readonly Data.AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ShiftMarketController(Data.AppDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    public async Task<IActionResult> Index(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      var appDbContext = _context.ShiftMarket.Include(s => s.District).Include(r => r.Region)
          .Skip(offset)
          .Take(pageSize);
      var result = new PagedResult<ShiftMarket>
      {
        Data = await appDbContext.AsNoTracking().ToListAsync(),
        TotalItems = _context.ShiftMarket.Count(),
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
      if (id == null || _context.PostMarketSurveillance == null)
      {
        return NotFound();
      }

      var preinspection = await _context.ShiftMarket
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
      ViewData["UnlicensedStatus"] = ListHelper.UnlicensedStatus();



      ViewBag.Users = _userManager;

      return View(new ShiftMarketDto { InspectionDate = DateTime.Now,CategoryOfpremises=10 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ShiftMarketDto compliance)
    {
      if (ModelState.IsValid)
      {
        ShiftMarket dataobject = new()
        {
          InspectionDate = compliance.InspectionDate,
          intRegion = compliance.intRegion,
          DistrictId = compliance.DistrictId,
          FacilityName = compliance.FacilityName,
          FacilityStatus = compliance.FacilityStatus, 
          PersonName = compliance.PersonName,
          Contact = compliance.Contact,
          Qualifications = compliance.Qualifications,
          CategoryOfpremises = compliance.CategoryOfpremises,
          RegulatoryAction= compliance.RegulatoryAction,
          Consignment= compliance.Consignment,
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
      return View(compliance);
    }

    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.PostMarketSurveillance == null)
      {
        return NotFound();
      }

      var complianceSupervision = await _context.ShiftMarket.FindAsync(id);
      if (complianceSupervision == null)
      {
        return NotFound();
      }
      ShiftMarketDto complianceSupervisionDto = new ShiftMarketDto
      {
        InspectionDate = complianceSupervision.InspectionDate,
        FacilityName = complianceSupervision.FacilityName,
        FacilityStatus = complianceSupervision.FacilityStatus, 
        PersonName = complianceSupervision.PersonName,
        Contact = complianceSupervision.Contact,
        Qualifications = complianceSupervision.Qualifications,
        CategoryOfpremises = complianceSupervision.CategoryOfpremises,
        RegulatoryAction = complianceSupervision.RegulatoryAction,
        Consignment=complianceSupervision.Consignment,
        intRegion = complianceSupervision.intRegion,
        DistrictId = complianceSupervision.DistrictId,
        Id = complianceSupervision.Id,
        InspectorId = complianceSupervision.InspectorId,
      };
      ViewData["Category"] = ListHelper.CategoryofPremises();
      ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
      ViewData["ProductClassification"] = ListHelper.ProductClassification();
      return View(complianceSupervisionDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ShiftMarketDto objectdto)
    {
      if (ModelState.IsValid)
      {
        try
        {
          ShiftMarket dataobject = new()
          {
            Id = objectdto.Id,
            InspectionDate = objectdto.InspectionDate,
            FacilityName = objectdto.FacilityName,
            FacilityStatus = objectdto.FacilityStatus, 
            PersonName = objectdto.PersonName,
            Contact = objectdto.Contact,
            Qualifications = objectdto.Qualifications,
            CategoryOfpremises = objectdto.CategoryOfpremises,
            RegulatoryAction= objectdto.RegulatoryAction,
            Consignment=  objectdto.Consignment,
            intRegion = objectdto.intRegion,
            DistrictId = objectdto.DistrictId,
            //InspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            InspectorId = objectdto.InspectorId,

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
      if (id == null || _context.PreinspectionsPharma == null)
      {
        return NotFound();
      }

      var dataobject = await _context.PreinspectionsPharma.Include(x => x.District)
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
      var dataobject = await _context.PreinspectionsPharma.FindAsync(id);
      if (dataobject != null)
      {
        _context.PreinspectionsPharma.Remove(dataobject);
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
