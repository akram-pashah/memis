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
    public class EnforcementController : Controller
    {
        private readonly Data.AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EnforcementController(Data.AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.Enforcement.Include(s => s.District).Include(r=>r.Region)
                .Skip(offset)
                .Take(pageSize);
            var result = new PagedResult<Enforcement>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.Enforcement.Count(),
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
            var appDbContext = _context.Enforcement.Include(s => s.District).Skip(offset).Take(pageSize); 
            //var appDbContext = _context.PreinspectionsPharma.Include(s => s.District).Skip(offset).Take(pageSize);
            var result = new PagedResult<Enforcement>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.Enforcement.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewBag.Users = _userManager;
            return View(result);
        }
         
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Enforcement == null)
            {
                return NotFound();
            }

            var enforcement = await _context.Enforcement
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
            ViewData["ConditionofPremises"] = ListHelper.ConditionofPremises();
            ViewData["RecordKeeping"] = ListHelper.RecordKeeping(); 
            ViewData["EnforcementAction"] = ListHelper.EnforcementAction();



            ViewBag.Users = _userManager;

            return View(new EnforcementDto { InspectionDate = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnforcementDto enforcement)
        {
            if (ModelState.IsValid)
            {
                Enforcement dataobject = new()
                {
                    InspectionDate = enforcement.InspectionDate, 
                    GPS = enforcement.GPS, 
                    intRegion= enforcement.intRegion,
                    DistrictId = enforcement.DistrictId,
                    FacilityName = enforcement.FacilityName,
                    FacilityStatus = enforcement.FacilityStatus,
                    FacilityPersonType = enforcement.FacilityPersonType,
                    PersonName = enforcement.PersonName,
                    Contact= enforcement.Contact,
                    Qualifications= enforcement.Qualifications,
                    CategoryOfpremises=enforcement.CategoryOfpremises,
                    LicenseStatus= enforcement.LicenseStatus,
                    CategoryStatus= enforcement.CategoryStatus,
                    EnfAction= enforcement.EnfAction,
                    Comments= enforcement.Comments,  
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
            ViewData["EnforcementAction"] = ListHelper.EnforcementAction();
            return View(enforcement);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Enforcement == null)
            {
                return NotFound();
            }

            var renewal = await _context.Enforcement.FindAsync(id);
            if (renewal == null)
            {
                return NotFound();
            }
            EnforcementDto renewaldto = new EnforcementDto
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
                EnfAction = renewal.EnfAction,
                Comments = renewal.Comments,  
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
            ViewData["EnforcementAction"] = ListHelper.EnforcementAction();
            return View(renewaldto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EnforcementDto objectdto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Enforcement dataobject = new()
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
                        EnfAction=  objectdto.EnfAction,
                        Comments= objectdto.Comments,  
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
            ViewData["EnforcementAction"] = ListHelper.EnforcementAction();
            return View(objectdto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Enforcement == null)
            {
                return NotFound();
            }

            var dataobject = await _context.Enforcement.Include(x => x.District)
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
            if (_context.Enforcement == null)
            {
                return Problem("Entity set 'AppDbContext.Enforcement'  is null.");
            }
            var dataobject = await _context.Enforcement.FindAsync(id);
            if (dataobject != null)
            {
                _context.Enforcement.Remove(dataobject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SDTMasterExists(int id)
        {
            return (_context.Enforcement?.Any(e => e.Id == id)).GetValueOrDefault();
        }
         

    }
}
