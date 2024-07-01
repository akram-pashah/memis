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
        var dat = _context.ActivityAssess.Include(m => m.StrategicAction).Include(m => m.StrategicIntervention).Include(m => m.ActivityFk)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<ActivityAssess>
        {
          Data = await dat.AsNoTracking().ToListAsync(),
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
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.ActivityAssess != null)
      {
        var dat = _context.ActivityAssess.Include(m => m.StrategicAction).Include(m => m.StrategicIntervention).Include(m => m.ActivityFk)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<ActivityAssess>
        {
          Data = await dat.AsNoTracking().ToListAsync(),
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
          BudgetCost = _context.ActivityAssess.Where(x => x.intDept == x.intDept && x.actType == 1).Sum(x => x.budgetAmount)
        }).Skip(offset)
            .Take(pageSize);

        var groupedData = await _context.ActivityAssess.Include(a => a.DepartmentFk)
            .Where(a => a.actType == 1)
            .GroupBy(a => a.intDept)
            .Select(g => new ActivityAssess
            {
              intDept = g.Key,
              budgetAmount = g.Sum(a => a.budgetAmount),
              QTarget = g.Sum(a => a.QTarget),
              comparativeTarget = g.Sum(a => a.comparativeTarget),
              StrategicIntervention = g.FirstOrDefault().StrategicIntervention,
              StrategicAction = g.FirstOrDefault().StrategicAction,
              ActivityFk = g.FirstOrDefault().ActivityFk,
              outputIndicator = g.FirstOrDefault().outputIndicator,
              baseline = g.FirstOrDefault().baseline,
              justification = g.FirstOrDefault().justification,
              Quarter = g.FirstOrDefault().Quarter,
              QBudget = g.FirstOrDefault().QBudget,
              ApprStatus = g.FirstOrDefault().ApprStatus,
              actType = g.FirstOrDefault().actType,
              IdentifiedRisks = g.FirstOrDefault().IdentifiedRisks,
            })
            .ToListAsync();

        // Fetch the data where actType is not equal to 1
        var nonAllocatedData = await _context.ActivityAssess.Include(a => a.DepartmentFk)
            .Where(a => a.actType != 1)
            .Skip(offset)
            .Take(pageSize)
            .ToListAsync();

        // Combine both datasets
        var combinedData = (groupedData).Concat(nonAllocatedData).ToList();

        var result = new PagedResult<ActivityAssess>
        {
          Data = combinedData,
          TotalItems = _context.ActivityAssess.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize
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


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AllocatetoRegion(int id,ActivityAssess dto)
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
        return  RedirectToAction(nameof(AllocatetoRegion)); 
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

      var deptPlan = await _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
          .Where(m => m.intActivity == id).FirstOrDefaultAsync();
      if (deptPlan == null)
      {
        return NotFound();
      }
      ActivityAssessDto activityDto = new ActivityAssessDto
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
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(activityDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HeadVerifyStatus(ActivityAssessDto objectdto)
    {
      if (objectdto.intActivity != null && objectdto.intActivity != 0)
      {
        try
        {
          var pp = await _context.ActivityAssess.FindAsync(objectdto.intActivity);
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
        return RedirectToAction(nameof(Verify));
      }
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(objectdto);
    }

    public IActionResult DirVerify(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.ActivityAssess != null)
      {
        var dat = _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
            .Where(x => x.ApprStatus == (int)(deptPlanApprStatus.hodreviewed))
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
    public async Task<IActionResult> DirVerifyStatus(int? id)
    {
      if (id == null || _context.ActivityAssess == null)
      {
        return NotFound();
      }

      var deptPlan = await _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
          .Where(m => m.intActivity == id).FirstOrDefaultAsync();
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
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DirVerifyStatus(ActivityAssessDto objectdto)
    {
      if (objectdto.intActivity != null && objectdto.intActivity != 0)
      {
        try
        {
          var pp = await _context.ActivityAssess.FindAsync(objectdto.intActivity);
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
        var dat = _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
            .Where(x => x.ApprStatus == (int)(deptPlanApprStatus.dirapprove))
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
    public async Task<IActionResult> ConsolidationStatus(int? id)
    {
      if (id == null || _context.ActivityAssess == null)
      {
        return NotFound();
      }

      var deptPlan = await _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
          .Where(m => m.intActivity == id).FirstOrDefaultAsync();
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
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConsolidationStatus(ActivityAssessDto objectdto)
    {
      if (objectdto.intActivity != null && objectdto.intActivity != 0)
      {
        try
        {
          var pp = await _context.ActivityAssess.FindAsync(objectdto.intActivity);
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
        var dat = _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
            .Where(x => x.ApprStatus == (int)(deptPlanApprStatus.meOfficerVerified))
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
    public async Task<IActionResult> HeadBpdVerifyStatus(int? id)
    {
      if (id == null || _context.ActivityAssess == null)
      {
        return NotFound();
      }

      var deptPlan = await _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
          .Where(m => m.intActivity == id).FirstOrDefaultAsync();
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
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HeadBpdVerifyStatus(ActivityAssessDto objectdto)
    {
      if (objectdto.intActivity != null && objectdto.intActivity != 0)
      {
        try
        {
          var pp = await _context.ActivityAssess.FindAsync(objectdto.intActivity);
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
        var dat = _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
            .Where(x => x.ApprStatus == (int)(deptPlanApprStatus.headBpdVerified))
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
    public async Task<IActionResult> DirApprStatus(int? id)
    {
      if (id == null || _context.ActivityAssess == null)
      {
        return NotFound();
      }

      var deptPlan = await _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
          .Where(m => m.intActivity == id).FirstOrDefaultAsync();
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
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DirApprStatus(ActivityAssessDto objectdto)
    {
      if (objectdto.intActivity != null && objectdto.intActivity != 0)
      {
        try
        {
          var pp = await _context.ActivityAssess.FindAsync(objectdto.intActivity);
          if (pp == null)
          {
            return NotFound();
          }
          if (objectdto.ApprStatus == 1)
          {
            pp.ApprStatus = (int)deptPlanApprStatus.dirapprapproved;
          }
          else if (objectdto.ApprStatus == 2)
          {
            pp.ApprStatus = (int)deptPlanApprStatus.dirapprrejected;
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

      var deptPlan = await _context.ActivityAssess.Include(m => m.StrategicIntervention).Include(m => m.StrategicAction).Include(s => s.ActivityFk)
          .FirstOrDefaultAsync(m => m.intActivity == id);
      if (deptPlan == null)
      {
        return NotFound();
      }
      ViewBag.Users = _userManager;
      return View(deptPlan);
    }
    public async Task<IActionResult> Create()
    {

      ViewBag.StrategicIntervention = _context.StrategicIntervention == null ? new List<StrategicIntervention>() : await _context.StrategicIntervention.ToListAsync();
      ViewBag.StrategicAction = _context.StrategicAction == null ? new List<StrategicAction>() : await _context.StrategicAction.ToListAsync();
      ViewBag.Activity = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["intDept"] = new SelectList(_context.Departments, "intDept", "deptName");
      ViewData["Quarter"] = ListHelper.Quarter();
      ActivityAssessDto activityAssessDto = new ActivityAssessDto();
      return View(activityAssessDto);
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
        if(dto.QuaterlyPlans.Count > 0)
        {
          dto.QuaterlyPlans.ForEach(ev => ev.ActivityAccessId = deptPlan.intAssess);
          _context.QuaterlyPlans.AddRange(dto.QuaterlyPlans);
          _context.SaveChanges();
        }

        DeptPlan dept = new DeptPlan()
        {
          intActivity = deptPlan.intActivity ?? 0,
          //StrategicObjective = deptPlan.Str
          strategicIntervention = deptPlan.intIntervention,
          StrategicAction = deptPlan.intAction,
          activity = deptPlan.intActivity?.ToString() ?? "",
          outputIndicator = deptPlan.outputIndicator,
          baseline = deptPlan.baseline,
          budgetCode = deptPlan.budgetCode,
          unitCost = deptPlan.QTarget,
          Q1Target = dto.QuaterlyPlans.Where(x => x.Quarter == "1").Select(x => x.QTarget).FirstOrDefault(),
          Q2Target = dto.QuaterlyPlans.Where( x=> x.Quarter == "2").Select(x => x.QTarget).FirstOrDefault(),
          Q3Target = dto.QuaterlyPlans.Where( x=> x.Quarter == "3").Select(x => x.QTarget).FirstOrDefault(),
          Q4Target = dto.QuaterlyPlans.Where( x=> x.Quarter == "4").Select(x => x.QTarget).FirstOrDefault(),
          Q1Budget = dto.QuaterlyPlans.Where(x => x.Quarter == "1").Select(x => x.QBudget).FirstOrDefault(),
          Q2Budget = dto.QuaterlyPlans.Where( x=> x.Quarter == "2").Select(x => x.QBudget).FirstOrDefault(),
          Q3Budget = dto.QuaterlyPlans.Where( x=> x.Quarter == "3").Select(x => x.QBudget).FirstOrDefault(),
          Q4Budget = dto.QuaterlyPlans.Where( x=> x.Quarter == "4").Select(x => x.QBudget).FirstOrDefault(),
          comparativeTarget = dto.comparativeTarget,
          justification = dto.justification,
          budgetAmount = dto.budgetAmount,
          IsVerified = false,
          DepartmentId = dto.intDept,
        };
        _context.DeptPlans?.Add(dept);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
      }
      ViewBag.StrategicIntervention = _context.StrategicIntervention == null ? new List<StrategicIntervention>() : await _context.StrategicIntervention.ToListAsync();
      ViewBag.StrategicAction = _context.StrategicAction == null ? new List<StrategicAction>() : await _context.StrategicAction.ToListAsync();
      ViewBag.Activity = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["intDept"] = new SelectList(_context.Departments, "intDept", "deptName",dto.intDept);
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
        QuaterlyPlans = await _context.QuaterlyPlans.Where(x => x.ActivityAccessId == deptPlan.intAssess).ToListAsync()
      };
      
      ViewBag.StrategicIntervention = _context.StrategicIntervention == null ? new List<StrategicIntervention>() : await _context.StrategicIntervention.ToListAsync();
      ViewBag.StrategicAction = _context.StrategicAction == null ? new List<StrategicAction>() : await _context.StrategicAction.ToListAsync();
      ViewBag.Activity = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Quarter"] = ListHelper.Quarter();
      ViewData["intDept"] = new SelectList(_context.Departments, "intDept", "deptName", dto.intDept);

      return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("intAssess,intIntervention,intAction,intActivity,outputIndicator,baseline,unitCost,comparativeTarget,justification,budgetAmount,QuaterlyPlans")] ActivityAssessDto deptPlan)
    {
      if (id != deptPlan.intAssess)
      {
        return NotFound();
      }

      var original = _context.ActivityAssess.Where(x => x.intAssess == id).FirstOrDefault();
      if(original == null)
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
              if(quat.Id != 0)
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
         
          _context.Entry(original).State = EntityState.Modified;
          await _context.SaveChangesAsync();

          DeptPlan dept = new DeptPlan()
          {
            intActivity = deptPlan.intActivity ?? 0,
            //StrategicObjective = deptPlan.Str
            strategicIntervention = deptPlan.intIntervention,
            StrategicAction = deptPlan.intAction,
            activity = deptPlan.intActivity?.ToString() ?? "",
            outputIndicator = deptPlan.outputIndicator,
            baseline = deptPlan.baseline,
            budgetCode = deptPlan.budgetCode,
            unitCost = deptPlan.QTarget,
            Q1Target = deptPlan.QuaterlyPlans.Where(x => x.Quarter == "1").Select(x => x.QTarget).FirstOrDefault(),
            Q2Target = deptPlan.QuaterlyPlans.Where(x => x.Quarter == "2").Select(x => x.QTarget).FirstOrDefault(),
            Q3Target = deptPlan.QuaterlyPlans.Where(x => x.Quarter == "3").Select(x => x.QTarget).FirstOrDefault(),
            Q4Target = deptPlan.QuaterlyPlans.Where(x => x.Quarter == "4").Select(x => x.QTarget).FirstOrDefault(),
            Q1Budget = deptPlan.QuaterlyPlans.Where(x => x.Quarter == "1").Select(x => x.QBudget).FirstOrDefault(),
            Q2Budget = deptPlan.QuaterlyPlans.Where(x => x.Quarter == "2").Select(x => x.QBudget).FirstOrDefault(),
            Q3Budget = deptPlan.QuaterlyPlans.Where(x => x.Quarter == "3").Select(x => x.QBudget).FirstOrDefault(),
            Q4Budget = deptPlan.QuaterlyPlans.Where(x => x.Quarter == "4").Select(x => x.QBudget).FirstOrDefault(),
            comparativeTarget = deptPlan.comparativeTarget,
            justification = deptPlan.justification,
            budgetAmount = deptPlan.budgetAmount,
            IsVerified = false,
            DepartmentId = deptPlan.intDept,
          };
          _context.Entry(dept).State = EntityState.Modified;
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
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
      ViewData["intDept"] = new SelectList(_context.Departments, "intDept", "deptName", deptPlan.intDept);

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
  }
}
