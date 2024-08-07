using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;
using MEMIS.Models;

namespace MEMIS.Controllers.ME
{
  public class ActivityAssessmentController : Controller
  {
    private readonly Data.AppDbContext _context;

    public ActivityAssessmentController(Data.AppDbContext context)
    {
      _context = context;
    }

    // GET: ActivityAssessment
    public async Task<IActionResult> Index()
    {
      var appDbContext = await GetActivityAssestsDetails(0);
      return View(appDbContext);
    }
    public async Task<IActionResult> HodVerification()
    {
      var appDbContext = await GetActivityAssestsDetails(0, true);
      return View(appDbContext);
    }
    public async Task<IActionResult> VerificationDir()
    {
      var appDbContext = await GetActivityAssestsDetails(1);
      return View(appDbContext);
    }
    public async Task<IActionResult> Consolidation()
    {
      //var deptAssessments = _context.ActivityAssessment.Where(x => x.ActivityAssesmentStatus == )
      var appDbContext = await GetActivityAssestsDetails(0);
      return View(appDbContext);
    }
    public async Task<IActionResult> VerificationBpd()
    {
      var appDbContext = await GetActivityAssestsDetails(3);
      return View(appDbContext);
    }
    public async Task<IActionResult> Approval()
    {
      var appDbContext = await GetActivityAssestsDetails(5);
      return View(appDbContext);
    }
    public async Task<IActionResult> RegionalIndex()
    {
      var appDbContext = await GetActivityAssestsRegionalDetails(0);
      return View(appDbContext);
    }
    public async Task<IActionResult> RegionalHodVerification()
    {
      var appDbContext = await GetActivityAssestsRegionalDetails(0, true);
      return View(appDbContext);
    }
    public async Task<IActionResult> RegionalVerificationDir()
    {
      var appDbContext = await GetActivityAssestsRegionalDetails(1);
      return View(appDbContext);
    }
    public async Task<IActionResult> RegionalConsolidation()
    {
      var appDbContext = await GetActivityAssestsRegionalDetails(0);
      return View(appDbContext);
    }
    public async Task<IActionResult> RegionalVerificationBpd()
    {
      var appDbContext = await GetActivityAssestsRegionalDetails(3);
      return View(appDbContext);
    }
    public async Task<IActionResult> RegionalApproval()
    {
      var appDbContext = await GetActivityAssestsRegionalDetails(5);
      return View(appDbContext);
    }



    private async Task<List<ActivityAssessment>> GetActivityAssestsDetails(int Status, bool isHod = false)
    {

      var query = _context.ActivityAssessment.Include(x => x.QuaterlyPlans).Include(a => a.ImplementationStatus).AsQueryable();

      if (Status != 0 && Status != null)
      {
        query = query.Where(s => s.ActivityAssesmentStatus == Status);
      }

      if (isHod)
      {
        query = query.Where(x => x.QuaterlyPlans.Any() && x.QuaterlyPlans.Where(y => !string.IsNullOrEmpty(y.QAchievement)).Any() && x.ActivityAssesmentStatus == Status);
      }

      var appDbContext = await query.ToListAsync();

      if (appDbContext.Count > 0)
      {
        foreach (var item in appDbContext)
        {
          // Retrieve QuaterlyPlans for each DeptPlan item
          var quaterlyplans = _context.QuaterlyPlans
              .Where(x => x.ActivityAssessmentId == item.intDeptPlan)
              .ToList();

          // Calculate sums for each quarter
          item.Q1Target = quaterlyplans.Where(x => x.Quarter == "1").Sum(x => x.QTarget);
          item.Q1Budget = quaterlyplans.Where(x => x.Quarter == "1").Sum(x => x.QBudget);
          item.Q1Actual = quaterlyplans.Where(x => x.Quarter == "1").Sum(x => x.QActual);
          item.Q1AmtSpent = quaterlyplans.Where(x => x.Quarter == "1").Sum(x => x.QAmtSpent);
          item.Q1Justification = quaterlyplans.Where(x => x.Quarter == "1").FirstOrDefault()?.QJustification;
          item.Q2Target = quaterlyplans.Where(x => x.Quarter == "2").Sum(x => x.QTarget);
          item.Q2Budget = quaterlyplans.Where(x => x.Quarter == "2").Sum(x => x.QBudget);
          item.Q2Actual = quaterlyplans.Where(x => x.Quarter == "2").Sum(x => x.QActual);
          item.Q2AmtSpent = quaterlyplans.Where(x => x.Quarter == "2").Sum(x => x.QAmtSpent);
          item.Q2Justification = quaterlyplans.Where(x => x.Quarter == "2").FirstOrDefault()?.QJustification;
          item.Q3Target = quaterlyplans.Where(x => x.Quarter == "3").Sum(x => x.QTarget);
          item.Q3Budget = quaterlyplans.Where(x => x.Quarter == "3").Sum(x => x.QBudget);
          item.Q3Actual = quaterlyplans.Where(x => x.Quarter == "3").Sum(x => x.QActual);
          item.Q3AmtSpent = quaterlyplans.Where(x => x.Quarter == "3").Sum(x => x.QAmtSpent);
          item.Q3Justification = quaterlyplans.Where(x => x.Quarter == "3").FirstOrDefault()?.QJustification;
          item.Q4Target = quaterlyplans.Where(x => x.Quarter == "4").Sum(x => x.QTarget);
          item.Q4Budget = quaterlyplans.Where(x => x.Quarter == "4").Sum(x => x.QBudget);
          item.Q4Actual = quaterlyplans.Where(x => x.Quarter == "4").Sum(x => x.QActual);
          item.Q4AmtSpent = quaterlyplans.Where(x => x.Quarter == "4").Sum(x => x.QAmtSpent);
          item.Q4Justification = quaterlyplans.Where(x => x.Quarter == "4").FirstOrDefault()?.QJustification;
        }
      }
      return appDbContext;
    }

