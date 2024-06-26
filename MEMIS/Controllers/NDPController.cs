using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;
using cloudscribe.Pagination.Models;
using System.IO.Compression;
using MEMIS.ViewModels;

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
    public IActionResult Index(int pageNumber = 1)
    {
      string financialYear = HttpContext.Session.GetString("FYEAR");
      NDPViewModel viewModel = new NDPViewModel();
      NDPFile nDPFile = new NDPFile();
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
        viewModel.NDP = result;
        viewModel.NDPFile = _context.NDPFile.Where(x => x.FinancialYear == int.Parse(financialYear)).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        
        return View(viewModel);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.NDP'  is null.");
      }

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
            var file = Request.Form.Files;
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
            var file = Request.Form.Files;

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

    public async Task<IActionResult> Download(int id)
    {
      var ndpFile = await _context.NDPFile.FindAsync(id)
;
      if (ndpFile == null || ndpFile.FileContent == null || ndpFile.FileContent.Length == 0)
      {
        return NotFound();
      }

      // Return the file as a stream
      return File(ndpFile.FileContent, "application/octet-stream", ndpFile.FileName);
    }

    public async Task<IActionResult> UploadFile()
    {
      var file = Request.Form.Files;
      string financialYear = HttpContext.Session.GetString("FYEAR");

      NDPFile nDPFile = new NDPFile();

      if (ModelState.IsValid)
      {
        if (file != null && file.Count > 0)
        {
          using (var memoryStream = new MemoryStream())
          {
            await file[0].CopyToAsync(memoryStream);
            nDPFile.FileContent = memoryStream.ToArray();
            nDPFile.FileName = file[0].FileName;
            nDPFile.FinancialYear = int.Parse(financialYear);
            nDPFile.CreatedDate = DateTime.Now;
          }
        }
        _context.Add(nDPFile);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View();
    }

  }
}
