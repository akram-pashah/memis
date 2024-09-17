using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;
using cloudscribe.Pagination.Models;
using MEMIS.Models;
using System.Collections.Generic;
using MEMIS.Data.Risk;
using MEMIS.Models.Risk;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MEMIS.Models.Project;
using MEMIS.Data.Project;

namespace MEMIS.Controllers
{
  public class ProjectInitiationController : Controller
  {
    private readonly Data.AppDbContext _context;

    public ProjectInitiationController(Data.AppDbContext context)
    {
      _context = context;
    }

    public async Task<IActionResult> Index(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      var appDbContext = _context.ProjectInitiations.Include(s => s.Department).Skip(offset).Take(pageSize);
      var result = new PagedResult<ProjectInitiation>
      {
        Data = await appDbContext.AsNoTracking().ToListAsync(),
        TotalItems = _context.ProjectInitiations.Count(),
        PageNumber = pageNumber,
        PageSize = pageSize
      };

      return View(result);
    }

    public async Task<IActionResult> Add(int id = 0)
    {
      var others = _context.ProjectOthersTab.Where(m => m.ProjectInitiationId == id).ToList();
      var activities = _context.ActivityPlans.Where(m => m.ProjectInitiationId == id).ToList();
      var payments = _context.ProjectPayments.Where(m => m.ProjectInitiationId == id).ToList();
      var ristIdentifications = _context.ProjectRiskIdentifications.Where(m => m.ProjectInitiationId == id).ToList();
      var stakeholder = _context.StakeHolder.Where(m => m.ProjectInitiationId == id).ToList();
      var monitoringAndControl = _context.MonitoringAndControls.Where(m => m.ProjectInitiationId == id).ToList();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      ViewData["Impact"] = ListHelper.Impact();
      ViewData["Influence"] = ListHelper.Influence();
      ViewData["ProjectInitiations"] = new SelectList(_context.ProjectInitiations.OrderBy(d => d.Name), "Id", "Name", id);

      return View("Create", new ProjectInitiationDetailsDto { ProjectInitId = id, ActivityPlans = activities, ProjectPayments = payments, ProjectRiskIdentifications = ristIdentifications, ProjectOthersTabs = others, StakeHolders = stakeholder, MonitoringAndControls = monitoringAndControl });
    }

    [HttpPost]
    public async Task<IActionResult> Create(int ProjectInitiationId = 0)
    {
      var others = _context.ProjectOthersTab.Where(m => m.ProjectInitiationId == ProjectInitiationId).ToList();
      var activities = _context.ActivityPlans.Where(m => m.ProjectInitiationId == ProjectInitiationId).ToList();
      var payments = _context.ProjectPayments.Where(m => m.ProjectInitiationId == ProjectInitiationId).ToList();
      var ristIdentifications = _context.ProjectRiskIdentifications.Where(m => m.ProjectInitiationId == ProjectInitiationId).ToList();
      var stakeholder = _context.StakeHolder.Where(m => m.ProjectInitiationId == ProjectInitiationId).ToList();
      var monitoringAndControl = _context.MonitoringAndControls.Where(m => m.ProjectInitiationId == ProjectInitiationId).ToList();

      ViewData["RiskRank"] = ListHelper.RiskRank();
      ViewData["Impact"] = ListHelper.Impact();
      ViewData["Influence"] = ListHelper.Influence();
      ViewData["ProjectInitiations"] = new SelectList(_context.ProjectInitiations.OrderBy(d => d.Name), "Id", "Name", ProjectInitiationId);
      var x = new ProjectInitiationDetailsDto { ProjectInitId = ProjectInitiationId, ActivityPlans = activities, ProjectPayments = payments, ProjectRiskIdentifications = ristIdentifications, ProjectOthersTabs = others, StakeHolders = stakeholder, MonitoringAndControls = monitoringAndControl };
      //x.ActivityPlan = new ActivityPlanDto();
      //x.ProjectRiskIdentification = new ProjectRiskIdentificationDto();
      //x.ProjectPayment = new ProjectPaymentDto();
      return View(x);
    }