    public async Task<IActionResult> EditRegionalTarget(int id)
    {
      try
      {
        ActivityAssessmentRegion? region = await _context.ActivityAssessmentRegion.Where(x => x.intRegionAssess == id).FirstOrDefaultAsync();
        if (region == null)
        {
          return NotFound();
        }

        ActivityAssessmentDto activityAssessDto = new ActivityAssessmentDto();
        activityAssessDto.intAssess = region.intAssess;
        activityAssessDto.intRegionAssess = region.intRegionAssess;
        activityAssessDto.budgetAmount = region.budgetAmount;
        activityAssessDto.QTarget = region.QTarget;
        activityAssessDto.QBudget = region.QBudget;
        activityAssessDto.QuaterlyPlans = await _context.QuaterlyPlans.Where(x => x.ActivityAccessId == region.intAssess).ToListAsync();
        ViewData["Quarter"] = ListHelper.Quarter();
        return View(activityAssessDto);
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }

    [HttpPost]
    public async Task<IActionResult> EditRegionalTarget(ActivityAssessmentDto region)
    {
      try
      {
        ActivityAssessRegion activityAssessRegion = _context.ActivityAssessRegion.Where(x => x.intAssess == region.intAssess).FirstOrDefault();

        activityAssessRegion.budgetAmount = region.budgetAmount;

        _context.ActivityAssessRegion.Update(activityAssessRegion);

        if (region.QuaterlyPlans.Count > 0)
        {

          foreach (var quat in region.QuaterlyPlans)
          {

            if (quat.Id != 0)
            {
              _context.Entry(quat).State = EntityState.Modified;
              await _context.SaveChangesAsync();
            }
            else
            {
              quat.ActivityAssessmentId = region.intAssess;
              await _context.QuaterlyPlans.AddAsync(quat);
              await _context.SaveChangesAsync();
            }
          }

          ActivityAssessmentRegion? activityAssessmentRegion = _context.ActivityAssessmentRegion.Where(x => x.intAssess == activityAssessRegion.intAssess).FirstOrDefault();
          if (activityAssessmentRegion == null)
          {
            activityAssessmentRegion = new ActivityAssessmentRegion()
            {
              intAssess = activityAssessRegion.intAssess,
              intRegion = activityAssessRegion.intRegion,
              budgetAmount = activityAssessRegion.budgetAmount,
              Quarter = activityAssessRegion.Quarter,
            };
            _context.ActivityAssessmentRegion.Add(activityAssessmentRegion);
          }
          else
          {
            activityAssessmentRegion.intAssess = activityAssessRegion.intAssess;
            activityAssessmentRegion.intRegion = activityAssessRegion.intRegion;
            activityAssessmentRegion.budgetAmount = activityAssessRegion.budgetAmount;
            activityAssessmentRegion.Quarter = activityAssessRegion.Quarter;

            foreach (var item in region.QuaterlyPlans)
            {
              item.ActivityAssessmentRegionId = activityAssessmentRegion.intRegionAssess;
            }
          }

          _context.SaveChanges();
        }
        ViewData["Quarter"] = ListHelper.Quarter();
        return RedirectToAction(nameof(RegionalIndex));
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }


    private async Task<List<ActivityAssessmentRegion>> GetActivityAssestsRegionalDetails(int Status, bool isHod = false)
    {
      var query = _context.ActivityAssessmentRegion
        .Include(a => a.ActivityAssessFk)
          .ThenInclude(x => x.QuaterlyPlans)
        .Include(x => x.ActivityAssessFk)
          .ThenInclude(x => x.StrategicIntervention)
            .ThenInclude(x => x.StrategicObjective)
        .Include(x => x.ActivityAssessFk)
          .ThenInclude(x => x.StrategicAction)
        .Include(x => x.ActivityAssessFk)
          .ThenInclude(x => x.ActivityFk)
          .Where(x => x.ApprStatus == Status)
        .AsQueryable();

      if (isHod)
      {
        query = query.Where(x => x.ActivityAssessFk.QuaterlyPlans.Where(x => !string.IsNullOrEmpty(x.QAchievement)).Any());
      } else if(Status == 0)
      {
        query = query.Where(x => x.ActivityAssessFk.QuaterlyPlans.Count != 4);
      }

      var appDbContext = await query.ToListAsync();

      if (appDbContext.Count > 0)
      {
        foreach (var item in appDbContext)
        {
          // Retrieve QuaterlyPlans for each DeptPlan item
          var quaterlyplans = _context.QuaterlyPlans
              .Where(x => x.ActivityAssessmentId == item.intRegionAssess)
              .ToList();

        }
      }
      return appDbContext;
    }

    // GET: ActivityAssessment/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null || _context.ActivityAssessment == null)
      {
        return NotFound();
      }

      var activityAssessment = await _context.ActivityAssessment
          .Include(a => a.ImplementationStatus)
          .FirstOrDefaultAsync(m => m.intDeptPlan == id);
      if (activityAssessment == null)
      {
        return NotFound();
      }
      if (activityAssessment != null)
      {

        // Retrieve QuaterlyPlans for each DeptPlan item
        var quaterlyplans = _context.QuaterlyPlans
            .Where(x => x.ActivityAssessmentId == activityAssessment.intDeptPlan)
            .ToList();

        // Calculate sums for each quarter
        activityAssessment.Q1Target = quaterlyplans.Where(x => x.Quarter == "1").Sum(x => x.QTarget);
        activityAssessment.Q1Budget = quaterlyplans.Where(x => x.Quarter == "1").Sum(x => x.QBudget);
        activityAssessment.Q1Actual = quaterlyplans.Where(x => x.Quarter == "1").Sum(x => x.QActual);
        activityAssessment.Q1AmtSpent = quaterlyplans.Where(x => x.Quarter == "1").Sum(x => x.QAmtSpent);
        activityAssessment.Q1Justification = quaterlyplans.Where(x => x.Quarter == "1").FirstOrDefault()?.QJustification;
        activityAssessment.Q2Target = quaterlyplans.Where(x => x.Quarter == "2").Sum(x => x.QTarget);
        activityAssessment.Q2Budget = quaterlyplans.Where(x => x.Quarter == "2").Sum(x => x.QBudget);
        activityAssessment.Q2Actual = quaterlyplans.Where(x => x.Quarter == "2").Sum(x => x.QActual);
        activityAssessment.Q2AmtSpent = quaterlyplans.Where(x => x.Quarter == "2").Sum(x => x.QAmtSpent);
        activityAssessment.Q2Justification = quaterlyplans.Where(x => x.Quarter == "2").FirstOrDefault()?.QJustification;
        activityAssessment.Q3Target = quaterlyplans.Where(x => x.Quarter == "3").Sum(x => x.QTarget);
        activityAssessment.Q3Budget = quaterlyplans.Where(x => x.Quarter == "3").Sum(x => x.QBudget);
        activityAssessment.Q3Actual = quaterlyplans.Where(x => x.Quarter == "3").Sum(x => x.QActual);
        activityAssessment.Q3AmtSpent = quaterlyplans.Where(x => x.Quarter == "3").Sum(x => x.QAmtSpent);
        activityAssessment.Q3Justification = quaterlyplans.Where(x => x.Quarter == "3").FirstOrDefault()?.QJustification;
        activityAssessment.Q4Target = quaterlyplans.Where(x => x.Quarter == "4").Sum(x => x.QTarget);
        activityAssessment.Q4Budget = quaterlyplans.Where(x => x.Quarter == "4").Sum(x => x.QBudget);
        activityAssessment.Q4Actual = quaterlyplans.Where(x => x.Quarter == "4").Sum(x => x.QActual);
        activityAssessment.Q4AmtSpent = quaterlyplans.Where(x => x.Quarter == "4").Sum(x => x.QAmtSpent);
        activityAssessment.Q4Justification = quaterlyplans.Where(x => x.Quarter == "4").FirstOrDefault()?.QJustification;
      }

      return View(activityAssessment);
    }

    // GET: ActivityAssessment/Create
    public IActionResult Create()
    {
      ViewData["ImpStatusId"] = new SelectList(_context.ImplementationStatus, "ImpStatusId", "ImpStatusName");
      ActivityAssessment activityAssessment = new ActivityAssessment();
      return View(activityAssessment);
    }

    // POST: ActivityAssessment/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("intDeptPlan,strategicObjective,strategicIntervention,StrategicAction,activity,outputIndicator,baseline,budgetCode,unitCost,comparativeTarget,justification,budgetAmount,Q1Target,Q1Budget,Q1Actual,Q1AmtSpent,Q1Justification,Q2Target,Q2Budget,Q2Actual,Q2AmtSpent,Q2Justification,Q3Target,Q3Budget,Q3Actual,Q3AmtSpent,Q3Justification,Q4Target,Q4Budget,Q4Actual,Q4AmtSpent,Q4Justification,AnnualAchievement,TotAmtSpent,ImpStatusId,AnnualJustification,QuaterlyPlans")] ActivityAssessment activityAssessment)
    {
      if (ModelState.IsValid)
      {
        _context.Add(activityAssessment);
        await _context.SaveChangesAsync();
        if (activityAssessment.QuaterlyPlans.Count > 0)
        {

          foreach (var quat in activityAssessment.QuaterlyPlans)
          {
            if (quat.Id != 0)
            {
              _context.Entry(quat).State = EntityState.Modified;
              await _context.SaveChangesAsync();
            }
            else
            {
              quat.ActivityAssessmentId = activityAssessment.intDeptPlan;
              await _context.QuaterlyPlans.AddAsync(quat);
              await _context.SaveChangesAsync();
            }
          }
        }

        return RedirectToAction(nameof(Index));
      }
      ViewData["ImpStatusId"] = new SelectList(_context.ImplementationStatus, "ImpStatusId", "ImpStatusName", activityAssessment.ImpStatusId);
      return View(activityAssessment);
    }

    // GET: ActivityAssessment/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.ActivityAssessment == null)
      {
        return NotFound();
      }

      ActivityAssessment? activityAssessment = await _context.ActivityAssessment.Include(x => x.QuaterlyPlans).Where(x => x.intDeptPlan == id).FirstOrDefaultAsync();
      if (activityAssessment == null)
      {
        return NotFound();
      }

      activityAssessment.QuaterlyPlans = await _context.QuaterlyPlans
          .Where(x => x.ActivityAssessmentId == activityAssessment.intDeptPlan)
          .ToListAsync();

      ViewData["Quarter"] = ListHelper.Quarter();
      ViewData["ImpStatusId"] = new SelectList(_context.ImplementationStatus, "ImpStatusId", "ImpStatusName", activityAssessment.ImpStatusId);
      return View(activityAssessment);
    }


