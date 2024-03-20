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
    public class RenewalController : Controller
    {
        private readonly Data.AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RenewalController(Data.AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.Renewal.Include(s => s.District).Include(r=>r.Region)
                .Skip(offset)
                .Take(pageSize);
            var result = new PagedResult<Renewal>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.Renewal.Count(),
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
            var appDbContext = _context.Renewal.Include(s => s.District).Skip(offset).Take(pageSize); 
            //var appDbContext = _context.PreinspectionsPharma.Include(s => s.District).Skip(offset).Take(pageSize);
            var result = new PagedResult<Renewal>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.Renewal.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewBag.Users = _userManager;
            return View(result);
        }
         
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Renewal == null)
            {
                return NotFound();
            }

            var renewal = await _context.Renewal
                .Include(s => s.District)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (renewal == null)
            {
                return NotFound();
            }
            ViewBag.Users = _userManager;
            
            return View(renewal);
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
            ViewData["LicenseRecommendation"] = ListHelper.LicenseRecommendation();



            ViewBag.Users = _userManager;

            return View(new RenewalDto { InspectionDate = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RenewalDto renewal)
        {
            if (ModelState.IsValid)
            {
                Renewal dataobject = new()
                {
                    InspectionDate = renewal.InspectionDate, 
                    GPS = renewal.GPS, 
                    intRegion= renewal.intRegion,
                    DistrictId = renewal.DistrictId,
                    FacilityName = renewal.FacilityName,
                    FacilityStatus = renewal.FacilityStatus,
                    FacilityPersonType = renewal.FacilityPersonType,
                    PersonName = renewal.PersonName,
                    Contact= renewal.Contact,
                    Qualifications= renewal.Qualifications,
                    CategoryOfpremises=renewal.CategoryOfpremises,
                    LicenseStatus= renewal.LicenseStatus,
                    CategoryStatus= renewal.CategoryStatus,
                    PremisesCondition= renewal.PremisesCondition,
                    RecordKeeping= renewal.RecordKeeping, 
                    LicAction= renewal.LicAction,
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
            ViewData["LicenseRecommendation"] = ListHelper.LicenseRecommendation();
            return View(renewal);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Renewal == null)
            {
                return NotFound();
            }

            var renewal = await _context.Renewal.FindAsync(id);
            if (renewal == null)
            {
                return NotFound();
            }
            RenewalDto renewaldto = new RenewalDto
            {
                InspectionDate = renewal.InspectionDate,
                GPS = renewal.GPS,
                FacilityName = renewal.FacilityName,
                FacilityStatus = renewal.FacilityStatus,
                FacilityPersonType = renewal.FacilityPersonType,
                PersonName = renewal.PersonName,
                Contact = renewal.Contact,
                Qualifications = renewal.Qualifications,
                CategoryOfpremises = renewal.CategoryOfpremises,
                LicenseStatus = renewal.LicenseStatus,
                CategoryStatus= renewal.CategoryStatus, 
                PremisesCondition = renewal.PremisesCondition,
                RecordKeeping = renewal.RecordKeeping, 
                LicAction= renewal.LicAction,
                intRegion= renewal.intRegion,
                DistrictId = renewal.DistrictId,
                Id = renewal.Id,
                InspectorId = renewal.InspectorId,
            };
            ViewData["intRegion"] = new SelectList(_context.Region, "intRegion", "regionName");
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["FacilityStatus"] = ListHelper.FacilityStatus();
            ViewData["PersonFoundatFacility"] = ListHelper.PersonFoundatFacility();
            ViewData["Category"] = ListHelper.CategoryofPremises();
            ViewData["LicenseStatus"] = ListHelper.LicenseStatus();
            ViewData["CategoryofDrugs"] = ListHelper.CategoryofDrugs();
            ViewData["ConditionofPremises"] = ListHelper.ConditionofPremises();
            ViewData["RecordKeeping"] = ListHelper.RecordKeeping();
            ViewData["LicenseRecommendation"] = ListHelper.LicenseRecommendation();
            return View(renewaldto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RenewalDto objectdto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Renewal dataobject = new()
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
                        PremisesCondition=  objectdto.PremisesCondition,
                        RecordKeeping= objectdto.RecordKeeping, 
                        LicAction= objectdto.LicAction,
                        intRegion= objectdto.intRegion,
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
            ViewData["ConditionofPremises"] = ListHelper.ConditionofPremises();
            ViewData["RecordKeeping"] = ListHelper.RecordKeeping();
            ViewData["LicenseRecommendation"] = ListHelper.LicenseRecommendation();
            return View(objectdto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Renewal == null)
            {
                return NotFound();
            }

            var dataobject = await _context.Renewal.Include(x => x.District)
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
            if (_context.Renewal == null)
            {
                return Problem("Entity set 'AppDbContext.Renewal'  is null.");
            }
            var dataobject = await _context.Renewal.FindAsync(id);
            if (dataobject != null)
            {
                _context.Renewal.Remove(dataobject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SDTMasterExists(int id)
        {
            return (_context.Renewal?.Any(e => e.Id == id)).GetValueOrDefault();
        }
         

    }
}
