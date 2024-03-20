using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;
using cloudscribe.Pagination.Models;
using MEMIS.Models;

namespace MEMIS.Controllers
{
    public class SDTMastersController : Controller
    {
        private readonly Data.AppDbContext _context;

        public SDTMastersController(Data.AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.SDTMasters.Include(s => s.DepartmentFk).Skip(offset).Take(pageSize);
            var result = new PagedResult<SDTMaster>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.SDTMasters.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewData["Departments"] = new SelectList(_context.Departments.OrderBy(d => d.deptName), "deptCode", "deptName");
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Index(int pageNumber = 1, string? deptCode = null,string? sdtname=null)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.SDTMasters.Include(s => s.DepartmentFk).Where(s=> (deptCode != null ? s.DepartmentFk.deptCode == deptCode : true) && (sdtname!=null?EF.Functions.Like(s.ServiceDeliveryTimeline,'%'+sdtname+'%'):true)).Skip(offset).Take(pageSize);
            var result = new PagedResult<SDTMaster>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.SDTMasters.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewData["Departments"] = new SelectList(_context.Departments.OrderBy(d => d.deptName), "deptCode", "deptName");
            return View(result);
            //if (id == null || _context.SDTMasters == null)
            //{
            //    return NotFound();
            //}

            //var sDTMaster = await _context.SDTMasters
            //    .Include(s => s.DepartmentFk)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (sDTMaster == null)
            //{
            //    return NotFound();
            //}

            //return View(sDTMaster);
        }
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.SDTMasters == null)
            {
                return NotFound();
            }

            var sDTMaster = await _context.SDTMasters
                .Include(s => s.DepartmentFk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sDTMaster == null)
            {
                return NotFound();
            }

            return View(sDTMaster);
        }

        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "intDept", "deptName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SDTMastersCreateEditDto sDTMaster)
        {
            if (ModelState.IsValid)
            {
                SDTMaster dto = new()
                {
                    Denominator = sDTMaster.Denominator,
                    Target = sDTMaster.Target,
                    ServiceDeliveryTimeline = sDTMaster.ServiceDeliveryTimeline,
                    Measure = sDTMaster.Measure,
                    EvaluationPeriod = sDTMaster.EvaluationPeriod,
                    DepartmentId = sDTMaster.DepartmentId,
                    Numerator = sDTMaster.Numerator,
                };
                _context.Add(dto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "intDept", "deptName", sDTMaster.DepartmentId);
            return View(sDTMaster);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SDTMasters == null)
            {
                return NotFound();
            }

            var sDTMaster = await _context.SDTMasters.FindAsync(id);
            if (sDTMaster == null)
            {
                return NotFound();
            }
            SDTMastersCreateEditDto dto = new SDTMastersCreateEditDto
            {
                Denominator = sDTMaster.Denominator,
                Numerator = sDTMaster.Numerator,
                EvaluationPeriod = sDTMaster.EvaluationPeriod,
                Measure = sDTMaster.Measure,
                DepartmentId = sDTMaster.DepartmentId,
                Id = sDTMaster.Id,
                ServiceDeliveryTimeline = sDTMaster.ServiceDeliveryTimeline,
                Target = sDTMaster.Target
            };
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "intDept", "deptName", sDTMaster.DepartmentId);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServiceDeliveryTimeline,Measure,EvaluationPeriod,Target,Numerator,Denominator,DepartmentId")] SDTMastersCreateEditDto sDTMaster)
        {
            if (id != sDTMaster.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SDTMaster dto = new()
                    {
                        Denominator = sDTMaster.Denominator,
                        Target = sDTMaster.Target,
                        ServiceDeliveryTimeline = sDTMaster.ServiceDeliveryTimeline,
                        Measure = sDTMaster.Measure,
                        EvaluationPeriod = sDTMaster.EvaluationPeriod,
                        DepartmentId = sDTMaster.DepartmentId,
                        Numerator = sDTMaster.Numerator,
                        Id = sDTMaster.Id
                    };
                    _context.Update(dto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SDTMasterExists(sDTMaster.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "intDept", "deptName", sDTMaster.DepartmentId);
            return View(sDTMaster);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SDTMasters == null)
            {
                return NotFound();
            }

            var sDTMaster = await _context.SDTMasters
                .Include(s => s.DepartmentFk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sDTMaster == null)
            {
                return NotFound();
            }

            return View(sDTMaster);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SDTMasters == null)
            {
                return Problem("Entity set 'AppDbContext.SDTMasters'  is null.");
            }
            var sDTMaster = await _context.SDTMasters.FindAsync(id);
            if (sDTMaster != null)
            {
                _context.SDTMasters.Remove(sDTMaster);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SDTMasterExists(int id)
        {
            return (_context.SDTMasters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
