using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;

namespace MEMIS.Controllers.ME
{
    public class ActivityAssessmentController : Controller
    {
        private readonly Data.AppDbContext _context;

        public ActivityAssessmentController(Data.AppDbContext context)
        {
            _context = context;
        }

        // GET: ActivityAssessment
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ActivityAssessment.Include(a => a.ImplementationStatus);
            return View(await appDbContext.ToListAsync());
        }
        public async Task<IActionResult> HodVerification()
        {
            var appDbContext = _context.ActivityAssessment.Include(a => a.ImplementationStatus);
            return View(await appDbContext.ToListAsync());
        }
        public async Task<IActionResult> VerificationDir()
        {
            var appDbContext = _context.ActivityAssessment.Include(a => a.ImplementationStatus);
            return View(await appDbContext.ToListAsync());
        }
        public async Task<IActionResult> Consolidation()
        {
            var appDbContext = _context.ActivityAssessment.Include(a => a.ImplementationStatus);
            return View(await appDbContext.ToListAsync());
        }
        public async Task<IActionResult> VerificationBpd()
        {
            var appDbContext = _context.ActivityAssessment.Include(a => a.ImplementationStatus);
            return View(await appDbContext.ToListAsync());
        }
        public async Task<IActionResult> Approval()
        {
            var appDbContext = _context.ActivityAssessment.Include(a => a.ImplementationStatus);
            return View(await appDbContext.ToListAsync());
        }

        // GET: ActivityAssessment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ActivityAssessment == null)
            {
                return NotFound();
            }

            var activityAssessment = await _context.ActivityAssessment
                .Include(a => a.ImplementationStatus)
                .FirstOrDefaultAsync(m => m.intDeptPlan == id);
            if (activityAssessment == null)
            {
                return NotFound();
            }

            return View(activityAssessment);
        }

        // GET: ActivityAssessment/Create
        public IActionResult Create()
        {
            ViewData["ImpStatusId"] = new SelectList(_context.ImplementationStatus, "ImpStatusId", "ImpStatusName");
            return View();
        }

        // POST: ActivityAssessment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("intDeptPlan,strategicObjective,strategicIntervention,StrategicAction,activity,outputIndicator,baseline,budgetCode,unitCost,comparativeTarget,justification,budgetAmount,Q1Target,Q1Budget,Q1Actual,Q1AmtSpent,Q1Justification,Q2Target,Q2Budget,Q2Actual,Q2AmtSpent,Q2Justification,Q3Target,Q3Budget,Q3Actual,Q3AmtSpent,Q3Justification,Q4Target,Q4Budget,Q4Actual,Q4AmtSpent,Q4Justification,AnnualAchievement,TotAmtSpent,ImpStatusId,AnnualJustification")] ActivityAssessment activityAssessment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activityAssessment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ImpStatusId"] = new SelectList(_context.ImplementationStatus, "ImpStatusId", "ImpStatusName", activityAssessment.ImpStatusId);
            return View(activityAssessment);
        }

        // GET: ActivityAssessment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActivityAssessment == null)
            {
                return NotFound();
            }

            var activityAssessment = await _context.ActivityAssessment.FindAsync(id);
            if (activityAssessment == null)
            {
                return NotFound();
            }
            ViewData["ImpStatusId"] = new SelectList(_context.ImplementationStatus, "ImpStatusId", "ImpStatusName", activityAssessment.ImpStatusId);
            return View(activityAssessment);
        }

        // POST: ActivityAssessment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("intDeptPlan,strategicObjective,strategicIntervention,StrategicAction,activity,outputIndicator,baseline,budgetCode,unitCost,comparativeTarget,justification,budgetAmount,Q1Target,Q1Budget,Q1Actual,Q1AmtSpent,Q1Justification,Q2Target,Q2Budget,Q2Actual,Q2AmtSpent,Q2Justification,Q3Target,Q3Budget,Q3Actual,Q3AmtSpent,Q3Justification,Q4Target,Q4Budget,Q4Actual,Q4AmtSpent,Q4Justification,AnnualAchievement,TotAmtSpent,ImpStatusId,AnnualJustification")] ActivityAssessment activityAssessment)
        {
            if (id != activityAssessment.intDeptPlan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityAssessment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityAssessmentExists(activityAssessment.intDeptPlan))
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
            ViewData["ImpStatusId"] = new SelectList(_context.ImplementationStatus, "ImpStatusId", "ImpStatusName", activityAssessment.ImpStatusId);
            return View(activityAssessment);
        }

        // GET: ActivityAssessment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActivityAssessment == null)
            {
                return NotFound();
            }

            var activityAssessment = await _context.ActivityAssessment
                .Include(a => a.ImplementationStatus)
                .FirstOrDefaultAsync(m => m.intDeptPlan == id);
            if (activityAssessment == null)
            {
                return NotFound();
            }

            return View(activityAssessment);
        }

        // POST: ActivityAssessment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActivityAssessment == null)
            {
                return Problem("Entity set 'AppDbContext.ActivityAssessment'  is null.");
            }
            var activityAssessment = await _context.ActivityAssessment.FindAsync(id);
            if (activityAssessment != null)
            {
                _context.ActivityAssessment.Remove(activityAssessment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityAssessmentExists(int id)
        {
          return (_context.ActivityAssessment?.Any(e => e.intDeptPlan == id)).GetValueOrDefault();
        }
    }
}
