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
    public class DepartmentsController : Controller
    {
        private readonly Data.AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public DepartmentsController(UserManager<ApplicationUser> userManager, Data.AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
              return _context.Departments != null ? 
                          View(await _context.Departments.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Departments'  is null.");
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.Include(d=>d.DirectorateFk)
                .FirstOrDefaultAsync(m => m.intDept == id);

         
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.DirectorateList = _context.Directorates == null ? new List<Directorate>() : await _context.Directorates.ToListAsync();
            var hod = _context.Users
                 .Select(u => new SelectListItem
                 {
                     Value = u.Id,
                     Text = u.UserName
                 })
                 .ToList();
            ViewBag.hod = hod;
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentDto viewModel)
        {

            if (ModelState.IsValid)
            {
                var department = new Department
                {
                    intDept = Guid.NewGuid(),
                    deptCode = viewModel.deptCode,
                    deptName = viewModel.deptName,
                    intDir = viewModel.intDir,
                    intHod = viewModel.intHod
                };

                _context.Add(department);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Departments");
            }



            return View(viewModel);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

           
            var department = await _context.Departments.FindAsync(id);

            DepartmentDto dto = new DepartmentDto
            {
                intDept = department.intDept,
                deptCode=department.deptCode,
                deptName=department.deptName,
                intDir = department.intDir,
                intHod=department.intHod
            };

            ViewBag.DirectorateList = _context.Directorates == null ? new List<Directorate>() : await _context.Directorates.ToListAsync();
            var hod = _context.Users
                 .Select(u => new SelectListItem
                 {
                     Value = u.Id,
                     Text = u.UserName
                 })
                 .ToList();
            ViewBag.hod = hod;

            if (department == null)
            {
                return NotFound();
            }
            return View(dto);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, DepartmentDto viewModel)
        {
            if (id != viewModel.intDept)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Department department = new Department
                    {
                        intDept = viewModel.intDept,
                        deptCode = viewModel.deptCode,
                        deptName = viewModel.deptName,
                        intDir = viewModel.intDir,
                        intHod = viewModel.intHod
                    };
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(viewModel.intDept))
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
            return View(viewModel);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.intDept == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        { 
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(Guid id)
        {
          return (_context.Departments?.Any(e => e.intDept == id)).GetValueOrDefault();
        }
    }
}
