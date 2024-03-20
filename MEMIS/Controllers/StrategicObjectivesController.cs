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
    public class StrategicObjectivesController : Controller
    {
        private readonly AppDbContext _context;

        public StrategicObjectivesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: StrategicObjectives
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.StrategicObjective.Include(s => s.FocusArea).Skip(offset).Take(pageSize);
            var result = new PagedResult<StrategicObjective>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.StrategicObjective.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
          
            return View(result);
        }
        //public async Task<IActionResult> Index()
        //{
        //    var appDbContext = _context.StrategicObjective.Include(s => s.FocusArea);
        //    return View(await appDbContext.ToListAsync());
        //}

        // GET: StrategicObjectives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StrategicObjective == null)
            {
                return NotFound();
            }

            var strategicObjective = await _context.StrategicObjective
                .Include(s => s.FocusArea)
                .FirstOrDefaultAsync(m => m.intObjective == id);
            if (strategicObjective == null)
            {
                return NotFound();
            }

            return View(strategicObjective);
        }

        // GET: StrategicObjectives/Create
        public IActionResult Create()
        {
            ViewData["intFocus"] = new SelectList(_context.FocusArea, "intFocus", "FocusAreaName");
            return View();
        }

        // POST: StrategicObjectives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("intObjective,ObjectiveCode,ObjectiveName,intFocus")] StrategicObjective strategicObjective)
        {
            if (ModelState.IsValid)
            {
                _context.Add(strategicObjective);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["intFocus"] = new SelectList(_context.FocusArea, "intFocus", "FocusAreaName", strategicObjective.intFocus);
            return View(strategicObjective);
        }

        // GET: StrategicObjectives/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StrategicObjective == null)
            {
                return NotFound();
            }

            var strategicObjective = await _context.StrategicObjective.FindAsync(id);
            if (strategicObjective == null)
            {
                return NotFound();
            }
            ViewData["intFocus"] = new SelectList(_context.FocusArea, "intFocus", "FocusAreaName", strategicObjective.intFocus);
            return View(strategicObjective);
        }

        // POST: StrategicObjectives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("intObjective,ObjectiveCode,ObjectiveName,intFocus")] StrategicObjective strategicObjective)
        {
            if (id != strategicObjective.intObjective)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(strategicObjective);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StrategicObjectiveExists(strategicObjective.intObjective))
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
            ViewData["intFocus"] = new SelectList(_context.FocusArea, "intFocus", "FocusAreaName", strategicObjective.intFocus);
            return View(strategicObjective);
        }

        // GET: StrategicObjectives/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StrategicObjective == null)
            {
                return NotFound();
            }

            var strategicObjective = await _context.StrategicObjective
                .Include(s => s.FocusArea)
                .FirstOrDefaultAsync(m => m.intObjective == id);
            if (strategicObjective == null)
            {
                return NotFound();
            }

            return View(strategicObjective);
        }

        // POST: StrategicObjectives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StrategicObjective == null)
            {
                return Problem("Entity set 'AppDbContext.StrategicObjective'  is null.");
            }
            var strategicObjective = await _context.StrategicObjective.FindAsync(id);
            if (strategicObjective != null)
            {
                _context.StrategicObjective.Remove(strategicObjective);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StrategicObjectiveExists(int id)
        {
          return (_context.StrategicObjective?.Any(e => e.intObjective == id)).GetValueOrDefault();
        }
    }
}
