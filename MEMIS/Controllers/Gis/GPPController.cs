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
    public class GPPController : Controller
    {
        private readonly Data.AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GPPController(Data.AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.GPP.Include(s => s.District).Include(r=>r.Region)
                .Skip(offset)
                .Take(pageSize);
            var result = new PagedResult<GPP>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.GPP.Count(),
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
            var appDbContext = _context.GPP.Include(s => s.District).Skip(offset).Take(pageSize); 
            //var appDbContext = _context.PreinspectionsPharma.Include(s => s.District).Skip(offset).Take(pageSize);
            var result = new PagedResult<GPP>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.GPP.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewBag.Users = _userManager;
            return View(result);
        }
         
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GPP == null)
            {
                return NotFound();
            }

            var enforcement = await _context.GPP
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
            ViewData["GPPRecommendation"] = ListHelper.GPPRecommendation();



            ViewBag.Users = _userManager;

            return View(new GPPDto { InspectionDate = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GPPDto gpp)
        {
            if (ModelState.IsValid)
            {
                GPP dataobject = new()
                {
                    InspectionDate = gpp.InspectionDate, 
                    GPS = gpp.GPS, 
                    intRegion= gpp.intRegion,
                    DistrictId = gpp.DistrictId,
                    FacilityName = gpp.FacilityName,
                    FacilityStatus = gpp.FacilityStatus,
                    FacilityPersonType = gpp.FacilityPersonType,
                    PersonName = gpp.PersonName,
                    Contact= gpp.Contact,
                    Qualifications= gpp.Qualifications,
                    CategoryOfpremises=gpp.CategoryOfpremises,
                    LicenseStatus= gpp.LicenseStatus,
                    CategoryStatus= gpp.CategoryStatus,
                    FacilityType= gpp.FacilityType,
                    certStatus= gpp.certStatus,
                    RecommendedforGPP=gpp.RecommendedforGPP,
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
            ViewData["GPPRecommendation"] = ListHelper.GPPRecommendation();
            return View(gpp);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GPP == null)
            {
                return NotFound();
            }

            var gpp = await _context.GPP.FindAsync(id);
            if (gpp == null)
            {
                return NotFound();
            }
            GPPDto gppdto = new GPPDto
            {
                InspectionDate = gpp.InspectionDate,
                GPS = gpp.GPS,
                FacilityName = gpp.FacilityName,
                FacilityStatus = gpp.FacilityStatus,
                FacilityPersonType = gpp.FacilityPersonType,
                PersonName = gpp.PersonName,
                Contact = gpp.Contact,
                Qualifications = gpp.Qualifications,
                CategoryOfpremises = gpp.CategoryOfpremises,
                LicenseStatus = gpp.LicenseStatus,
                CategoryStatus= gpp.CategoryStatus,
                FacilityType = gpp.FacilityType,
                certStatus = gpp.certStatus,
                RecommendedforGPP = gpp.RecommendedforGPP,
                intRegion = gpp.intRegion,
                DistrictId = gpp.DistrictId,
                Id = gpp.Id,
                InspectorId = gpp.InspectorId,
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
            ViewData["GPPRecommendation"] = ListHelper.GPPRecommendation();
            return View(gppdto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GPPDto objectdto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    GPP dataobject = new()
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
                        CategoryStatus= objectdto.CategoryStatus,
                        FacilityType = objectdto.FacilityType,
                        certStatus = objectdto.certStatus,
                        RecommendedforGPP = objectdto.RecommendedforGPP,
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
            ViewData["GPPRecommendation"] = ListHelper.GPPRecommendation();
            return View(objectdto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GPP == null)
            {
                return NotFound();
            }

            var dataobject = await _context.GPP.Include(x => x.District)
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
            if (_context.GPP == null)
            {
                return Problem("Entity set 'AppDbContext.GPP'  is null.");
            }
            var dataobject = await _context.GPP.FindAsync(id);
            if (dataobject != null)
            {
                _context.GPP.Remove(dataobject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SDTMasterExists(int id)
        {
            return (_context.GPP?.Any(e => e.Id == id)).GetValueOrDefault();
        }
         

    }
}
