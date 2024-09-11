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
using Microsoft.AspNetCore.Identity;
using MEMIS.Helpers.ExcelReports;

namespace MEMIS.Controllers
{
  public class AnnualImplemtationPlansController : Controller
  {
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    public AnnualImplemtationPlansController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    // GET: AnnualImplemtationPlans
    public async Task<IActionResult> Index(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      var appDbContext = _context.AnnualImplemtationPlan.Include(a => a.ActivityFk).Include(a => a.DepartmentFk).Include(a => a.FocusAreaFk).Include(a => a.StrategicActionFk).Include(a => a.StrategicInterventionFk).Include(a => a.StrategicObjectiveFk);
      var result = new PagedResult<AnnualImplemtationPlan>
      {
        Data = await appDbContext.AsNoTracking().ToListAsync(),
        TotalItems = _context.AnnualImplemtationPlan.Count(),
        PageNumber = pageNumber,
        PageSize = pageSize
      };

      return View(result);
    }

    public async Task<IActionResult> ExportToExcel()
    {
      try
      {
        var list = await _context.ProgramImplementationPlan.ToListAsync();
        var stream = ExportHandler.StrategicImplementationPlanReport(list);
        stream.Position = 0;
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Strategic Implementation Plan.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }


    // GET: AnnualImplemtationPlans/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null || _context.AnnualImplemtationPlan == null)
      {
        return NotFound();
      }

      var annualImplemtationPlan = await _context.AnnualImplemtationPlan
          .Include(a => a.ActivityFk)
          .Include(a => a.DepartmentFk)
          .Include(a => a.FocusAreaFk)
          .Include(a => a.StrategicActionFk)
          .Include(a => a.StrategicInterventionFk)
          .Include(a => a.StrategicObjectiveFk)
          .FirstOrDefaultAsync(m => m.Id == id);
      if (annualImplemtationPlan == null)
      {
        return NotFound();
      }

      return View(annualImplemtationPlan);
    }

    // GET: AnnualImplemtationPlans/Create
    public IActionResult Create()
    {
      ViewData["intActivity"] = new SelectList(_context.Activity, "intActivity", "activityName");
      ViewData["intDept"] = new SelectList(_context.Departments, "intDept", "deptName");
      ViewData["intFocus"] = new SelectList(_context.FocusArea, "intFocus", "FocusAreaName");
      ViewData["intAction"] = new SelectList(_context.StrategicAction, "intAction", "actionName");
      ViewData["intIntervention"] = new SelectList(_context.StrategicIntervention, "intIntervention", "InterventionName");
      ViewData["intObjective"] = new SelectList(_context.StrategicObjective, "intObjective", "ObjectiveName"); 
      return View();
    }

    // POST: AnnualImplemtationPlans/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,subProgram,intFocus,intObjective,intIntervention,intAction,intActivity,baseline,Year,annualTarget,meansofVerification,Risk,intDept")] AnnualImplemtationPlan annualImplemtationPlan)
    {
      if (ModelState.IsValid)
      {
        _context.Add(annualImplemtationPlan);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      ViewData["intActivity"] = new SelectList(_context.Activity, "intActivity", "activityName", annualImplemtationPlan.intActivity);
      ViewData["intDept"] = new SelectList(_context.Departments, "intDept", "deptName", annualImplemtationPlan.intDept);
      ViewData["intFocus"] = new SelectList(_context.FocusArea, "intFocus", "FocusAreaName", annualImplemtationPlan.intFocus);
      ViewData["intAction"] = new SelectList(_context.StrategicAction, "intAction", "actionName", annualImplemtationPlan.intAction);
      ViewData["intIntervention"] = new SelectList(_context.StrategicIntervention, "intIntervention", "InterventionName", annualImplemtationPlan.intIntervention);
      ViewData["intObjective"] = new SelectList(_context.StrategicObjective, "intObjective", "ObjectiveName", annualImplemtationPlan.intObjective);
      return View(annualImplemtationPlan);
    }

    // GET: AnnualImplemtationPlans/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.AnnualImplemtationPlan == null)
      {
        return NotFound();
      }

      var annualImplemtationPlan = await _context.AnnualImplemtationPlan.FindAsync(id);
      if (annualImplemtationPlan == null)
      {
        return NotFound();
      }
      ViewData["intActivity"] = new SelectList(_context.Activity, "intActivity", "activityName", annualImplemtationPlan.intActivity);
      ViewData["intDept"] = new SelectList(_context.Departments, "intDept", "deptName", annualImplemtationPlan.intDept);
      ViewData["intFocus"] = new SelectList(_context.FocusArea, "intFocus", "FocusAreaName", annualImplemtationPlan.intFocus);
      ViewData["intAction"] = new SelectList(_context.StrategicAction, "intAction", "actionName", annualImplemtationPlan.intAction);
      ViewData["intIntervention"] = new SelectList(_context.StrategicIntervention, "intIntervention", "InterventionName", annualImplemtationPlan.intIntervention);
      ViewData["intObjective"] = new SelectList(_context.StrategicObjective, "intObjective", "ObjectiveName", annualImplemtationPlan.intObjective);
      return View(annualImplemtationPlan);
    }

