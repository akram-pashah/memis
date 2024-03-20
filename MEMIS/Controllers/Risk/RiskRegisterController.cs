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

            var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m=>m.RiskIdentificationFk)
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
                    pp.ApprStatus =(int)riskWorkFlowStatus.tolerence;  
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
                    .Where(x => x.ApprStatus == 1)
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
                AdditionalMitigation = riskIdentification.AdditionalMitigation,
                ResourcesRequired= riskIdentification.ResourcesRequired,
                ExpectedDate= riskIdentification.ExpectedDate,
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
        public async Task<IActionResult> RiskTreatmentSubmit(RiskTreatmentDto objectdto)
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
                    pp.ApprStatus = (int)riskWorkFlowStatus.treatmentsubmitted;
                    pp.AdditionalMitigation = objectdto.AdditionalMitigation;
                    pp.ResourcesRequired = objectdto.ResourcesRequired;
                    pp.ExpectedDate=   objectdto.ExpectedDate;
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
            ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
            ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
            ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
            ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
            ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            ViewData["RiskRank"] = ListHelper.RiskRank();
            return View(objectdto);
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
                AdditionalMitigation = riskIdentification.AdditionalMitigation,
                ResourcesRequired = riskIdentification.ResourcesRequired,
                ExpectedDate = riskIdentification.ExpectedDate,
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
                    }else if(objectdto.ApprStatus == 2)
                    {
                        pp.ApprStatus = (int)riskWorkFlowStatus.treatmenthodrejected;
                    }
                    pp.AdditionalMitigation = objectdto.AdditionalMitigation;
                    pp.ResourcesRequired = objectdto.ResourcesRequired;
                    pp.ExpectedDate = objectdto.ExpectedDate;
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
                AdditionalMitigation = riskIdentification.AdditionalMitigation,
                ResourcesRequired = riskIdentification.ResourcesRequired,
                ExpectedDate = riskIdentification.ExpectedDate,
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
                    pp.AdditionalMitigation = objectdto.AdditionalMitigation;
                    pp.ResourcesRequired = objectdto.ResourcesRequired;
                    pp.ExpectedDate = objectdto.ExpectedDate;
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
                AdditionalMitigation = riskIdentification.AdditionalMitigation,
                ResourcesRequired = riskIdentification.ResourcesRequired,
                ExpectedDate = riskIdentification.ExpectedDate,
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
                    pp.AdditionalMitigation = objectdto.AdditionalMitigation;
                    pp.ResourcesRequired = objectdto.ResourcesRequired;
                    pp.ExpectedDate = objectdto.ExpectedDate;
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
                AdditionalMitigation = riskIdentification.AdditionalMitigation,
                ResourcesRequired = riskIdentification.ResourcesRequired,
                ExpectedDate = riskIdentification.ExpectedDate,
                ActionTaken=riskIdentification.ActionTaken,
                ActualDate=riskIdentification.ActualDate,
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
        public async Task<IActionResult> RiskMonitoringSubmit(RiskMonitoringDto objectdto)
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
                    pp.ApprStatus = (int)riskWorkFlowStatus.monitoringsubmitted;
                    pp.ActionTaken = objectdto.ActionTaken;
                    pp.ActualDate = objectdto.ActualDate;
                    pp.ActualBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
                return RedirectToAction(nameof(RiskMonitoringList));
            }
            ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
            ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
            ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
            ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
            ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            ViewData["RiskRank"] = ListHelper.RiskRank();
            return View(objectdto);
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
                AdditionalMitigation = riskIdentification.AdditionalMitigation,
                ResourcesRequired = riskIdentification.ResourcesRequired,
                ExpectedDate = riskIdentification.ExpectedDate,
                ActionTaken=riskIdentification.ActionTaken,
                ActualDate= riskIdentification.ActualDate,
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
        public async Task<IActionResult> RiskMonitoringHodReview(RiskMonitoringDto objectdto)
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
                        pp.ApprStatus = (int)riskWorkFlowStatus.monitoringhodreviewed;
                    }
                    else if (objectdto.ApprStatus == 2)
                    {
                        pp.ApprStatus = (int)riskWorkFlowStatus.monitoringhodrejected;
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
                return RedirectToAction(nameof(RiskMonitoringHodReviewList));
            }
            ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
            ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
            ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
            ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
            ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            ViewData["RiskRank"] = ListHelper.RiskRank();
            return View(objectdto);
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

            var riskIdentification = await _context.RiskRegister.Include(m => m.StrategicPlanFk).Include(m => m.ActivityFk).Include(m => m.FocusAreaFk).Include(m => m.RiskIdentificationFk)
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
                AdditionalMitigation = riskIdentification.AdditionalMitigation,
                ResourcesRequired = riskIdentification.ResourcesRequired,
                ExpectedDate = riskIdentification.ExpectedDate,
                ActionTaken= riskIdentification.ActionTaken,    
                ActualDate= riskIdentification.ActualDate,
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
        public async Task<IActionResult> RiskMonitoringDirVerify(RiskMonitoringDto objectdto)
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
                        pp.ApprStatus = (int)riskWorkFlowStatus.monitoringdirapprove;
                    }
                    else if (objectdto.ApprStatus == 2)
                    {
                        pp.ApprStatus = (int)riskWorkFlowStatus.monitoringdirrejected;
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
                return RedirectToAction(nameof(RiskMonitoringDirVerifyList));
            }
            ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
            ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
            ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
            ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
            ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            ViewData["RiskRank"] = ListHelper.RiskRank();
            return View(objectdto);
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
                AdditionalMitigation = riskIdentification.AdditionalMitigation,
                ResourcesRequired = riskIdentification.ResourcesRequired,
                ExpectedDate = riskIdentification.ExpectedDate,
                ActionTaken=riskIdentification.ActionTaken,
                ActualDate= riskIdentification.ActualDate,
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
        public async Task<IActionResult> RiskMonitoringRmoVerify(RiskMonitoringDto objectdto)
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
                        pp.ApprStatus = (int)riskWorkFlowStatus.monitoringrmoapproved;
                    }
                    else if (objectdto.ApprStatus == 2)
                    {
                        pp.ApprStatus = (int)riskWorkFlowStatus.monitoringrmorejected;
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
                return RedirectToAction(nameof(RiskMonitoringRmoVerifyList));
            }
            ViewBag.RiskConsequenceList = GetSelectListForRiskConsequence();
            ViewBag.RiskLikelihoodList = GetSelectListForRiskLikelihood();
            ViewBag.StrategicPlanList = _context.StrategicObjective == null ? new List<StrategicObjective>() : await _context.StrategicObjective.ToListAsync();
            ViewBag.FocusArea = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
            ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            ViewData["RiskRank"] = ListHelper.RiskRank();
            return View(objectdto);
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
                AdditionalMitigation = riskIdentification.AdditionalMitigation,
                ResourcesRequired = riskIdentification.ResourcesRequired,
                ExpectedDate = riskIdentification.ExpectedDate,
                RiskResidualConsequenceId=riskIdentification.RiskResidualConsequenceId,
                RiskResidualLikelihoodId=riskIdentification.RiskResidualLikelihoodId,
                RiskResidualScore=riskIdentification.RiskResidualScore,
                RiskResidualRank=riskIdentification.RiskResidualRank,
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
                    pp.RiskResidualRank= objectdto.RiskResidualRank;
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
                AdditionalMitigation = riskIdentification.AdditionalMitigation,
                ResourcesRequired = riskIdentification.ResourcesRequired,
                ExpectedDate = riskIdentification.ExpectedDate,
                RiskResidualConsequenceId = riskIdentification.RiskResidualConsequenceId,
                RiskResidualLikelihoodId = riskIdentification.RiskResidualLikelihoodId,
                RiskResidualScore = riskIdentification.RiskResidualScore,
                RiskResidualRank = riskIdentification.RiskResidualRank,
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
                AdditionalMitigation = riskIdentification.AdditionalMitigation,
                ResourcesRequired = riskIdentification.ResourcesRequired,
                ExpectedDate = riskIdentification.ExpectedDate,
                RiskResidualConsequenceId = riskIdentification.RiskResidualConsequenceId,
                RiskResidualLikelihoodId = riskIdentification.RiskResidualLikelihoodId,
                RiskResidualScore = riskIdentification.RiskResidualScore,
                RiskResidualRank = riskIdentification.RiskResidualRank,
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
                AdditionalMitigation = riskIdentification.AdditionalMitigation,
                ResourcesRequired = riskIdentification.ResourcesRequired,
                ExpectedDate = riskIdentification.ExpectedDate,
                RiskResidualConsequenceId = riskIdentification.RiskResidualConsequenceId,
                RiskResidualLikelihoodId = riskIdentification.RiskResidualLikelihoodId,
                RiskResidualScore = riskIdentification.RiskResidualScore,
                RiskResidualRank = riskIdentification.RiskResidualRank,
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
}
