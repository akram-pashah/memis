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
    public class RelocationPharmaController : Controller
    {
        private readonly Data.AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RelocationPharmaController(Data.AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.RelocationPharma.Include(s => s.District).Skip(offset).Take(pageSize);
            var result = new PagedResult<RelocationPharma>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.RelocationPharma.Count(),
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
            var appDbContext = _context.RelocationPharma.Include(s => s.District).Skip(offset).Take(pageSize).Where(e=> e.ApprovalStatusInspector==0);
            //var appDbContext = _context.RelocationPharma.Include(s => s.District).Skip(offset).Take(pageSize);
            var result = new PagedResult<RelocationPharma>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.RelocationPharma.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewBag.Users = _userManager;
            return View(result);
        }

        public async Task<IActionResult> VerifyStatus(int? id)
        {
            if (id == null || _context.RelocationPharma == null)
            {
                return NotFound();
            }

            var RelocationPhrama = await _context.RelocationPharma.Include(x => x.District).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (RelocationPhrama == null)
            {
                return NotFound();
            }
            RelocationPhramaDto preinspection = new RelocationPhramaDto
            {
                InspectionDate = RelocationPhrama.InspectionDate,
                Applicant = RelocationPhrama.Applicant,
                Business = RelocationPhrama.Business,
                CRoad = RelocationPhrama.CRoad,
                CZone = RelocationPhrama.PZone,
                CVillage = RelocationPhrama.CVillage,
                CCountry = RelocationPhrama.CCountry,
                CTelephone = RelocationPhrama.CTelephone,
                CEmail = RelocationPhrama.CEmail,
                CGPS = RelocationPhrama.CGPS,
                PRoad = RelocationPhrama.PRoad,
                PZone = RelocationPhrama.PZone,
                PVillage = RelocationPhrama.PVillage,
                PCountry = RelocationPhrama.PCountry,
                PTelephone = RelocationPhrama.PTelephone,
                PEmail = RelocationPhrama.PEmail,
                PGPS = RelocationPhrama.PGPS,
                ProductClassification = RelocationPhrama.ProductClassification, 
                comments = RelocationPhrama.comments,
                ApprovalStatusInspector = RelocationPhrama.ApprovalStatusInspector,
                DistrictId = RelocationPhrama.DistrictId,
                Id = RelocationPhrama.Id,
                InspectorId = RelocationPhrama.InspectorId,
                //InspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier),

                //DistrictName = RelocationPhrama.District.Name
            };
            ViewData["Category"] = ListHelper.CategoryofPremises();
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(preinspection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> VerifyStatus(RelocationPhramaDto objectdto)
        {
            if (objectdto.Id != null && objectdto.Id != 0) 
            {
                try
                {
                    var pp = await _context.RelocationPharma.FindAsync(objectdto.Id);
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
            if (id == null || _context.RelocationPharma == null)
            {
                return NotFound();
            }

            var preinspection = await _context.RelocationPharma
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

            return View(new RelocationPhramaDto { ApprovalStatusInspector = 0, ApprovalStatusHead = 0, ApprovalStatusDir = 0, InspectionDate = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RelocationPhramaDto preinspection)
        {
            if (ModelState.IsValid)
            {
                RelocationPharma dataobject = new()
                {
                    InspectionDate = preinspection.InspectionDate,
                    Applicant = preinspection.Applicant,
                    Business = preinspection.Business,
                    CRoad = preinspection.CRoad,
                    CZone = preinspection.CZone,
                    CVillage = preinspection.CVillage,
                    CCountry = preinspection.CCountry,
                    CTelephone = preinspection.CTelephone,
                    CEmail = preinspection.CEmail,
                    CGPS = preinspection.CGPS,
                    PRoad = preinspection.PRoad,
                    PZone = preinspection.PZone,
                    PVillage = preinspection.PVillage,
                    PCountry = preinspection.PCountry,
                    PTelephone = preinspection.PTelephone,
                    PEmail = preinspection.PEmail,
                    PGPS = preinspection.PGPS,
                    ProductClassification = preinspection.ProductClassification, 
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
            if (id == null || _context.RelocationPharma == null)
            {
                return NotFound();
            }

            var RelocationPhrama = await _context.RelocationPharma.FindAsync(id);
            if (RelocationPhrama == null)
            {
                return NotFound();
            }
            RelocationPhramaDto preinspection = new RelocationPhramaDto
            {
                InspectionDate = RelocationPhrama.InspectionDate,
                Applicant = RelocationPhrama.Applicant,
                Business = RelocationPhrama.Business,
                CRoad = RelocationPhrama.CRoad,
                CZone = RelocationPhrama.CZone,
                CVillage = RelocationPhrama.CVillage,
                CCountry = RelocationPhrama.CCountry,
                CTelephone = RelocationPhrama.CTelephone,
                CEmail = RelocationPhrama.CEmail,
                CGPS = RelocationPhrama.CGPS,
                PRoad = RelocationPhrama.PRoad,
                PZone = RelocationPhrama.PZone,
                PVillage = RelocationPhrama.PVillage,
                PCountry = RelocationPhrama.PCountry,
                PTelephone = RelocationPhrama.PTelephone,
                PEmail = RelocationPhrama.PEmail,
                PGPS = RelocationPhrama.PGPS,
                ProductClassification = RelocationPhrama.ProductClassification,
                
                comments = RelocationPhrama.comments,
                ApprovalStatusInspector = RelocationPhrama.ApprovalStatusInspector,
                ApprovalStatusDir = RelocationPhrama.ApprovalStatusDir,
                ApprovalStatusHead = RelocationPhrama.ApprovalStatusHead,
                DistrictId = RelocationPhrama.DistrictId,
                Id = RelocationPhrama.Id,
                InspectorId = RelocationPhrama.InspectorId,
            };
            ViewData["Category"] = ListHelper.CategoryofPremises();
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            return View(preinspection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RelocationPhramaDto objectdto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    RelocationPharma dataobject = new()
                    {
                        Id = objectdto.Id,
                        InspectionDate = objectdto.InspectionDate,
                        Applicant = objectdto.Applicant,
                        Business = objectdto.Business,
                        CRoad = objectdto.CRoad,
                        CZone = objectdto.CZone,
                        CVillage = objectdto.CVillage,
                        CCountry = objectdto.CCountry,
                        CTelephone = objectdto.CTelephone,
                        CEmail = objectdto.CEmail,
                        CGPS = objectdto.CGPS,
                        PRoad = objectdto.PRoad,
                        PZone = objectdto.PZone,
                        PVillage = objectdto.PVillage,
                        PCountry = objectdto.PCountry,
                        PTelephone = objectdto.PTelephone,
                        PEmail = objectdto.PEmail,
                        PGPS = objectdto.PGPS,
                        ProductClassification = objectdto.ProductClassification,                         
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
            if (id == null || _context.RelocationPharma == null)
            {
                return NotFound();
            }

            var dataobject = await _context.RelocationPharma.Include(x => x.District)
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
            var dataobject = await _context.RelocationPharma.FindAsync(id);
            if (dataobject != null)
            {
                _context.RelocationPharma.Remove(dataobject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SDTMasterExists(int id)
        {
            return (_context.RelocationPharma?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> HeadVerify(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.RelocationPharma.Include(s => s.District).Skip(offset).Take(pageSize).Where(e=> e.ApprovalStatusHead== 0 && e.ApprovalStatusInspector==1);
            //var appDbContext = _context.RelocationPharma.Include(s => s.District).Skip(offset).Take(pageSize);

            var result = new PagedResult<RelocationPharma>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.RelocationPharma.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewBag.Users = _userManager;
            return View(result);
        }

        public async Task<IActionResult> HeadVerifyStatus(int? id)
        {
            if (id == null || _context.RelocationPharma == null)
            {
                return NotFound();
            }

            var RelocationPhrama = await _context.RelocationPharma.Include(x => x.District).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (RelocationPhrama == null)
            {
                return NotFound();
            }
            RelocationPhramaDto preinspection = new RelocationPhramaDto
            {
                InspectionDate = RelocationPhrama.InspectionDate,
                Applicant = RelocationPhrama.Applicant,
                Business = RelocationPhrama.Business,
                CRoad = RelocationPhrama.CRoad,
                CZone = RelocationPhrama.CZone,
                CVillage = RelocationPhrama.CVillage,
                CCountry = RelocationPhrama.CCountry,
                CTelephone = RelocationPhrama.CTelephone,
                CEmail = RelocationPhrama.CEmail,
                CGPS = RelocationPhrama.CGPS,
                PRoad = RelocationPhrama.PRoad,
                PZone = RelocationPhrama.PZone,
                PVillage = RelocationPhrama.PVillage,
                PCountry = RelocationPhrama.PCountry,
                PTelephone = RelocationPhrama.PTelephone,
                PEmail = RelocationPhrama.PEmail,
                PGPS = RelocationPhrama.PGPS,
                ProductClassification = RelocationPhrama.ProductClassification, 
                comments = RelocationPhrama.comments,
                ApprovalStatusInspector = RelocationPhrama.ApprovalStatusInspector,
                DistrictId = RelocationPhrama.DistrictId,
                Id = RelocationPhrama.Id,
                //InspectorId = RelocationPhrama.InspectorId,
                HeadInspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier),

                //DistrictName = RelocationPhrama.District.Name
            };
            ViewData["Category"] = ListHelper.CategoryofPremises();
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(preinspection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HeadVerifyStatus(RelocationPhramaDto objectdto)
        {
            if (objectdto.Id != null && objectdto.Id != 0)
            {
                try
                {
                    var pp = await _context.RelocationPharma.FindAsync(objectdto.Id);
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
            var appDbContext = _context.RelocationPharma.Include(s => s.District).Skip(offset).Take(pageSize).Where(e => e.ApprovalStatusHead == 1 && e.ApprovalStatusInspector == 1 && e.ApprovalStatusDir==0);
            //var appDbContext = _context.RelocationPharma.Include(s => s.District).Skip(offset).Take(pageSize);

            var result = new PagedResult<RelocationPharma>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.RelocationPharma.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewBag.Users = _userManager;
            return View(result);
        }

        public async Task<IActionResult> DirVerifyStatus(int? id)
        {
            if (id == null || _context.RelocationPharma == null)
            {
                return NotFound();
            }

            var RelocationPhrama = await _context.RelocationPharma.Include(x => x.District).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (RelocationPhrama == null)
            {
                return NotFound();
            }
            RelocationPhramaDto preinspection = new RelocationPhramaDto
            {
                InspectionDate = RelocationPhrama.InspectionDate,
                Applicant = RelocationPhrama.Applicant,
                Business = RelocationPhrama.Business,
                CRoad = RelocationPhrama.CRoad,
                CZone = RelocationPhrama.CZone,
                CVillage = RelocationPhrama.CVillage,
                CCountry = RelocationPhrama.CCountry,
                CTelephone = RelocationPhrama.CTelephone,
                CEmail = RelocationPhrama.CEmail,
                CGPS = RelocationPhrama.CGPS,
                PRoad = RelocationPhrama.PRoad,
                PZone = RelocationPhrama.PZone,
                PVillage = RelocationPhrama.PVillage,
                PCountry = RelocationPhrama.PCountry,
                PTelephone = RelocationPhrama.PTelephone,
                PEmail = RelocationPhrama.PEmail,
                PGPS = RelocationPhrama.PGPS,
                ProductClassification = RelocationPhrama.ProductClassification, 
                comments = RelocationPhrama.comments,
                ApprovalStatusInspector = RelocationPhrama.ApprovalStatusInspector,
                DistrictId = RelocationPhrama.DistrictId,
                Id = RelocationPhrama.Id,
                //InspectorId = RelocationPhrama.InspectorId,
                DirectorInspectorId = User.FindFirstValue(ClaimTypes.NameIdentifier),

                //DistrictName = RelocationPhrama.District.Name
            };
            ViewData["Category"] = ListHelper.CategoryofPremises();
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ProductClassification"] = ListHelper.ProductClassification();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(preinspection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DirVerifyStatus(RelocationPhramaDto objectdto)
        {
            if (objectdto.Id != null && objectdto.Id != 0)
            {
                try
                {
                    var pp = await _context.RelocationPharma.FindAsync(objectdto.Id);
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