    // POST: AnnualImplemtationPlans/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,subProgram,intFocus,intObjective,intIntervention,intAction,intActivity,baseline,Year,annualTarget,meansofVerification,Risk,intDept")] AnnualImplemtationPlan annualImplemtationPlan)
    {
      if (id != annualImplemtationPlan.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(annualImplemtationPlan);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!AnnualImplemtationPlanExists(annualImplemtationPlan.Id))
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
      ViewData["intActivity"] = new SelectList(_context.Activity, "intActivity", "activityName", annualImplemtationPlan.intActivity);
      ViewData["intDept"] = new SelectList(_context.Departments, "intDept", "deptName", annualImplemtationPlan.intDept);
      ViewData["intFocus"] = new SelectList(_context.FocusArea, "intFocus", "FocusAreaName", annualImplemtationPlan.intFocus);
      ViewData["intAction"] = new SelectList(_context.StrategicAction, "intAction", "actionName", annualImplemtationPlan.intAction);
      ViewData["intIntervention"] = new SelectList(_context.StrategicIntervention, "intIntervention", "InterventionName", annualImplemtationPlan.intIntervention);
      ViewData["intObjective"] = new SelectList(_context.StrategicObjective, "intObjective", "ObjectiveName", annualImplemtationPlan.intObjective);
      return View(annualImplemtationPlan);
    }

    // GET: AnnualImplemtationPlans/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.AnnualImplemtationPlan == null)
      {
        return NotFound();
      }

      var annualImplemtationPlan = await _context.AnnualImplemtationPlan
          .Include(a => a.ActivityFk)
          .Include(a => a.DepartmentFk)
          .Include(a => a.FocusAreaFk)
          .Include(a => a.StrategicActionFk)
          .Include(a => a.StrategicInterventionFk)
          .Include(a => a.StrategicObjectiveFk)
          .FirstOrDefaultAsync(m => m.Id == id);
      if (annualImplemtationPlan == null)
      {
        return NotFound();
      }

      return View(annualImplemtationPlan);
    }
    public async Task<IActionResult> RegionSettings(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;

      var appDbContext = _context.AnnualImplemtationPlan.Include(a => a.ActivityFk).Include(a => a.DepartmentFk).Include(a => a.FocusAreaFk).Include(a => a.StrategicActionFk).Include(a => a.StrategicInterventionFk).Include(a => a.StrategicObjectiveFk)
      .Where(m => m.regStatus == 0);

      var result = new PagedResult<AnnualImplemtationPlan>
      {
        Data = await appDbContext.AsNoTracking().ToListAsync(),
        TotalItems = _context.AnnualImplemtationPlan.Count(),
        PageNumber = pageNumber,
        PageSize = pageSize
      };

      return View(result);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      if (_context.AnnualImplemtationPlan == null)
      {
        return Problem("Entity set 'AppDbContext.AnnualImplemtationPlan'  is null.");
      }
      var annualImplemtationPlan = await _context.AnnualImplemtationPlan.FindAsync(id);
      if (annualImplemtationPlan != null)
      {
        _context.AnnualImplemtationPlan.Remove(annualImplemtationPlan);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    // POST: AnnualImplemtationPlans/Delete/5
    [HttpPost, ActionName("RegionSettings")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToRegion(int id)
    {
      if (_context.AnnualImplemtationPlan == null)
      {
        return Problem("Entity set 'AppDbContext.AnnualImplemtationPlan'  is null.");
      }
      var annualImplemtationPlan = await _context.AnnualImplemtationPlan.FindAsync(id);
      if (annualImplemtationPlan != null)
      {
        annualImplemtationPlan.regStatus = 1;
        await _context.SaveChangesAsync();
      }
      return RedirectToAction(nameof(RegionSettings));
    }

    private bool AnnualImplemtationPlanExists(int id)
    {
      return (_context.AnnualImplemtationPlan?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
