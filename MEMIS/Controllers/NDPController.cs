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
    public class NDPController : Controller
    {
        private readonly Data.AppDbContext _context;

        public NDPController(Data.AppDbContext context)
        {
            _context = context;
        }

        // GET: NDP
        public  IActionResult Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            if (_context.NDP != null)
            {
                var dat = _context.NDP
                    .Skip(offset)
                    .Take(pageSize);

                var result = new PagedResult<NDP>
                {
                    Data = dat.AsNoTracking().ToList(),
                    TotalItems = _context.NDP.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize

                };
                return View(result);
            }
            else
            {
                return Problem("Entity set 'AppDbContext.NDP'  is null.");
            }
            //return _context.NDP != null ? 
            //            View(await _context.NDP.ToListAsync()) :
            //            Problem("Entity set 'AppDbContext.NDP'  is null.");
        }

        // GET: NDP/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NDP == null)
            {
                return NotFound();
            }

            var nDP = await _context.NDP
                .FirstOrDefaultAsync(m => m.id == id);
            if (nDP == null)
            {
                return NotFound();
            }

            return View(nDP);
        }

        // GET: NDP/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NDP/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Programme,ProgrammeObjective,SubProgramme,SubProgrammeObjective,ProgrammeIntervention")] NDP nDP)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nDP);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nDP);
        }

        // GET: NDP/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NDP == null)
            {
                return NotFound();
            }

            var nDP = await _context.NDP.FindAsync(id);
            if (nDP == null)
            {
                return NotFound();
            }
            return View(nDP);
        }

        // POST: NDP/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Programme,ProgrammeObjective,SubProgramme,SubProgrammeObjective,ProgrammeIntervention")] NDP nDP)
        {
            if (id != nDP.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nDP);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NDPExists(nDP.id))
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
            return View(nDP);
        }

        // GET: NDP/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NDP == null)
            {
                return NotFound();
            }

            var nDP = await _context.NDP
                .FirstOrDefaultAsync(m => m.id == id);
            if (nDP == null)
            {
                return NotFound();
            }

            return View(nDP);
        }

        // POST: NDP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NDP == null)
            {
                return Problem("Entity set 'AppDbContext.NDP'  is null.");
            }
            var nDP = await _context.NDP.FindAsync(id);
            if (nDP != null)
            {
                _context.NDP.Remove(nDP);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NDPExists(int id)
        {
          return (_context.NDP?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
