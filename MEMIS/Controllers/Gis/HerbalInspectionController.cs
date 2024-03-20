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
    public class HerbalInspectionController : Controller
    {
        private readonly Data.AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HerbalInspectionController(Data.AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.HerbalInspection.Include(s => s.District).Include(r=>r.Region)
                .Skip(offset)
                .Take(pageSize);
            var result = new PagedResult<HerbalInspection>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.HerbalInspection.Count(),
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
            var appDbContext = _context.HerbalInspection.Include(s => s.District).Skip(offset).Take(pageSize); 
            //var appDbContext = _context.PreinspectionsPharma.Include(s => s.District).Skip(offset).Take(pageSize);
            var result = new PagedResult<HerbalInspection>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.HerbalInspection.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewBag.Users = _userManager;
            return View(result);
        }
         
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HerbalInspection == null)
            {
                return NotFound();
            }

            var enforcement = await _context.HerbalInspection
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

            return View(new HerbalInspectionDto { InspectionDate = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HerbalInspectionDto herbalinspection)
        {
            if (ModelState.IsValid)
            {
                HerbalInspection dataobject = new()
                {
                    InspectionDate = herbalinspection.InspectionDate, 
                    GPS = herbalinspection.GPS, 
                    intRegion= herbalinspection.intRegion,
                    DistrictId = herbalinspection.DistrictId,
                    FacilityName = herbalinspection.FacilityName,
                    FacilityStatus = herbalinspection.FacilityStatus,
                    FacilityPersonType = herbalinspection.FacilityPersonType,
                    PersonName = herbalinspection.PersonName,
                    Contact= herbalinspection.Contact,
                    Qualifications= herbalinspection.Qualifications,
                    CategoryOfpremises=herbalinspection.CategoryOfpremises,
                    LicenseStatus= herbalinspection.LicenseStatus, 
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
            return View(herbalinspection);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HerbalInspection == null)
            {
                return NotFound();
            }

            var herbalinspection = await _context.HerbalInspection.FindAsync(id);
            if (herbalinspection == null)
            {
                return NotFound();
            }
            HerbalInspectionDto herbalinspactiondto = new HerbalInspectionDto
            {
                InspectionDate = herbalinspection.InspectionDate,
                GPS = herbalinspection.GPS,
                FacilityName = herbalinspection.FacilityName,
                FacilityStatus = herbalinspection.FacilityStatus,
                FacilityPersonType = herbalinspection.FacilityPersonType,
                PersonName = herbalinspection.PersonName,
                Contact = herbalinspection.Contact,
                Qualifications = herbalinspection.Qualifications,
                CategoryOfpremises = herbalinspection.CategoryOfpremises,
                LicenseStatus = herbalinspection.LicenseStatus, 
                intRegion = herbalinspection.intRegion,
                DistrictId = herbalinspection.DistrictId,
                Id = herbalinspection.Id,
                InspectorId = herbalinspection.InspectorId,
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
            return View(herbalinspactiondto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HerbalInspectionDto objectdto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HerbalInspection dataobject = new()
                    {
                        Id = objectdto.Id,
                        InspectionDate = objectdto.InspectionDate, 
                        GPS = objectdto.GPS, 
                        FacilityName= objectdto.FacilityName,
                        FacilityStatus= objectdto.FacilityStatus,
                        FacilityPersonType= objectdto.FacilityPersonType,
                        PersonName= objectdto.PersonName,
                        Contact=    objectdto.Contact,
                        Qualifications= objectdto.Qualifications,   
                        CategoryOfpremises= objectdto.CategoryOfpremises,
                        LicenseStatus= objectdto.LicenseStatus, 
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
            if (id == null || _context.HerbalInspection == null)
            {
                return NotFound();
            }

            var dataobject = await _context.HerbalInspection.Include(x => x.District)
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
            if (_context.HerbalInspection == null)
            {
                return Problem("Entity set 'AppDbContext.HerbalInspection'  is null.");
            }
            var dataobject = await _context.HerbalInspection.FindAsync(id);
            if (dataobject != null)
            {
                _context.HerbalInspection.Remove(dataobject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SDTMasterExists(int id)
        {
            return (_context.HerbalInspection?.Any(e => e.Id == id)).GetValueOrDefault();
        }
         

    }
}
