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
    public class ProgramImplementationPlansController : Controller
    {
        private readonly AppDbContext _context;

        public ProgramImplementationPlansController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ProgramImplementationPlans
        public IActionResult Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            if (_context.ProgramImplementationPlan != null)
            {
                var dat = _context.ProgramImplementationPlan
                    .Skip(offset)
                    .Take(pageSize);

                var result = new PagedResult<ProgramImplementationPlan>
                {
                    Data = dat.AsNoTracking().ToList(),
                    TotalItems = _context.ProgramImplementationPlan.Count(),
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

        // GET: ProgramImplementationPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProgramImplementationPlan == null)
            {
                return NotFound();
            }

            var programImplementationPlan = await _context.ProgramImplementationPlan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programImplementationPlan == null)
            {
                return NotFound();
            }

            return View(programImplementationPlan);
        }

        // GET: ProgramImplementationPlans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProgramImplementationPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Activity,Output,ResponsibleParty,DateofAction,Baseline,FY1,FY2,FY3,FY4,FY5")] ProgramImplementationPlan programImplementationPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(programImplementationPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(programImplementationPlan);
        }

        // GET: ProgramImplementationPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProgramImplementationPlan == null)
            {
                return NotFound();
            }

            var programImplementationPlan = await _context.ProgramImplementationPlan.FindAsync(id);
            if (programImplementationPlan == null)
            {
                return NotFound();
            }
            return View(programImplementationPlan);
        }

        // POST: ProgramImplementationPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Activity,Output,ResponsibleParty,DateofAction,Baseline,FY1,FY2,FY3,FY4,FY5")] ProgramImplementationPlan programImplementationPlan)
        {
            if (id != programImplementationPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programImplementationPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgramImplementationPlanExists(programImplementationPlan.Id))
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
            return View(programImplementationPlan);
        }

        // GET: ProgramImplementationPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProgramImplementationPlan == null)
            {
                return NotFound();
            }

            var programImplementationPlan = await _context.ProgramImplementationPlan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programImplementationPlan == null)
            {
                return NotFound();
            }

            return View(programImplementationPlan);
        }

        // POST: ProgramImplementationPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProgramImplementationPlan == null)
            {
                return Problem("Entity set 'AppDbContext.ProgramImplementationPlan'  is null.");
            }
            var programImplementationPlan = await _context.ProgramImplementationPlan.FindAsync(id);
            if (programImplementationPlan != null)
            {
                _context.ProgramImplementationPlan.Remove(programImplementationPlan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgramImplementationPlanExists(int id)
        {
          return (_context.ProgramImplementationPlan?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
