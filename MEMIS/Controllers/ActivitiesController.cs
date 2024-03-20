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
    public class ActivitiesController : Controller
    {
        private readonly AppDbContext _context;

        public ActivitiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Activities
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.Activity.Include(s => s.StrategicAction).Skip(offset).Take(pageSize);
            var result = new PagedResult<Activity>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.Activity.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return View(result);
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .Include(a => a.StrategicAction)
                .FirstOrDefaultAsync(m => m.intActivity == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            ViewData["intAction"] = new SelectList(_context.StrategicAction, "intAction", "actionCode");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("intActivity,activityCode,activityName,intAction")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["intAction"] = new SelectList(_context.StrategicAction, "intAction", "actionCode", activity.intAction);
            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["intAction"] = new SelectList(_context.StrategicAction, "intAction", "actionCode", activity.intAction);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("intActivity,activityCode,activityName,intAction")] Activity activity)
        {
            if (id != activity.intActivity)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.intActivity))
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
            ViewData["intAction"] = new SelectList(_context.StrategicAction, "intAction", "actionCode", activity.intAction);
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .Include(a => a.StrategicAction)
                .FirstOrDefaultAsync(m => m.intActivity == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Activity == null)
            {
                return Problem("Entity set 'AppDbContext.Activity'  is null.");
            }
            var activity = await _context.Activity.FindAsync(id);
            if (activity != null)
            {
                _context.Activity.Remove(activity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
          return (_context.Activity?.Any(e => e.intActivity == id)).GetValueOrDefault();
        }
    }
}
