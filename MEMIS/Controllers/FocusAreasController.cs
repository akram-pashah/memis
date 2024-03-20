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
    public class FocusAreasController : Controller
    {
        private readonly AppDbContext _context;

        public FocusAreasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FocusAreas
        public IActionResult Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            if (_context.NDP != null)
            {
                var dat = _context.FocusArea
                    .Skip(offset)
                    .Take(pageSize);

                var result = new PagedResult<FocusArea>
                {
                    Data = dat.AsNoTracking().ToList(),
                    TotalItems = _context.FocusArea.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize

                };
                return View(result);
            }
            else
            {
                return Problem("Entity set 'AppDbContext.NDP'  is null.");
            }
           
        }

        // GET: FocusAreas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FocusArea == null)
            {
                return NotFound();
            }

            var focusArea = await _context.FocusArea
                .FirstOrDefaultAsync(m => m.intFocus == id);
            if (focusArea == null)
            {
                return NotFound();
            }

            return View(focusArea);
        }

        // GET: FocusAreas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FocusAreas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("intFocus,FocusAreacode,FocusAreaName")] FocusArea focusArea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(focusArea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(focusArea);
        }

        // GET: FocusAreas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FocusArea == null)
            {
                return NotFound();
            }

            var focusArea = await _context.FocusArea.FindAsync(id);
            if (focusArea == null)
            {
                return NotFound();
            }
            return View(focusArea);
        }

        // POST: FocusAreas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("intFocus,FocusAreacode,FocusAreaName")] FocusArea focusArea)
        {
            if (id != focusArea.intFocus)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(focusArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FocusAreaExists(focusArea.intFocus))
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
            return View(focusArea);
        }

        // GET: FocusAreas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FocusArea == null)
            {
                return NotFound();
            }

            var focusArea = await _context.FocusArea
                .FirstOrDefaultAsync(m => m.intFocus == id);
            if (focusArea == null)
            {
                return NotFound();
            }

            return View(focusArea);
        }

        // POST: FocusAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FocusArea == null)
            {
                return Problem("Entity set 'AppDbContext.FocusArea'  is null.");
            }
            var focusArea = await _context.FocusArea.FindAsync(id);
            if (focusArea != null)
            {
                _context.FocusArea.Remove(focusArea);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FocusAreaExists(int id)
        {
          return (_context.FocusArea?.Any(e => e.intFocus == id)).GetValueOrDefault();
        }
    }
}
