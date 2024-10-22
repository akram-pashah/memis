using cloudscribe.Pagination.Models;
using MEMIS.Data;
using MEMIS.Data.Risk;
using MEMIS.Models;
using MEMIS.Models.Risk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MEMIS.Controllers.Risk
{
  [Authorize]
  public class RiskIdentificationsController : Controller
  {
    private readonly Data.AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public RiskIdentificationsController(Data.AppDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }


    public IActionResult Index(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskIdentifications != null)
      {
        var dat = _context.RiskIdentifications.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskIdentification>
        {
          Data = dat.AsNoTracking().OrderByDescending(x => x.RiskId).ToList(),
          TotalItems = _context.RiskIdentifications.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskIdentifications'  is null.");
      }
    }

    private string[] getUserRoles()
    {
      string userRolesString = HttpContext.Session.GetString("UserRoles");
      string[] userRoles = userRolesString?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
      return userRoles;
    }

    public async Task<IActionResult> Dashboard()
    {
      var userRoles = getUserRoles();
      Guid departmentId = Guid.Parse(HttpContext.Session.GetString("Department"));

      return View();
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

    public IActionResult Verify(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskIdentifications != null)
      {
        var dat = _context.RiskIdentifications.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk)
            .Where(x => x.ApprStatus == 0)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskIdentification>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskIdentifications.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskIdentifications'  is null.");
      }
    }
    public async Task<IActionResult> HeadVerifyStatus(int? id)
    {
      if (id == null || _context.RiskIdentifications == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskIdentifications.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk)
          .Where(m => m.RiskId == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskIdentificationCreateEditDto riskDto = new RiskIdentificationCreateEditDto
      {
        RiskId = riskIdentification.RiskId,
        Activity = riskIdentification.Activity,
        EvalCriteria = riskIdentification.EvalCriteria,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        IsVerified = riskIdentification.IsVerified,
        Events = await _context.Events.Where(x => x.RiskId == id).ToListAsync(),
        RiskCause = await _context.RiskCauses.Where(x => x.RiskId == id).ToListAsync(),
        RiskConsequence = await _context.RiskConsequenceDetails.Where(x => x.RiskId == id).ToListAsync(),
        RiskSource = await _context.RiskSources.Where(x => x.RiskId == id).ToListAsync(),
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        StrategicObjective = riskIdentification.StrategicObjective,
      };
      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HeadVerifyStatus(RiskIdentificationCreateEditDto objectdto)
    {
      if (objectdto.RiskId != null && objectdto.RiskId != 0)
      {
        try
        {
          var pp = await _context.RiskIdentifications.FindAsync(objectdto.RiskId);
          if (pp == null)
          {
            return NotFound();
          }
          if (objectdto.ApprStatus == 1)
          {
            pp.ApprStatus = (int)riskIdentifyApprStatus.hodreviewed;
          }
          else if (objectdto.ApprStatus == 2)
          {
            pp.ApprStatus = (int)riskIdentifyApprStatus.hodrejected;
          }
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(objectdto.RiskId))
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
      ViewData["Category"] = ListHelper.CategoryofPremises();
      ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
      ViewData["ProductClassification"] = ListHelper.ProductClassification();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(objectdto);
    }

    public IActionResult DirVerify(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskIdentifications != null)
      {
        var dat = _context.RiskIdentifications.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk)
            .Where(x => x.ApprStatus == (int)(riskIdentifyApprStatus.hodreviewed))
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskIdentification>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskIdentifications.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskIdentifications'  is null.");
      }
    }
    public async Task<IActionResult> DirVerifyStatus(int? id)
    {
      if (id == null || _context.RiskIdentifications == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskIdentifications.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk)
          .Where(m => m.RiskId == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskIdentificationCreateEditDto riskDto = new RiskIdentificationCreateEditDto
      {
        RiskId = riskIdentification.RiskId,
        Activity = riskIdentification.Activity,
        EvalCriteria = riskIdentification.EvalCriteria,
        //Events = riskIdentification.Events,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        IsVerified = riskIdentification.IsVerified,
        //RiskCause = riskIdentification.RiskCause,
        //RiskConsequence = riskIdentification.RiskConsequence,
        Events = await _context.Events.Where(x => x.RiskId == id).ToListAsync(),
        RiskCause = await _context.RiskCauses.Where(x => x.RiskId == id).ToListAsync(),
        RiskConsequence = await _context.RiskConsequenceDetails.Where(x => x.RiskId == id).ToListAsync(),
        RiskSource = await _context.RiskSources.Where(x => x.RiskId == id).ToListAsync(),
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        //RiskSource = riskIdentification.RiskSource,
        StrategicObjective = riskIdentification.StrategicObjective,
      };
      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DirVerifyStatus(RiskIdentificationCreateEditDto objectdto)
    {
      if (objectdto.RiskId != null && objectdto.RiskId != 0)
      {
        try
        {
          var pp = await _context.RiskIdentifications.FindAsync(objectdto.RiskId);
          if (pp == null)
          {
            return NotFound();
          }
          if (objectdto.ApprStatus == 1)
          {
            pp.ApprStatus = (int)riskIdentifyApprStatus.dirapprove;
          }
          else if (objectdto.ApprStatus == 2)
          {
            pp.ApprStatus = (int)riskIdentifyApprStatus.dirrejected;
          }

          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(objectdto.RiskId))
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
      ViewData["Category"] = ListHelper.CategoryofPremises();
      ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
      ViewData["ProductClassification"] = ListHelper.ProductClassification();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(objectdto);
    }
    public IActionResult RmoVerify(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskIdentifications != null)
      {
        var dat = _context.RiskIdentifications.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk)
            .Where(x => x.ApprStatus == (int)(riskIdentifyApprStatus.dirapprove))
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskIdentification>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskIdentifications.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskIdentifications'  is null.");
      }
    }
    public async Task<IActionResult> RmoVerifyStatus(int? id)
    {
      if (id == null || _context.RiskIdentifications == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskIdentifications.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk)
          .Where(m => m.RiskId == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskIdentificationCreateEditDto riskDto = new RiskIdentificationCreateEditDto
      {
        RiskId = riskIdentification.RiskId,
        Activity = riskIdentification.Activity,
        EvalCriteria = riskIdentification.EvalCriteria,
        //Events = riskIdentification.Events,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        IsVerified = riskIdentification.IsVerified,
        //RiskCause = riskIdentification.RiskCause,
        //RiskConsequence = riskIdentification.RiskConsequence,
        Events = await _context.Events.Where(x => x.RiskId == id).ToListAsync(),
        RiskCause = await _context.RiskCauses.Where(x => x.RiskId == id).ToListAsync(),
        RiskConsequence = await _context.RiskConsequenceDetails.Where(x => x.RiskId == id).ToListAsync(),
        RiskSource = await _context.RiskSources.Where(x => x.RiskId == id).ToListAsync(),
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        //RiskSource = riskIdentification.RiskSource,
        StrategicObjective = riskIdentification.StrategicObjective,

      };
      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RmoVerifyStatus(RiskIdentificationCreateEditDto objectdto)
    {
      if (objectdto.RiskId != null && objectdto.RiskId != 0)
      {
        try
        {
          var pp = await _context.RiskIdentifications.FindAsync(objectdto.RiskId);
          if (pp == null)
          {
            return NotFound();
          }
          string category = await _context.RiskCategorys.Where(x => x.intCategory == objectdto.intCategory).Select(x => x.CategoryCode).FirstOrDefaultAsync() ?? "";
          string deptCode = await _context.Departments.Where(x => x.intDept == objectdto.intDept).Select(x => x.deptCode).FirstOrDefaultAsync() ?? "";
          if (objectdto.ApprStatus == 1)
          {
            pp.ApprStatus = (int)riskIdentifyApprStatus.rmoapproved;
            RiskRegister riskRegister = new()
            {
              RiskId = objectdto.RiskId,
              RiskCode = $"NDA/{category}/{deptCode}",
              Activity = objectdto.Activity,
              EvalCriteria = objectdto.EvalCriteria,
              //Events = objectdto.Events,
              FocusArea = objectdto.FocusArea,
              IdentifiedDate = objectdto.IdentifiedDate,
              //RiskCause = objectdto.RiskCause,
              //RiskConsequence = objectdto.RiskConsequence,
              RiskConsequenceId = objectdto.RiskConsequenceId,
              RiskDescription = objectdto.RiskDescription,
              RiskLikelihoodId = objectdto.RiskLikelihoodId,
              RiskRank = objectdto.RiskRank,
              RiskScore = objectdto.RiskScore,
              //RiskSource = objectdto.RiskSource,
              StrategicObjective = objectdto.StrategicObjective,
              intCategory = objectdto.intCategory,
              intDept = objectdto.intDept,
              ExistingMitigation = objectdto.ExistingMitigation,
              Additional_Mitigation = objectdto.Additional_Mitigation,
              Opportunity = objectdto.Opportunity,
              Supporting_Owners = objectdto.Supporting_Owners,
              Weakness = objectdto.Weakness,
              RiskOwner = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };
            _context.Add(riskRegister);
          }
          else if (objectdto.ApprStatus == 2)
          {
            pp.ApprStatus = (int)riskIdentifyApprStatus.rmorejected;
          }

          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(objectdto.RiskId))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(RmoVerify));
      }
      ViewData["Category"] = ListHelper.CategoryofPremises();
      ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
      ViewData["ProductClassification"] = ListHelper.ProductClassification();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(objectdto);
    }
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null || _context.RiskIdentifications == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskIdentifications.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk)
          .FirstOrDefaultAsync(m => m.RiskId == id);
      if (riskIdentification == null)
      {
        return NotFound();
      }
      ViewBag.Users = _userManager;
      return View(riskIdentification);
    }


    public async Task<IActionResult> Create()
    {

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewBag.RiskCategory = _context.RiskCategorys == null ? new List<RiskCategory>() : await _context.RiskCategorys.ToListAsync();
      ViewBag.Department = _context.Departments == null ? new List<Department>() : await _context.Departments.ToListAsync();
      RiskIdentificationCreateEditDto riskIdentificationCreateEditDto = new RiskIdentificationCreateEditDto();
      riskIdentificationCreateEditDto.IdentifiedDate = DateTime.Today;
      return View(riskIdentificationCreateEditDto);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(
          "IdentifiedDate,StrategicObjective,FocusArea,Activity,BudgetCode" +
          ",RiskDescription,Events,RiskSource,RiskCause,RiskConsequence,RiskConsequenceId," +
          "RiskLikelihoodId,RiskScore,RiskRank,EvalCriteria,IsVerified,EventsList,intCategory,ExistingMitigation,Weakness,Additional_Mitigation,intDept,Opportunity,Supporting_Owners")]RiskIdentificationCreateEditDto dto)
    {
      if (ModelState.IsValid && dto.RiskConsequenceId != 0 && dto.RiskLikelihoodId != 0)
      {
        RiskIdentification riskIdentification = new RiskIdentification
        {
          Activity = dto.Activity,
          EvalCriteria = dto.EvalCriteria,
          FocusArea = dto.FocusArea,
          IdentifiedDate = dto.IdentifiedDate,
          IsVerified = dto.IsVerified,
          RiskConsequenceId = dto.RiskConsequenceId,
          RiskDescription = dto.RiskDescription,
          RiskLikelihoodId = dto.RiskLikelihoodId,
          RiskOwner = User.FindFirstValue(ClaimTypes.NameIdentifier),
          RiskRank = dto.RiskRank,
          RiskScore = dto.RiskScore,
          StrategicObjective = dto.StrategicObjective,
          intCategory = dto.intCategory,
          ExistingMitigation = dto.ExistingMitigation,
          Weakness = dto.Weakness,
          Additional_Mitigation = dto.Additional_Mitigation,
          Opportunity = dto.Opportunity,
          Supporting_Owners = dto.Supporting_Owners,
          intDept = dto.intDept
        };

        _context.Add(riskIdentification);
        await _context.SaveChangesAsync();
        dto.Events.ForEach(ev => ev.RiskId = riskIdentification.RiskId);
        dto.RiskCause.ForEach(ev => ev.RiskId = riskIdentification.RiskId);
        dto.RiskSource.ForEach(ev => ev.RiskId = riskIdentification.RiskId);
        dto.RiskConsequence.ForEach(ev => ev.RiskId = riskIdentification.RiskId);
        _context.Events.AddRange(dto.Events);
        _context.RiskCauses.AddRange(dto.RiskCause);
        _context.RiskSources.AddRange(dto.RiskSource);
        _context.RiskConsequenceDetails.AddRange(dto.RiskConsequence);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
      }

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewBag.RiskCategory = _context.RiskCategorys == null ? new List<RiskCategory>() : await _context.RiskCategorys.ToListAsync();
      ViewBag.Department = _context.Departments == null ? new List<Department>() : await _context.Departments.ToListAsync();
      return View(dto);
    }


    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.RiskIdentifications == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskIdentifications.FindAsync(id);
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskIdentificationCreateEditDto dto = new RiskIdentificationCreateEditDto
      {
        Activity = riskIdentification.Activity,
        EvalCriteria = riskIdentification.EvalCriteria,
        Events = await _context.Events.Where(x => x.RiskId == id).ToListAsync(),
        FocusArea = (int)riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        IsVerified = riskIdentification.IsVerified,
        RiskCause = await _context.RiskCauses.Where(x => x.RiskId == id).ToListAsync(),
        RiskConsequence = await _context.RiskConsequenceDetails.Where(x => x.RiskId == id).ToListAsync(),
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskId = riskIdentification.RiskId,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        RiskSource = await _context.RiskSources.Where(x => x.RiskId == id).ToListAsync(),
        StrategicObjective = (int)riskIdentification.StrategicObjective,

      };
      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      return View(dto);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("RiskId," +
          "IdentifiedDate,StrategicObjective,FocusArea,Activity,BudgetCode" +
          ",RiskDescription,Events,RiskSource,RiskCause,RiskConsequence,RiskConsequenceId," +
          "RiskLikelihoodId,RiskScore,RiskRank,EvalCriteria,IsVerified,EventsList")] RiskIdentificationCreateEditDto riskIdentification)
    {
      if (id != riskIdentification.RiskId)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          RiskIdentification rd = new RiskIdentification
          {
            Activity = riskIdentification.Activity,
            EvalCriteria = riskIdentification.EvalCriteria,
            Events = riskIdentification.Events,
            FocusArea = riskIdentification.FocusArea,
            IdentifiedDate = riskIdentification.IdentifiedDate,
            IsVerified = riskIdentification.IsVerified,
            RiskCauses = riskIdentification.RiskCause,
            RiskConsequenceDetails = riskIdentification.RiskConsequence,
            RiskConsequenceId = riskIdentification.RiskConsequenceId,
            RiskDescription = riskIdentification.RiskDescription,
            RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
            RiskOwner = User.FindFirstValue(ClaimTypes.NameIdentifier),
            RiskRank = riskIdentification.RiskRank,
            RiskScore = riskIdentification.RiskScore,
            RiskSources = riskIdentification.RiskSource,
            StrategicObjective = riskIdentification.StrategicObjective,
            RiskId = riskIdentification.RiskId
          };
          _context.Update(rd);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(riskIdentification.RiskId))
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
      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      return View(riskIdentification);
    }


    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.RiskIdentifications == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskIdentifications.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk)
          .FirstOrDefaultAsync(m => m.RiskId == id);
      if (riskIdentification == null)
      {
        return NotFound();
      }
      ViewBag.Users = _userManager;
      return View(riskIdentification);
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      if (_context.RiskIdentifications == null)
      {
        return Problem("Entity set 'AppDbContext.RiskIdentifications'  is null.");
      }
      var riskIdentification = await _context.RiskIdentifications.FindAsync(id);
      if (riskIdentification != null)
      {
        _context.RiskIdentifications.Remove(riskIdentification);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool RiskIdentificationExists(int id)
    {
      return (_context.RiskIdentifications?.Any(e => e.RiskId == id)).GetValueOrDefault();
    }

    public SelectList GetSelectListForRiskConsequence()
    {
      var enumData = new List<GetSelectListForEnumDto>();
      enumData.Add(new GetSelectListForEnumDto { ID = 0, Name = "--Select--" });
      enumData.AddRange(from RiskConsequence e in Enum.GetValues(typeof(RiskConsequence))
                        select new GetSelectListForEnumDto
                        {
                          ID = (int)e,
                          Name = e.GetDisplayName(),
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
                          Name = e.GetDisplayName()
                        });

      return new SelectList(enumData, "ID", "Name");

    }


    public RiskMatrix GetRiskMatrix(int RCId, int RLId)
    {
      if (_context.RiskMatrixes != null)
      {
        var risk = _context.RiskMatrixes.Where(e => e.RiskConsequenceId == RCId && e.RiskLikelihoodId == RLId).FirstOrDefault();
        return risk;
      }
      else
      {
        return new RiskMatrix();
      }
    }

    public async Task<IActionResult> VerifyRisk(int id, bool IsVerified)
    {
      var res = await _context.RiskIdentifications.FirstOrDefaultAsync(m => m.RiskId == id);
      if (res != null)
      {
        res.IsVerified = IsVerified;
      }
      await _context.SaveChangesAsync();

      return RedirectToAction("Verify");
    }
  }

}
