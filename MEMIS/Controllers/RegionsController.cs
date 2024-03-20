using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;
using Microsoft.AspNetCore.Identity;
using MEMIS.Models;

namespace MEMIS.Controllers
{
    public class RegionsController : Controller
    {
        private readonly Data.AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegionsController(UserManager<ApplicationUser> userManager, Data.AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Regions
        public async Task<IActionResult> Index()
        {
              return _context.Region != null ? 
                          View(await _context.Region.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Region'  is null.");
        }

        // GET: Regions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Region == null)
            {
                return NotFound();
            }

            var region = await _context.Region 
                .FirstOrDefaultAsync(m => m.intRegion == id);
            if (region == null)
            {
                return NotFound();
            }

            return View(region);
        }

        // GET: Regions/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.DirectorateList = _context.Directorates == null ? new List<Directorate>() : await _context.Directorates.ToListAsync();
            var hor = _context.Users
                 .Select(u => new SelectListItem
                 {
                     Value = u.Id,
                     Text = u.UserName
                 })
                 .ToList();
            var regCord = _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                })
                .ToList();
            ViewBag.hor = hor;
            ViewBag.regCord = regCord;
            return View();
        }

        // POST: Regions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( RegionDto dto)
        {
            if (ModelState.IsValid)
            {
                var region = new Region
                {
                    intRegion = Guid.NewGuid(),
                    regionCode = dto.regionCode , 
                    regionName = dto.regionName , 
                    regCoordinator = dto.regCoordinator ,
                    intHead = dto.intHead 
                }; 
                _context.Add(region);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } 
            var hor = _context.Users
                 .Select(u => new SelectListItem
                 {
                     Value = u.Id,
                     Text = u.UserName
                 })
                 .ToList();
            var regCord = _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                })
                .ToList();
            ViewBag.hor = hor;
            ViewBag.regCord = regCord;
            return View(dto);
        }

        // GET: Regions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Region == null)
            {
                return NotFound();
            }

            var region = await _context.Region.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            RegionDto dto = new RegionDto()
            {
                intRegion = region.intRegion,
                regionCode = region.regionCode,
                regionName = region.regionName,
                regCoordinator = region.regCoordinator,
                intHead = region.intHead
            };
            var hor = _context.Users
                 .Select(u => new SelectListItem
                 {
                     Value = u.Id,
                     Text = u.UserName
                 })
                 .ToList();
            var regCord = _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                })
                .ToList();
            ViewBag.hor = hor;
            ViewBag.regCord = regCord;
            return View(dto);
        }

        // POST: Regions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("intRegion,regionCode,regionName,intHead,regCoordinator")] RegionDto dto)
        {
            if (id != dto.intRegion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Region region = new Region()
                    {
                        intRegion =dto.intRegion,
                        regionCode = dto.regionCode,
                        regionName = dto.regionName,
                        regCoordinator = dto.regCoordinator,
                        intHead = dto.intHead
                    };
                    _context.Update(region);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index)); 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegionExists(dto.intRegion))
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
            var hor = _context.Users
                 .Select(u => new SelectListItem
                 {
                     Value = u.Id,
                     Text = u.UserName
                 })
                 .ToList();
            var regCord = _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                })
                .ToList();
            ViewBag.hor = hor;
            ViewBag.regCord = regCord;
            return View(dto);
        }

        // GET: Regions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Region == null)
            {
                return NotFound();
            }

            var region = await _context.Region
                .FirstOrDefaultAsync(m => m.intRegion == id);
            if (region == null)
            {
                return NotFound();
            }

            return View(region);
        }

        // POST: Regions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Region == null)
            {
                return Problem("Entity set 'AppDbContext.Region'  is null.");
            }
            var region = await _context.Region.FindAsync(id);
            if (region != null)
            {
                _context.Region.Remove(region);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegionExists(Guid id)
        {
          return (_context.Region?.Any(e => e.intRegion == id)).GetValueOrDefault();
        }
    }
}
