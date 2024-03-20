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
    public class PreInspectionsController : Controller
    {
        private readonly Data.AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PreInspectionsController(Data.AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.Preinspections.Include(s => s.District).Skip(offset).Take(pageSize);
            var result = new PagedResult<Preinspection>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.Preinspections.Count(),
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
            var appDbContext = _context.Preinspections.Include(s => s.District).Skip(offset).Take(pageSize);
            var result = new PagedResult<Preinspection>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.Preinspections.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewBag.Users = _userManager;
            return View(result);
        }

        public async Task<IActionResult> VerifyStatus(int? id)
        {
            if (id == null || _context.Preinspections == null)
            {
                return NotFound();
            }

            var preinspections = await _context.Preinspections.Include(x => x.District).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (preinspections == null)
            {
                return NotFound();
            }
            PreinspectionDto preinspection = new PreinspectionDto
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
                Latitude = preinspections.Latitude,
                Longitude = preinspections.Longitude,
                ProductClassification = preinspections.ProductClassification,
                NearestPharmaName = preinspections.NearestPharmaName,
                NearestPharmaRoad = preinspections.NearestPharmaRoad,
                NearestPharmaDistance = preinspections.NearestPharmaDistance,
                NearestDrugShopName = preinspections.NearestDrugShopName,
                NearestDrugShopRoad = preinspections.NearestDrugShopRoad,
                NearestDrugShopDistance = preinspections.NearestDrugShopDistance,
                comments = preinspections.comments,
                ApprovalStatus = preinspections.ApprovalStatus,
                DistrictId = preinspections.DistrictId,
                Id = preinspections.Id,
                InspectorId = preinspections.InspectorId,
                //InspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier),

                DistrictName = preinspections.District.Name
            };
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(preinspection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyStatus(PreinspectionDto objectdto)
        {
            if (objectdto.Id != null && objectdto.Id != 0)
            {
                try
                {
                    var pp = await _context.Preinspections.FindAsync(objectdto.Id);
                    if (pp == null)
                    {
                        return NotFound();
                    }
                    pp.comments = objectdto.comments;
                    pp.ApprovalStatus = objectdto.ApprovalStatus;
                    pp.InspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
                return RedirectToAction(nameof(Verify));
            }
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(objectdto);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Preinspections == null)
            {
                return NotFound();
            }

            var preinspection = await _context.Preinspections
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
            ViewBag.Users = _userManager;

            return View(new PreinspectionDto { ApprovalStatus = 0, InspectionDate = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PreinspectionDto preinspection)
        {
            if (ModelState.IsValid)
            {
                Preinspection dataobject = new()
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
                    Latitude = preinspection.Latitude,
                    Longitude = preinspection.Longitude,
                    ProductClassification = preinspection.ProductClassification,
                    NearestPharmaName = preinspection.NearestPharmaName,
                    NearestPharmaRoad = preinspection.NearestPharmaRoad,
                    NearestPharmaDistance = preinspection.NearestPharmaDistance,
                    NearestDrugShopName = preinspection.NearestDrugShopName,
                    NearestDrugShopRoad = preinspection.NearestDrugShopRoad,
                    NearestDrugShopDistance = preinspection.NearestDrugShopDistance,
                    comments = preinspection.comments,
                    ApprovalStatus = preinspection.ApprovalStatus,
                    DistrictId = preinspection.DistrictId,
                    //InspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier),

                };
                _context.Add(dataobject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            return View(preinspection);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Preinspections == null)
            {
                return NotFound();
            }

            var preinspections = await _context.Preinspections.FindAsync(id);
            if (preinspections == null)
            {
                return NotFound();
            }
            PreinspectionDto preinspection = new PreinspectionDto
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
                Latitude = preinspections.Latitude,
                Longitude = preinspections.Longitude,
                ProductClassification = preinspections.ProductClassification,
                NearestPharmaName = preinspections.NearestPharmaName,
                NearestPharmaRoad = preinspections.NearestPharmaRoad,
                NearestPharmaDistance = preinspections.NearestPharmaDistance,
                NearestDrugShopName = preinspections.NearestDrugShopName,
                NearestDrugShopRoad = preinspections.NearestDrugShopRoad,
                NearestDrugShopDistance = preinspections.NearestDrugShopDistance,
                comments = preinspections.comments,
                ApprovalStatus = preinspections.ApprovalStatus,
                DistrictId = preinspections.DistrictId,
                Id = preinspections.Id,
                InspectorId = preinspections.InspectorId,
            };
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            return View(preinspection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PreinspectionDto objectdto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Preinspection dataobject = new()
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
                        Latitude = objectdto.Latitude,
                        Longitude = objectdto.Longitude,
                        ProductClassification = objectdto.ProductClassification,
                        NearestPharmaName = objectdto.NearestPharmaName,
                        NearestPharmaRoad = objectdto.NearestPharmaRoad,
                        NearestPharmaDistance = objectdto.NearestPharmaDistance,
                        NearestDrugShopName = objectdto.NearestDrugShopName,
                        NearestDrugShopRoad = objectdto.NearestDrugShopRoad,
                        NearestDrugShopDistance = objectdto.NearestDrugShopDistance,
                        comments = objectdto.comments,
                        ApprovalStatus = objectdto.ApprovalStatus,
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
            return View(objectdto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Preinspections == null)
            {
                return NotFound();
            }

            var dataobject = await _context.Preinspections.Include(x => x.District)
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
            var dataobject = await _context.Preinspections.FindAsync(id);
            if (dataobject != null)
            {
                _context.Preinspections.Remove(dataobject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SDTMasterExists(int id)
        {
            return (_context.Preinspections?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
