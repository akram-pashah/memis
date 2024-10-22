using cloudscribe.Pagination.Models;
using MEMIS.Data;
using MEMIS.Data.Project;
using MEMIS.Models;
using MEMIS.ViewModels.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MEMIS.Controllers
{
  public class ProjectInitiationController : Controller
  {
    private readonly Data.AppDbContext _context;

    public ProjectInitiationController(Data.AppDbContext context)
    {
      _context = context;
    }

    private string[] getUserRoles()
    {
      string userRolesString = HttpContext.Session.GetString("UserRoles");
      string[] userRoles = userRolesString?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
      return userRoles;
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

    public async Task<IActionResult> Dashboard()
    {
      var userRoles = getUserRoles();
      Guid departmentId = Guid.Parse(HttpContext.Session.GetString("Department"));

      List<double> projectsByType = [];

      var constructions = _context.ProjectInitiations.Where(x => x.Type == 1).Count();
      var works = _context.ProjectInitiations.Where(x => x.Type == 2).Count();
      var services = _context.ProjectInitiations.Where(x => x.Type == 3).Count();
      var programs = _context.ProjectInitiations.Where(x => x.Type == 4).Count();

      projectsByType.Add(constructions);
      projectsByType.Add(works);
      projectsByType.Add(services);
      projectsByType.Add(programs);

      List<double> risksByRank = [];

      var veryLow = _context.ProjectRiskIdentifications.Where(x => x.Rank == 1).Count();
      var low = _context.ProjectRiskIdentifications.Where(x => x.Rank == 2).Count();
      var medium = _context.ProjectRiskIdentifications.Where(x => x.Rank == 3).Count();
      var high = _context.ProjectRiskIdentifications.Where(x => x.Rank == 4).Count();
      var veryHigh = _context.ProjectRiskIdentifications.Where(x => x.Rank == 5).Count();

      risksByRank.Add(veryLow);
      risksByRank.Add(low);
      risksByRank.Add(medium);
      risksByRank.Add(high);
      risksByRank.Add(veryHigh);

      double totalProjects = _context.ProjectInitiations.Count();
      double totalProjectsCompleted = _context.ProjectInitiations.Include(x => x.ActivityPlans).Where(x => x.ActivityPlans.All(y => y.Status == 1)).Count();
      double totalProjectsPending = _context.ProjectInitiations.Include(x => x.ActivityPlans).Where(x => x.ActivityPlans.Any(y => y.Status != 1)).Count();
      double totalProjectsOverdue = _context.ProjectInitiations.Include(x => x.ActivityPlans).Where(x => x.ActivityPlans.Any(y => y.Status != 1) && x.EndDate > DateTime.Now).Count();
      double totalProjectsInProgress = _context.ProjectInitiations.Include(x => x.ActivityPlans).Where(x => x.ActivityPlans.Any(y => y.Status == 2)).Count();

      List<double> activitiesByStatus = [];
      activitiesByStatus.Add(totalProjects);
      activitiesByStatus.Add(totalProjectsCompleted);
      activitiesByStatus.Add(totalProjectsPending);
      activitiesByStatus.Add(totalProjectsOverdue);
      activitiesByStatus.Add(totalProjectsInProgress);

      List<double> tasksByStatus = [];

      tasksByStatus.Add(_context.MonitoringAndControls.Where(x => x.Status == "Completed").Count());
      tasksByStatus.Add(_context.MonitoringAndControls.Where(x => x.Status == "Work on Progress").Count());
      tasksByStatus.Add(_context.MonitoringAndControls.Where(x => x.Status == "Not Started").Count());

      PMDashboardViewModel data = new()
      {
        TotalProjects = totalProjects,
        TotalProjectsCompleted = totalProjectsCompleted,
        TotalProjectsPending = totalProjectsPending,
        TotalProjectsOverdue = totalProjectsOverdue,
        TotalProjectsInProgress = totalProjectsInProgress,
        ProjectsByType = projectsByType,
        RisksByRank = risksByRank,
        ActivitiesByStatus = activitiesByStatus,
        TasksByStatus = tasksByStatus
      };

      return View(data);
    }

    static List<string> GenerateRandomColors(int count)
    {
      Random random = new Random();
      HashSet<string> colors = new HashSet<string>();

      while (colors.Count < count)
      {
        // Generate random RGB values
        int r = random.Next(256);
        int g = random.Next(256);
        int b = random.Next(256);

        // Convert RGB to hexadecimal format
        string hexColor = $"#{r:X2}{g:X2}{b:X2}";

        // Add only unique colors
        colors.Add(hexColor);
      }

      return new List<string>(colors);
    }

    private double GetCompletionValue(int impStatusId)
    {
      return impStatusId switch
      {
        1 => 0,    // 0% completion
        2 => 0.5,  // 50% completion
        3 => 1,    // 100% completion
        _ => 0     // Default to 0 if impStatusId is invalid or unexpected
      };
    }

    private static List<int> GetFinancialYears()
    {
      int currentYear = DateTime.Now.Year;
      List<int> financialYears = new List<int>();

      // Find the starting year (FY1) based on the current year
      int startYear = currentYear - ((currentYear - 1) % 5); // FY1 starts at the closest year divisible by 5 + 1

      // Calculate 5 financial years starting from the determined startYear
      for (int i = 0; i < 5; i++)
      {
        int fy = startYear + i;
        financialYears.Add(fy);
      }

      return financialYears;
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
      if (projectInitiationDto.StakeHolder != null && projectInitiationDto.StakeHolder.StakeholderName != null)
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