    // POST: ActivityAssessment/Edit/5

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("intDeptPlan,strategicObjective,strategicIntervention,StrategicAction,activity,outputIndicator,baseline,budgetCode,unitCost,comparativeTarget,justification,budgetAmount,Q1Target,Q1Budget,Q1Actual,Q1AmtSpent,Q1Justification,Q2Target,Q2Budget,Q2Actual,Q2AmtSpent,Q2Justification,Q3Target,Q3Budget,Q3Actual,Q3AmtSpent,Q3Justification,Q4Target,Q4Budget,Q4Actual,Q4AmtSpent,Q4Justification,AnnualAchievement,TotAmtSpent,ImpStatusId,AnnualJustification,QuaterlyPlans")] ActivityAssessment activityAssessment)
    {
      if (id != activityAssessment.intDeptPlan)
      {
        return NotFound();
      }
      activityAssessment.ImplementationStatus = await _context.ImplementationStatus.Where(x => x.ImpStatusId == activityAssessment.ImpStatusId).FirstOrDefaultAsync();

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(activityAssessment);
          await _context.SaveChangesAsync();

          if (activityAssessment.QuaterlyPlans.Count > 0)
          {

            foreach (var quat in activityAssessment.QuaterlyPlans)
            {
              if (quat.Id != 0)
              {
                _context.Entry(quat).State = EntityState.Modified;
                await _context.SaveChangesAsync();
              }
              else
              {
                quat.ActivityAssessmentId = activityAssessment.intDeptPlan;
                await _context.QuaterlyPlans.AddAsync(quat);
                await _context.SaveChangesAsync();
              }
            }
          }
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!ActivityAssessmentExists(activityAssessment.intDeptPlan))
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
      ViewData["ImpStatusId"] = new SelectList(_context.ImplementationStatus, "ImpStatusId", "ImpStatusName", activityAssessment.ImpStatusId);
      return View(activityAssessment);
    }

    // GET: ActivityAssessment/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.ActivityAssessment == null)
      {
        return NotFound();
      }

      var activityAssessment = await _context.ActivityAssessment
          .Include(a => a.ImplementationStatus)
          .FirstOrDefaultAsync(m => m.intDeptPlan == id);
      if (activityAssessment == null)
      {
        return NotFound();
      }

      return View(activityAssessment);
    }

    // POST: ActivityAssessment/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      if (_context.ActivityAssessment == null)
      {
        return Problem("Entity set 'AppDbContext.ActivityAssessment'  is null.");
      }
      var activityAssessment = await _context.ActivityAssessment.FindAsync(id);
      if (activityAssessment != null)
      {
        _context.ActivityAssessment.Remove(activityAssessment);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool ActivityAssessmentExists(int id)
    {
      return (_context.ActivityAssessment?.Any(e => e.intDeptPlan == id)).GetValueOrDefault();
    }

    public async Task<IActionResult> VerificationUpdate(List<int> selectedIds, int apprStatus)
    {
      if (selectedIds != null && selectedIds.Count > 0 && apprStatus != null && apprStatus != 0)
      {
        foreach (int id in selectedIds)
        {
          var activityAssessment = _context.ActivityAssessment.Where(x => x.intDeptPlan == id).FirstOrDefault();
          if (activityAssessment != null)
          {
            activityAssessment.ActivityAssesmentStatus = apprStatus;
            _context.SaveChanges();
          }
        }
      }

      if (apprStatus == 1 || apprStatus == 2)
      {
        return RedirectToAction(nameof(HodVerification));

      }
      else if (apprStatus == 3 || apprStatus == 4)
      {
        return RedirectToAction(nameof(VerificationDir));
      }
      else if (apprStatus == 5 || apprStatus == 6)
      {
        return RedirectToAction(nameof(VerificationBpd));
      }
      else if (apprStatus == 7 || apprStatus == 8)
      {
        return RedirectToAction(nameof(Approval));
      }

      return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> RegionalVerificationUpdate(List<int> selectedIds, int apprStatus)
    {
      if (selectedIds != null && selectedIds.Count > 0 && apprStatus != null && apprStatus != 0)
      {
        foreach (int id in selectedIds)
        {
          var activityAssessment = _context.ActivityAssessmentRegion.Where(x => x.intRegionAssess == id).FirstOrDefault();
          if (activityAssessment != null)
          {
            activityAssessment.ApprStatus = apprStatus;
            _context.SaveChanges();
          }
        }
      }

      if (apprStatus == 1 || apprStatus == 2)
      {
        return RedirectToAction(nameof(RegionalHodVerification));

      }
      else if (apprStatus == 3 || apprStatus == 4)
      {
        return RedirectToAction(nameof(RegionalVerificationDir));
      }
      else if (apprStatus == 5 || apprStatus == 6)
      {
        return RedirectToAction(nameof(RegionalVerificationBpd));
      }
      else if (apprStatus == 7 || apprStatus == 8)
      {
        return RedirectToAction(nameof(RegionalApproval));
      }

      return RedirectToAction(nameof(RegionalIndex));
    }
  }
}
