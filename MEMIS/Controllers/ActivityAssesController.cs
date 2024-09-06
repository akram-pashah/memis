using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;
using MEMIS.Data.Risk;
using MEMIS.Models;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MEMIS.Models.Risk;
using MEMIS.ViewModels;
using MEMIS.Helpers.ExcelReports;
using Humanizer;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Drawing;
using MEMIS.Pages.Shared;
using DocumentFormat.OpenXml.Bibliography;
using MEMIS.ViewModels.Planning;

namespace MEMIS.Controllers
{
  //[Authorize]
  public class ActivityAssesController : Controller
  {
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ActivityAssesController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }


    public async Task<IActionResult> Index(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.ActivityAssess != null)
      {
        var dat = _context.ActivityAssess.Include(m => m.StrategicAction).Include(m => m.StrategicIntervention).Include(m => m.ActivityFk);

        var result = new PagedResult<ActivityAssess>
        {
          Data = await dat.AsNoTracking().ToListAsync(),
          TotalItems = _context.ActivityAssess.Count(),
          PageNumber = pageNumber,
          PageSize = _context.ActivityAssess.Count(),
        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.ActivityAssess'  is null.");
      }
    }

    public async Task<IActionResult> Dashboard()
    {
      var userRoles = getUserRoles();
      Guid departmentId = Guid.Parse(HttpContext.Session.GetString("Department"));

      var query = _context.ActivityAssess
        .Include(x => x.StrategicIntervention)
          .ThenInclude(x => x.StrategicObjective)
        .Include(x => x.QuaterlyPlans)
        .Include(x => x.ActivityAssessRegions)
          .ThenInclude(x => x.QuaterlyPlans)
        .AsQueryable();
      var activitiesQuery = query;
      if (!userRoles.Contains("SuperAdmin"))
      {
        activitiesQuery = activitiesQuery.Where(x => x.intDept == departmentId);
      }

      var activities = await activitiesQuery.ToListAsync();

      var totalActivities = await query.ToListAsync();

      var departments = await _context.Departments.OrderBy(x => x.deptName).ToListAsync();

      var focusAreas = await _context.FocusArea.OrderBy(x => x.FocusAreaName).ToListAsync();

      TotalActivitiesViewModel data = new()
      {
        TotalActivities = activities.Count,
        TotalBudget = activities.Sum(x => x.QuaterlyPlans.Sum(x => x.QBudget) + x.ActivityAssessRegions.Sum(x => x.QuaterlyPlans.Sum(x => x.QBudget))),
        TotalTarget = activities.Sum(x => x.QuaterlyPlans.Sum(x => x.QTarget) + x.ActivityAssessRegions.Sum(x => x.QuaterlyPlans.Sum(x => x.QTarget))),
        PendingActivities = activities.Where(x => x.ApprStatus != (int)deptPlanApprStatus.dirapprapproved).Count(),
        ActivitiesCount = departments.Select(x => totalActivities.Where(a => a.intDept == x.intDept).Count()).ToList(),
        DepartmentBudgets = departments.Select(x => totalActivities.Where(a => a.intDept == x.intDept).Sum(x => x.QuaterlyPlans.Sum(x => x.QBudget) + x.ActivityAssessRegions.Sum(x => x.QuaterlyPlans.Sum(x => x.QBudget)))).ToList(),
        Departments = departments.Select(x => x.deptName).ToList(),
        FocusAreaActivitiesCount = focusAreas.Select(x => totalActivities.Where(a => a.StrategicIntervention.StrategicObjective.intFocus == x.intFocus).Count()).ToList(),
        FocusAreas = focusAreas.Select(x => x.FocusAreaName).ToList(),
      };

      foreach (var dept in departments)
      {
        data.BudgetWithDepartment.Add(new DepartmentBudget()
        {
          Name = dept.deptName,
          Code = dept.deptCode,
          Budget = totalActivities.Where(a => a.intDept == dept.intDept).Sum(x => x.QuaterlyPlans.Sum(x => x.QBudget) + x.ActivityAssessRegions.Sum(x => x.QuaterlyPlans.Sum(x => x.QBudget)))
        });
      }

      return View(data);
    }

    public async Task<IActionResult> AddAnnualPlan()
    {
      if (HttpContext.Session.GetString("Department") == null)
      {
        return View();
      }

      Guid departmentId = Guid.Parse(HttpContext.Session.GetString("Department"));
      var userRoles = getUserRoles();
      var contextData = _context.AnnualImplemtationPlan.Include(a => a.ActivityFk).Include(a => a.DepartmentFk).Include(a => a.FocusAreaFk).Include(a => a.StrategicActionFk).Include(a => a.StrategicInterventionFk).Include(a => a.StrategicObjectiveFk);

      return View(userRoles.Contains("SuperAdmin") ? await contextData.ToListAsync() : await contextData.Where(x => x.intDept == departmentId).ToListAsync());
    }

    public async Task<IActionResult> MoveAnualPlan(int Id)
    {
      AnnualImplemtationPlan? AnnualPlan = await _context.AnnualImplemtationPlan.Include(a => a.ActivityFk).Include(a => a.DepartmentFk).Include(a => a.FocusAreaFk).Include(a => a.StrategicActionFk).Include(a => a.StrategicInterventionFk).Include(a => a.StrategicObjectiveFk).Where(x => x.Id == Id).FirstOrDefaultAsync();
      if (AnnualPlan != null)
      {
        ActivityAssess activityAssess = new()
        {
          intIntervention = AnnualPlan.intIntervention,
          intAction = AnnualPlan.intAction,
          intActivity = AnnualPlan.intActivity,
          baseline = AnnualPlan.baseline,
          intDept = AnnualPlan.intDept,
          IdentifiedRisks = AnnualPlan.Risk,
        };
        _context.ActivityAssess.Add(activityAssess);
        await _context.SaveChangesAsync();

        //_context.AnnualImplemtationPlan.Remove(AnnualPlan);
        //await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Edit), new { Id = activityAssess.intAssess });
      }
      else
      {
        return NotFound();
      }

    }

