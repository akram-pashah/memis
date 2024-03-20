using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;
using cloudscribe.Pagination.Models;

namespace MEMIS.Controllers
{
    public class StrategicInterventionsController : Controller
    {
        private readonly AppDbContext _context;

        public StrategicInterventionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: StrategicInterventions
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.StrategicIntervention.Include(s => s.StrategicObjective).Skip(offset).Take(pageSize);
            var result = new PagedResult<StrategicIntervention>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.StrategicIntervention.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return View(result);
        }

        // GET: StrategicInterventions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StrategicIntervention == null)
            {
                return NotFound();
            }

            var strategicIntervention = await _context.StrategicIntervention
                .Include(s => s.StrategicObjective)
                .FirstOrDefaultAsync(m => m.intIntervention == id);
            if (strategicIntervention == null)
            {
                return NotFound();
            }

            return View(strategicIntervention);
        }

        // GET: StrategicInterventions/Create
        public IActionResult Create()
        {
            ViewData["intObjective"] = new SelectList(_context.StrategicObjective, "intObjective", "ObjectiveCode");
            return View();
        }

        // POST: StrategicInterventions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("intIntervention,InterventionCode,InterventionName,intObjective")] StrategicIntervention strategicIntervention)
        {
            if (ModelState.IsValid)
            {
                _context.Add(strategicIntervention);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["intObjective"] = new SelectList(_context.StrategicObjective, "intObjective", "ObjectiveCode", strategicIntervention.intObjective);
            return View(strategicIntervention);
        }

        // GET: StrategicInterventions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StrategicIntervention == null)
            {
                return NotFound();
            }

            var strategicIntervention = await _context.StrategicIntervention.FindAsync(id);
            if (strategicIntervention == null)
            {
                return NotFound();
            }
            ViewData["intObjective"] = new SelectList(_context.StrategicObjective, "intObjective", "ObjectiveCode", strategicIntervention.intObjective);
            return View(strategicIntervention);
        }

        // POST: StrategicInterventions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("intIntervention,InterventionCode,InterventionName,intObjective")] StrategicIntervention strategicIntervention)
        {
            if (id != strategicIntervention.intIntervention)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(strategicIntervention);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StrategicInterventionExists(strategicIntervention.intIntervention))
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
            ViewData["intObjective"] = new SelectList(_context.StrategicObjective, "intObjective", "ObjectiveCode", strategicIntervention.intObjective);
            return View(strategicIntervention);
        }

        // GET: StrategicInterventions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StrategicIntervention == null)
            {
                return NotFound();
            }

            var strategicIntervention = await _context.StrategicIntervention
                .Include(s => s.StrategicObjective)
                .FirstOrDefaultAsync(m => m.intIntervention == id);
            if (strategicIntervention == null)
            {
                return NotFound();
            }

            return View(strategicIntervention);
        }

        // POST: StrategicInterventions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StrategicIntervention == null)
            {
                return Problem("Entity set 'AppDbContext.StrategicIntervention'  is null.");
            }
            var strategicIntervention = await _context.StrategicIntervention.FindAsync(id);
            if (strategicIntervention != null)
            {
                _context.StrategicIntervention.Remove(strategicIntervention);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StrategicInterventionExists(int id)
        {
          return (_context.StrategicIntervention?.Any(e => e.intIntervention == id)).GetValueOrDefault();
        }
    }
}
