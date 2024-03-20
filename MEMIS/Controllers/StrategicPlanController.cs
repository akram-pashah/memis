using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Identity;

namespace MEMIS.Controllers
{
    public class StrategicPlanController : Controller
    {
        private readonly Data.AppDbContext _context;

        public StrategicPlanController(Data.AppDbContext context)
        {
            _context = context;
        }

        // GET: StrategicPlan
        public  IActionResult  Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            if (_context.StrategicPlan != null)
            {
                var dat = _context.StrategicPlan
                    .Skip(offset)
                    .Take(pageSize);

                var result = new PagedResult<StrategicPlan>
                {
                    Data = dat.AsNoTracking().ToList(),
                    TotalItems = _context.StrategicPlan.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize

                }; 
                return View(result);
            }
            else
            {
                return Problem("Entity set 'AppDbContext.StrategicPlan'  is null.");
            }
            //return _context.StrategicPlan != null ? 
            //            View(await _context.StrategicPlan.ToListAsync()) :
            //            Problem("Entity set 'AppDbContext.StrategicPlan'  is null.");
        }

        // GET: StrategicPlan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StrategicPlan == null)
            {
                return NotFound();
            }

            var strategicPlan = await _context.StrategicPlan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (strategicPlan == null)
            {
                return NotFound();
            }

            return View(strategicPlan);
        }

        // GET: StrategicPlan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StrategicPlan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ndp,program,programObjective,subProgram,focusArea,strategicObjective,strategicIntervention,StrategicAction")] StrategicPlan strategicPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(strategicPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(strategicPlan);
        }

        // GET: StrategicPlan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StrategicPlan == null)
            {
                return NotFound();
            }

            var strategicPlan = await _context.StrategicPlan.FindAsync(id);
            if (strategicPlan == null)
            {
                return NotFound();
            }
            return View(strategicPlan);
        }

        // POST: StrategicPlan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ndp,program,programObjective,subProgram,focusArea,strategicObjective,strategicIntervention,StrategicAction")] StrategicPlan strategicPlan)
        {
            if (id != strategicPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(strategicPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StrategicPlanExists(strategicPlan.Id))
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
            return View(strategicPlan);
        }

        // GET: StrategicPlan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StrategicPlan == null)
            {
                return NotFound();
            }

            var strategicPlan = await _context.StrategicPlan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (strategicPlan == null)
            {
                return NotFound();
            }

            return View(strategicPlan);
        }

        // POST: StrategicPlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StrategicPlan == null)
            {
                return Problem("Entity set 'AppDbContext.StrategicPlan'  is null.");
            }
            var strategicPlan = await _context.StrategicPlan.FindAsync(id);
            if (strategicPlan != null)
            {
                _context.StrategicPlan.Remove(strategicPlan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StrategicPlanExists(int id)
        {
          return (_context.StrategicPlan?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
