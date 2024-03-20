using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;
using cloudscribe.Pagination.Models;
using MEMIS.Models;

namespace MEMIS.Controllers
{
    public class KPIMasterController : Controller
    {
        private readonly Data.AppDbContext _context;

		public KPIMasterController(Data.AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.KPIMasters.Include(s => s.StrategicPlanFk).Skip(offset).Take(pageSize);
            var result = new PagedResult<KPIMaster>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.KPIMasters.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewData["TypeofIndicator"] = ListHelper.TypeofIndicator();
            ViewData["Indicatorclassification"] = ListHelper.Indicatorclassification();
            ViewData["FrequencyofReporting"] = ListHelper.FrequencyofReporting();
            ViewData["StrategicObjective"] = new SelectList(_context.StrategicPlan.OrderBy(d => d.strategicObjective), "Id", "strategicObjective");
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Index(int pageNumber = 1, string? perfind=null)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.KPIMasters.Include(s => s.StrategicPlanFk).Where(s=>  (perfind!=null?EF.Functions.Like(s.PerformanceIndicator,'%'+perfind+'%'):true)).Skip(offset).Take(pageSize);
            var result = new PagedResult<KPIMaster>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.KPIMasters.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewData["TypeofIndicator"] = ListHelper.TypeofIndicator();
            ViewData["Indicatorclassification"] = ListHelper.Indicatorclassification();
            ViewData["FrequencyofReporting"] = ListHelper.FrequencyofReporting();
            ViewData["StrategicObjective"] = new SelectList(_context.StrategicPlan.OrderBy(d => d.strategicObjective), "Id", "strategicObjective");
            return View(result);
            //if (id == null || _context.KPIMasters == null)
            //{
            //    return NotFound();
            //}

            //var KPIMaster = await _context.KPIMasters
            //    .Include(s => s.StrategicPlanFk)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (KPIMaster == null)
            //{
            //    return NotFound();
            //}

            //return View(KPIMaster);
        }
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.KPIMasters == null)
            {
                return NotFound();
            }

            var KPIMaster = await _context.KPIMasters
                .Include(s => s.StrategicPlanFk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (KPIMaster == null)
            {
                return NotFound();
            }

            return View(KPIMaster);
        }

