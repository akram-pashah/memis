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
    public class DepartmentPlanController : Controller
    {
        private readonly Data.AppDbContext _context;

        public DepartmentPlanController(Data.AppDbContext context)
        {
            _context = context;
        }

        // GET: DepartmentPlan
        public async Task<IActionResult> Index()
        {
              return _context.DepartmentPlan != null ? 
                          View(await _context.DepartmentPlan.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.DepartmentPlan'  is null.");
        }

        public async Task<IActionResult> VerficationHod()
        {
            return _context.DepartmentPlan != null ?
                        View(await _context.DepartmentPlan.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.DepartmentPlan'  is null.");
        }
        public async Task<IActionResult> VerificationDir()
        {
            return _context.DepartmentPlan != null ?
                        View(await _context.DepartmentPlan.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.DepartmentPlan'  is null.");
        }
        public async Task<IActionResult> Consolidation()
        {
            return _context.DepartmentPlan != null ?
                        View(await _context.DepartmentPlan.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.DepartmentPlan'  is null.");
        }
        public async Task<IActionResult> VerificationBpd()
        {
            return _context.DepartmentPlan != null ?
                        View(await _context.DepartmentPlan.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.DepartmentPlan'  is null.");
        }
        public async Task<IActionResult> Approval()
        {
            return _context.DepartmentPlan != null ?
                        View(await _context.DepartmentPlan.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.DepartmentPlan'  is null.");
        }

        // GET: DepartmentPlan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DepartmentPlan == null)
            {
                return NotFound();
            }

            var departmentPlan = await _context.DepartmentPlan
                .FirstOrDefaultAsync(m => m.intDeptPlan == id);
            if (departmentPlan == null)
            {
                return NotFound();
            }

            return View(departmentPlan);
        }

        // GET: DepartmentPlan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DepartmentPlan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("intDeptPlan,strategicObjective,strategicIntervention,StrategicAction,activity,outputIndicator,baseline,budgetCode,unitCost,Q1Target,Q1Budget,Q2Target,Q2Budget,Q3Target,Q3Budget,Q4Target,Q4Budget,comparativeTarget,justification,budgetAmount")] DepartmentPlan departmentPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departmentPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(departmentPlan);
        }

        // GET: DepartmentPlan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DepartmentPlan == null)
            {
                return NotFound();
            }

            var departmentPlan = await _context.DepartmentPlan.FindAsync(id);
            if (departmentPlan == null)
            {
                return NotFound();
            }
            return View(departmentPlan);
        }

        // POST: DepartmentPlan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("intDeptPlan,strategicObjective,strategicIntervention,StrategicAction,activity,outputIndicator,baseline,budgetCode,unitCost,Q1Target,Q1Budget,Q2Target,Q2Budget,Q3Target,Q3Budget,Q4Target,Q4Budget,comparativeTarget,justification,budgetAmount")] DepartmentPlan departmentPlan)
        {
            if (id != departmentPlan.intDeptPlan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departmentPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentPlanExists(departmentPlan.intDeptPlan))
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
            return View(departmentPlan);
        }

        // GET: DepartmentPlan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DepartmentPlan == null)
            {
                return NotFound();
            }

            var departmentPlan = await _context.DepartmentPlan
                .FirstOrDefaultAsync(m => m.intDeptPlan == id);
            if (departmentPlan == null)
            {
                return NotFound();
            }

            return View(departmentPlan);
        }

        // POST: DepartmentPlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DepartmentPlan == null)
            {
                return Problem("Entity set 'AppDbContext.DepartmentPlan'  is null.");
            }
            var departmentPlan = await _context.DepartmentPlan.FindAsync(id);
            if (departmentPlan != null)
            {
                _context.DepartmentPlan.Remove(departmentPlan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentPlanExists(int id)
        {
          return (_context.DepartmentPlan?.Any(e => e.intDeptPlan == id)).GetValueOrDefault();
        }
    }
}
