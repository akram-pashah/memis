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
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

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
                //Events = riskIdentification.Events,
                FocusArea = riskIdentification.FocusArea,
                IdentifiedDate = riskIdentification.IdentifiedDate,
                IsVerified = riskIdentification.IsVerified,
                //RiskCause = riskIdentification.RiskCause,
                //RiskConsequence = riskIdentification.RiskConsequence,
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
            ViewBag.FocusArea=_context.FocusArea==null? new List<FocusArea>() :await _context.FocusArea.ToListAsync();
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
                    if (objectdto.ApprStatus == 1)
                    {
                        pp.ApprStatus = (int)riskIdentifyApprStatus.rmoapproved;
                        RiskRegister riskRegister = new()
                        {
                            RiskId = objectdto.RiskId,
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
            ViewBag.FocusArea   = _context.FocusArea == null ? new List<FocusArea>() : await _context.FocusArea.ToListAsync();
            ViewBag.ActivityList = _context.Activity == null ? new List<Activity>() : await _context.Activity.ToListAsync();
            RiskIdentificationCreateEditDto riskIdentificationCreateEditDto = new RiskIdentificationCreateEditDto();
            return View(riskIdentificationCreateEditDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(
          "IdentifiedDate,StrategicObjective,FocusArea,Activity,BudgetCode" +
          ",RiskDescription,Events,RiskSource,RiskCause,RiskConsequence,RiskConsequenceId," +
          "RiskLikelihoodId,RiskScore,RiskRank,EvalCriteria,IsVerified,EventsList")]RiskIdentificationCreateEditDto dto)
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
                Events = _context.Events.Where(x => x.RiskId == id).ToListAsync().Result,
                FocusArea = (int)riskIdentification.FocusArea,
                IdentifiedDate = riskIdentification.IdentifiedDate,
                IsVerified = riskIdentification.IsVerified,
                RiskCause =  _context.RiskCauses.Where(x => x.RiskId == id).ToListAsync().Result,
                RiskConsequence =  _context.RiskConsequenceDetails.Where(x => x.RiskId == id).ToListAsync().Result,
                RiskConsequenceId = riskIdentification.RiskConsequenceId,
                RiskDescription = riskIdentification.RiskDescription,
                RiskId = riskIdentification.RiskId,
                RiskLikelihoodId = riskIdentification.RiskLikelihoodId,
                RiskRank = riskIdentification.RiskRank,
                RiskScore = riskIdentification.RiskScore,
                RiskSource = _context.RiskSources.Where(x => x.RiskId == id).ToListAsync().Result,
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
