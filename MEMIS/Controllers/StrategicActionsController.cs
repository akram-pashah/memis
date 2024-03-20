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
    public class StrategicActionsController : Controller
    {
        private readonly AppDbContext _context;

        public StrategicActionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: StrategicActions
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.StrategicAction.Include(s => s.StrategicIntervention).Skip(offset).Take(pageSize);
            var result = new PagedResult<StrategicAction>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.StrategicAction.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return View(result);
        }

        // GET: StrategicActions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StrategicAction == null)
            {
                return NotFound();
            }

            var strategicAction = await _context.StrategicAction
                .Include(s => s.StrategicIntervention)
                .FirstOrDefaultAsync(m => m.intAction == id);
            if (strategicAction == null)
            {
                return NotFound();
            }

            return View(strategicAction);
        }

        // GET: StrategicActions/Create
        public IActionResult Create()
        {
            ViewData["intIntervention"] = new SelectList(_context.StrategicIntervention, "intIntervention", "InterventionCode");
            return View();
        }

        // POST: StrategicActions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("intAction,actionCode,actionName,intIntervention")] StrategicAction strategicAction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(strategicAction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["intIntervention"] = new SelectList(_context.StrategicIntervention, "intIntervention", "InterventionCode", strategicAction.intIntervention);
            return View(strategicAction);
        }

        // GET: StrategicActions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StrategicAction == null)
            {
                return NotFound();
            }

            var strategicAction = await _context.StrategicAction.FindAsync(id);
            if (strategicAction == null)
            {
                return NotFound();
            }
            ViewData["intIntervention"] = new SelectList(_context.StrategicIntervention, "intIntervention", "InterventionCode", strategicAction.intIntervention);
            return View(strategicAction);
        }

        // POST: StrategicActions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("intAction,actionCode,actionName,intIntervention")] StrategicAction strategicAction)
        {
            if (id != strategicAction.intAction)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(strategicAction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StrategicActionExists(strategicAction.intAction))
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
            ViewData["intIntervention"] = new SelectList(_context.StrategicIntervention, "intIntervention", "InterventionCode", strategicAction.intIntervention);
            return View(strategicAction);
        }

        // GET: StrategicActions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StrategicAction == null)
            {
                return NotFound();
            }

            var strategicAction = await _context.StrategicAction
                .Include(s => s.StrategicIntervention)
                .FirstOrDefaultAsync(m => m.intAction == id);
            if (strategicAction == null)
            {
                return NotFound();
            }

            return View(strategicAction);
        }

        // POST: StrategicActions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StrategicAction == null)
            {
                return Problem("Entity set 'AppDbContext.StrategicAction'  is null.");
            }
            var strategicAction = await _context.StrategicAction.FindAsync(id);
            if (strategicAction != null)
            {
                _context.StrategicAction.Remove(strategicAction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StrategicActionExists(int id)
        {
          return (_context.StrategicAction?.Any(e => e.intAction == id)).GetValueOrDefault();
        }
    }
}
