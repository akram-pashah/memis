using MEMIS.Data;
using MEMIS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
namespace MEMIS.Controllers
{
    public class DirectoratesController : Controller
    {
        private readonly Data.AppDbContext _context;

        public DirectoratesController(Data.AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            return _context.Directorates != null ?
                          View(await _context.Directorates.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Directorates'  is null."); 
        }
        public IActionResult Create()
        {
            var directors = _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                })
                .ToList();

            ViewBag.Directors = directors;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DirectorateDto viewModel)
        {

            if (ModelState.IsValid)
            {
                var directorate = new Directorate
                {
                    intDir = Guid.NewGuid(),
                    dirCode = viewModel.DirCode,
                    dirName = viewModel.DirName,
                    director = viewModel.director
                };

                _context.Add(directorate);
                await _context.SaveChangesAsync();
                var directors = _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                })
                .ToList();

                ViewBag.Directors = directors;

                return RedirectToAction("Index", "Directorates");
            }

        

            return View(viewModel);
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            var directorate = await _context.Directorates.FindAsync(id);
            if (directorate == null)
            {
                return NotFound();
            }

            var directors = _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName,
                    Selected = u.Id == directorate.director
                })
                .ToList();

            var viewModel = new DirectorateDto
            {
                IntDir = directorate.intDir,
                DirCode = directorate.dirCode,
                DirName = directorate.dirName,
                director = directorate.director 
            };
            ViewBag.Directors = directors;
            return View(viewModel);
        }

        // POST: Directorate/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, DirectorateDto viewModel)
        {
            if (id != viewModel.IntDir)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var directorate = await _context.Directorates.FindAsync(id);
                    if (directorate == null)
                    {
                        return NotFound();
                    }

                    directorate.dirCode = viewModel.DirCode;
                    directorate.dirName = viewModel.DirName;
                    directorate.director = viewModel.director;

                    _context.Update(directorate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectorateExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index", "Directorates");
            }

            //viewModel.Directors = _context.Users
            //    .Select(u => new SelectListItem
            //    {
            //        Value = u.Id,
            //        Text = u.UserName
            //    })
            //    .ToList();

            return View(viewModel);
        }

        private bool DirectorateExists(Guid id)
        {
            return _context.Directorates.Any(d => d.intDir == id);
        }
    }
}
