using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;
using cloudscribe.Pagination.Models;
using MEMIS.Models;
namespace MEMIS.Controllers
{
    public class NDP_HDController : Controller
    {
        private readonly AppDbContext _context;

        public NDP_HDController(AppDbContext context)
        {
            _context = context;
        }

        // GET: NDP_HD
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.NDP_HD.Skip(offset).Take(pageSize);
            var result = new PagedResult<NDP_HD>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.NDP_HD.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            }; 
            return View(result);
        }

        // GET: NDP_HD/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NDP_HD == null)
            {
                return NotFound();
            }

            var nDP_HD = await _context.NDP_HD
                .FirstOrDefaultAsync(m => m.ID == id);
            if (nDP_HD == null)
            {
                return NotFound();
            }

            return View(nDP_HD);
        }

        // GET: NDP_HD/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NDP_HD/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ndpname,fromyear,toyear")] NDP_HD nDP_HD)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nDP_HD);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nDP_HD);
        }

        // GET: NDP_HD/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NDP_HD == null)
            {
                return NotFound();
            }

            var nDP_HD = await _context.NDP_HD.FindAsync(id);
            if (nDP_HD == null)
            {
                return NotFound();
            }
            return View(nDP_HD);
        }

        // POST: NDP_HD/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ndpname,active,fromyear,toyear")] NDP_HD nDP_HD)
        {
            if (id != nDP_HD.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nDP_HD);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NDP_HDExists(nDP_HD.ID))
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
            return View(nDP_HD);
        }

        // GET: NDP_HD/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NDP_HD == null)
            {
                return NotFound();
            }

            var nDP_HD = await _context.NDP_HD
                .FirstOrDefaultAsync(m => m.ID == id);
            if (nDP_HD == null)
            {
                return NotFound();
            }

            return View(nDP_HD);
        }

        // POST: NDP_HD/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NDP_HD == null)
            {
                return Problem("Entity set 'AppDbContext.NDP_HD'  is null.");
            }
            var nDP_HD = await _context.NDP_HD.FindAsync(id);
            if (nDP_HD != null)
            {
                _context.NDP_HD.Remove(nDP_HD);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NDP_HDExists(int id)
        {
          return (_context.NDP_HD?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