    public FileResult Download(int id)
    {
      var other = _context.ProjectOthersTab.FirstOrDefault(m => m.Id == id);
      if (other != null)
      {
        return File(other.Attachment, "application/octet-stream", other.FileName);
      }
      return null;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddActivity(ActivityPlanDto activityPlan)
    {
      if (ModelState.IsValid)
      {
        ActivityPlan _data = new()
        {
          ProjectInitiationId = activityPlan.ProjectInitiationId,
          Activity = activityPlan.Activity,
          StartDate = activityPlan.StartDate,
          EndDate = activityPlan.EndDate,
          Person = activityPlan.Person,
          Cost = activityPlan.Cost,
          Status = activityPlan.Status
        };
        _context.Add(_data);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Add), new { id = activityPlan.ProjectInitiationId });
      }
      return RedirectToAction(nameof(Add));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteActivity(int id)
    {
      if (_context.ActivityPlans == null)
      {
        return Problem("Entity set 'AppDbContext.SDTAssessment'  is null.");
      }
      var activityPlan = await _context.ActivityPlans.FindAsync(id);

      if (activityPlan != null)
      {
        _context.ActivityPlans.Remove(activityPlan);
      }

      try
      {
        await _context.SaveChangesAsync();
        return Ok();
      }
      catch (Exception ex)
      {
        return NotFound(ex.Message);
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRiskIdentified(ProjectRiskIdentificationDto riskIdentification)
    {
      if (ModelState.IsValid)
      {
        ProjectRiskIdentification _data = new()
        {
          ProjectInitiationId = riskIdentification.ProjectInitiationId,
          Stage = riskIdentification.Stage,
          Risk = riskIdentification.Risk,
          //Rank = riskIdentification.Rank,
          Likelihood = riskIdentification.Likelihood,
          Severity = riskIdentification.Severity,
          Consequence = riskIdentification.Consequence,
          Mitigation = riskIdentification.Mitigation,
          RiskImplementationCost = riskIdentification.RiskImplementationCost,
          Ownership = riskIdentification.Ownership,

        };
        _context.Add(_data);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Add), new { id = riskIdentification.ProjectInitiationId });
      }
      return RedirectToAction(nameof(Add));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteRiskIdentification(int id)
    {
      if (_context.ProjectRiskIdentifications == null)
      {
        return Problem("Entity set 'AppDbContext.ProjectRiskIdentifications'  is null.");
      }
      var risk = await _context.ProjectRiskIdentifications.FindAsync(id);

      if (risk != null)
      {
        _context.ProjectRiskIdentifications.Remove(risk);
      }

      try
      {
        await _context.SaveChangesAsync();
        return Ok();
      }
      catch (Exception ex)
      {
        return NotFound(ex.Message);
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPaymentSchedule(ProjectPaymentDto projectPayment)
    {
      if (ModelState.IsValid)
      {
        ProjectPayment _data = new()
        {
          ProjectInitiationId = projectPayment.ProjectInitiationId,
          DueDate = projectPayment.DueDate,
          Activity = projectPayment.Activity,
          Amount = projectPayment.Amount,
        };
        _context.Add(_data);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Add), new { id = projectPayment.ProjectInitiationId });
      }
      return RedirectToAction(nameof(Add));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePaymentSchedule(int id)
    {
      if (_context.ProjectPayments == null)
      {
        return Problem("Entity set 'AppDbContext.ProjectPayments'  is null.");
      }
      var payment = await _context.ProjectPayments.FindAsync(id);

      if (payment != null)
      {
        _context.ProjectPayments.Remove(payment);
      }

      try
      {
        await _context.SaveChangesAsync();
        return Ok();
      }
      catch (Exception ex)
      {
        return NotFound(ex.Message);
      }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMonitoringControl(int id)
    {
      if (_context.MonitoringAndControls == null)
      {
        return Problem("Entity set 'AppDbContext.MonitoringAndControls'  is null.");
      }
      var monitoring = await _context.MonitoringAndControls.FindAsync(id);

      if (monitoring != null)
      {
        _context.MonitoringAndControls.Remove(monitoring);
      }

      try
      {
        await _context.SaveChangesAsync();
        return Ok();
      }
      catch (Exception ex)
      {
        return NotFound(ex.Message);
      }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddprojectOthersTab(ProjectOthersTabDto projectOthersTab)
    {
      var file = Request.Form.Files;
      if (projectOthersTab?.ProjectInitiationId != 0 && projectOthersTab?.Resourses != null)
      {
        if (file != null && file.Count > 0)
        {
          using (var memoryStream = new MemoryStream())
          {
            await file[0].CopyToAsync(memoryStream);
            ProjectOthersTab _data = new()
            {
              FileName = file[0].FileName,
              ProjectInitiationId = projectOthersTab.ProjectInitiationId,
              Attachment = memoryStream.ToArray(),
              Resourses = projectOthersTab.Resourses,
            };
            _context.Add(_data);
            await _context.SaveChangesAsync();
          }

        }
        return RedirectToAction(nameof(Add), new { id = projectOthersTab.ProjectInitiationId });
      }
      return RedirectToAction(nameof(Add));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddMonitoringAndControl(ProjectInitiationDetailsDto projectInitiationDto)
    {
      if (projectInitiationDto.MonitoringAndControl != null)
      {
        var monitoringAndControlDto = projectInitiationDto.MonitoringAndControl;
        MonitoringAndControl _data = new()
        {
          TaskName = monitoringAndControlDto.TaskName,
          Duration = monitoringAndControlDto.Duration,
          StartDate = monitoringAndControlDto.StartDate,
          EndDate = monitoringAndControlDto.EndDate,
          ImplementationStatus = monitoringAndControlDto.ImplementationStatus,
          CompletedDate = monitoringAndControlDto.CompletedDate,
          Status = monitoringAndControlDto.Status,
          ProjectInitiationId = monitoringAndControlDto.ProjectInitiationId
        };
        _context.Add(_data);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Add), new { id = monitoringAndControlDto.ProjectInitiationId });

      }

      return RedirectToAction(nameof(Add));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteprojectOthersTabFile(int id)
    {
      if (_context.ProjectOthersTab == null)
      {
        return Problem("Entity set 'AppDbContext.ProjectPayments'  is null.");
      }
      var file = await _context.ProjectOthersTab.FindAsync(id);

      if (file != null)
      {
        _context.ProjectOthersTab.Remove(file);
      }

      try
      {
        await _context.SaveChangesAsync();
        return Ok();
      }
      catch (Exception ex)
      {
        return NotFound(ex.Message);
      }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteprojectOthersTab(int id)
    {
      if (_context.ProjectPayments == null)
      {
        return Problem("Entity set 'AppDbContext.ProjectPayments'  is null.");
      }
      var payment = await _context.ProjectPayments.FindAsync(id);

      if (payment != null)
      {
        _context.ProjectPayments.Remove(payment);
      }

      try
      {
        await _context.SaveChangesAsync();
        return Ok();
      }
      catch (Exception ex)
      {
        return NotFound(ex.Message);
      }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddStakeholder(ProjectInitiationDetailsDto projectInitiationDto)
    {
      if (projectInitiationDto.StakeHolder != null&&projectInitiationDto.StakeHolder.StakeholderName!=null)
      {
        var stakeholderdto = projectInitiationDto.StakeHolder;
        StakeHolder _data = new()
        {
          ProjectInitiationId = stakeholderdto.ProjectInitiationId,
          StakeholderName = stakeholderdto.StakeholderName,
          ContactPersonName = stakeholderdto.ContactPersonName,
          ContactPersonEmail = stakeholderdto.ContactPersonEmail,
          ContactPersonAddress = stakeholderdto.ContactPersonAddress,
          ContactPersonPhone = stakeholderdto.ContactPersonPhone,
          ContactPersonWebsite = stakeholderdto.ContactPersonWebsite,
          Impact = stakeholderdto.Impact,
          Influence = stakeholderdto.Influence,
          StakeHolderImportant = stakeholderdto.StakeHolderImportant,
          StakeholderContribution = stakeholderdto.StakeholderContribution,
          Stakeholderblock = stakeholderdto.Stakeholderblock,
          StakeholderStrategy = stakeholderdto.StakeholderStrategy
        };
        _context.Add(_data);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Add), new { id = stakeholderdto.ProjectInitiationId });
      }
      return RedirectToAction(nameof(Add));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteStakeholder(int id)
    {
      if (_context.StakeHolder == null)
      {
        return Problem("Entity set 'AppDbContext.StakeHolder'  is null.");
      }
      var stakeholder = await _context.StakeHolder.FindAsync(id);

      if (stakeholder != null)
      {
        _context.StakeHolder.Remove(stakeholder);
      }

      try
      {
        await _context.SaveChangesAsync();
        return Ok();
      }
      catch (Exception ex)
      {
        return NotFound(ex.Message);
      }
    }

    public IActionResult ProjectCreate()
    {
      ViewData["ProjectType"] = ListHelper.ProjectType();
      ViewData["DepartmentId"] = new SelectList(_context.Departments, "intDept", "deptName");

      return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProjectCreate(ProjectInitiationDto dto)
    {
      if (ModelState.IsValid)
      {
        ProjectInitiation project = new ProjectInitiation
        {
          Code = dto.Code,
          Name = dto.Name,
          Type = dto.Type,
          Section = dto.Section,
          Program = dto.Program,
          SubProgram = dto.SubProgram,
          StartDate = dto.StartDate,
          EndDate = dto.EndDate,
          Members = dto.Members,
          BudgetCode = dto.BudgetCode,
          DepartmentId = dto.DepartmentId,
          Desc = dto.Desc,
          Manager = dto.Manager,
        };
        _context.Add(project);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }

      return View(dto);
    }
  }

}
