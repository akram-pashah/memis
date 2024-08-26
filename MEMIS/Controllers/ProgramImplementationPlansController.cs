using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;
using cloudscribe.Pagination.Models;
using MEMIS.Helpers.ExcelReports;

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
            .Include(p=>p.StrategicObjectiveFK)
            .Include(p => p.StrategicInterventionFK)
            .Include(p => p.StrategicActionFK)
            .Include(p => p.ActivityFK)
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
      ViewData["intObjective"] = new SelectList(_context.StrategicObjective, "intObjective", "ObjectiveName");
      ViewData["intIntervention"] = new SelectList(_context.StrategicIntervention.Select(s => new { intIntervention = s.intIntervention, InterventionName = '(' + s.InterventionCode + ')' + s.InterventionName }), "intIntervention", "InterventionName");
      ViewData["intAction"] = new SelectList(_context.StrategicAction.Select(s => new { intAction = s.intAction, actionName = '(' + s.actionCode + ')' + s.actionName }), "intAction", "actionName");
      ViewData["intActivity"] = new SelectList(_context.Activity.Select(a => new { intActivity = a.intActivity, activityName = '(' + a.activityCode + ')' + a.activityName }), "intActivity", "activityName");
      return View();
    }

    // POST: ProgramImplementationPlans/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,intObjective,intIntervention,intAction,intActivity,Output,OutputTarget,ResponsibleParty,MeansofVerification,FY1,FY2,FY3,FY4,FY5")] ProgramImplementationPlan programImplementationPlan)
    {
      if (ModelState.IsValid)
      {
        _context.Add(programImplementationPlan);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      ViewData["intActivity"] = new SelectList(_context.Activity.Select(a => new { intActivity = a.intActivity, activityName = '(' + a.activityCode + ')' + a.activityName }), "intActivity", "activityName", programImplementationPlan.intActivity); 
      ViewData["intAction"] = new SelectList(_context.StrategicAction.Select(s => new { intAction = s.intAction, actionName = '(' + s.actionCode + ')' + s.actionName }), "intAction", "actionName", programImplementationPlan.intAction);
      ViewData["intIntervention"] = new SelectList(_context.StrategicIntervention.Select(s => new { intIntervention = s.intIntervention, InterventionName = '(' + s.InterventionCode + ')' + s.InterventionName }), "intIntervention", "InterventionName", programImplementationPlan.intIntervention);
      ViewData["intObjective"] = new SelectList(_context.StrategicObjective, "intObjective", "ObjectiveName", programImplementationPlan.intObjective);
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
      ViewData["intActivity"] = new SelectList(_context.Activity.Select(a => new { intActivity=a.intActivity, activityName = '(' + a.activityCode + ')' + a.activityName }), "intActivity", "activityName", programImplementationPlan.intActivity);
      ViewData["intAction"] = new SelectList(_context.StrategicAction.Select(s => new { intAction = s.intAction, actionName = '(' + s.actionCode + ')' + s.actionName }), "intAction", "actionName", programImplementationPlan.intAction);
      ViewData["intIntervention"] = new SelectList(_context.StrategicIntervention.Select(s => new { intIntervention =s.intIntervention, InterventionName = '('+s.InterventionCode+')' +s.InterventionName }), "intIntervention", "InterventionName", programImplementationPlan.intIntervention);
      ViewData["intObjective"] = new SelectList(_context.StrategicObjective, "intObjective", "ObjectiveName", programImplementationPlan.intObjective);
      return View(programImplementationPlan);
    }

    // POST: ProgramImplementationPlans/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,intObjective,intIntervention,intAction,intActivity,Output,OutputTarget,ResponsibleParty,MeansofVerification,FY1,FY2,FY3,FY4,FY5")] ProgramImplementationPlan programImplementationPlan)
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
      ViewData["intActivity"] = new SelectList(_context.Activity, "intActivity", "activityName", programImplementationPlan.intActivity);
      ViewData["intAction"] = new SelectList(_context.StrategicAction, "intAction", "actionName", programImplementationPlan.intAction);
      ViewData["intIntervention"] = new SelectList(_context.StrategicIntervention, "intIntervention", "InterventionName", programImplementationPlan.intIntervention);
      ViewData["intObjective"] = new SelectList(_context.StrategicObjective, "intObjective", "ObjectiveName", programImplementationPlan.intObjective);
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
