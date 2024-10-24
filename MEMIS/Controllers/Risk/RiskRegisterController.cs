using cloudscribe.Pagination.Models;
using MEMIS.Data;
using MEMIS.Data.Risk;
using MEMIS.Models;
using MEMIS.Models.Risk;
using MEMIS.ViewModels.ME;
using MEMIS.ViewModels.RiskManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MEMIS.Controllers.Risk
{
  [Authorize]
  public class RiskRegisterController : Controller
  {
    private readonly Data.AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public RiskRegisterController(Data.AppDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    private string[] getUserRoles()
    {
      string userRolesString = HttpContext.Session.GetString("UserRoles");
      string[] userRoles = userRolesString?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
      return userRoles;
    }

    public IActionResult Index(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskRegister != null)
      {
        var dat = _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskRegister>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskRegister.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskRegister'  is null.");
      }
    }

    public async Task<IActionResult> Dashboard()
    {
      var userRoles = getUserRoles();
      Guid departmentId = Guid.Parse(HttpContext.Session.GetString("Department"));

      var focusAreas = await _context.FocusArea
        .Select(x => new
        {
          Id = x.intFocus,
          Name = x.FocusAreaName
        })
        .Distinct()
        .OrderBy(x => x.Name)
        .ToListAsync();

      List<ChartDataSeries> focusAreaWiseRisks = [];
      List<double> noOfRisks = [];
      List<double> noOfMitigationActions = [];
      double totalNoOfRisks = 0;
      double totalNoOfActions = 0;

      foreach (var focusArea in focusAreas)
      {
        int focusAreaRisks = _context.RiskRegister.Where(x => x.FocusArea == focusArea.Id).Count();
        int totalActions = _context.RiskRegister.Include(x => x.RiskTreatmentPlans).ThenInclude(x => x.QuarterlyRiskActions)
          .ToList()
          .Where(x => x.FocusArea == focusArea.Id && x.RiskTreatmentPlans.Any() && x.RiskTreatmentPlans.Any(y => y.QuarterlyRiskActions != null && y.QuarterlyRiskActions.Any())).Sum(x => x.RiskTreatmentPlans.Sum(y => y.QuarterlyRiskActions.Count));
        noOfRisks.Add(focusAreaRisks);
        noOfMitigationActions.Add(totalActions);
      }

      var chartFocusAreas = focusAreas;
      chartFocusAreas.Add(new
      {
        Id = DateTime.Now.Nanosecond,
        Name = "Total"
      });
      noOfRisks.Add(totalNoOfRisks);
      noOfMitigationActions.Add(totalNoOfActions);

      focusAreaWiseRisks.Add(new ChartDataSeries()
      {
        name = "No. of Risks",
        data = noOfRisks
      });
      focusAreaWiseRisks.Add(new ChartDataSeries()
      {
        name = "Mitigation Actions",
        data = noOfMitigationActions
      });


      List<double> implementedCounts = [];

      double fullyImplementedCount = _context.QuarterlyRiskActions.Where(x => x.ImpStatusId == 3).Count();
      double partiallyImplementedCount = _context.QuarterlyRiskActions.Where(x => x.ImpStatusId == 2).Count();
      double notImplementedCount = _context.QuarterlyRiskActions.Where(x => x.ImpStatusId == 1).Count();

      implementedCounts.Add(fullyImplementedCount + partiallyImplementedCount + notImplementedCount);
      implementedCounts.Add(fullyImplementedCount);
      implementedCounts.Add(partiallyImplementedCount);
      implementedCounts.Add(notImplementedCount);

      List<double> implementedCorporateCounts = [];

      double fullyImplementedCorporateCount = _context.QuarterlyRiskActions.Where(x => x.ImpStatusId == 3).Count();
      double partiallyImplementedCorporateCount = _context.QuarterlyRiskActions.Where(x => x.ImpStatusId == 2).Count();
      double notImplementedCorporateCount = _context.QuarterlyRiskActions.Where(x => x.ImpStatusId == 1).Count();

      implementedCorporateCounts.Add(fullyImplementedCorporateCount + partiallyImplementedCorporateCount + notImplementedCorporateCount);
      implementedCorporateCounts.Add(fullyImplementedCorporateCount);
      implementedCorporateCounts.Add(partiallyImplementedCorporateCount);
      implementedCorporateCounts.Add(notImplementedCorporateCount);

      var categories = _context.RiskCategorys.OrderBy(x => x.CategoryName).Select(x => new
      {
        Id = x.intCategory,
        Name = x.CategoryName
      }).ToList();

      List<ChartDataSeries> categoryWiseRisksDataSeries = [];

      List<double> CategoryRisks = [];
      List<double> CurrentYearCategoryRisks = [];

      foreach (var category in categories)
      {
        var categoryRisks = _context.RiskRegister.Include(x => x.RiskTreatmentPlans).ThenInclude(x => x.QuarterlyRiskActions).Where(x => x.intCategory == category.Id && x.IdentifiedDate.Year == DateTime.Now.Year).SelectMany(x => x.RiskTreatmentPlans.SelectMany(y => y.QuarterlyRiskActions)).ToList();
        var quarter1Risks = categoryRisks.Where(x => x.Quarter == 1).Sum(x => x.NoOfIncedents);
        var quarter2Risks = categoryRisks.Where(x => x.Quarter == 2).Sum(x => x.NoOfIncedents);
        var quarter3Risks = categoryRisks.Where(x => x.Quarter == 3).Sum(x => x.NoOfIncedents);
        var quarter4Risks = categoryRisks.Where(x => x.Quarter == 4).Sum(x => x.NoOfIncedents);

        List<double> quartersData = [];
        quartersData.Add(quarter1Risks ?? 0);
        quartersData.Add(quarter2Risks ?? 0);
        quartersData.Add(quarter3Risks ?? 0);
        quartersData.Add(quarter4Risks ?? 0);

        categoryWiseRisksDataSeries.Add(new ChartDataSeries
        {
          name = category.Name,
          data = quartersData,
        });

        double? categoryRiskCount = _context.RiskRegister.Where(x => x.intCategory == category.Id).Count();
        CategoryRisks.Add(categoryRiskCount ?? 0);
        double? currentYearCategoryRiskCount = _context.RiskRegister.Where(x => x.intCategory == category.Id && x.IdentifiedDate.Year == DateTime.Now.Year).Count();
        CurrentYearCategoryRisks.Add(currentYearCategoryRiskCount ?? 0);
      }

      var currentRisks = _context.RiskRegister.Where(x => x.IdentifiedDate.Year == DateTime.Now.Year).Count();
      var previousYearRisks = _context.RiskRegister.Where(x => x.IdentifiedDate.Year == (DateTime.Now.Year - 1)).Count();

      List<double> quarterlyIncidents = new List<double>();
      quarterlyIncidents.Add(_context.Incidents.Include(x => x.QuarterlyRiskAction).Where(x => x.QuarterlyRiskAction.Quarter == 1).Count());
      quarterlyIncidents.Add(_context.Incidents.Include(x => x.QuarterlyRiskAction).Where(x => x.QuarterlyRiskAction.Quarter == 2).Count());
      quarterlyIncidents.Add(_context.Incidents.Include(x => x.QuarterlyRiskAction).Where(x => x.QuarterlyRiskAction.Quarter == 3).Count());
      quarterlyIncidents.Add(_context.Incidents.Include(x => x.QuarterlyRiskAction).Where(x => x.QuarterlyRiskAction.Quarter == 4).Count());

      RiskDashboardViewModel data = new()
      {
        TotalRiskInRiskRegister = _context.RiskRegister.Count(),
        TotalActions = _context.RiskRegister.Include(x => x.RiskTreatmentPlans).ThenInclude(x => x.QuarterlyRiskActions).ToList().Where(x => x.RiskTreatmentPlans.Any() && x.RiskTreatmentPlans.Any(x => x.QuarterlyRiskActions != null && x.QuarterlyRiskActions.Any())).Sum(x => x.RiskTreatmentPlans.Sum(y => y.QuarterlyRiskActions.Count)),
        TotalActionsImplemented = _context.RiskRegister.Include(x => x.RiskTreatmentPlans).ThenInclude(x => x.QuarterlyRiskActions).ToList().Where(x => x.RiskTreatmentPlans.Any() && x.RiskTreatmentPlans.Any(x => x.QuarterlyRiskActions != null && x.QuarterlyRiskActions.Any() && x.QuarterlyRiskActions.Any(z => z.ImpStatusId == 3))).Sum(x => x.RiskTreatmentPlans.Sum(y => y.QuarterlyRiskActions.Where(x => x.ImpStatusId == 3).Count())),
        TotalActionsNotImplemented = _context.RiskRegister.Include(x => x.RiskTreatmentPlans).ThenInclude(x => x.QuarterlyRiskActions).ToList().Where(x => x.RiskTreatmentPlans.Any() && x.RiskTreatmentPlans.Any(x => x.QuarterlyRiskActions != null && x.QuarterlyRiskActions.Any() && x.QuarterlyRiskActions.Any(z => z.ImpStatusId != 3))).Sum(x => x.RiskTreatmentPlans.Sum(y => y.QuarterlyRiskActions.Where(x => x.ImpStatusId != 3).Count())),
        ImplementedCounts = implementedCounts,
        CorporateImplementedCounts = implementedCorporateCounts,
        CategoryWiseRiskMovementTrend = categoryWiseRisksDataSeries,
        Colors = GenerateRandomColors(categories.Count),
        Categories = categories.Select(x => x.Name).ToList(),
        CategoryRisks = CategoryRisks,
        CurrentYearCategoryRisks = CurrentYearCategoryRisks,
        TotalRisksReduced = previousYearRisks - currentRisks > 0 ? previousYearRisks - currentRisks : 0,
        TotalRisksIncreased = currentRisks - previousYearRisks > 0 ? currentRisks - previousYearRisks : 0,

        FocusAreas = chartFocusAreas.Select(x => x.Name).ToList(),
        RisksFocusAreaTrend = focusAreaWiseRisks,
        TotalIncidents = _context.Incidents.Count(),
        QuarterlyIncidents = quarterlyIncidents
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

    public IActionResult RiskTolerence(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskRegister != null)
      {
        var dat = _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
            .Where(x => x.ApprStatus == 0)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskRegister>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskRegister.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskRegister'  is null.");
      }
    }
    public async Task<IActionResult> RiskTolerenceCreate(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
          .Where(m => m.RiskRefID == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskRegisterDto riskDto = new RiskRegisterDto
      {
        RiskRefID = riskIdentification.RiskRefID,
        Activity = riskIdentification.Activity,
        EvalCriteria = riskIdentification.EvalCriteria,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        StrategicObjective = riskIdentification.StrategicObjective,
      };
      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(riskDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RiskTolerenceCreate(RiskRegisterDto objectdto)
    {
      if (objectdto.RiskRefID != null && objectdto.RiskRefID != 0)
      {
        try
        {
          var pp = await _context.RiskRegister.FindAsync(objectdto.RiskRefID);
          if (pp == null)
          {
            return NotFound();
          }
          pp.ApprStatus = (int)riskWorkFlowStatus.tolerence;
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(objectdto.RiskRefID))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(RiskTolerence));
      }

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(objectdto);
    }
    public IActionResult RiskTreatmentList(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskRegister != null)
      {
        var dat = _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
          .Include(x => x.RiskTreatmentPlans)
          .ThenInclude(x => x.QuarterlyRiskActions)
            .Where(x => x.ApprStatus == 1 || (x.ApprStatus == (int)riskWorkFlowStatus.monitoringrmoapproved && x.RiskTreatmentPlans.Where(x => x.QuarterlyRiskActions != null && x.QuarterlyRiskActions.Count != 4).Any()))
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskRegister>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskRegister.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskRegister'  is null.");
      }
    }
    public async Task<IActionResult> RiskTreatmentSubmit(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
            .Include(m => m.RiskTreatmentPlans)
            .ThenInclude(x => x.QuarterlyRiskActions)
          .Where(m => m.RiskRefID == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskTreatmentDto riskDto = new RiskTreatmentDto
      {
        RiskRefID = riskIdentification.RiskRefID,
        Activity = riskIdentification.Activity,
        EvalCriteria = riskIdentification.EvalCriteria,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired= riskIdentification.ResourcesRequired,
        //ExpectedDate= riskIdentification.ExpectedDate
        RiskTreatmentPlans = riskIdentification.RiskTreatmentPlans,
      };
      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskRatingList = new SelectList(riskLikelihoodList, "Value", "Text", riskIdentification.RiskRatingId);

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(riskIdentification);
    }

    public async Task<int> GetRiskConsequence(int RiskRefId)
    {
      RiskRegister riskRegister = await _context.RiskRegister
          .Include(x => x.RiskTreatmentPlans)
          .ThenInclude(x => x.QuarterlyRiskActions)
          .Where(x => x.RiskRefID == RiskRefId)
          .AsNoTracking().FirstAsync();

      double? totalIncidentValue = riskRegister.RiskTreatmentPlans
          .Where(x => x.QuarterlyRiskActions != null)
          .Sum(x => x.QuarterlyRiskActions.Sum(y => y.IncidentValue));

      if (totalIncidentValue != null && totalIncidentValue > 0)
      {
        double? activityBudget = riskRegister.RiskTreatmentPlans.Sum(x => x.CumulativeTarget);

        if (totalIncidentValue > 100000000) // Financial Loss > 100M
        {
          return 4; // Major
        }
        else if (totalIncidentValue > 10000000) // Financial Loss 10M-100M
        {
          return 3; // Severe
        }
        else if (totalIncidentValue > 1000000) // Financial Loss 1M-10M
        {
          return 2; // Serious
        }
        else if (totalIncidentValue < 1000000) // Financial Loss < 1M
        {
          return 1; // Minor
        }

        if (activityBudget != null && totalIncidentValue > 0.5 * activityBudget)
        {
          return 5; // Catastrophic
        }
      }

      return 0;
    }

    public async Task<int> GetRiskLikelihood(int RiskRefId)
    {
      RiskRegister riskRegister = await _context.RiskRegister
          .Include(x => x.RiskTreatmentPlans)
          .ThenInclude(x => x.QuarterlyRiskActions)
          .Where(x => x.RiskRefID == RiskRefId)
          .AsNoTracking().FirstAsync();

      double? noOfIncidents = riskRegister.RiskTreatmentPlans
          .Where(x => x.QuarterlyRiskActions != null)
          .Sum(x => x.QuarterlyRiskActions.Sum(y => y.NoOfIncedents));

      if (noOfIncidents != null && noOfIncidents > 0)
      {
        double? totalTarget = riskRegister.RiskTreatmentPlans.Sum(x => x.CumulativeTarget);

        if (totalTarget != null && totalTarget > 0)
        {
          double percentage = ((double)noOfIncidents / (double)totalTarget) * 100;

          if (percentage > 95)
          {
            return 5; // Almost certain
          }
          else if (percentage > 50 && percentage <= 95)
          {
            return 4; // Likely
          }
          else if (percentage > 15 && percentage <= 50)
          {
            return 3; // Possible
          }
          else if (percentage > 5 && percentage <= 15)
          {
            return 2; // Unlikely
          }
          else if (percentage > 0 && percentage <= 5)
          {
            return 1; // Rare
          }
        }
      }

      return 0;
    }

    public (int RiskRating, string RiskCategory, string Color) GetRiskRating(int likelihood, int consequence)
    {
      int riskRating = likelihood * consequence;

      string riskCategory = "Very Low";
      string color = "green";

      if (riskRating >= 1 && riskRating <= 4)
      {
        riskCategory = "Very Low";
        color = "green";
      }
      else if (riskRating >= 5 && riskRating <= 8)
      {
        riskCategory = "Low";
        color = "yellow";
      }
      else if (riskRating >= 9 && riskRating <= 12)
      {
        riskCategory = "Medium";
        color = "orange";
      }
      else if (riskRating >= 13 && riskRating <= 15)
      {
        riskCategory = "High";
        color = "peach";
      }
      else if (riskRating >= 16 && riskRating <= 25)
      {
        riskCategory = "Very High";
        color = "red";
      }

      return (riskRating, riskCategory, color);
    }

    public (double RiskRating, string RiskCategory, string Color) GetRiskRatingByImpact(double incidentImpact, double financialImpact, double operationalImpact)
    {
      double probability = Math.Round((incidentImpact + financialImpact + operationalImpact) / 3, 2);

      double RiskRating = 0;
      string riskCategory = "Very Low";
      string color = "green";

      if (probability > 5)
      {
        riskCategory = "Very Low";
        color = "green";
        RiskRating = 1;
      }
      else if (probability >= 5 && probability <= 15)
      {
        riskCategory = "Low";
        color = "yellow";
        RiskRating = 2;
      }
      else if (probability >= 15 && probability <= 50)
      {
        riskCategory = "Medium";
        color = "orange";
        RiskRating = 3;
      }
      else if (probability >= 50 && probability <= 95)
      {
        riskCategory = "High";
        color = "peach";
        RiskRating = 4;
      }
      else if (probability > 95)
      {
        riskCategory = "Very High";
        color = "red";
        RiskRating = 5;
      }

      return (RiskRating, riskCategory, color);
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public IActionResult RiskTreatmentSubmit(RiskRegister riskRegister)
    {
      if (ModelState.IsValid && riskRegister.RiskRefID == 0)
      {
        string category = _context.RiskCategorys.Where(x => x.intCategory == riskRegister.intCategory).Select(x => x.CategoryCode).FirstOrDefault() ?? "";
        string deptCode = _context.Departments.Where(x => x.intDept == riskRegister.intDept).Select(x => x.deptCode).FirstOrDefault() ?? "";
        riskRegister.RiskCode = $"NDA/{category}/{deptCode}";
        _context.RiskRegister.Add(riskRegister);
        _context.SaveChangesAsync();
        return RedirectToAction(nameof(RiskTolerence));
      }
      else
      {
        if (riskRegister.RiskRefID != 0)
        {
          try
          {
            //var pp = await _context.RiskRegister.FindAsync(riskRegister.RiskRefID);
            //if (pp == null)
            //{
            //  return NotFound();
            //}
            riskRegister.RiskResidualConsequenceId = GetRiskConsequence(riskRegister.RiskRefID).Result;
            riskRegister.RiskResidualLikelihoodId = GetRiskLikelihood(riskRegister.RiskRefID).Result;
            //riskRegister.RiskConsequenceId = await GetRiskConsequence(riskRegister.RiskRefID);
            //riskRegister.RiskLikelihoodId = await GetRiskLikelihood(riskRegister.RiskRefID);
            (int RiskRating, string RiskCategory, string Color) = GetRiskRating(riskRegister.RiskLikelihoodId, riskRegister.RiskConsequenceId);
            riskRegister.RiskRatingId = RiskRating;
            riskRegister.RiskRatingCategory = RiskCategory;
            riskRegister.RiskRatingColor = Color;
            riskRegister.ApprStatus = (int)riskWorkFlowStatus.treatmentsubmitted;
            //pp.AdditionalMitigation = objectdto.AdditionalMitigation;
            //pp.ResourcesRequired = objectdto.ResourcesRequired;
            //pp.ExpectedDate=   objectdto.ExpectedDate;
            _context.Update(riskRegister);
            _context.SaveChanges();


          }
          catch (DbUpdateConcurrencyException)
          {
            if (!RiskIdentificationExists(riskRegister.RiskRefID))
            {
              return NotFound();
            }
            else
            {
              throw;
            }
          }
          return RedirectToAction(nameof(RiskTreatmentList));
        }
      }
      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : _context.StrategicObjective.ToList();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : _context.FocusArea.ToList();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : _context.Activity.ToList();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(riskRegister);
    }

    [HttpPost]
    public IActionResult AddTreatmentPlan(RiskTreatmentPlan treatmentPlan)
    {
      if (ModelState.IsValid)
      {
        // Add the treatment plan to the database with the correct RiskRefID
        _context.RiskTreatmentPlans.Add(treatmentPlan);
        _context.SaveChanges();

        // Return the updated grid
        var treatmentPlans = _context.RiskTreatmentPlans
                                     .Where(tp => tp.RiskRefID == treatmentPlan.RiskRefID)
                                     .ToList();
        return RedirectToAction(nameof(RiskTreatmentSubmit), new { id = treatmentPlan.RiskRefID });
      }
      return RedirectToAction(nameof(RiskTreatmentSubmit), new { id = treatmentPlan.RiskRefID });
    }

    [HttpPost]
    public async Task<IActionResult> AddQuarterlyTreatmentAction(QuarterlyRiskAction quarterlyRiskAction)
    {
      if (ModelState.IsValid)
      {
        QuarterlyRiskAction qr = new QuarterlyRiskAction
        {
          Quarter = quarterlyRiskAction.Quarter,
          TreatmentPlanId = quarterlyRiskAction.TreatmentPlanId,
          RiskDescription = quarterlyRiskAction.RiskDescription,
          RiskTreatmentPlan = quarterlyRiskAction.RiskTreatmentPlan,
          ImplementationStatus = quarterlyRiskAction.ImplementationStatus,
          NoOfIncedents = quarterlyRiskAction.Incidents.Sum(x => x.NoOfIncedents),
          ImpStatusId = quarterlyRiskAction.ImpStatusId,
          IncidentValue = quarterlyRiskAction.IncidentValue,
          //Incidents = quarterlyRiskAction.Incidents,
        };
        //_context.Update(qr);
        _context.QuarterlyRiskActions.Add(qr);
        _context.SaveChanges();

        foreach (var incident in quarterlyRiskAction.Incidents)
        {
          incident.QuarterlyRiskActionId = qr.Id;
          _context.Incidents.Add(incident);
        }
        _context.SaveChanges();

        if (quarterlyRiskAction.Quarter == 1)
        {
          int riskId = (int)_context.RiskTreatmentPlans.Where(x => x.RiskRefID != null && x.TreatmentPlanId == quarterlyRiskAction.TreatmentPlanId).Select(x => x.RiskRefID).First();
          Data.Risk.RiskRegister riskRegister = _context.RiskRegister.First(x => x.RiskRefID == riskId);
          riskRegister.RiskConsequenceId = await GetRiskConsequence(riskRegister.RiskRefID);
          riskRegister.RiskLikelihoodId = await GetRiskLikelihood(riskRegister.RiskRefID);
          _context.RiskRegister.Update(riskRegister);
          _context.SaveChanges();
          //riskRegister.RiskConsequenceId = await GetRiskConsequence(riskRegister.RiskRefID);
          //riskRegister.RiskLikelihoodId = await GetRiskLikelihood(riskRegister.RiskRefID);
          (int RiskRating, string RiskCategory, string Color) = GetRiskRating(riskRegister.RiskLikelihoodId, riskRegister.RiskConsequenceId);
        }

        // Return the updated grid
        var treatmentPlans = _context.RiskTreatmentPlans
                                     .Where(tp => tp.TreatmentPlanId == quarterlyRiskAction.TreatmentPlanId)
                                     .FirstOrDefault();
        return RedirectToAction(nameof(RiskTreatmentPlanEdit), new { id = quarterlyRiskAction.TreatmentPlanId });
      }
      return BadRequest();
    }

    public async Task<IActionResult> RiskTreatmentPlanEdit(int? id)
    {
      if (id == null || _context.RiskTreatmentPlans == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskTreatmentPlans.Include(m => m.QuarterlyRiskActions)
            .ThenInclude(m => m.ImplementationStatus)

          .Where(m => m.TreatmentPlanId == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(riskIdentification);
    }

    public IActionResult RemoveTreatmentPlan(int id)
    {
      var treatmentPlan = _context.RiskTreatmentPlans.Find(id);
      if (treatmentPlan != null)
      {
        var riskRefID = treatmentPlan.RiskRefID;
        _context.RiskTreatmentPlans.Remove(treatmentPlan);
        _context.SaveChanges();

        // Return the updated grid
        var treatmentPlans = _context.RiskTreatmentPlans
                                     .Where(tp => tp.RiskRefID == riskRefID)
                                     .ToList();
        return RedirectToAction(nameof(RiskTreatmentSubmit), new { id = riskRefID });
        //return PartialView("_TreatmentPlanGrid", treatmentPlans);
      }
      return BadRequest();
    }

    public IActionResult RiskTreatmentHodReviewList(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskRegister != null)
      {
        var dat = _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
            .Where(x => x.ApprStatus == (int)riskWorkFlowStatus.treatmentsubmitted)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskRegister>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskRegister.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskRegister'  is null.");
      }
    }
    public async Task<IActionResult> RiskTreatmentHodReview(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
        .Include(x => x.RiskTreatmentPlans)
        .Where(m => m.RiskRefID == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskTreatmentDto riskDto = new RiskTreatmentDto
      {
        RiskRefID = riskIdentification.RiskRefID,
        Activity = riskIdentification.Activity,
        EvalCriteria = riskIdentification.EvalCriteria,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        StrategicObjective = riskIdentification.StrategicObjective,
        ActivityBudget = riskIdentification.ActivityBudget,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        riskTolerenceJustification = riskIdentification.riskTolerenceJustification,
        RiskTreatmentPlans = riskIdentification.RiskTreatmentPlans,

      };
      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RiskTreatmentHodReview(RiskTreatmentDto objectdto)
    {
      if (objectdto.RiskRefID != null && objectdto.RiskRefID != 0)
      {
        try
        {
          var pp = await _context.RiskRegister.FindAsync(objectdto.RiskRefID);
          if (pp == null)
          {
            return NotFound();
          }
          if (objectdto.ApprStatus == 1)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.treatmenthodreviewed;
          }
          else if (objectdto.ApprStatus == 2)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.treatmenthodrejected;
          }
          //pp.AdditionalMitigation = objectdto.AdditionalMitigation;
          //pp.ResourcesRequired = objectdto.ResourcesRequired;
          //pp.ExpectedDate = objectdto.ExpectedDate;
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(objectdto.RiskRefID))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(RiskTreatmentHodReviewList));
      }
      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(objectdto);
    }
    public IActionResult RiskTreatmentDirVerifyList(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskRegister != null)
      {
        var dat = _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
            .Where(x => x.ApprStatus == (int)riskWorkFlowStatus.treatmenthodreviewed)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskRegister>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskRegister.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskRegister'  is null.");
      }
    }
    public async Task<IActionResult> RiskTreatmentDirVerify(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
          .Include(x => x.RiskTreatmentPlans)
          .Where(m => m.RiskRefID == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskTreatmentDto riskDto = new RiskTreatmentDto
      {
        RiskRefID = riskIdentification.RiskRefID,
        Activity = riskIdentification.Activity,
        EvalCriteria = riskIdentification.EvalCriteria,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        ActivityBudget = riskIdentification.ActivityBudget,
        riskTolerenceJustification = riskIdentification.riskTolerenceJustification,
        RiskTreatmentPlans = riskIdentification.RiskTreatmentPlans
      };
      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RiskTreatmentDirVerify(RiskTreatmentDto objectdto)
    {
      if (objectdto.RiskRefID != null && objectdto.RiskRefID != 0)
      {
        try
        {
          var pp = await _context.RiskRegister.FindAsync(objectdto.RiskRefID);
          if (pp == null)
          {
            return NotFound();
          }
          if (objectdto.ApprStatus == 1)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.treatmentdirapprove;
          }
          else if (objectdto.ApprStatus == 2)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.treatmentdirrejected;
          }
          //pp.AdditionalMitigation = objectdto.AdditionalMitigation;
          //pp.ResourcesRequired = objectdto.ResourcesRequired;
          //pp.ExpectedDate = objectdto.ExpectedDate;
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(objectdto.RiskRefID))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(RiskTreatmentDirVerifyList));
      }
      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(objectdto);
    }
    public IActionResult RiskTreatmentRmoVerifyList(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskRegister != null)
      {
        var dat = _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
            .Where(x => x.ApprStatus == (int)riskWorkFlowStatus.treatmentdirapprove)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskRegister>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskRegister.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskRegister'  is null.");
      }
    }
    public async Task<IActionResult> RiskTreatmentRmoVerify(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
          .Include(x => x.RiskTreatmentPlans)
          .Where(m => m.RiskRefID == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskTreatmentDto riskDto = new RiskTreatmentDto
      {
        RiskRefID = riskIdentification.RiskRefID,
        Activity = riskIdentification.Activity,
        EvalCriteria = riskIdentification.EvalCriteria,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        StrategicObjective = riskIdentification.StrategicObjective,
        RiskTreatmentPlans = riskIdentification.RiskTreatmentPlans,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        ActivityBudget = riskIdentification.ActivityBudget,
        riskTolerenceJustification = riskIdentification.riskTolerenceJustification,
      };

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RiskTreatmentRmoVerify(RiskTreatmentDto objectdto)
    {
      if (objectdto.RiskRefID != null && objectdto.RiskRefID != 0)
      {
        try
        {
          var pp = await _context.RiskRegister.FindAsync(objectdto.RiskRefID);
          if (pp == null)
          {
            return NotFound();
          }
          if (objectdto.ApprStatus == 1)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.treatmentrmoapproved;
          }
          else if (objectdto.ApprStatus == 2)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.treatmentrmorejected;
          }
          //pp.AdditionalMitigation = objectdto.AdditionalMitigation;
          //pp.ResourcesRequired = objectdto.ResourcesRequired;
          //pp.ExpectedDate = objectdto.ExpectedDate;
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(objectdto.RiskRefID))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(RiskTreatmentRmoVerifyList));
      }

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(objectdto);
    }
    public IActionResult RiskMonitoringList(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskRegister != null)
      {
        var dat = _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
            .Where(x => x.ApprStatus == (int)riskWorkFlowStatus.treatmentrmoapproved)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskRegister>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskRegister.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskRegister'  is null.");
      }
    }
    public async Task<IActionResult> RiskMonitoringSubmit(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
        .Include(m => m.RiskTreatmentPlans)
          .Where(m => m.RiskRefID == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskMonitoringDto riskDto = new RiskMonitoringDto
      {
        RiskRefID = riskIdentification.RiskRefID,
        Activity = riskIdentification.Activity,
        EvalCriteria = riskIdentification.EvalCriteria,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        ActionTaken = riskIdentification.ActionTaken,
        ActualDate = riskIdentification.ActualDate,
        RiskTreatmentPlans = riskIdentification.RiskTreatmentPlans,
      };
      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskRatingList = new SelectList(riskLikelihoodList, "Value", "Text", riskIdentification.RiskRatingId);

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(riskIdentification);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RiskMonitoringSubmit(RiskRegister pp)
    {
      if (pp.RiskRefID != 0)
      {
        try
        {
          //var pp = await _context.RiskRegister.FindAsync(objectdto.RiskRefID);
          //if (pp == null)
          //{
          //  return NotFound();
          //}
          pp.ApprStatus = (int)riskWorkFlowStatus.monitoringsubmitted;
          //pp.ActionTaken = objectdto.ActionTaken;
          //pp.ActualDate = objectdto.ActualDate;
          pp.ActualBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
          _context.RiskRegister.Update(pp);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(pp.RiskRefID))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(RiskMonitoringList));
      }

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(pp);
    }

    public async Task<IActionResult> RemoveTreatmentPlanEdit(int TreatmentPlanId)
    {
      try
      {
        RiskTreatmentPlan? riskTreatmentPlan = await _context.RiskTreatmentPlans
          .Where(x => x.TreatmentPlanId == TreatmentPlanId)
          .Include(x => x.QuarterlyRiskActions)
          .ThenInclude(q => q.ImplementationStatus)
          .FirstOrDefaultAsync();
        if (riskTreatmentPlan == null)
        {
          return NotFound();
        }

        return Ok(riskTreatmentPlan);
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex);
      }
    }

    public IActionResult RiskMonitoringHodReviewList(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskRegister != null)
      {
        var dat = _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
            .Where(x => x.ApprStatus == (int)riskWorkFlowStatus.monitoringsubmitted)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskRegister>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskRegister.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskRegister'  is null.");
      }
    }
    public async Task<IActionResult> RiskMonitoringHodReview(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
        .Include(x => x.RiskTreatmentPlans)
        .ThenInclude(x => x.QuarterlyRiskActions)
          .Where(m => m.RiskRefID == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskMonitoringDto riskDto = new RiskMonitoringDto
      {
        RiskRefID = riskIdentification.RiskRefID,
        Activity = riskIdentification.Activity,
        EvalCriteria = riskIdentification.EvalCriteria,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        ActionTaken = riskIdentification.ActionTaken,
        ActualDate = riskIdentification.ActualDate,
        RiskTreatmentPlans = riskIdentification.RiskTreatmentPlans,
      };
      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskIdentification);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RiskMonitoringHodReview(RiskRegister pp)
    {
      if (pp.RiskRefID != 0)
      {
        try
        {

          if (pp.ApprStatus == 1)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.monitoringhodreviewed;
          }
          else if (pp.ApprStatus == 2)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.monitoringhodrejected;
          }
          _context.RiskRegister.Update(pp);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(pp.RiskRefID))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(RiskMonitoringHodReviewList));
      }
      var riskLikelihoodList = new List<SelectListItem>
        {
            new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
        new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
        new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
        new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
        new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
        };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(pp);
    }
    public IActionResult RiskMonitoringDirVerifyList(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskRegister != null)
      {
        var dat = _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
            .Where(x => x.ApprStatus == (int)riskWorkFlowStatus.monitoringhodreviewed)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskRegister>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskRegister.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskRegister'  is null.");
      }
    }
    public async Task<IActionResult> RiskMonitoringDirVerify(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk).Include(x => x.RiskTreatmentPlans)
        .ThenInclude(x => x.QuarterlyRiskActions)
          .Where(m => m.RiskRefID == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskMonitoringDto riskDto = new RiskMonitoringDto
      {
        RiskRefID = riskIdentification.RiskRefID,
        Activity = riskIdentification.Activity,
        EvalCriteria = riskIdentification.EvalCriteria,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        ActionTaken = riskIdentification.ActionTaken,
        ActualDate = riskIdentification.ActualDate,
        RiskTreatmentPlans = riskIdentification.RiskTreatmentPlans,
      };
      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskIdentification);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RiskMonitoringDirVerify(RiskRegister pp)
    {
      if (pp.RiskRefID != 0)
      {
        try
        {
          if (pp.ApprStatus == 1)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.monitoringdirapprove;
          }
          else if (pp.ApprStatus == 2)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.monitoringdirrejected;
          }
          _context.RiskRegister.Update(pp);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(pp.RiskRefID))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(RiskMonitoringDirVerifyList));
      }
      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(pp);
    }
    public IActionResult RiskMonitoringRmoVerifyList(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskRegister != null)
      {
        var dat = _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
            .Where(x => x.ApprStatus == (int)riskWorkFlowStatus.monitoringdirapprove)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskRegister>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskRegister.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskRegister'  is null.");
      }
    }
    public async Task<IActionResult> RiskMonitoringRmoVerify(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
        .Include(x => x.RiskTreatmentPlans)
        .ThenInclude(x => x.QuarterlyRiskActions)
          .Where(m => m.RiskRefID == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskMonitoringDto riskDto = new RiskMonitoringDto
      {
        RiskRefID = riskIdentification.RiskRefID,
        Activity = riskIdentification.Activity,
        EvalCriteria = riskIdentification.EvalCriteria,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        ActionTaken = riskIdentification.ActionTaken,
        ActualDate = riskIdentification.ActualDate,
        RiskTreatmentPlans = riskIdentification.RiskTreatmentPlans,
      };

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskIdentification);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RiskMonitoringRmoVerify(RiskRegister pp)
    {
      if (pp.RiskRefID != 0)
      {
        try
        {
          if (pp.ApprStatus == 1)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.monitoringrmoapproved;
          }
          else if (pp.ApprStatus == 2)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.monitoringrmorejected;
          }
          _context.RiskRegister.Update(pp);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(pp.RiskRefID))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(RiskMonitoringRmoVerifyList));
      }

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(pp);
    }

    public IActionResult RiskResidualList(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskRegister != null)
      {
        var dat = _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
            .Where(x => x.ApprStatus == (int)riskWorkFlowStatus.monitoringrmoapproved)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskRegister>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskRegister.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskRegister'  is null.");
      }
    }

    public async Task<IActionResult> RiskResidualSubmit(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
         .Include(x => x.RiskTreatmentPlans)
         .ThenInclude(x => x.QuarterlyRiskActions)
         .ThenInclude(x => x.Incidents)
          .Where(m => m.RiskRefID == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }

      double incidents = riskIdentification.RiskTreatmentPlans.Sum(x => x.QuarterlyRiskActions.Sum(y => y.Incidents.Count()));
      double sampleSize = riskIdentification.RiskTreatmentPlans.Sum(x => x.SampleSize);

      double? incidentValue = riskIdentification.RiskTreatmentPlans.Sum(x => x.QuarterlyRiskActions.Sum(y => y.IncidentValue));
      double financialImpact = 0;
      if (incidentValue != null && riskIdentification?.ActivityBudget > 0)
      {
        financialImpact = ((double)incidentValue / riskIdentification.ActivityBudget) * 100;
      }
      RiskResidualDto riskDto = new RiskResidualDto
      {
        RiskRefID = riskIdentification.RiskRefID,
        Activity = riskIdentification.Activity,
        EvalCriteria = riskIdentification.EvalCriteria,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        RiskResidualConsequenceId = riskIdentification.RiskResidualConsequenceId,
        RiskResidualLikelihoodId = riskIdentification.RiskResidualLikelihoodId,
        RiskResidualScore = riskIdentification.RiskResidualScore,
        RiskResidualRank = riskIdentification.RiskResidualRank,
        RiskTreatmentPlans = riskIdentification.RiskTreatmentPlans,
        IncidentImpact = incidents > 0 && sampleSize > 0 ? Math.Round((incidents / sampleSize) * 100, 2) : 0,
        //FinancialImpact = riskIdentification.RiskTreatmentPlans.Sum(x => x.QuarterlyRiskActions.Sum(y => y.IncidentValue)) / (double)riskIdentification.ActivityBudget * 100
        FinancialImpact = Math.Round(financialImpact, 2),
        ActivityBudget = riskIdentification.ActivityBudget,
      };

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(riskDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RiskResidualSubmit(RiskResidualDto objectdto)
    {
      if (objectdto.RiskRefID != null && objectdto.RiskRefID != 0)
      {
        try
        {
          var pp = await _context.RiskRegister.FindAsync(objectdto.RiskRefID);
          if (pp == null)
          {
            return NotFound();
          }
          pp.ApprStatus = (int)riskWorkFlowStatus.resdassesssubmitted;
          pp.RiskResidualConsequenceId = objectdto.RiskResidualConsequenceId;
          pp.RiskResidualLikelihoodId = objectdto.RiskResidualLikelihoodId;
          pp.RiskResidualScore = objectdto.RiskResidualScore;
          pp.RiskResidualRank = objectdto.RiskResidualRank;
          pp.IncidentImpact = objectdto.IncidentImpact;
          pp.FinancialImpact = objectdto.FinancialImpact;
          pp.OperationGovernanceImpact = objectdto.OperationGovernanceImpact;
          (double riskRating, string category, string color) = GetRiskRatingByImpact(objectdto.IncidentImpact ?? 0, objectdto.FinancialImpact ?? 0, objectdto.OperationGovernanceImpact ?? 0);
          pp.RiskRatingId = (int)riskRating;
          pp.RiskRatingCategory = category;
          pp.RiskRatingColor = color;
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(objectdto.RiskRefID))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(RiskResidualList));
      }

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(objectdto);
    }

    public IActionResult RiskResidualHodReviewList(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskRegister != null)
      {
        var dat = _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
            .Where(x => x.ApprStatus == (int)riskWorkFlowStatus.resdassesssubmitted)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskRegister>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskRegister.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskRegister'  is null.");
      }
    }
    public async Task<IActionResult> RiskResidualHodReview(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
          .Include(x => x.RiskTreatmentPlans)
             .ThenInclude(x => x.QuarterlyRiskActions)
             .ThenInclude(x => x.Incidents)
          .Where(m => m.RiskRefID == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      (double riskRating, string category, string color) = GetRiskRatingByImpact(riskIdentification.IncidentImpact ?? 0, riskIdentification.FinancialImpact ?? 0, riskIdentification.OperationGovernanceImpact ?? 0);
      RiskResidualDto riskDto = new RiskResidualDto
      {
        RiskRefID = riskIdentification.RiskRefID,
        Activity = riskIdentification.Activity,

        EvalCriteria = riskIdentification.EvalCriteria,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        RiskResidualConsequenceId = riskIdentification.RiskResidualConsequenceId,
        RiskResidualLikelihoodId = riskIdentification.RiskResidualLikelihoodId,
        RiskResidualScore = riskIdentification.RiskResidualScore,
        RiskResidualRank = riskIdentification.RiskResidualRank,
        RiskTreatmentPlans = riskIdentification.RiskTreatmentPlans,
        ActivityBudget = riskIdentification.ActivityBudget,
        IncidentImpact = riskIdentification.IncidentImpact,
        FinancialImpact = riskIdentification.FinancialImpact,
        OperationGovernanceImpact = riskIdentification.OperationGovernanceImpact,
        RiskRatingId = (int)riskRating,
        RiskRatingCategory = category,
        RiskRatingColor = color,
      };

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RiskResidualHodReview(RiskResidualDto objectdto)
    {
      if (objectdto.RiskRefID != null && objectdto.RiskRefID != 0)
      {
        try
        {
          var pp = await _context.RiskRegister.FindAsync(objectdto.RiskRefID);
          if (pp == null)
          {
            return NotFound();
          }
          if (objectdto.ApprStatus == 1)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.resdassesshodreviewed;
          }
          else if (objectdto.ApprStatus == 2)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.resdassesshodrejected;
          }
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(objectdto.RiskRefID))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(RiskResidualHodReviewList));
      }

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(objectdto);
    }
    public IActionResult RiskResidualDirVerifyList(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskRegister != null)
      {
        var dat = _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
            .Where(x => x.ApprStatus == (int)riskWorkFlowStatus.resdassesshodreviewed)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskRegister>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskRegister.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskRegister'  is null.");
      }
    }
    public async Task<IActionResult> RiskResidualDirVerify(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
        .Include(x => x.RiskTreatmentPlans)
             .ThenInclude(x => x.QuarterlyRiskActions)
             .ThenInclude(x => x.Incidents)
          .Where(m => m.RiskRefID == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskResidualDto riskDto = new RiskResidualDto
      {
        RiskRefID = riskIdentification.RiskRefID,
        Activity = riskIdentification.Activity,

        EvalCriteria = riskIdentification.EvalCriteria,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        RiskResidualConsequenceId = riskIdentification.RiskResidualConsequenceId,
        RiskResidualLikelihoodId = riskIdentification.RiskResidualLikelihoodId,
        RiskResidualScore = riskIdentification.RiskResidualScore,
        RiskResidualRank = riskIdentification.RiskResidualRank,
        RiskTreatmentPlans = riskIdentification.RiskTreatmentPlans,
        ActivityBudget = riskIdentification.ActivityBudget,
        IncidentImpact = riskIdentification.IncidentImpact,
        FinancialImpact = riskIdentification.FinancialImpact,
        OperationGovernanceImpact = riskIdentification.OperationGovernanceImpact
      };

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RiskResidualDirVerify(RiskResidualDto objectdto)
    {
      if (objectdto.RiskRefID != null && objectdto.RiskRefID != 0)
      {
        try
        {
          var pp = await _context.RiskRegister.FindAsync(objectdto.RiskRefID);
          if (pp == null)
          {
            return NotFound();
          }
          if (objectdto.ApprStatus == 1)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.resdassessdirapprove;
          }
          else if (objectdto.ApprStatus == 2)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.resdassessdirrejected;
          }
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(objectdto.RiskRefID))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(RiskResidualDirVerifyList));
      }
      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(objectdto);
    }
    public IActionResult RiskResidualRmoVerifyList(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;
      if (_context.RiskRegister != null)
      {
        var dat = _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
            .Where(x => x.ApprStatus == (int)riskWorkFlowStatus.resdassessdirapprove)
            .Skip(offset)
            .Take(pageSize);

        var result = new PagedResult<RiskRegister>
        {
          Data = dat.AsNoTracking().ToList(),
          TotalItems = _context.RiskRegister.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize

        };
        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.RiskRegister'  is null.");
      }
    }
    public async Task<IActionResult> RiskResidualRmoVerify(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
        .Include(x => x.RiskTreatmentPlans)
             .ThenInclude(x => x.QuarterlyRiskActions)
             .ThenInclude(x => x.Incidents)
          .Where(m => m.RiskRefID == id).FirstOrDefaultAsync();
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskResidualDto riskDto = new RiskResidualDto
      {
        RiskRefID = riskIdentification.RiskRefID,
        Activity = riskIdentification.Activity,

        EvalCriteria = riskIdentification.EvalCriteria,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        RiskResidualConsequenceId = riskIdentification.RiskResidualConsequenceId,
        RiskResidualLikelihoodId = riskIdentification.RiskResidualLikelihoodId,
        RiskResidualScore = riskIdentification.RiskResidualScore,
        RiskResidualRank = riskIdentification.RiskResidualRank,
        RiskTreatmentPlans = riskIdentification.RiskTreatmentPlans,
        ActivityBudget = riskIdentification.ActivityBudget,
        IncidentImpact = riskIdentification.IncidentImpact,
        FinancialImpact = riskIdentification.FinancialImpact,
        OperationGovernanceImpact = riskIdentification.OperationGovernanceImpact
      };

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      return View(riskDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RiskResidualRmoVerify(RiskResidualDto objectdto)
    {
      if (objectdto.RiskRefID != null && objectdto.RiskRefID != 0)
      {
        try
        {
          var pp = await _context.RiskRegister.FindAsync(objectdto.RiskRefID);
          if (pp == null)
          {
            return NotFound();
          }
          if (objectdto.ApprStatus == 1)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.resdassessrmoapproved;
          }
          else if (objectdto.ApprStatus == 2)
          {
            pp.ApprStatus = (int)riskWorkFlowStatus.resdassessrmorejected;
          }
          pp.ControlEffectiveness = objectdto.ControlEffectiveness;
          pp.Effectiveness = objectdto.Effectiveness;
          pp.Recommendation = objectdto.Recommendation;
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(objectdto.RiskRefID))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(RiskResidualRmoVerifyList));
      }

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      ViewData["Approval"] = ListHelper.ApprovalStatus();
      ViewData["RiskRank"] = ListHelper.RiskRank();
      return View(objectdto);
    }
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk)
          .FirstOrDefaultAsync(m => m.RiskRefID == id);
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
      return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RiskRegisterDto dto)
    {
      if (ModelState.IsValid && dto.RiskConsequenceId != 0 && dto.RiskLikelihoodId != 0)
      {
        RiskRegister riskIdentification = new RiskRegister
        {
          Activity = dto.Activity,
          EvalCriteria = dto.EvalCriteria,
          FocusArea = dto.FocusArea,
          IdentifiedDate = dto.IdentifiedDate,
          RiskConsequenceId = dto.RiskConsequenceId,
          RiskDescription = dto.RiskDescription,
          RiskLikelihoodId = dto.RiskLikelihoodId,
          RiskOwner = User.FindFirstValue(ClaimTypes.NameIdentifier),
          RiskRank = dto.RiskRank,
          RiskScore = dto.RiskScore,
          StrategicObjective = dto.StrategicObjective,
        };
        _context.Add(riskIdentification);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      return View(dto);
    }


    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.FindAsync(id);
      if (riskIdentification == null)
      {
        return NotFound();
      }
      RiskRegisterDto dto = new RiskRegisterDto
      {
        Activity = riskIdentification.Activity,

        EvalCriteria = riskIdentification.EvalCriteria,
        FocusArea = (int)riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskRefID = riskIdentification.RiskRefID,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        StrategicObjective = (int)riskIdentification.StrategicObjective,

      };

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
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
    public async Task<IActionResult> Edit(int id, [Bind("RiskRefID,IdentifiedDate,StrategicObjective,FocusArea,Activity,BudgetCode,RiskDescription,Events,RiskSource,RiskCause,RiskConsequence,RiskConsequenceId,RiskLikelihoodId,RiskScore,RiskRank,EvalCriteria,IsVerified")] RiskRegisterDto riskIdentification)
    {
      if (id != riskIdentification.RiskRefID)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          RiskRegister rd = new RiskRegister
          {
            Activity = riskIdentification.Activity,

            EvalCriteria = riskIdentification.EvalCriteria,
            FocusArea = riskIdentification.FocusArea,
            IdentifiedDate = riskIdentification.IdentifiedDate,
            RiskConsequenceId = riskIdentification.RiskConsequenceId,
            RiskDescription = riskIdentification.RiskDescription,
            RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
            RiskOwner = User.FindFirstValue(ClaimTypes.NameIdentifier),
            RiskRank = riskIdentification.RiskRank,
            RiskScore = riskIdentification.RiskScore,
            StrategicObjective = riskIdentification.StrategicObjective,
            RiskRefID = riskIdentification.RiskRefID
          };
          _context.Update(rd);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RiskIdentificationExists(riskIdentification.RiskRefID))
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

      var riskLikelihoodList = new List<SelectListItem>
    {
        new CustomSelectListItem { Value = "1", Text = "Very Low", Color = "green" },
    new CustomSelectListItem { Value = "2", Text = "Low", Color = "yellow" },
    new CustomSelectListItem { Value = "3", Text = "Medium", Color = "orange" },
    new CustomSelectListItem { Value = "4", Text = "High", Color = "peach" },
    new CustomSelectListItem { Value = "5", Text = "Very High", Color = "red" }
    };

      ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
      ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
      return View(riskIdentification);
    }


    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.RiskRegister == null)
      {
        return NotFound();
      }

      var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk)
          .FirstOrDefaultAsync(m => m.RiskRefID == id);
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
      if (_context.RiskRegister == null)
      {
        return Problem("Entity set 'AppDbContext.RiskRegister'  is null.");
      }
      var riskIdentification = await _context.RiskRegister.FindAsync(id);
      if (riskIdentification != null)
      {
        _context.RiskRegister.Remove(riskIdentification);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool RiskIdentificationExists(int id)
    {
      return (_context.RiskRegister?.Any(e => e.RiskRefID == id)).GetValueOrDefault();
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

    public async Task<IActionResult> VerifyRisk(int id, bool IsVerified)
    {
      var res = await _context.RiskRegister.FirstOrDefaultAsync(m => m.RiskRefID == id);
      if (res != null)
      {

      }
      await _context.SaveChangesAsync();

      return RedirectToAction("Verify");
    }
  }

  public class CustomSelectListItem : SelectListItem
  {
    public string Color { get; set; }
  }
}
