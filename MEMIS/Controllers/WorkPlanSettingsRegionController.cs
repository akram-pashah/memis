using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;

namespace MEMIS.Controllers
{
    public class WorkPlanSettingsRegionController : Controller
    {
        private readonly Data.AppDbContext _context;

        public WorkPlanSettingsRegionController(Data.AppDbContext context)
        {
            _context = context;
        }

        // GET: WorkPlanSettingsRegion
        public async Task<IActionResult> Index()
        {
              return _context.WorkPlanSettingsRegion != null ? 
                          View(await _context.WorkPlanSettingsRegion.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.WorkPlanSettingsRegion'  is null.");
        }
        
        public async Task<IActionResult> Verification()
        {
            return _context.WorkPlanSettingsRegion != null ?
                        View(await _context.WorkPlanSettingsRegion.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.WorkPlanSettingsRegion'  is null.");
        }
        public async Task<IActionResult> Consolidation()
        {
            return _context.WorkPlanSettingsRegion != null ?
                        View(await _context.WorkPlanSettingsRegion.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.WorkPlanSettingsRegion'  is null.");
        }

        // GET: WorkPlanSettingsRegion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.WorkPlanSettingsRegion == null)
            {
                return NotFound();
            }

            var workPlanSettingsRegion = await _context.WorkPlanSettingsRegion
                .FirstOrDefaultAsync(m => m.intWrokPlan == id);
            if (workPlanSettingsRegion == null)
            {
                return NotFound();
            }

            return View(workPlanSettingsRegion);
        }

        // GET: WorkPlanSettingsRegion/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkPlanSettingsRegion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("intWrokPlan,strategicObjective,strategicIntervention,StrategicAction,activity,outputIndicator,baseline,budgetCode,unitCost,Q1Target,Q1Budget,Q2Target,Q2Budget,Q3Target,Q3Budget,Q4Target,Q4Budget,comparativeTarget,budgetAmount")] WorkPlanSettingsRegion workPlanSettingsRegion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workPlanSettingsRegion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workPlanSettingsRegion);
        }

        // GET: WorkPlanSettingsRegion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WorkPlanSettingsRegion == null)
            {
                return NotFound();
            }

            var workPlanSettingsRegion = await _context.WorkPlanSettingsRegion.FindAsync(id);
            if (workPlanSettingsRegion == null)
            {
                return NotFound();
            }
            return View(workPlanSettingsRegion);
        }

        // POST: WorkPlanSettingsRegion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("intWrokPlan,strategicObjective,strategicIntervention,StrategicAction,activity,outputIndicator,baseline,budgetCode,unitCost,Q1Target,Q1Budget,Q2Target,Q2Budget,Q3Target,Q3Budget,Q4Target,Q4Budget,comparativeTarget,budgetAmount")] WorkPlanSettingsRegion workPlanSettingsRegion)
        {
            if (id != workPlanSettingsRegion.intWrokPlan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workPlanSettingsRegion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkPlanSettingsRegionExists(workPlanSettingsRegion.intWrokPlan))
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
            return View(workPlanSettingsRegion);
        }

        // GET: WorkPlanSettingsRegion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.WorkPlanSettingsRegion == null)
            {
                return NotFound();
            }

            var workPlanSettingsRegion = await _context.WorkPlanSettingsRegion
                .FirstOrDefaultAsync(m => m.intWrokPlan == id);
            if (workPlanSettingsRegion == null)
            {
                return NotFound();
            }

            return View(workPlanSettingsRegion);
        }

        // POST: WorkPlanSettingsRegion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.WorkPlanSettingsRegion == null)
            {
                return Problem("Entity set 'AppDbContext.WorkPlanSettingsRegion'  is null.");
            }
            var workPlanSettingsRegion = await _context.WorkPlanSettingsRegion.FindAsync(id);
            if (workPlanSettingsRegion != null)
            {
                _context.WorkPlanSettingsRegion.Remove(workPlanSettingsRegion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkPlanSettingsRegionExists(int id)
        {
          return (_context.WorkPlanSettingsRegion?.Any(e => e.intWrokPlan == id)).GetValueOrDefault();
        }
    }
}
