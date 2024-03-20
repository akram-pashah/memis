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
    public class PreInspectionsPharmaController : Controller
    {
        private readonly Data.AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PreInspectionsPharmaController(Data.AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.PreinspectionsPharma.Include(s => s.District).Skip(offset).Take(pageSize);
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

        public async Task<IActionResult> Verify(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.PreinspectionsPharma.Include(s => s.District).Skip(offset).Take(pageSize).Where(e=> e.ApprovalStatusInspector==0);
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

        public async Task<IActionResult> VerifyStatus(int? id)
        {
            if (id == null || _context.PreinspectionsPharma == null)
            {
                return NotFound();
            }

            var PreInspectionsPharma = await _context.PreinspectionsPharma.Include(x => x.District).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (PreInspectionsPharma == null)
            {
                return NotFound();
            }
            PreinspectionPharmaDto preinspection = new PreinspectionPharmaDto
            {
                InspectionDate = PreInspectionsPharma.InspectionDate,
                Applicant = PreInspectionsPharma.Applicant,
                Business = PreInspectionsPharma.Business,
                Road = PreInspectionsPharma.Road,
                Zone = PreInspectionsPharma.Zone,
                Village = PreInspectionsPharma.Village,
                Country = PreInspectionsPharma.Country,
                Telephone = PreInspectionsPharma.Telephone,
                Email = PreInspectionsPharma.Email,
                GPS = PreInspectionsPharma.GPS,
                ProductClassification = PreInspectionsPharma.ProductClassification,
                NearestPharmaName = PreInspectionsPharma.NearestPharmaName,
                NearestPharmaRoad = PreInspectionsPharma.NearestPharmaRoad,
                NearestPharmaDistance = PreInspectionsPharma.NearestPharmaDistance,
                comments = PreInspectionsPharma.comments,
                ApprovalStatusInspector = PreInspectionsPharma.ApprovalStatusInspector,
                DistrictId = PreInspectionsPharma.DistrictId,
                Id = PreInspectionsPharma.Id,
                InspectorId = PreInspectionsPharma.InspectorId,
                //InspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier),

                //DistrictName = PreInspectionsPharma.District.Name
            };
            ViewData["Category"] = ListHelper.CategoryofPremises();
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(preinspection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> VerifyStatus(PreinspectionPharmaDto objectdto)
        {
            if (objectdto.Id != null && objectdto.Id != 0) 
            {
                try
                {
                    var pp = await _context.PreinspectionsPharma.FindAsync(objectdto.Id);
                    if (pp == null)
                    {
                        return NotFound();
                    }
                    pp.Inspectorcomments = objectdto.Inspectorcomments;
                    pp.ApprovalStatusInspector = objectdto.ApprovalStatusInspector;
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
            ViewData["Category"] = ListHelper.CategoryofPremises();
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(objectdto);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PreinspectionsPharma == null)
            {
                return NotFound();
            }

            var preinspection = await _context.PreinspectionsPharma
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
            ViewData["Category"] = ListHelper.CategoryofPremises();
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewBag.Users = _userManager;

            return View(new PreinspectionPharmaDto { ApprovalStatusInspector = 0, ApprovalStatusHead = 0, ApprovalStatusDir = 0, InspectionDate = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PreinspectionPharmaDto preinspection)
        {
            if (ModelState.IsValid)
            {
                PreinspectionPharma dataobject = new()
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
                    NearestPharmaName = preinspection.NearestPharmaName,
                    NearestPharmaRoad = preinspection.NearestPharmaRoad,
                    NearestPharmaDistance = preinspection.NearestPharmaDistance,
                    comments = preinspection.comments,
                    ApprovalStatusInspector = preinspection.ApprovalStatusInspector,
                    DistrictId = preinspection.DistrictId,
                    //InspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier),

                };
                _context.Add(dataobject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Category"] = ListHelper.CategoryofPremises();
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            return View(preinspection);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PreinspectionsPharma == null)
            {
                return NotFound();
            }

            var PreInspectionsPharma = await _context.PreinspectionsPharma.FindAsync(id);
            if (PreInspectionsPharma == null)
            {
                return NotFound();
            }
            PreinspectionPharmaDto preinspection = new PreinspectionPharmaDto
            {
                InspectionDate = PreInspectionsPharma.InspectionDate,
                Applicant = PreInspectionsPharma.Applicant,
                Business = PreInspectionsPharma.Business,
                Road = PreInspectionsPharma.Road,
                Zone = PreInspectionsPharma.Zone,
                Village = PreInspectionsPharma.Village,
                Country = PreInspectionsPharma.Country,
                Telephone = PreInspectionsPharma.Telephone,
                Email = PreInspectionsPharma.Email,
                GPS = PreInspectionsPharma.GPS,
                ProductClassification = PreInspectionsPharma.ProductClassification,
                NearestPharmaName = PreInspectionsPharma.NearestPharmaName,
                NearestPharmaRoad = PreInspectionsPharma.NearestPharmaRoad,
                NearestPharmaDistance = PreInspectionsPharma.NearestPharmaDistance,
                comments = PreInspectionsPharma.comments,
                ApprovalStatusInspector = PreInspectionsPharma.ApprovalStatusInspector,
                ApprovalStatusDir = PreInspectionsPharma.ApprovalStatusDir,
                ApprovalStatusHead = PreInspectionsPharma.ApprovalStatusHead,
                DistrictId = PreInspectionsPharma.DistrictId,
                Id = PreInspectionsPharma.Id,
                InspectorId = PreInspectionsPharma.InspectorId,
            };
            ViewData["Category"] = ListHelper.CategoryofPremises();
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            return View(preinspection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PreinspectionPharmaDto objectdto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PreinspectionPharma dataobject = new()
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
                        NearestPharmaName = objectdto.NearestPharmaName,
                        NearestPharmaRoad = objectdto.NearestPharmaRoad,
                        NearestPharmaDistance = objectdto.NearestPharmaDistance,
                        comments = objectdto.comments,
                        ApprovalStatusInspector = objectdto.ApprovalStatusInspector,
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

        public async Task<IActionResult> HeadVerify(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.PreinspectionsPharma.Include(s => s.District).Skip(offset).Take(pageSize).Where(e=> e.ApprovalStatusHead== 0 && e.ApprovalStatusInspector==1);
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

        public async Task<IActionResult> HeadVerifyStatus(int? id)
        {
            if (id == null || _context.PreinspectionsPharma == null)
            {
                return NotFound();
            }

            var PreInspectionsPharma = await _context.PreinspectionsPharma.Include(x => x.District).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (PreInspectionsPharma == null)
            {
                return NotFound();
            }
            PreinspectionPharmaDto preinspection = new PreinspectionPharmaDto
            {
                InspectionDate = PreInspectionsPharma.InspectionDate,
                Applicant = PreInspectionsPharma.Applicant,
                Business = PreInspectionsPharma.Business,
                Road = PreInspectionsPharma.Road,
                Zone = PreInspectionsPharma.Zone,
                Village = PreInspectionsPharma.Village,
                Country = PreInspectionsPharma.Country,
                Telephone = PreInspectionsPharma.Telephone,
                Email = PreInspectionsPharma.Email,
                GPS = PreInspectionsPharma.GPS,
                ProductClassification = PreInspectionsPharma.ProductClassification,
                NearestPharmaName = PreInspectionsPharma.NearestPharmaName,
                NearestPharmaRoad = PreInspectionsPharma.NearestPharmaRoad,
                NearestPharmaDistance = PreInspectionsPharma.NearestPharmaDistance,
                comments = PreInspectionsPharma.comments,
                ApprovalStatusInspector = PreInspectionsPharma.ApprovalStatusInspector,
                DistrictId = PreInspectionsPharma.DistrictId,
                Id = PreInspectionsPharma.Id,
                //InspectorId = PreInspectionsPharma.InspectorId,
                HeadInspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier),

                //DistrictName = PreInspectionsPharma.District.Name
            };
            ViewData["Category"] = ListHelper.CategoryofPremises();
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(preinspection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HeadVerifyStatus(PreinspectionPharmaDto objectdto)
        {
            if (objectdto.Id != null && objectdto.Id != 0)
            {
                try
                {
                    var pp = await _context.PreinspectionsPharma.FindAsync(objectdto.Id);
                    if (pp == null)
                    {
                        return NotFound();
                    }
                    pp.HeadInspectorcomments = objectdto.HeadInspectorcomments;
                    pp.HeadInspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    pp.ApprovalStatusHead = objectdto.ApprovalStatusHead;
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
            ViewData["Category"] = ListHelper.CategoryofPremises();
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(objectdto);
        }


        public async Task<IActionResult>DirVerify(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.PreinspectionsPharma.Include(s => s.District).Skip(offset).Take(pageSize).Where(e => e.ApprovalStatusHead == 1 && e.ApprovalStatusInspector == 1 && e.ApprovalStatusDir==0);
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

        public async Task<IActionResult> DirVerifyStatus(int? id)
        {
            if (id == null || _context.PreinspectionsPharma == null)
            {
                return NotFound();
            }

            var PreInspectionsPharma = await _context.PreinspectionsPharma.Include(x => x.District).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (PreInspectionsPharma == null)
            {
                return NotFound();
            }
            PreinspectionPharmaDto preinspection = new PreinspectionPharmaDto
            {
                InspectionDate = PreInspectionsPharma.InspectionDate,
                Applicant = PreInspectionsPharma.Applicant,
                Business = PreInspectionsPharma.Business,
                Road = PreInspectionsPharma.Road,
                Zone = PreInspectionsPharma.Zone,
                Village = PreInspectionsPharma.Village,
                Country = PreInspectionsPharma.Country,
                Telephone = PreInspectionsPharma.Telephone,
                Email = PreInspectionsPharma.Email,
                GPS = PreInspectionsPharma.GPS,
                ProductClassification = PreInspectionsPharma.ProductClassification,
                NearestPharmaName = PreInspectionsPharma.NearestPharmaName,
                NearestPharmaRoad = PreInspectionsPharma.NearestPharmaRoad,
                NearestPharmaDistance = PreInspectionsPharma.NearestPharmaDistance,
                comments = PreInspectionsPharma.comments,
                ApprovalStatusInspector = PreInspectionsPharma.ApprovalStatusInspector,
                DistrictId = PreInspectionsPharma.DistrictId,
                Id = PreInspectionsPharma.Id,
                //InspectorId = PreInspectionsPharma.InspectorId,
                DirectorInspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier),

                //DistrictName = PreInspectionsPharma.District.Name
            };
            ViewData["Category"] = ListHelper.CategoryofPremises();
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(preinspection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DirVerifyStatus(PreinspectionPharmaDto objectdto)
        {
            if (objectdto.Id != null && objectdto.Id != 0)
            {
                try
                {
                    var pp = await _context.PreinspectionsPharma.FindAsync(objectdto.Id);
                    if (pp == null)
                    {
                        return NotFound();
                    }
                    pp.DirectorInspectorcomments = objectdto.DirectorInspectorcomments;
                    pp.DirectorInspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    pp.ApprovalStatusDir = objectdto.ApprovalStatusDir;

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
            ViewData["Category"] = ListHelper.CategoryofPremises();
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(objectdto);
        }

    }
}
