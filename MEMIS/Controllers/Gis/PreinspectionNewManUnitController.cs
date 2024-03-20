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
    public class PreInspectionNewManUnitController : Controller
    {
        private readonly Data.AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PreInspectionNewManUnitController(Data.AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.PreinspectionNewManUnit.Include(s => s.District).Skip(offset).Take(pageSize);
            var result = new PagedResult<PreinspectionNewManUnit>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.PreinspectionNewManUnit.Count(),
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
            var appDbContext = _context.PreinspectionNewManUnit.Include(s => s.District).Skip(offset).Take(pageSize);
            var result = new PagedResult<PreinspectionNewManUnit>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.PreinspectionNewManUnit.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewBag.Users = _userManager;
            return View(result);
        }

        

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PreinspectionNewManUnit == null)
            {
                return NotFound();
            }

            var preinspection = await _context.PreinspectionNewManUnit
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
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewData["Category"] = ListHelper.CategoryofProducts();
            ViewBag.Users = _userManager;

            return View(new PreinspectionNewManUnitDto { InspectionDate = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PreinspectionNewManUnitDto preinspection)
        {
            if (ModelState.IsValid)
            {
                PreinspectionNewManUnit dataobject = new()
                {
                    InspectionDate = preinspection.InspectionDate,
                    Applicant = preinspection.Applicant,
                    Business = preinspection.Business,
                    Road = preinspection.Road,
                    Zone = preinspection.Zone,
                    Village = preinspection.Village,
                    Country = preinspection.Country,
                    Telephone = preinspection.Telephone,
                    Email = preinspection.Email,
                    GPS = preinspection.GPS,
                    ProductClassification = preinspection.ProductClassification, 
                    CategoryOfpremises = preinspection.CategoryOfpremises,
                    comments = preinspection.comments, 
                    DistrictId = preinspection.DistrictId,
                    //InspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier),

                };
                _context.Add(dataobject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewData["Category"] = ListHelper.CategoryofProducts();
            return View(preinspection);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PreinspectionNewManUnit == null)
            {
                return NotFound();
            }

            var preinspections = await _context.PreinspectionNewManUnit.FindAsync(id);
            if (preinspections == null)
            {
                return NotFound();
            }
            PreinspectionNewManUnit preinspection = new PreinspectionNewManUnit
            {
                InspectionDate = preinspections.InspectionDate,
                Applicant = preinspections.Applicant,
                Business = preinspections.Business,
                Road = preinspections.Road,
                Zone = preinspections.Zone,
                Village = preinspections.Village,
                Country = preinspections.Country,
                Telephone = preinspections.Telephone,
                Email = preinspections.Email,
                GPS = preinspections.GPS,
                ProductClassification = preinspections.ProductClassification, 
                CategoryOfpremises = preinspections.ProductClassification,
                comments = preinspections.comments,
                DistrictId = preinspections.DistrictId,
                Id = preinspections.Id,
                InspectorId = preinspections.InspectorId,
            };
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewData["Category"] = ListHelper.CategoryofProducts();
            return View(preinspection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PreinspectionNewManUnit objectdto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PreinspectionNewManUnit dataobject = new()
                    {
                        Id = objectdto.Id,
                        InspectionDate = objectdto.InspectionDate,
                        Applicant = objectdto.Applicant,
                        Business = objectdto.Business,
                        Road = objectdto.Road,
                        Zone = objectdto.Zone,
                        Village = objectdto.Village,
                        Country = objectdto.Country,
                        Telephone = objectdto.Telephone,
                        Email = objectdto.Email,
                        GPS = objectdto.GPS,
                        ProductClassification = objectdto.ProductClassification, 
                        CategoryOfpremises = objectdto.CategoryOfpremises,
                        comments = objectdto.comments,
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
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewData["Category"] = ListHelper.CategoryofProducts();
            return View(objectdto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null ||   _context.PreinspectionNewManUnit == null)
            {
                return NotFound();
            }

            var dataobject = await _context.PreinspectionNewManUnit.Include(x => x.District)
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
            var dataobject = await _context.PreinspectionNewManUnit.FindAsync(id);
            if (dataobject != null)
            {
                _context.PreinspectionNewManUnit.Remove(dataobject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SDTMasterExists(int id)
        {
            return (_context.PreinspectionNewManUnit?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
