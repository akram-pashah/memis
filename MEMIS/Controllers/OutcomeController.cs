using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;
using cloudscribe.Pagination.Models;
using Org.BouncyCastle.Asn1.X509;

namespace MEMIS.Controllers
{
    public class OutcomeController : Controller
    {
        private readonly AppDbContext _context;

        public OutcomeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Outcomes
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.Outcome.Include(s => s.StrategicObjective).Skip(offset).Take(pageSize);
            var result = new PagedResult<Outcome>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.Outcome.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return View(result);
        }

        // GET: Outcomes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Outcome == null)
            {
                return NotFound();
            }

            var outcome = await _context.Outcome
                .Include(s => s.StrategicObjective)
                .FirstOrDefaultAsync(m => m.intOutcome == id);
            if (outcome == null)
            {
                return NotFound();
            }

            return View(outcome);
        }

        // GET: Outcomes/Create
        public IActionResult Create()
        {
            ViewData["intObjective"] = new SelectList(_context.StrategicObjective.Select(o => new {o.intObjective,DisplayText=o.ObjectiveCode + " - " + o.ObjectiveName}), "intObjective", "DisplayText");
            return View();
        }

        // POST: Outcomes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("intOutcome,OutcomeCode,OutcomeName,intObjective")] Outcome outcome)
        {
            if (ModelState.IsValid)
            {
                _context.Add(outcome);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } 
      ViewData["intObjective"] = new SelectList(_context.StrategicObjective.Select(o => new { o.intObjective, DisplayText = o.ObjectiveCode + " - " + o.ObjectiveName }), "intObjective", "DisplayText", outcome.intObjective);

      return View(outcome);
        }

        // GET: Outcomes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Outcome == null)
            {
                return NotFound();
            }

            var outcome = await _context.Outcome.FindAsync(id);
            if (outcome == null)
            {
                return NotFound();
            }
      ViewData["intObjective"] = new SelectList(_context.StrategicObjective.Select(o => new { o.intObjective, DisplayText = o.ObjectiveCode + " - " + o.ObjectiveName }), "intObjective", "DisplayText");
      return View(outcome);
        }

        // POST: Outcomes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("intOutcome,OutcomeCode,OutcomeName,intObjective")] Outcome outcome)
        {
            if (id != outcome.intOutcome)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(outcome);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OutcomeExists(outcome.intOutcome))
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
      ViewData["intObjective"] = new SelectList(_context.StrategicObjective.Select(o => new { o.intObjective, DisplayText = o.ObjectiveCode + " - " + o.ObjectiveName }), "intObjective", "DisplayText", outcome.intObjective);
            return View(outcome);
        }

        // GET: Outcomes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Outcome == null)
            {
                return NotFound();
            }

            var outcome = await _context.Outcome
                .Include(s => s.StrategicObjective)
                .FirstOrDefaultAsync(m => m.intOutcome == id);
            if (outcome == null)
            {
                return NotFound();
            }

            return View(outcome);
        }

        // POST: Outcomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Outcome == null)
            {
                return Problem("Entity set 'AppDbContext.Outcome'  is null.");
            }
            var outcome = await _context.Outcome.FindAsync(id);
            if (outcome != null)
            {
                _context.Outcome.Remove(outcome);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OutcomeExists(int id)
        {
          return (_context.Outcome?.Any(e => e.intOutcome == id)).GetValueOrDefault();
        }
    }
}
