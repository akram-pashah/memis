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
        Events = riskIdentification.Events,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskCause = riskIdentification.RiskCause,
        RiskConsequence = riskIdentification.RiskConsequence,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        RiskSource = riskIdentification.RiskSource,
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
        Events = riskIdentification.Events,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskCause = riskIdentification.RiskCause,
        RiskConsequence = riskIdentification.RiskConsequence,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        RiskSource = riskIdentification.RiskSource,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired= riskIdentification.ResourcesRequired,
        //ExpectedDate= riskIdentification.ExpectedDate,
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RiskTreatmentSubmit(RiskRegister riskRegister)
    {
      if (ModelState.IsValid && riskRegister.RiskRefID == 0)
      {
        _context.RiskRegister.Add(riskRegister);
        await _context.SaveChangesAsync();
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
            riskRegister.RiskConsequenceId = await GetRiskConsequence(riskRegister.RiskRefID);
            riskRegister.RiskLikelihoodId = await GetRiskLikelihood(riskRegister.RiskRefID);
            (int RiskRating, string RiskCategory, string Color) = GetRiskRating(riskRegister.RiskLikelihoodId, riskRegister.RiskConsequenceId);
            riskRegister.RiskRatingId = RiskRating;
            riskRegister.RiskRatingCategory = RiskCategory;
            riskRegister.RiskRatingColor = Color;
            riskRegister.ApprStatus = (int)riskWorkFlowStatus.treatmentsubmitted;
            //pp.AdditionalMitigation = objectdto.AdditionalMitigation;
            //pp.ResourcesRequired = objectdto.ResourcesRequired;
            //pp.ExpectedDate=   objectdto.ExpectedDate;
            _context.Update(riskRegister);
            await _context.SaveChangesAsync();
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
      ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
      ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
      ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
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
      return BadRequest();
    }

    [HttpPost]
    public IActionResult AddQuarterlyTreatmentAction(QuarterlyRiskAction quarterlyRiskAction)
    {
      if (ModelState.IsValid)
      {
        // Add the treatment plan to the database with the correct RiskRefID
        _context.QuarterlyRiskActions.Add(quarterlyRiskAction);
        _context.SaveChanges();

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
        Events = riskIdentification.Events,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskCause = riskIdentification.RiskCause,
        RiskConsequence = riskIdentification.RiskConsequence,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        RiskSource = riskIdentification.RiskSource,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
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
        Events = riskIdentification.Events,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskCause = riskIdentification.RiskCause,
        RiskConsequence = riskIdentification.RiskConsequence,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        RiskSource = riskIdentification.RiskSource,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
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
        Events = riskIdentification.Events,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskCause = riskIdentification.RiskCause,
        RiskConsequence = riskIdentification.RiskConsequence,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        RiskSource = riskIdentification.RiskSource,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
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
        Events = riskIdentification.Events,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskCause = riskIdentification.RiskCause,
        RiskConsequence = riskIdentification.RiskConsequence,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        RiskSource = riskIdentification.RiskSource,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        ActionTaken = riskIdentification.ActionTaken,
        ActualDate = riskIdentification.ActualDate,
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
        Events = riskIdentification.Events,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskCause = riskIdentification.RiskCause,
        RiskConsequence = riskIdentification.RiskConsequence,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        RiskSource = riskIdentification.RiskSource,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        ActionTaken = riskIdentification.ActionTaken,
        ActualDate = riskIdentification.ActualDate,
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
        Events = riskIdentification.Events,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskCause = riskIdentification.RiskCause,
        RiskConsequence = riskIdentification.RiskConsequence,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        RiskSource = riskIdentification.RiskSource,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        ActionTaken = riskIdentification.ActionTaken,
        ActualDate = riskIdentification.ActualDate,
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
        Events = riskIdentification.Events,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskCause = riskIdentification.RiskCause,
        RiskConsequence = riskIdentification.RiskConsequence,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        RiskSource = riskIdentification.RiskSource,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        ActionTaken = riskIdentification.ActionTaken,
        ActualDate = riskIdentification.ActualDate,
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
        Events = riskIdentification.Events,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskCause = riskIdentification.RiskCause,
        RiskConsequence = riskIdentification.RiskConsequence,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        RiskSource = riskIdentification.RiskSource,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        RiskResidualConsequenceId = riskIdentification.RiskResidualConsequenceId,
        RiskResidualLikelihoodId = riskIdentification.RiskResidualLikelihoodId,
        RiskResidualScore = riskIdentification.RiskResidualScore,
        RiskResidualRank = riskIdentification.RiskResidualRank,
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
        Events = riskIdentification.Events,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskCause = riskIdentification.RiskCause,
        RiskConsequence = riskIdentification.RiskConsequence,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        RiskSource = riskIdentification.RiskSource,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        RiskResidualConsequenceId = riskIdentification.RiskResidualConsequenceId,
        RiskResidualLikelihoodId = riskIdentification.RiskResidualLikelihoodId,
        RiskResidualScore = riskIdentification.RiskResidualScore,
        RiskResidualRank = riskIdentification.RiskResidualRank,
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
        Events = riskIdentification.Events,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskCause = riskIdentification.RiskCause,
        RiskConsequence = riskIdentification.RiskConsequence,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        RiskSource = riskIdentification.RiskSource,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        RiskResidualConsequenceId = riskIdentification.RiskResidualConsequenceId,
        RiskResidualLikelihoodId = riskIdentification.RiskResidualLikelihoodId,
        RiskResidualScore = riskIdentification.RiskResidualScore,
        RiskResidualRank = riskIdentification.RiskResidualRank,
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
        Events = riskIdentification.Events,
        FocusArea = riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskCause = riskIdentification.RiskCause,
        RiskConsequence = riskIdentification.RiskConsequence,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        RiskSource = riskIdentification.RiskSource,
        StrategicObjective = riskIdentification.StrategicObjective,
        //AdditionalMitigation = riskIdentification.AdditionalMitigation,
        //ResourcesRequired = riskIdentification.ResourcesRequired,
        //ExpectedDate = riskIdentification.ExpectedDate,
        RiskResidualConsequenceId = riskIdentification.RiskResidualConsequenceId,
        RiskResidualLikelihoodId = riskIdentification.RiskResidualLikelihoodId,
        RiskResidualScore = riskIdentification.RiskResidualScore,
        RiskResidualRank = riskIdentification.RiskResidualRank,
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
          Events = dto.Events,
          FocusArea = dto.FocusArea,
          IdentifiedDate = dto.IdentifiedDate,
          RiskCause = dto.RiskCause,
          RiskConsequence = dto.RiskConsequence,
          RiskConsequenceId = dto.RiskConsequenceId,
          RiskDescription = dto.RiskDescription,
          RiskLikelihoodId = dto.RiskLikelihoodId,
          RiskOwner = User.FindFirstValue(ClaimTypes.NameIdentifier),
          RiskRank = dto.RiskRank,
          RiskScore = dto.RiskScore,
          RiskSource = dto.RiskSource,
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
        Events = riskIdentification.Events,
        FocusArea = (int)riskIdentification.FocusArea,
        IdentifiedDate = riskIdentification.IdentifiedDate,
        RiskCause = riskIdentification.RiskCause,
        RiskConsequence = riskIdentification.RiskConsequence,
        RiskConsequenceId = riskIdentification.RiskConsequenceId,
        RiskDescription = riskIdentification.RiskDescription,
        RiskRefID = riskIdentification.RiskRefID,
        RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
        RiskRank = riskIdentification.RiskRank,
        RiskScore = riskIdentification.RiskScore,
        RiskSource = riskIdentification.RiskSource,
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
            Events = riskIdentification.Events,
            FocusArea = riskIdentification.FocusArea,
            IdentifiedDate = riskIdentification.IdentifiedDate,
            RiskCause = riskIdentification.RiskCause,
            RiskConsequence = riskIdentification.RiskConsequence,
            RiskConsequenceId = riskIdentification.RiskConsequenceId,
            RiskDescription = riskIdentification.RiskDescription,
            RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
            RiskOwner = User.FindFirstValue(ClaimTypes.NameIdentifier),
            RiskRank = riskIdentification.RiskRank,
            RiskScore = riskIdentification.RiskScore,
            RiskSource = riskIdentification.RiskSource,
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