        public IActionResult  Create()
        {
            ViewData["TypeofIndicator"] = ListHelper.TypeofIndicator();
            ViewData["Indicatorclassification"] = ListHelper.Indicatorclassification();
            ViewData["FrequencyofReporting"] = ListHelper.FrequencyofReporting();
            ViewData["StrategicObjective"] = new SelectList(_context.StrategicPlan, "Id", "strategicObjective");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KPIMasterCreateEditDto KPIMaster)
        {
            if (ModelState.IsValid)
            {
                KPIMaster dto = new()
                {
                    StrategicObjective=KPIMaster.StrategicObjective,
                    PerformanceIndicator = KPIMaster.PerformanceIndicator,
                    TypeofIndicator = KPIMaster.TypeofIndicator, 
                    IndicatorFormulae= KPIMaster.IndicatorFormulae,
                    IndicatorDefinition= KPIMaster.IndicatorDefinition,
                    OriginalBaseline= KPIMaster.OriginalBaseline,
                    Indicatorclassification= KPIMaster.Indicatorclassification,
                    DataType=KPIMaster.DataType,
                    Unitofmeasure=KPIMaster.Unitofmeasure,
                    FrequencyofReporting=KPIMaster.FrequencyofReporting,
                    FY1=KPIMaster.FY1,
                    FY2=KPIMaster.FY2,
                    FY3=KPIMaster.FY3,
                    FY4=KPIMaster.FY4,
                    FY5=KPIMaster.FY5,
                    MeansofVerification=KPIMaster.MeansofVerification,
                    ResponsibleParty=KPIMaster.ResponsibleParty,
                };
                _context.Add(dto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeofIndicator"] = ListHelper.TypeofIndicator();
            ViewData["Indicatorclassification"] = ListHelper.Indicatorclassification();
            ViewData["FrequencyofReporting"] = ListHelper.FrequencyofReporting();
            ViewData["StrategicObjective"] = new SelectList(_context.StrategicPlan, "Id", "strategicObjective", KPIMaster.StrategicObjective);
            return View(KPIMaster);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KPIMasters == null)
            {
                return NotFound();
            }

            var KPIMaster = await _context.KPIMasters.FindAsync(id);
            if (KPIMaster == null)
            {
                return NotFound();
            }
            KPIMasterCreateEditDto dto = new KPIMasterCreateEditDto
			{
				PerformanceIndicator = KPIMaster.PerformanceIndicator,
				TypeofIndicator = KPIMaster.TypeofIndicator,
				IndicatorFormulae = KPIMaster.IndicatorFormulae,
				IndicatorDefinition = KPIMaster.IndicatorDefinition,
				OriginalBaseline = KPIMaster.OriginalBaseline,
				Indicatorclassification = KPIMaster.Indicatorclassification,
				DataType = KPIMaster.DataType,
				Unitofmeasure = KPIMaster.Unitofmeasure,
				FrequencyofReporting = KPIMaster.FrequencyofReporting,
				FY1 = KPIMaster.FY1,
				FY2 = KPIMaster.FY2,
				FY3 = KPIMaster.FY3,
				FY4 = KPIMaster.FY4,
				FY5 = KPIMaster.FY5,
				MeansofVerification = KPIMaster.MeansofVerification,
				ResponsibleParty = KPIMaster.ResponsibleParty
			};
            ViewData["TypeofIndicator"] = ListHelper.TypeofIndicator();
            ViewData["Indicatorclassification"] = ListHelper.Indicatorclassification();
            ViewData["FrequencyofReporting"] = ListHelper.FrequencyofReporting();
            ViewData["StrategicObjective"] = new SelectList(_context.StrategicPlan, "Id", "strategicObjective", KPIMaster.StrategicObjective);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PerformanceIndicator,TypeofIndicator,IndicatorFormulae,IndicatorDefinition,OriginalBaseline,Indicatorclassification,DataType,Unitofmeasure,FrequencyofReporting,FY1,FY2,FY3,FY4,FY5,MeansofVerification,ResponsibleParty")] KPIMasterCreateEditDto KPIMaster)
        {
            if (id != KPIMaster.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    KPIMaster dto = new()
                    {
						PerformanceIndicator = KPIMaster.PerformanceIndicator,
						TypeofIndicator = KPIMaster.TypeofIndicator,
						IndicatorFormulae = KPIMaster.IndicatorFormulae,
						IndicatorDefinition = KPIMaster.IndicatorDefinition,
						OriginalBaseline = KPIMaster.OriginalBaseline,
						Indicatorclassification = KPIMaster.Indicatorclassification,
						DataType = KPIMaster.DataType,
						Unitofmeasure = KPIMaster.Unitofmeasure,
						FrequencyofReporting = KPIMaster.FrequencyofReporting,
						FY1 = KPIMaster.FY1,
						FY2 = KPIMaster.FY2,
						FY3 = KPIMaster.FY3,
						FY4 = KPIMaster.FY4,
						FY5 = KPIMaster.FY5,
						MeansofVerification = KPIMaster.MeansofVerification,
						ResponsibleParty = KPIMaster.ResponsibleParty,
                        StrategicObjective = KPIMaster.StrategicObjective,
						Id = KPIMaster.Id
                    };
                    _context.Update(dto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KPIMasterExists(KPIMaster.Id))
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
            ViewData["TypeofIndicator"] = ListHelper.TypeofIndicator();
            ViewData["Indicatorclassification"] = ListHelper.Indicatorclassification();
            ViewData["FrequencyofReporting"] = ListHelper.FrequencyofReporting();
            ViewData["StrategicObjective"] = new SelectList(_context.StrategicPlan, "Id", "strategicObjective", KPIMaster.StrategicObjective);
            return View(KPIMaster);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KPIMasters == null)
            {
                return NotFound();
            }

            var KPIMaster = await _context.KPIMasters
                .Include(s => s.StrategicPlanFk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (KPIMaster == null)
            {
                return NotFound();
            }

            return View(KPIMaster);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KPIMasters == null)
            {
                return Problem("Entity set 'AppDbContext.KPIMasters'  is null.");
            }
            var KPIMaster = await _context.KPIMasters.FindAsync(id);
            if (KPIMaster != null)
            {
                _context.KPIMasters.Remove(KPIMaster);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KPIMasterExists(int id)
        {
            return (_context.KPIMasters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