    public async Task<IActionResult> ExportToExcel()
    {
      try
      {
        var list = await _context.ActivityAssess.Include(m => m.StrategicAction).Include(m => m.StrategicIntervention).Include(m => m.ActivityFk).ToListAsync();
        var stream = ExportHandler.AnnualDetailedResultsFrameworkReport(list);
        stream.Position = 0;
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Annual Detailed Results Framework.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public async Task<IActionResult> AllocatetoRegion(int pageNumber = 1)
    {
      //int pageSize = 10;
      //var offset = (pageSize * pageNumber) - pageSize;
      if (_context.ActivityAssess != null)
      {
        var dat = _context.ActivityAssess.Include(m => m.StrategicAction).Include(m => m.StrategicIntervention).Include(m => m.ActivityFk);

        var result = new PagedResult<ActivityAssess>
        {
          Data = await dat.AsNoTracking().ToListAsync(),
          TotalItems = _context.ActivityAssess.Count(),
          PageNumber = pageNumber,
          PageSize = _context.ActivityAssess.Count(),

        };

        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.ActivityAssess'  is null.");
      }
    }

    private string[] getUserRoles()
    {
      string userRolesString = HttpContext.Session.GetString("UserRoles");
      string[] userRoles = userRolesString?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
      return userRoles;
    }

    public async Task<IActionResult> RegionalAllocations(int pageNumber = 1)
    {
      if (_context.ActivityAssess != null)
      {
        var region = Guid.Parse(HttpContext.Session.GetString("Region"));
        var userRoles = getUserRoles();

        var query = _context.ActivityAssess
            .Include(x => x.ActivityAssessRegions)
            .ThenInclude(x => x.Region)
              .Include(m => m.StrategicAction)
              .Include(m => m.StrategicIntervention)
              .Include(m => m.ActivityFk)
            .Where(x => x.ApprStatus == 0 && x.ApprStatus == 0 && !x.ActivityAssessRegions.Any(ar => ar.intRegion == region));

        List<ActivityAssessRegion> dat = await query.Select(x => new ActivityAssessRegion()
        {
          intAssess = x.intAssess,
          intRegion = region,
          Quarter = x.Quarter,
          QTarget = x.QuaterlyPlans.Sum(x => x.QTarget),
          QBudget = x.QuaterlyPlans.Sum(x => x.QBudget),
          budgetAmount = x.budgetAmount,
          ActivityAssessFk = x,
        }).ToListAsync();

        ViewBag.Users = _userManager;
        return View(dat);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.ActivityAssess'  is null.");
      }
    }

    public async Task<IActionResult> SetRegionalTarget(int id)
    {
      try
      {
        ActivityAssessRegion? activityAssessRegion = new();
        ActivityAssess? assess = _context.ActivityAssess?.Include(x => x.QuaterlyPlans).Where(x => x.intAssess == id).FirstOrDefault();
        if (assess != null)
        {
          var region = Guid.Parse(HttpContext.Session.GetString("Region"));
          //region.ActivityAssessFk.actType = 2;
          activityAssessRegion = new()
          {
            intAssess = id,
            intRegion = region,
            Quarter = assess.Quarter,
            QTarget = assess.QuaterlyPlans.Sum(x => x.QTarget),
            QBudget = assess.QuaterlyPlans.Sum(x => x.QBudget),
            budgetAmount = assess.budgetAmount,
          };

          _context.ActivityAssessRegion.Add(activityAssessRegion);

          await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(EditRegionalTarget), new { Id = activityAssessRegion.intRegionAssess });
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }
    public async Task<IActionResult> MnERegionalTarget(int id)
    {
      try
      {
        ActivityAssessmentRegion? region = _context.ActivityAssessmentRegion?.Include(x => x.ActivityAssessFk).Where(x => x.intAssess == id).FirstOrDefault();
        if (region != null)
        {
          _context.ActivityAssessmentRegion?.Update(region);

          await _context.SaveChangesAsync();

        }
        return RedirectToAction(nameof(EditRegionalTarget), new { Id = region?.intRegionAssess });
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }

    public async Task<IActionResult> EditRegionalTarget(int id)
    {
      try
      {
        ActivityAssessRegion? region = await _context.ActivityAssessRegion
          .Include(x => x.QuaterlyPlans).Where(x => x.intRegionAssess == id).FirstOrDefaultAsync();
        if (region == null)
        {
          return NotFound();
        }

        ActivityAssessDto activityAssessDto = new ActivityAssessDto();
        activityAssessDto.intAssess = id;
        activityAssessDto.budgetAmount = region.budgetAmount;
        activityAssessDto.QTarget = region.QTarget;
        activityAssessDto.QBudget = region.QBudget;
        activityAssessDto.QuaterlyPlans = region.QuaterlyPlans.ToList();
        ViewData["Quarter"] = ListHelper.Quarter();
        return View(activityAssessDto);
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }

    [HttpPost]
    public async Task<IActionResult> SetRegionalTarget(ActivityAssessDto region)
    {
      try
      {
        ActivityAssessRegion activityAssessRegion = _context.ActivityAssessRegion.Where(x => x.intRegionAssess == region.intAssess).FirstOrDefault();

        //activityAssessRegion.intAssess = region.intAssess;
        //activityAssessRegion.intRegion = region.intRegion;
        activityAssessRegion.budgetAmount = region.budgetAmount;

        _context.ActivityAssessRegion.Update(activityAssessRegion);
        //activityAssessRegion.QTarget = 0;
        //activityAssessRegion.QBudget = 0;
        if (region.QuaterlyPlans?.Count > 0)
        {

          foreach (var quat in region.QuaterlyPlans)
          {
            //activityAssessRegion.QTarget += quat.QTarget ?? 0;
            //activityAssessRegion.QBudget += quat.QBudget ?? 0;

            if (quat.Id != 0)
            {
              quat.ActivityAssessRegionId = region.intAssess;
              _context.Entry(quat).State = EntityState.Modified;
              await _context.SaveChangesAsync();
            }
            else
            {
              quat.ActivityAssessRegionId = region.intAssess;
              await _context.QuaterlyPlans.AddAsync(quat);
              await _context.SaveChangesAsync();
            }
          }

          //ActivityAssessmentRegion? activityAssessmentRegion = _context.ActivityAssessmentRegion.Where(x => x.intRegionAssess == region.intAssess).FirstOrDefault();
          //if (activityAssessmentRegion == null)
          //{
          //  activityAssessmentRegion = new ActivityAssessmentRegion()
          //  {
          //    intAssess = activityAssessRegion.intAssess,
          //    intRegion = activityAssessRegion.intRegion,
          //    budgetAmount = activityAssessRegion.budgetAmount,
          //    Quarter = activityAssessRegion.Quarter,
          //  };
          //  _context.ActivityAssessmentRegion.Add(activityAssessmentRegion);
          //}
          //else
          //{
          //  activityAssessmentRegion.intAssess = activityAssessRegion.intAssess;
          //  activityAssessmentRegion.intRegion = activityAssessRegion.intRegion;
          //  activityAssessmentRegion.budgetAmount = activityAssessRegion.budgetAmount;
          //  activityAssessmentRegion.Quarter = activityAssessRegion.Quarter;

          //  foreach (var item in region.QuaterlyPlans)
          //  {
          //    item.ActivityAssessmentRegionId = activityAssessmentRegion.intRegionAssess;
          //  }
          //}

          _context.SaveChanges();
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(RegionalAllocations));
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }

    public async Task<IActionResult> RegionalActivityAsses()
    {
      try
      {
        var query = _context.ActivityAssessRegion
          .Include(x => x.ActivityAssessFk)
          .ThenInclude(x => x.StrategicIntervention)
          .Include(x => x.ActivityAssessFk)
          .ThenInclude(x => x.StrategicAction)
          .Include(x => x.ActivityAssessFk)
          .ThenInclude(x => x.ActivityFk);
        var region = HttpContext.Session.GetString("Region");
        var userRoles = getUserRoles();

        List<ActivityAssessRegion> activityAssessRegions = userRoles.Contains("SuperAdmin") ? await query.ToListAsync() : await query.Where(x => x.intRegion == Guid.Parse(region)).ToListAsync();
        return View(activityAssessRegions);
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }

    public async Task<IActionResult> DeleteRegionalTarget(int id)
    {
      try
      {
        ActivityAssessRegion? region = _context.ActivityAssessRegion?.Where(x => x.intRegionAssess == id).FirstOrDefault();
        if (region == null)
        {
          return NotFound();
        }
        else
        {
          _context.Remove(region);
          await _context.SaveChangesAsync();

          return RedirectToAction(nameof(RegionalActivityAsses));
        }
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }

    public async Task<IActionResult> ConsolidatedDeptPlan(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.ActivityAssess != null && _context.Departments != null)
      {
        var consolidatedDeptPlanViewModels = _context.Departments.Select(x => new ConsolidatedDeptPlanViewModel()
        {
          Department = x,
          AllocatedActivityAssesses = _context.ActivityAssess.Where(x => x.intDept == x.intDept && x.actType == 1).ToList(),
          ActivityAssesses = _context.ActivityAssess.Where(x => x.intDept == x.intDept && x.actType != 1).ToList(),
          BudgetCost = _context.ActivityAssess.Where(x => x.intDept == x.intDept && (x.actType == 1)).Sum(x => x.budgetAmount)
        });

        //if (activityAssess.actType == 1)
        //{
        //  var quarterlyPlans = await _context.ActivityAssessRegion
        //    .Include(x => x.QuaterlyPlans)
        //    .Where(x => x.intAssess == id)
        //    .SelectMany(x => x.QuaterlyPlans)
        //    .GroupBy(x => x.Quarter)
        //    .Select(x => new QuaterlyPlan()
        //    {
        //      Quarter = x.Key,
        //      QTarget = x.Sum(y => y.QTarget),
        //      QBudget = x.Sum(y => y.QBudget),
        //    })
        //    .ToListAsync();

        //  activityAssess.QuaterlyPlans = quarterlyPlans;
        //}

        var consolidatedRegionalPlan = await _context.ActivityAssessRegion
          .Include(x => x.ActivityAssessFk)
          .ThenInclude(x => x.StrategicIntervention)
            .Include(x => x.ActivityAssessFk)
            .ThenInclude(x => x.StrategicAction)
            .Include(x => x.ActivityAssessFk)
            .ThenInclude(x => x.ActivityFk)
          .Where(r => r.ActivityAssessFk.actType == 1 && r.ActivityAssessFk.ApprStatus == 0)
          .GroupBy(r => r.intRegion)
          .Select(g => new ActivityAssess
          {
            intDept = g.Key,
            intAssess = g.Select(x => x.intAssess).FirstOrDefault(),
            budgetAmount = g.Sum(a => a.budgetAmount),
            QTarget = g.SelectMany(a => a.ActivityAssessFk.QuaterlyPlans).Sum(a => a.QTarget),
            comparativeTarget = g.Sum(a => a.ActivityAssessFk.comparativeTarget),
            StrategicIntervention = g.FirstOrDefault().ActivityAssessFk.StrategicIntervention,
            StrategicAction = g.FirstOrDefault().ActivityAssessFk.StrategicAction,
            ActivityFk = g.FirstOrDefault().ActivityAssessFk.ActivityFk,
            outputIndicator = g.FirstOrDefault().ActivityAssessFk.outputIndicator,
            baseline = g.FirstOrDefault().ActivityAssessFk.baseline,
            justification = g.FirstOrDefault().ActivityAssessFk.justification,
            QBudget = g.SelectMany(a => a.ActivityAssessFk.QuaterlyPlans).Sum(a => a.QBudget),
            ApprStatus = g.FirstOrDefault().ActivityAssessFk.ApprStatus,
            actType = 2,
            IdentifiedRisks = g.FirstOrDefault().ActivityAssessFk.IdentifiedRisks,
            QuaterlyPlans = g.SelectMany(a => a.ActivityAssessFk.QuaterlyPlans).ToList()
          }).ToListAsync();

        var groupedDeptData = await _context.ActivityAssess.Include(a => a.DepartmentFk)
            .Where(a => (a.actType == 1) && a.ApprStatus == 0)
            .GroupBy(a => a.intDept)
            .Select(g => new ActivityAssess
            {
              intDept = g.Key,
              intAssess = g.Select(x => x.intAssess).FirstOrDefault(),
              budgetAmount = g.Sum(a => a.budgetAmount),
              QTarget = g.SelectMany(a => a.QuaterlyPlans).Sum(a => a.QTarget),
              comparativeTarget = g.Sum(a => a.comparativeTarget),
              StrategicIntervention = g.FirstOrDefault().StrategicIntervention,
              StrategicAction = g.FirstOrDefault().StrategicAction,
              ActivityFk = g.FirstOrDefault().ActivityFk,
              outputIndicator = g.FirstOrDefault().outputIndicator,
              baseline = g.FirstOrDefault().baseline,
              justification = g.FirstOrDefault().justification,
              QBudget = g.SelectMany(a => a.QuaterlyPlans).Sum(a => a.QBudget),
              ApprStatus = g.FirstOrDefault().ApprStatus,
              actType = 1,
              IdentifiedRisks = g.FirstOrDefault().IdentifiedRisks,
              QuaterlyPlans = g.SelectMany(a => a.QuaterlyPlans).ToList()
            })
            .ToListAsync();

        // Fetch the data where actType is not equal to 1
        var nonAllocatedData = await _context.ActivityAssess.Include(a => a.DepartmentFk)
            .Where(a => a.actType != 1 && a.actType != 2)
            .ToListAsync();

        // Combine both datasets
        var combinedData = (groupedDeptData).Concat(consolidatedRegionalPlan).Concat(nonAllocatedData).ToList();

        List<ActivityAssess> deptPlans = await _context.ActivityAssess
          .Include(a => a.DepartmentFk)
          .Include(x => x.StrategicIntervention)
          .Include(x => x.StrategicAction)
          .Include(x => x.ActivityFk)
          .Include(x => x.QuaterlyPlans)
          .Include(x => x.ActivityAssessRegions)
            .ThenInclude(x => x.QuaterlyPlans)
            .Where(a => (a.actType == 0) && a.ApprStatus == 0).ToListAsync();

        List<ActivityAssess> regPlans = await _context.ActivityAssess
          .Include(x => x.ActivityAssessRegions)
          .ThenInclude(x => x.QuaterlyPlans)
          .Where(x => x.actType == 1 && x.ApprStatus == 0 && x.ActivityAssessRegions.Where(x => x.ApprStatus == 1).Any())
          .ToListAsync();

        var result = new PagedResult<ActivityAssess>
        {
          Data = deptPlans.ToList(),
          //Data = deptPlans.Concat(regPlans).ToList(),
          TotalItems = _context.ActivityAssess.Count(),
          PageNumber = pageNumber,
          PageSize = combinedData.Count()
        };

        var totalAllocatedBudget = await _context.ActivityAssess
            .Where(a => a.actType == 1)
            .SumAsync(a => a.budgetAmount ?? 0);

        ViewBag.TotalAllocatedBudget = totalAllocatedBudget;
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.ActivityAssess'  is null.");
      }
    }

    public async Task<IActionResult> ShowConsolidatedDeptPlan(int id)
    {
      try
      {
        var deptId = await _context.ActivityAssess.Where(x => x.intAssess == id).Select(x => x.intDept).FirstOrDefaultAsync();
        List<ActivityAssess> activityAssess = await _context.ActivityAssess.Where(x => (x.actType == 1 || x.actType == 2) && x.ApprStatus == 0 && x.intDept == deptId).ToListAsync();
        return View(activityAssess);
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }

    public async Task<IActionResult> ShowConsolidatedRegionalAssessment(int id)
    {
      try
      {
        var regionId = await _context.ActivityAssessRegion.Where(x => x.intAssess == id).Select(x => x.intRegion).FirstOrDefaultAsync();
        List<ActivityAssessRegion> activityAssessRegions = await _context.ActivityAssessRegion
            .Include(x => x.ActivityAssessFk)
            .ThenInclude(x => x.StrategicIntervention)
            .Include(x => x.ActivityAssessFk)
            .ThenInclude(x => x.StrategicAction)
            .Include(x => x.ActivityAssessFk)
            .ThenInclude(x => x.ActivityFk)
            .Where(x => x.intRegion == regionId)
            .ToListAsync();

        return View(activityAssessRegions);
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AllocatetoRegion(int id, ActivityAssess dto)
    {
      var activity = await _context.ActivityAssess.FindAsync(id);
      if (activity == null)
      {
        return NotFound();

      }
      try
      {
        activity.actType = 1;
        _context.Update(activity);
        await _context.SaveChangesAsync();

        var region = HttpContext.Session.GetString("Region");
        if (region != null)
        {
          var groupedData = await _context.ActivityAssess.Include(a => a.DepartmentFk)
          .Where(a => a.actType == 1 && a.intAssess == activity.intAssess)
          .GroupBy(a => a.intDept)
          .Select(g => new ActivityAssess
          {
            intDept = g.Key,
            budgetAmount = g.Sum(a => a.budgetAmount),
            QTarget = g.Sum(a => a.QTarget),
            Quarter = g.FirstOrDefault().Quarter,
            QBudget = g.FirstOrDefault().QBudget
          })
          .FirstOrDefaultAsync();
          ViewData["Quarter"] = ListHelper.Quarter();

          //var regions = await _context.Region.Select(x => x.intRegion).ToListAsync();
          //foreach (var item in regions)
          //{
          //  ActivityAssessRegion activityAssessRegion = new();
          //  activityAssessRegion.intAssess = activity.intAssess;
          //  activityAssessRegion.intRegion = item;
          //  activityAssessRegion.Quarter = activity.Quarter;
          //  if (groupedData != null)
          //  {
          //    activityAssessRegion.QTarget = activity.QTarget;
          //    activityAssessRegion.QBudget = activity.QBudget;
          //  }
          //  activityAssessRegion.ApprStatus = activity.ApprStatus;
          //  activityAssessRegion.budgetAmount = activity.budgetAmount;

          //  _context.ActivityAssessRegion.Add(activityAssessRegion);
          //}
          _context.SaveChanges();
        }
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!ActivityAssessExists(activity.intAssess))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }
      return RedirectToAction(nameof(AllocatetoRegion));
    }


    public async Task<IActionResult> RegionalVerify(int pageNumber = 1)
    {
      var query = _context.ActivityAssessRegion.Include(x => x.QuaterlyPlans).Where(x => x.ApprStatus == 0 && x.QuaterlyPlans.Where(q => q.QTarget > 0).Any()).Include(x => x.Region);

      var region = HttpContext.Session.GetString("Region");
      var userRoles = getUserRoles();

      var result = userRoles.Contains("SuperAdmin") ? await query.ToListAsync() : await query.Where(x => x.intRegion == Guid.Parse(region)).ToListAsync();

      return View(result);
    }

    [HttpGet]
    public IActionResult GetActivityAssessDetails(int id)
    {
      var activityAssess = _context.ActivityAssessRegion
        .Include(x => x.QuaterlyPlans)
        .Include(x => x.Region)
        .Include(x => x.ActivityAssessFk)
          .ThenInclude(m => m.StrategicAction)
        .Include(x => x.ActivityAssessFk)
          .ThenInclude(m => m.StrategicIntervention)
            .ThenInclude(x => x.StrategicObjective)
        .Include(x => x.ActivityAssessFk)
          .ThenInclude(m => m.ActivityFk)
        .Include(x => x.ActivityAssessFk)
          .ThenInclude(x => x.QuaterlyPlans)
        .Include(x => x.ActivityAssessFk)
          .ThenInclude(x => x.DepartmentFk)
        .FirstOrDefault(x => x.intRegionAssess == id);
      if (activityAssess == null)
      {
        return NotFound();
      }

      return PartialView("_ActivityAssessDetails", activityAssess);
    }

    public IActionResult RegionalHodVerification(int pageNumber = 1)
    {
      var result = _context.ActivityAssessRegion.Where(x => x.ApprStatus == 1).Include(x => x.Region).ToList();

      return View(result);
    }
    public IActionResult RegionalVerificationDir(int pageNumber = 1)
    {
      var result = _context.ActivityAssessRegion.Where(x => x.ApprStatus == 3).Include(x => x.Region).ToList();

      return View(result);
    }
    public IActionResult RegionalVerificationBpd(int pageNumber = 1)
    {
      var result = _context.ActivityAssessRegion.Where(x => x.ApprStatus == 5).Include(x => x.Region).ToList();

      return View(result);
    }
    public IActionResult RegionalApproval(int pageNumber = 1)
    {
      var result = _context.ActivityAssessRegion.Where(x => x.ApprStatus == 7).Include(x => x.Region).ToList();

      return View(result);
    }

    public IActionResult Verify(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.ActivityAssess != null)
      {
        var dat = _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
            .Where(x => x.ApprStatus == 0)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<ActivityAssess>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.ActivityAssess.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.ActivityAssess'  is null.");
      }
    }
    public async Task<IActionResult> HeadVerifyStatus(int? id)
    {
      if (id == null || _context.ActivityAssess == null)
      {
        return NotFound();
      }

      ActivityAssess activityAssess = await _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
        .Include(x => x.QuaterlyPlans)
        .Include(x => x.ActivityAssessRegions)
          .ThenInclude(x => x.Region)
        .Include(x => x.ActivityAssessRegions)
          .ThenInclude(x => x.QuaterlyPlans)
        .Where(x => x.intAssess == id).FirstOrDefaultAsync();

      if (activityAssess.actType == 1)
      {
        var quarterlyPlans = await _context.ActivityAssessRegion
          .Include(x => x.QuaterlyPlans)
          .Where(x => x.intAssess == id)
          .SelectMany(x => x.QuaterlyPlans)
          .GroupBy(x => x.Quarter)
          .Select(x => new QuaterlyPlan()
          {
            Quarter = x.Key,
            QTarget = x.Sum(y => y.QTarget),
            QBudget = x.Sum(y => y.QBudget),
          })
          .ToListAsync();

        activityAssess.QuaterlyPlans = quarterlyPlans;
      }
      ViewBag.StrategicIntervention = _context.StrategicIntervention == null ? new List<StrategicIntervention>() : await _context.StrategicIntervention.ToListAsync();
      ViewBag.StrategicAction = _context.StrategicAction == null ? new List<StrategicAction>() : await _context.StrategicAction.ToListAsync();
      ViewBag.Activity = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Quarter"] = ListHelper.Quarter();
      ViewBag.Depts = await _context.Departments.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(activityAssess);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HeadVerifyStatus(ActivityAssess objectdto)
    {
      if (objectdto.intAssess != 0)
      {
        try
        {
          var pp = await _context.ActivityAssess.FindAsync(objectdto.intAssess);
          if (pp == null)
          {
            return NotFound();
          }
          if (objectdto.ApprStatus == 1)
          {
            pp.ApprStatus = (int)deptPlanApprStatus.hodreviewed;
          }
          else if (objectdto.ApprStatus == 2)
          {
            pp.ApprStatus = (int)deptPlanApprStatus.hodrejected;
          }
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!ActivityAssessExists(objectdto.intAssess))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(ConsolidatedDeptPlan));
      }
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(objectdto);
    }

    public async Task<IActionResult> DirVerify(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      List<ActivityAssess> activityAssesses = new List<ActivityAssess>();

      if (_context.ActivityAssess != null)
      {
        var userRoles = getUserRoles();
        var query = _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(y => y.QuaterlyPlans).Include(s => s.ActivityFk).Include(v => v.ActivityAssessRegions).ThenInclude(x => x.QuaterlyPlans)
            .Where(x => x.ApprStatus == (int)(deptPlanApprStatus.hodreviewed));

        activityAssesses =  userRoles.Contains("SuperAdmin") ? await query.ToListAsync() : await query.Where(x => x.intDept == Guid.Parse(HttpContext.Session.GetString("Department"))).ToListAsync();

        //var result = new PagedResult<ActivityAssess>
        //{
        //  Data = dat.AsNoTracking().ToList(),
        //  TotalItems = _context.ActivityAssess.Count(),
        //  PageNumber = pageNumber,
        //  PageSize = _context.ActivityAssess.Count()
        //};
        ViewBag.Users = _userManager;
        return View(activityAssesses);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.ActivityAssess'  is null.");
      }
    }
    public async Task<IActionResult> DirVerifyStatus(int? id)
    {
      if (id == null || _context.ActivityAssess == null)
      {
        return NotFound();
      }

      ActivityAssess activityAssess = await _context.ActivityAssess
        .Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
        .Include(x => x.QuaterlyPlans)
        .Include(x => x.ActivityAssessRegions)
        .ThenInclude(x => x.Region)
        .Include(x => x.ActivityAssessRegions)
        .ThenInclude(x => x.QuaterlyPlans).Where(x => x.intAssess == id).FirstOrDefaultAsync();

      if (activityAssess.actType == 1)
      {
        var quarterlyPlans = await _context.ActivityAssessRegion
          .Include(x => x.QuaterlyPlans)
          .Where(x => x.intAssess == id)
          .SelectMany(x => x.QuaterlyPlans)
          .GroupBy(x => x.Quarter)
          .Select(x => new QuaterlyPlan()
          {
            Quarter = x.Key,
            QTarget = x.Sum(y => y.QTarget),
            QBudget = x.Sum(y => y.QBudget),
          })
          .ToListAsync();

        activityAssess.QuaterlyPlans = quarterlyPlans;
      }

      ViewBag.StrategicIntervention = _context.StrategicIntervention == null ? new List<StrategicIntervention>() : await _context.StrategicIntervention.ToListAsync();
      ViewBag.StrategicAction = _context.StrategicAction == null ? new List<StrategicAction>() : await _context.StrategicAction.ToListAsync();
      ViewBag.Activity = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewBag.Depts = await _context.Departments.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(activityAssess);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DirVerifyStatus(ActivityAssess objectdto)
    {
      if (objectdto.intAssess != 0)
      {
        try
        {
          var pp = await _context.ActivityAssess.FindAsync(objectdto.intAssess);
          if (pp == null)
          {
            return NotFound();
          }
          if (objectdto.ApprStatus == 1)
          {
            pp.ApprStatus = (int)deptPlanApprStatus.dirapprove;
          }
          else if (objectdto.ApprStatus == 2)
          {
            pp.ApprStatus = (int)deptPlanApprStatus.dirrejected;
          }

          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!ActivityAssessExists(objectdto.intAssess))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(DirVerify));
      }
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(objectdto);
    }

    public IActionResult Consolidation(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.ActivityAssess != null)
      {
        var dat = _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk).Include(x => x.ActivityAssessRegions).ThenInclude(x => x.QuaterlyPlans).Include(x => x.QuaterlyPlans)
            .Where(x => x.ApprStatus == (int)(deptPlanApprStatus.dirapprove)).ToList();

        //var result = new PagedResult<ActivityAssess>
        //{
        //  Data = dat.AsNoTracking().ToList(),
        //  TotalItems = _context.ActivityAssess.Count(),
        //  PageNumber = pageNumber,
        //  PageSize = pageSize
        //};
        ViewBag.Users = _userManager;
        return View(dat);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.ActivityAssess'  is null.");
      }
    }
    public async Task<IActionResult> ConsolidationStatus(int? id)
    {
      if (id == null || _context.ActivityAssess == null)
      {
        return NotFound();
      }

      var deptPlan = await _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
        .Include(x => x.QuaterlyPlans)
        .Include(x => x.ActivityAssessRegions)
        .ThenInclude(x => x.Region)
        .Include(x => x.ActivityAssessRegions)
        .ThenInclude(x => x.QuaterlyPlans)
          .Where(m => m.intAssess == id).FirstOrDefaultAsync();
      if (deptPlan == null)
      {
        return NotFound();
      }
      ActivityAssessDto riskDto = new ActivityAssessDto
      {
        intAssess = deptPlan.intAssess,
        intIntervention = deptPlan.intIntervention,
        intAction = deptPlan.intAction,
        intActivity = deptPlan.intActivity,
        outputIndicator = deptPlan.outputIndicator,
        baseline = deptPlan.baseline,
        QTarget = deptPlan.QTarget,
        QBudget = deptPlan.QBudget,
        comparativeTarget = deptPlan.comparativeTarget,
        justification = deptPlan.justification,
        budgetAmount = deptPlan.budgetAmount,
      };
      ViewBag.StrategicIntervention = _context.StrategicIntervention == null ? new List<StrategicIntervention>() : await _context.StrategicIntervention.ToListAsync();
      ViewBag.StrategicAction = _context.StrategicAction == null ? new List<StrategicAction>() : await _context.StrategicAction.ToListAsync();
      ViewBag.Activity = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewBag.Depts = await _context.Departments.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(deptPlan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConsolidationStatus(ActivityAssessDto objectdto)
    {
      if (objectdto.intAssess != null && objectdto.intAssess != 0)
      {
        try
        {
          var pp = await _context.ActivityAssess.FindAsync(objectdto.intAssess);
          if (pp == null)
          {
            return NotFound();
          }
          if (objectdto.ApprStatus == 1)
          {
            pp.ApprStatus = (int)deptPlanApprStatus.meOfficerVerified;
          }
          else if (objectdto.ApprStatus == 2)
          {
            pp.ApprStatus = (int)deptPlanApprStatus.meOfficerRejected;
          }

          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!ActivityAssessExists(objectdto.intAssess))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(Consolidation));
      }
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(objectdto);
    }
    public IActionResult HeadBpdVerify(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.ActivityAssess != null)
      {
        var dat = _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk).Include(x => x.QuaterlyPlans).Include(y => y.ActivityAssessRegions).ThenInclude(z => z.QuaterlyPlans)
            .Where(x => x.ApprStatus == (int)(deptPlanApprStatus.meOfficerVerified)).ToList();

        //var result = new PagedResult<ActivityAssess>
        //{
        //  Data = dat.AsNoTracking().ToList(),
        //  TotalItems = _context.ActivityAssess.Count(),
        //  PageNumber = pageNumber,
        //  PageSize = pageSize
        //};
        ViewBag.Users = _userManager;
        return View(dat);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.ActivityAssess'  is null.");
      }
    }
    public async Task<IActionResult> HeadBpdVerifyStatus(int? id)
    {
      if (id == null || _context.ActivityAssess == null)
      {
        return NotFound();
      }

      var deptPlan = await _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
        .Include(x => x.QuaterlyPlans)
        .Include(x => x.ActivityAssessRegions)
        .ThenInclude(x => x.Region)
        .Include(x => x.ActivityAssessRegions)
        .ThenInclude(x => x.QuaterlyPlans)
          .Where(m => m.intAssess == id).FirstOrDefaultAsync();
      if (deptPlan == null)
      {
        return NotFound();
      }
      ActivityAssessDto riskDto = new ActivityAssessDto
      {
        intAssess = deptPlan.intAssess,
        intIntervention = deptPlan.intIntervention,
        intAction = deptPlan.intAction,
        intActivity = deptPlan.intActivity,
        outputIndicator = deptPlan.outputIndicator,
        baseline = deptPlan.baseline,
        QTarget = deptPlan.QTarget,
        QBudget = deptPlan.QBudget,
        comparativeTarget = deptPlan.comparativeTarget,
        justification = deptPlan.justification,
        budgetAmount = deptPlan.budgetAmount,
      };
      ViewBag.StrategicIntervention = _context.StrategicIntervention == null ? new List<StrategicIntervention>() : await _context.StrategicIntervention.ToListAsync();
      ViewBag.StrategicAction = _context.StrategicAction == null ? new List<StrategicAction>() : await _context.StrategicAction.ToListAsync();
      ViewBag.Activity = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewBag.Depts = await _context.Departments.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(deptPlan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HeadBpdVerifyStatus(ActivityAssess objectdto)
    {
      if (objectdto.intActivity != null && objectdto.intActivity != 0)
      {
        try
        {
          var pp = await _context.ActivityAssess.FindAsync(objectdto.intAssess);
          if (pp == null)
          {
            return NotFound();
          }
          if (objectdto.ApprStatus == 1)
          {
            pp.ApprStatus = (int)deptPlanApprStatus.headBpdVerified;
          }
          else if (objectdto.ApprStatus == 2)
          {
            pp.ApprStatus = (int)deptPlanApprStatus.headBpdRejected;
          }

          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!ActivityAssessExists(objectdto.intAssess))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(HeadBpdVerify));
      }
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(objectdto);
    }
    public IActionResult DirAppr(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.ActivityAssess != null)
      {
        var dat = _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk).Include(x => x.QuaterlyPlans).Include(x => x.ActivityAssessRegions).ThenInclude(x => x.QuaterlyPlans)
            .Where(x => x.ApprStatus == (int)(deptPlanApprStatus.headBpdVerified)).ToList();

        ViewBag.Users = _userManager;
        return View(dat);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.ActivityAssess'  is null.");
      }
    }
    public async Task<IActionResult> DirApprStatus(int? id)
    {
      if (id == null || _context.ActivityAssess == null)
      {
        return NotFound();
      }

      var deptPlan = await _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
        .Include(x => x.QuaterlyPlans)
        .Include(x => x.ActivityAssessRegions)
        .ThenInclude(x => x.Region)
        .Include(x => x.ActivityAssessRegions)
        .ThenInclude(x => x.QuaterlyPlans)
          .Where(m => m.intAssess == id).FirstOrDefaultAsync();
      if (deptPlan == null)
      {
        return NotFound();
      }
      ActivityAssessDto riskDto = new ActivityAssessDto
      {
        intAssess = deptPlan.intAssess,
        intIntervention = deptPlan.intIntervention,
        intAction = deptPlan.intAction,
        intActivity = deptPlan.intActivity,
        outputIndicator = deptPlan.outputIndicator,
        baseline = deptPlan.baseline,
        QTarget = deptPlan.QTarget,
        QBudget = deptPlan.QBudget,
        comparativeTarget = deptPlan.comparativeTarget,
        justification = deptPlan.justification,
        budgetAmount = deptPlan.budgetAmount,
      };
      ViewBag.StrategicIntervention = _context.StrategicIntervention == null ? new List<StrategicIntervention>() : await _context.StrategicIntervention.ToListAsync();
      ViewBag.StrategicAction = _context.StrategicAction == null ? new List<StrategicAction>() : await _context.StrategicAction.ToListAsync();
      ViewBag.Activity = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewBag.Depts = await _context.Departments.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(deptPlan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DirApprStatus(ActivityAssess objectdto)
    {
      if (objectdto.intAssess != 0)
      {
        try
        {
          var activityAsses = await _context.ActivityAssess.Include(m => m.StrategicIntervention)
            .ThenInclude(x => x.StrategicObjective)
            .Include(m => m.StrategicAction).Include(s => s.ActivityFk)
        .Include(x => x.QuaterlyPlans)
        .Include(x => x.ActivityAssessRegions)
        .ThenInclude(x => x.Region)
        .Include(x => x.ActivityAssessRegions)
        .ThenInclude(x => x.QuaterlyPlans).Where(x => x.intAssess == objectdto.intAssess).FirstOrDefaultAsync();
          if (activityAsses == null)
          {
            return NotFound();
          }
          if (objectdto.ApprStatus == 1)
          {
            activityAsses.ApprStatus = (int)deptPlanApprStatus.dirapprapproved;

            Guid departmentId = Guid.Parse(HttpContext.Session.GetString("Department"));
            ActivityAssessment activityAssessment = new()
            {
              intFocus = activityAsses?.StrategicIntervention?.StrategicObjective?.intFocus,
              strategicObjective = activityAsses?.StrategicIntervention?.StrategicObjective?.ObjectiveName ?? "",
              strategicIntervention = activityAsses?.StrategicIntervention?.InterventionName ?? "",
              StrategicAction = activityAsses?.StrategicAction?.actionName ?? "",
              activity = activityAsses?.ActivityFk?.activityName ?? "",
              outputIndicator = activityAsses?.outputIndicator ?? "",
              baseline = activityAsses?.baseline,
              budgetCode = activityAsses?.budgetCode,
              comparativeTarget = activityAsses?.comparativeTarget,
              justification = activityAsses?.justification,
              budgetAmount = activityAsses?.budgetAmount,
              intDept = departmentId,
              actType = activityAsses?.actType,
            };

            _context.Add(activityAssessment);
            _context.SaveChanges();

            //List<QuaterlyPlan> quaterlyPlans = activityAsses.QuaterlyPlans.Select(x => new QuaterlyPlan()
            //{
            //  ActivityAssessmentId = activityAssessment.intDeptPlan,
            //  Quarter = x.Quarter,
            //  QTarget = x.QTarget,
            //  QBudget = x.QBudget,
            //  ActivityAccessId = x.ActivityAccessId,
            //  DeptPlanId = x.DeptPlanId,
            //}).ToList();
            //if (activityAsses.actType == 0)
            //{
            foreach (var quarter in activityAsses.QuaterlyPlans)
            {
              quarter.ActivityAssessmentId = activityAssessment.intDeptPlan;
            }
            _context.SaveChanges();
            //}
            //else
            //{
            foreach (var region in activityAsses.ActivityAssessRegions)
            {
              ActivityAssessmentRegion activityAssessmentRegion = new ActivityAssessmentRegion()
              {
                intAssess = objectdto.intAssess,
                intAssessment = activityAssessment.intDeptPlan,
                intRegion = region.intRegion,
                budgetAmount = region?.budgetAmount,
                Quarter = region.Quarter,
                QTarget = region.QTarget,
                QBudget = region.QBudget,
              };

              _context.ActivityAssessmentRegion.Add(activityAssessmentRegion);
              _context.SaveChanges();

              foreach (var quarter in region.QuaterlyPlans)
              {
                quarter.ActivityAssessmentRegionId = activityAssessmentRegion.intRegionAssess;
                quarter.ActivityAssessmentId = activityAssessment.intDeptPlan;
              }
            }
            _context.SaveChanges();
          }
          else if (objectdto.ApprStatus == 2)
          {
            activityAsses.ApprStatus = (int)deptPlanApprStatus.dirapprrejected;
          }

          await _context.SaveChangesAsync();

          //}
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!ActivityAssessExists(objectdto.intAssess))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(DirAppr));
      }
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(objectdto);
    }
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null || _context.ActivityAssess == null)
      {
        return NotFound();
      }

      ActivityAssess activityAssess = await _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
        .Include(x => x.QuaterlyPlans)
        .Include(x => x.ActivityAssessRegions)
        .ThenInclude(x => x.QuaterlyPlans)
        .Include(x => x.ActivityAssessRegions)
        .ThenInclude(x => x.Region)
        .Include(x => x.DepartmentFk)
        .Where(x => x.intAssess == id).FirstOrDefaultAsync();

      if (activityAssess.actType == 1)
      {
        var quarterlyPlans = await _context.ActivityAssessRegion
          .Include(x => x.QuaterlyPlans)
          .Where(x => x.intAssess == id)
          .SelectMany(x => x.QuaterlyPlans)
          .GroupBy(x => x.Quarter)
          .Select(x => new QuaterlyPlan()
          {
            Quarter = x.Key,
            QTarget = x.Sum(y => y.QTarget),
            QBudget = x.Sum(y => y.QBudget),
          })
          .ToListAsync();

        activityAssess.QuaterlyPlans = quarterlyPlans;
      }

      var deptPlan = await _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
        .Include(x => x.QuaterlyPlans)
        .Include(x => x.ActivityAssessRegions)
          .ThenInclude(y => y.QuaterlyPlans)
          .FirstOrDefaultAsync(m => m.intAssess == id);
      if (deptPlan == null)
      {
        return NotFound();
      }
      ViewBag.Users = _userManager;
      ViewBag.StrategicIntervention = _context.StrategicIntervention == null ? new List<StrategicIntervention>() : await _context.StrategicIntervention.ToListAsync();
      ViewBag.StrategicAction = _context.StrategicAction == null ? new List<StrategicAction>() : await _context.StrategicAction.ToListAsync();
      ViewBag.Activity = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["intDept"] = new SelectList(_context.Departments, "intDept", "deptName");
      ViewData["Quarter"] = ListHelper.Quarter();
      return View(activityAssess);
    }
    public async Task<IActionResult> Create(int Id = 0)
    {

      ViewBag.StrategicIntervention = _context.StrategicIntervention == null ? new List<StrategicIntervention>() : await _context.StrategicIntervention.ToListAsync();
      //ViewBag.StrategicAction = _context.StrategicAction == null ? new List<StrategicAction>() : await _context.StrategicAction.ToListAsync();
      //ViewBag.Activity = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["intDept"] = new SelectList(_context.Departments, "intDept", "deptName");
      ViewData["Quarter"] = ListHelper.Quarter();
      ActivityAssessDto activityAssessDto = new ActivityAssessDto();
      return View(activityAssessDto);
    }

    [HttpGet]
    public JsonResult GetStrategicObjectives(int focusAreaId)
    {
      var strategicObjectives = _context.StrategicObjective
                                        .Where(so => so.intFocus == focusAreaId)
                                        .Select(so => new { so.intObjective, so.ObjectiveName })
                                        .ToList();
      return Json(strategicObjectives);
    }

    [HttpGet]
    public JsonResult GetStrategicInterventions(int objectiveId)
    {
      var interventions = _context.StrategicIntervention
                                  .Where(si => si.intObjective == objectiveId)
                                  .Select(si => new { si.intIntervention, si.InterventionName })
                                  .ToList();
      return Json(interventions);
    }

    [HttpGet]
    public JsonResult GetStrategicActions(int interventionId)
    {
      var actions = _context.StrategicAction
                            .Where(sa => sa.intIntervention == interventionId)
                            .Select(sa => new { sa.intAction, sa.actionName })
                            .ToList();
      return Json(actions);
    }

    [HttpGet]
    public JsonResult GetActivities(int actionId)
    {
      var activities = _context.Activity
                               .Where(a => a.intAction == actionId)
                               .Select(a => new { a.intActivity, a.activityName })
                               .ToList();
      return Json(activities);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ActivityAssessDto dto)
    {
      if (ModelState.IsValid)
      {
        ActivityAssess deptPlan = new ActivityAssess
        {
          intIntervention = dto.intIntervention,
          intAction = dto.intAction,
          intActivity = dto.intActivity,
          outputIndicator = dto.outputIndicator,
          baseline = dto.baseline,
          budgetCode = dto.budgetCode,
          Quarter = dto.Quarter,
          QTarget = dto.QTarget,
          QBudget = dto.QBudget,
          comparativeTarget = dto.comparativeTarget,
          justification = dto.justification,
          budgetAmount = dto.budgetAmount,
          intDept = dto.intDept,
          IdentifiedRisks = dto.IdentifiedRisks,

        };
        _context.Add(deptPlan);

        await _context.SaveChangesAsync();
        if (dto.QuaterlyPlans?.Count > 0)
        {
          dto.QuaterlyPlans.ForEach(ev => ev.ActivityAccessId = deptPlan.intAssess);
          _context.QuaterlyPlans.AddRange(dto.QuaterlyPlans);
          _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
      }
      else
      {
        if (dto.QuaterlyPlans == null || (dto.QuaterlyPlans?.Count == 0))
        {
          ViewBag.ErrorMessage = "Please enter querterly targets";
        }
      }

      ViewBag.StrategicIntervention = _context.StrategicIntervention == null ? new List<StrategicIntervention>() : await _context.StrategicIntervention.ToListAsync();
      ViewBag.StrategicAction = _context.StrategicAction == null ? new List<StrategicAction>() : await _context.StrategicAction.ToListAsync();
      ViewBag.Activity = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["intDept"] = new SelectList(_context.Departments, "intDept", "deptName", dto.intDept);
      return View(dto);
    }

    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.ActivityAssess == null)
      {
        return NotFound();
      }

      var deptPlan = await _context.ActivityAssess.FindAsync(id);
      if (deptPlan == null)
      {
        return NotFound();
      }
      ActivityAssessDto dto = new ActivityAssessDto
      {
        intAssess = deptPlan.intAssess,
        intIntervention = deptPlan.intIntervention,
        intAction = deptPlan.intAction,
        intActivity = deptPlan.intActivity,
        outputIndicator = deptPlan.outputIndicator,
        baseline = deptPlan.baseline,
        budgetCode = deptPlan.budgetCode,
        Quarter = deptPlan.Quarter,
        QTarget = deptPlan.QTarget,
        QBudget = deptPlan.QBudget,
        comparativeTarget = deptPlan.comparativeTarget,
        justification = deptPlan.justification,
        budgetAmount = deptPlan.budgetAmount,
        IdentifiedRisks = deptPlan.IdentifiedRisks,
        intDept = (Guid)deptPlan.intDept,
        QuaterlyPlans = await _context.QuaterlyPlans.Where(x => x.ActivityAccessId == deptPlan.intAssess).ToListAsync()
      };

      ViewBag.StrategicIntervention = _context.StrategicIntervention == null ? new List<StrategicIntervention>() : await _context.StrategicIntervention.ToListAsync();
      ViewBag.StrategicAction = _context.StrategicAction
                            .Where(sa => sa.intIntervention == dto.intIntervention)
                            .Select(sa => new { sa.intAction, sa.actionName })
                            .ToList();
     
      ViewBag.Activity = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();

      ViewData["Quarter"] = ListHelper.Quarter();
      ViewBag.Depts = await _context.Departments.ToListAsync();
      //ViewData["intDept"] = new SelectList(_context.Departments, "intDept", "deptName", dto.intDept);

      return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("intAssess,intIntervention,intAction,intActivity,outputIndicator,baseline,budgetCode,unitCost,comparativeTarget,justification,budgetAmount,IdentifiedRisks,intDept,QuaterlyPlans")] ActivityAssessDto deptPlan)
    {
      if (id != deptPlan.intAssess)
      {
        return NotFound();
      }

      var original = _context.ActivityAssess.Where(x => x.intAssess == id).FirstOrDefault();
      if (original == null)
      {
        return NotFound();
      }


      if (ModelState.IsValid)
      {
        try
        {

          if (deptPlan.QuaterlyPlans.Count > 0)
          {

            foreach (var quat in deptPlan.QuaterlyPlans)
            {
              if (quat.Id != 0)
              {
                _context.Entry(quat).State = EntityState.Modified;
                await _context.SaveChangesAsync();
              }
              else
              {
                quat.ActivityAccessId = deptPlan.intAssess;
                await _context.QuaterlyPlans.AddAsync(quat);
                await _context.SaveChangesAsync();
              }
            }
          }
          original.intIntervention = deptPlan.intIntervention;
          original.intAction = deptPlan.intAction;
          original.intActivity = deptPlan.intActivity;
          original.outputIndicator = deptPlan.outputIndicator;
          original.baseline = deptPlan.baseline;
          original.budgetCode = deptPlan.budgetCode;
          original.comparativeTarget = deptPlan.comparativeTarget;
          original.justification = deptPlan.justification;
          original.budgetAmount = deptPlan.budgetAmount;
          original.IdentifiedRisks = deptPlan.IdentifiedRisks;
          original.intDept = deptPlan.intDept;

          _context.ActivityAssess.Update(original);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
          if (!ActivityAssessExists(deptPlan.intAssess))
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
      ViewBag.StrategicIntervention = _context.StrategicIntervention == null ? new List<StrategicIntervention>() : await _context.StrategicIntervention.ToListAsync();
      ViewBag.StrategicAction = _context.StrategicAction == null ? new List<StrategicAction>() : await _context.StrategicAction.ToListAsync();
      ViewBag.Activity = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Quarter"] = ListHelper.Quarter();
      ViewBag.Depts = await _context.Departments.ToListAsync();

      return View(deptPlan);
    }


    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.ActivityAssess == null)
      {
        return NotFound();
      }

      var deptPlan = await _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
          .FirstOrDefaultAsync(m => m.intActivity == id);
      if (deptPlan == null)
      {
        return NotFound();
      }
      if (deptPlan != null)
      {
        _context.ActivityAssess.Remove(deptPlan);
      }

      await _context.SaveChangesAsync();
      //ViewBag.Users = _userManager;
      return View(nameof(Index));
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      if (_context.ActivityAssess == null)
      {
        return Problem("Entity set 'AppDbContext.ActivityAssess'  is null.");
      }
      var deptPlan = await _context.ActivityAssess.FindAsync(id);
      if (deptPlan != null)
      {
        _context.ActivityAssess.Remove(deptPlan);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool ActivityAssessExists(int id)
    {
      return (_context.ActivityAssess?.Any(e => e.intActivity == id)).GetValueOrDefault();
    }

    public SelectList GetSelectListForRiskConsequence()
    {
      var enumData = new List<GetSelectListForEnumDto>();
      enumData.Add(new GetSelectListForEnumDto { ID = 0, Name = "--Select--" });
      enumData.AddRange(from RiskConsequence e in Enum.GetValues(typeof(RiskConsequence))
                        select new GetSelectListForEnumDto
                        {
                          ID = (int)e,
                          Name = e.ToString()
                        });

      return new SelectList(enumData, "ID", "Name");

    }

    public SelectList GetSelectListForRiskLikelihood()
    {
      var enumData = new List<GetSelectListForEnumDto>();
      enumData.Add(new GetSelectListForEnumDto { ID = 0, Name = "--Select--" });
      enumData.AddRange(from RiskLikelihood e in Enum.GetValues(typeof(RiskLikelihood))
                        select new GetSelectListForEnumDto
                        {
                          ID = (int)e,
                          Name = e.ToString()
                        });

      return new SelectList(enumData, "ID", "Name");

    }

    public RiskMatrix GetRiskMatrix(int RCId, int RLId)
    {
      if (_context.RiskMatrixes != null)
      {
        return _context.RiskMatrixes.Where(e => e.RiskConsequenceId == RCId && e.RiskLikelihoodId == RLId).FirstOrDefault();
      }
      else
      {
        return new RiskMatrix();
      }
    }

    public async Task<IActionResult> RegionalAssessmentVerification(List<int> selectedIds, int apprStatus)
    {
      if (selectedIds != null && selectedIds.Count > 0 && apprStatus != null && apprStatus != 0)
      {
        foreach (int id in selectedIds)
        {
          var activityAssessmentRegion = _context.ActivityAssessRegion.Where(x => x.intRegionAssess == id).FirstOrDefault();
          if (activityAssessmentRegion != null)
          {
            activityAssessmentRegion.ApprStatus = apprStatus;
            _context.SaveChanges();
          }
        }
      }
      if (apprStatus == 1 || apprStatus == 2)
      {
        return RedirectToAction(nameof(RegionalVerify));
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


      return RedirectToAction(nameof(RegionalVerify));
    }
    public async Task<IActionResult> DeptPlanVerification(List<int> selectedIds, int apprStatus)
    {
      if (selectedIds != null && selectedIds.Count > 0 && apprStatus != null && apprStatus != 0)
      {
        foreach (int id in selectedIds)
        {
          var activityAsses = _context.ActivityAssess
            .Include(x => x.ActivityAssessRegions)
            .ThenInclude(x => x.QuaterlyPlans)
            .Include(x => x.StrategicIntervention)
            .ThenInclude(x => x.StrategicObjective)
            .Include(x => x.StrategicAction)
            .Include(x => x.ActivityFk)
            .Include(x => x.QuaterlyPlans)
            .Where(x => x.intAssess == id).FirstOrDefault();

          if (activityAsses != null)
          {
            Guid departmentId = Guid.Parse(HttpContext.Session.GetString("Department"));
            activityAsses.ApprStatus = apprStatus;
            if (apprStatus == 9)
            {
              ActivityAssessment activityAssessment = new()
              {
                intFocus = activityAsses?.StrategicIntervention?.StrategicObjective?.intFocus,
                strategicObjective = activityAsses?.StrategicIntervention?.StrategicObjective?.ObjectiveName ?? "",
                strategicIntervention = activityAsses?.StrategicIntervention?.InterventionName ?? "",
                StrategicAction = activityAsses?.StrategicAction?.actionName ?? "",
                activity = activityAsses?.ActivityFk?.activityName ?? "",
                outputIndicator = activityAsses?.outputIndicator ?? "",
                baseline = activityAsses?.baseline,
                budgetCode = activityAsses?.budgetCode,
                comparativeTarget = activityAsses?.comparativeTarget,
                justification = activityAsses?.justification,
                budgetAmount = activityAsses?.budgetAmount,
                intDept = departmentId,
                actType = activityAsses?.actType,
              };

              _context.Add(activityAssessment);
              _context.SaveChanges();

              //List<QuaterlyPlan> quaterlyPlans = activityAsses.QuaterlyPlans.Select(x => new QuaterlyPlan()
              //{
              //  ActivityAssessmentId = activityAssessment.intDeptPlan,
              //  Quarter = x.Quarter,
              //  QTarget = x.QTarget,
              //  QBudget = x.QBudget,
              //  ActivityAccessId = x.ActivityAccessId,
              //  DeptPlanId = x.DeptPlanId,
              //}).ToList();
              if (activityAsses.actType == 0)
              {
                foreach (var quarter in activityAsses.QuaterlyPlans)
                {
                  quarter.ActivityAssessmentId = activityAssessment.intDeptPlan;
                }
                _context.SaveChanges();
              }
              else
              {
                foreach (var region in activityAsses.ActivityAssessRegions)
                {
                  ActivityAssessmentRegion activityAssessmentRegion = new ActivityAssessmentRegion()
                  {
                    intAssess = id,
                    intAssessment = activityAssessment.intDeptPlan,
                    intRegion = region.intRegion,
                    budgetAmount = region?.budgetAmount,
                    Quarter = region.Quarter,
                    QTarget = region.QTarget,
                  };

                  _context.ActivityAssessmentRegion.Add(activityAssessmentRegion);
                  _context.SaveChanges();

                  foreach (var quarter in region.QuaterlyPlans)
                  {
                    quarter.ActivityAssessmentRegionId = activityAssessmentRegion.intRegionAssess;
                    quarter.ActivityAssessmentId = activityAssessment.intDeptPlan;
                  }
                }
                _context.SaveChanges();
              }

              //_context.QuaterlyPlans.UpdateRange(quaterlyPlans);
            }
            _context.SaveChanges();

          }
        }
      }

      if (apprStatus == 1 || apprStatus == 2)
      {
        return RedirectToAction(nameof(ShowConsolidatedDeptPlan));
      }
      if (apprStatus == 3 || apprStatus == 4)
      {
        return RedirectToAction(nameof(DirVerify));
      }
      else if (apprStatus == 5 || apprStatus == 6)
      {
        return RedirectToAction(nameof(Consolidation));
      }
      else if (apprStatus == 7 || apprStatus == 8)
      {
        return RedirectToAction(nameof(HeadBpdVerify));
      }
      return RedirectToAction(nameof(DirAppr));
    }
  }
}
