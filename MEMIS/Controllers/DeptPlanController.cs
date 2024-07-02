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

namespace MEMIS.Controllers
{
    [Authorize]
    public class DeptPlanController : Controller
    {
        private readonly Data.AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeptPlanController(Data.AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


    public IActionResult Index(int pageNumber = 1)
    {
      int pageSize = 10;
      var offset = (pageSize * pageNumber) - pageSize;

      if (_context.DeptPlans != null)
      {
        // Retrieve DeptPlans with related entities
        var dat = _context.DeptPlans
            .Include(m => m.StrategicPlanFk)
            .Include(m => m.StrategicInterventionFk)
            .Include(m => m.StrategicActionFk)
            .Include(s => s.DepartmentFk)
            .Skip(offset)
            .Take(pageSize)
            .ToList(); // Execute query to materialize data

        // Check if any records are returned
        if (dat.Count > 0)
        {
          foreach (var item in dat)
          {
            // Retrieve QuaterlyPlans for each DeptPlan item
            var quaterlyplans = _context.QuaterlyPlans
                .Where(x => x.DeptPlanId == item.intActivity)
                .ToList();

            // Calculate sums for each quarter
            item.Q1Target = quaterlyplans.Where(x => x.Quarter == "1").Sum(x => x.QTarget);
            item.Q1Budget = quaterlyplans.Where(x => x.Quarter == "1").Sum(x => x.QBudget);
            item.Q2Target = quaterlyplans.Where(x => x.Quarter == "2").Sum(x => x.QTarget);
            item.Q2Budget = quaterlyplans.Where(x => x.Quarter == "2").Sum(x => x.QBudget);
            item.Q3Target = quaterlyplans.Where(x => x.Quarter == "3").Sum(x => x.QTarget);
            item.Q3Budget = quaterlyplans.Where(x => x.Quarter == "3").Sum(x => x.QBudget);
            item.Q4Target = quaterlyplans.Where(x => x.Quarter == "4").Sum(x => x.QTarget);
            item.Q4Budget = quaterlyplans.Where(x => x.Quarter == "4").Sum(x => x.QBudget);
          }
        }

        // Create a PagedResult object
        var result = new PagedResult<DeptPlan>
        {
          Data = dat,
          TotalItems = _context.DeptPlans.Count(),
          PageNumber = pageNumber,
          PageSize = pageSize
        };

        ViewBag.Users = _userManager;
        return View(result);
      }
      else
      {
        return Problem("Entity set 'AppDbContext.DeptPlans' is null.");
      }
    }

    public IActionResult Verify(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            if (_context.DeptPlans != null)
            {
                var dat = _context.DeptPlans.Include(m => m.StrategicPlanFk).Include(m => m.StrategicInterventionFk).Include(m => m.StrategicActionFk).Include(m => m.DepartmentFk)
                    .Where(x => x.ApprStatus == 0)
                    .Skip(offset)
                    .Take(pageSize);

                var result = new PagedResult<DeptPlan>
                {
                    Data = dat.AsNoTracking().ToList(),
                    TotalItems = _context.DeptPlans.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize

                };
                ViewBag.Users = _userManager;
                return View(result);
            }
            else
            {
                return Problem("Entity set 'AppDbContext.DeptPlans'  is null.");
            }
        }
        public async Task<IActionResult> HeadVerifyStatus(int? id)
        { 
            if (id == null || _context.DeptPlans == null)
            {
                return NotFound();
            }

            var deptPlan = await _context.DeptPlans.Include(m => m.StrategicPlanFk).Include(m => m.StrategicInterventionFk).Include(m => m.StrategicActionFk).Include(m => m.DepartmentFk)
                .Where(m => m.intActivity == id).FirstOrDefaultAsync();
            if (deptPlan == null)
            {
                return NotFound();
            }
            DeptPlanDto deptDto = new DeptPlanDto
            {
                intActivity = deptPlan.intActivity,
                StrategicObjective = deptPlan.StrategicObjective,
                strategicIntervention = deptPlan.strategicIntervention,
                StrategicAction = deptPlan.StrategicAction,
                activity = deptPlan.activity,
                outputIndicator = deptPlan.outputIndicator,
                baseline = deptPlan.baseline,
                budgetCode = deptPlan.budgetCode,
                unitCost = deptPlan.unitCost,
                Q1Target = deptPlan.Q1Target,
                Q1Budget = deptPlan.Q1Budget,
                Q2Target = deptPlan.Q2Target,
                Q2Budget = deptPlan.Q2Budget,
                Q3Target = deptPlan.Q3Target,
                Q3Budget = deptPlan.Q3Budget,
                Q4Target = deptPlan.Q4Target,
                Q4Budget = deptPlan.Q4Budget,
                comparativeTarget = deptPlan.comparativeTarget,
                justification = deptPlan.justification,
                budgetAmount = deptPlan.budgetAmount,
            }; 
            ViewBag.StrategicPlanList = _context.StrategicPlan == null ? new List<StrategicPlan>() : await _context.StrategicPlan.ToListAsync();
            ViewBag.DeptList = _context.Departments == null ? new List<Department>() : await _context.Departments.ToListAsync();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(deptDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HeadVerifyStatus(DeptPlanDto objectdto)
        {
            if (objectdto.intActivity != null && objectdto.intActivity != 0)
            {
                try
                {
                    var pp = await _context.DeptPlans.FindAsync(objectdto.intActivity);
                    if (pp == null)
                    {
                        return NotFound();
                    } 
                    if(objectdto.ApprStatus==1)
                    {
                        pp.ApprStatus =(int)deptPlanApprStatus.hodreviewed;
                    }
                    else if (objectdto.ApprStatus == 2)
                    {
                        pp.ApprStatus = (int)deptPlanApprStatus.hodrejected;
                    }                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RiskIdentificationExists(objectdto.intActivity))
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
            if (_context.DeptPlans != null)
            {
                var dat = _context.DeptPlans.Include(m => m.StrategicPlanFk).Include(m => m.StrategicInterventionFk).Include(m => m.StrategicActionFk).Include(m => m.DepartmentFk)
                    .Where(x => x.ApprStatus ==(int) (deptPlanApprStatus.hodreviewed))
                    .Skip(offset)
                    .Take(pageSize);

                var result = new PagedResult<DeptPlan>
                {
                    Data = dat.AsNoTracking().ToList(),
                    TotalItems = _context.DeptPlans.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                ViewBag.Users = _userManager;
                return View(result);
            }
            else
            {
                return Problem("Entity set 'AppDbContext.DeptPlans'  is null.");
            }
        }
        public async Task<IActionResult> DirVerifyStatus(int? id)
        {
            if (id == null || _context.DeptPlans == null)
            {
                return NotFound();
            }

            var deptPlan = await _context.DeptPlans.Include(m => m.StrategicPlanFk).Include(m => m.StrategicInterventionFk).Include(m => m.StrategicActionFk).Include(m => m.DepartmentFk)
                .Where(m => m.intActivity == id).FirstOrDefaultAsync();
            if (deptPlan == null)
            {
                return NotFound();
            }
            DeptPlanDto riskDto = new DeptPlanDto
            {
                intActivity = deptPlan.intActivity,
                StrategicObjective = deptPlan.StrategicObjective,
                strategicIntervention = deptPlan.strategicIntervention,
                StrategicAction = deptPlan.StrategicAction,
                activity = deptPlan.activity,
                outputIndicator = deptPlan.outputIndicator,
                baseline = deptPlan.baseline,
                budgetCode = deptPlan.budgetCode,
                unitCost = deptPlan.unitCost,
                Q1Target = deptPlan.Q1Target,
                Q1Budget = deptPlan.Q1Budget,
                Q2Target = deptPlan.Q2Target,
                Q2Budget = deptPlan.Q2Budget,
                Q3Target = deptPlan.Q3Target,
                Q3Budget = deptPlan.Q3Budget,
                Q4Target = deptPlan.Q4Target,
                Q4Budget = deptPlan.Q4Budget,
                comparativeTarget = deptPlan.comparativeTarget,
                justification = deptPlan.justification,
                budgetAmount = deptPlan.budgetAmount,
            }; 
            ViewBag.StrategicPlanList = _context.StrategicPlan == null ? new List<StrategicPlan>() : await _context.StrategicPlan.ToListAsync();
            ViewBag.DeptList = _context.Departments == null ? new List<Department>() : await _context.Departments.ToListAsync();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(riskDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DirVerifyStatus(DeptPlanDto objectdto)
        {
            if (objectdto.intActivity != null && objectdto.intActivity != 0)
            {
                try
                {
                    var pp = await _context.DeptPlans.FindAsync(objectdto.intActivity);
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
                    if (!RiskIdentificationExists(objectdto.intActivity))
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
            if (_context.DeptPlans != null)
            {
                var dat = _context.DeptPlans.Include(m => m.StrategicPlanFk).Include(m => m.StrategicInterventionFk).Include(m => m.StrategicActionFk).Include(m => m.DepartmentFk)
                    .Where(x => x.ApprStatus == (int)(deptPlanApprStatus.dirapprove))
                    .Skip(offset)
                    .Take(pageSize);

                var result = new PagedResult<DeptPlan>
                {
                    Data = dat.AsNoTracking().ToList(),
                    TotalItems = _context.DeptPlans.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                ViewBag.Users = _userManager;
                return View(result);
            }
            else
            {
                return Problem("Entity set 'AppDbContext.DeptPlans'  is null.");
            }
        }
        public async Task<IActionResult> ConsolidationStatus(int? id)
        {
            if (id == null || _context.DeptPlans == null)
            {
                return NotFound();
            }

            var deptPlan = await _context.DeptPlans.Include(m => m.StrategicPlanFk).Include(m => m.StrategicInterventionFk).Include(m => m.StrategicActionFk).Include(m => m.DepartmentFk)
                .Where(m => m.intActivity == id).FirstOrDefaultAsync();
            if (deptPlan == null)
            {
                return NotFound();
            }
            DeptPlanDto riskDto = new DeptPlanDto
            {
                intActivity = deptPlan.intActivity,
                StrategicObjective = deptPlan.StrategicObjective,
                strategicIntervention = deptPlan.strategicIntervention,
                StrategicAction = deptPlan.StrategicAction,
                activity = deptPlan.activity,
                outputIndicator = deptPlan.outputIndicator,
                baseline = deptPlan.baseline,
                budgetCode = deptPlan.budgetCode,
                unitCost = deptPlan.unitCost,
                Q1Target = deptPlan.Q1Target,
                Q1Budget = deptPlan.Q1Budget,
                Q2Target = deptPlan.Q2Target,
                Q2Budget = deptPlan.Q2Budget,
                Q3Target = deptPlan.Q3Target,
                Q3Budget = deptPlan.Q3Budget,
                Q4Target = deptPlan.Q4Target,
                Q4Budget = deptPlan.Q4Budget,
                comparativeTarget = deptPlan.comparativeTarget,
                justification = deptPlan.justification,
                budgetAmount = deptPlan.budgetAmount,
            };
            ViewBag.StrategicPlanList = _context.StrategicPlan == null ? new List<StrategicPlan>() : await _context.StrategicPlan.ToListAsync();
            ViewBag.DeptList = _context.Departments == null ? new List<Department>() : await _context.Departments.ToListAsync();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(riskDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConsolidationStatus(DeptPlanDto objectdto)
        {
            if (objectdto.intActivity != null && objectdto.intActivity != 0)
            {
                try
                {
                    var pp = await _context.DeptPlans.FindAsync(objectdto.intActivity);
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
                    if (!RiskIdentificationExists(objectdto.intActivity))
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
            if (_context.DeptPlans != null)
            {
                var dat = _context.DeptPlans.Include(m => m.StrategicPlanFk).Include(m => m.StrategicInterventionFk).Include(m => m.StrategicActionFk).Include(m => m.DepartmentFk)
                    .Where(x => x.ApprStatus == (int)(deptPlanApprStatus.meOfficerVerified))
                    .Skip(offset)
                    .Take(pageSize);

                var result = new PagedResult<DeptPlan>
                {
                    Data = dat.AsNoTracking().ToList(),
                    TotalItems = _context.DeptPlans.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                ViewBag.Users = _userManager;
                return View(result);
            }
            else
            {
                return Problem("Entity set 'AppDbContext.DeptPlans'  is null.");
            }
        }
        public async Task<IActionResult> HeadBpdVerifyStatus(int? id)
        {
            if (id == null || _context.DeptPlans == null)
            {
                return NotFound();
            }

            var deptPlan = await _context.DeptPlans.Include(m => m.StrategicPlanFk).Include(m => m.StrategicInterventionFk).Include(m => m.StrategicActionFk).Include(m => m.DepartmentFk)
                .Where(m => m.intActivity == id).FirstOrDefaultAsync();
            if (deptPlan == null)
            {
                return NotFound();
            }
            DeptPlanDto riskDto = new DeptPlanDto
            {
                intActivity = deptPlan.intActivity,
                StrategicObjective = deptPlan.StrategicObjective,
                strategicIntervention = deptPlan.strategicIntervention,
                StrategicAction = deptPlan.StrategicAction,
                activity = deptPlan.activity,
                outputIndicator = deptPlan.outputIndicator,
                baseline = deptPlan.baseline,
                budgetCode = deptPlan.budgetCode,
                unitCost = deptPlan.unitCost,
                Q1Target = deptPlan.Q1Target,
                Q1Budget = deptPlan.Q1Budget,
                Q2Target = deptPlan.Q2Target,
                Q2Budget = deptPlan.Q2Budget,
                Q3Target = deptPlan.Q3Target,
                Q3Budget = deptPlan.Q3Budget,
                Q4Target = deptPlan.Q4Target,
                Q4Budget = deptPlan.Q4Budget,
                comparativeTarget = deptPlan.comparativeTarget,
                justification = deptPlan.justification,
                budgetAmount = deptPlan.budgetAmount,
            };
            ViewBag.StrategicPlanList = _context.StrategicPlan == null ? new List<StrategicPlan>() : await _context.StrategicPlan.ToListAsync();
            ViewBag.DeptList = _context.Departments == null ? new List<Department>() : await _context.Departments.ToListAsync();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(riskDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HeadBpdVerifyStatus(DeptPlanDto objectdto)
        {
            if (objectdto.intActivity != null && objectdto.intActivity != 0)
            {
                try
                {
                    var pp = await _context.DeptPlans.FindAsync(objectdto.intActivity);
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
                    if (!RiskIdentificationExists(objectdto.intActivity))
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
            if (_context.DeptPlans != null)
            {
                var dat = _context.DeptPlans.Include(m => m.StrategicPlanFk).Include(m => m.StrategicInterventionFk).Include(m => m.StrategicActionFk).Include(m => m.DepartmentFk)
                    .Where(x => x.ApprStatus == (int)(deptPlanApprStatus.headBpdVerified))
                    .Skip(offset)
                    .Take(pageSize);

                var result = new PagedResult<DeptPlan>
                {
                    Data = dat.AsNoTracking().ToList(),
                    TotalItems = _context.DeptPlans.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                ViewBag.Users = _userManager;
                return View(result);
            }
            else
            {
                return Problem("Entity set 'AppDbContext.DeptPlans'  is null.");
            }
        }
        public async Task<IActionResult> DirApprStatus(int? id)
        {
            if (id == null || _context.DeptPlans == null)
            {
                return NotFound();
            }

            var deptPlan = await _context.DeptPlans.Include(m => m.StrategicPlanFk).Include(m => m.StrategicInterventionFk).Include(m => m.StrategicActionFk).Include(m => m.DepartmentFk)
                .Where(m => m.intActivity == id).FirstOrDefaultAsync();
            if (deptPlan == null)
            {
                return NotFound();
            }
            DeptPlanDto riskDto = new DeptPlanDto
            {
                intActivity = deptPlan.intActivity,
                StrategicObjective = deptPlan.StrategicObjective,
                strategicIntervention = deptPlan.strategicIntervention,
                StrategicAction = deptPlan.StrategicAction,
                activity = deptPlan.activity,
                outputIndicator = deptPlan.outputIndicator,
                baseline = deptPlan.baseline,
                budgetCode = deptPlan.budgetCode,
                unitCost = deptPlan.unitCost,
                Q1Target = deptPlan.Q1Target,
                Q1Budget = deptPlan.Q1Budget,
                Q2Target = deptPlan.Q2Target,
                Q2Budget = deptPlan.Q2Budget,
                Q3Target = deptPlan.Q3Target,
                Q3Budget = deptPlan.Q3Budget,
                Q4Target = deptPlan.Q4Target,
                Q4Budget = deptPlan.Q4Budget,
                comparativeTarget = deptPlan.comparativeTarget,
                justification = deptPlan.justification,
                budgetAmount = deptPlan.budgetAmount,
            };
            ViewBag.StrategicPlanList = _context.StrategicPlan == null ? new List<StrategicPlan>() : await _context.StrategicPlan.ToListAsync();
            ViewBag.DeptList = _context.Departments == null ? new List<Department>() : await _context.Departments.ToListAsync();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(riskDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DirApprStatus(DeptPlanDto objectdto)
        {
            if (objectdto.intActivity != null && objectdto.intActivity != 0)
            {
                try
                {
                    var pp = await _context.DeptPlans.FindAsync(objectdto.intActivity);
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
                    if (!RiskIdentificationExists(objectdto.intActivity))
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
            if (id == null || _context.DeptPlans == null)
            {
                return NotFound();
            }

            var deptPlan = await _context.DeptPlans.Include(m => m.StrategicPlanFk).Include(m => m.StrategicInterventionFk).Include(m => m.StrategicActionFk).Include(m => m.DepartmentFk)
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
 
            ViewBag.StrategicPlanList = _context.StrategicPlan == null ? new List<StrategicPlan>() : await _context.StrategicPlan.ToListAsync();
            ViewBag.DeptList = _context.Departments == null ? new List<Department>() : await _context.Departments.ToListAsync();
            ViewData["Quarter"] = ListHelper.Quarter();
            DeptPlanDto deptPlanDto = new DeptPlanDto();
            return View(deptPlanDto);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeptPlanDto dto)
        {
            if (ModelState.IsValid  )
            {
                DeptPlan deptPlan = new DeptPlan
                {
                    StrategicObjective = dto.StrategicObjective,
                    strategicIntervention=dto.strategicIntervention,
                    StrategicAction=dto.StrategicAction,
                    activity=dto.activity,
                    outputIndicator=dto.outputIndicator,
                    baseline=dto.baseline,
                    budgetCode=dto.budgetCode,
                    unitCost=dto.unitCost,
                    Q1Target=dto.Q1Target,
                    Q1Budget=dto.Q1Budget,
                    Q2Target=dto.Q2Target,
                    Q2Budget=dto.Q2Budget,
                    Q3Target=dto.Q3Target,
                    Q3Budget=dto.Q3Budget,
                    Q4Target=dto.Q4Target,
                    Q4Budget=dto.Q4Budget,
                    comparativeTarget=dto.comparativeTarget,
                    justification=dto.justification,
                    budgetAmount=dto.budgetAmount,                    
                    DepartmentId=dto.DepartmentId,
                };
                _context.Add(deptPlan);

                await _context.SaveChangesAsync();
                if (dto.QuaterlyPlans.Count > 0)
                {
                  dto.QuaterlyPlans.ForEach(ev => ev.DeptPlanId = deptPlan.intActivity);
                  _context.QuaterlyPlans.AddRange(dto.QuaterlyPlans);
                  _context.SaveChanges();
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }            
            ViewBag.StrategicPlanList = _context.StrategicPlan == null ? new List<StrategicPlan>() : await _context.StrategicPlan.ToListAsync();
            ViewBag.DeptList = _context.Departments == null ? new List<Department>() : await _context.Departments.ToListAsync();
            return View(dto);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DeptPlans == null)
            {
                return NotFound();
            }

            var deptPlan = await _context.DeptPlans.FindAsync(id);
            if (deptPlan == null)
            {
                return NotFound();
            }

            DeptPlanDto dto = new DeptPlanDto
            {
                StrategicObjective = deptPlan.StrategicObjective,
                strategicIntervention = deptPlan.strategicIntervention,
                StrategicAction = deptPlan.StrategicAction,
                activity = deptPlan.activity,
                outputIndicator = deptPlan.outputIndicator,
                baseline = deptPlan.baseline,
                budgetCode = deptPlan.budgetCode,
                unitCost = deptPlan.unitCost,
                Q1Target = deptPlan.Q1Target,
                Q1Budget = deptPlan.Q1Budget,
                Q2Target = deptPlan.Q2Target,
                Q2Budget = deptPlan.Q2Budget,
                Q3Target = deptPlan.Q3Target,
                Q3Budget = deptPlan.Q3Budget,
                Q4Target = deptPlan.Q4Target,
                Q4Budget = deptPlan.Q4Budget,
                comparativeTarget = deptPlan.comparativeTarget,
                justification = deptPlan.justification,
                budgetAmount = deptPlan.budgetAmount,
               DepartmentId= deptPlan.DepartmentId,
               intActivity= deptPlan.intActivity,
               QuaterlyPlans = await _context.QuaterlyPlans.Where(x => x.DeptPlanId == deptPlan.intActivity).ToListAsync()
            };

            ViewBag.StrategicPlanList = _context.StrategicPlan == null ? new List<StrategicPlan>() : await _context.StrategicPlan.ToListAsync();
            ViewBag.DeptList = _context.Departments == null ? new List<Department>() : await _context.Departments.ToListAsync();
            ViewData["Quarter"] = ListHelper.Quarter();
            return View(dto);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("intActivity,StrategicObjective,strategicIntervention,StrategicAction,activity,outputIndicator,baseline,budgetCode,unitCost,Q1Target,Q1Budget,Q2Target,Q2Budget,Q3Target,Q3Budget,Q4Target,Q4Budget,comparativeTarget,justification,budgetAmount,DepartmentId,QuaterlyPlans")] DeptPlanDto deptPlan)
        {
            if (id != deptPlan.intActivity)
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
                          quat.DeptPlanId = deptPlan.intActivity;
                          await _context.QuaterlyPlans.AddAsync(quat);
                          await _context.SaveChangesAsync();
                        }
                      }
                    }

                    DeptPlan rd = new DeptPlan
                    {
                        StrategicObjective = (int)deptPlan.StrategicObjective,
                        strategicIntervention = (int)deptPlan.strategicIntervention,
                        StrategicAction = (int)deptPlan.StrategicAction,
                        activity = deptPlan.activity,
                        outputIndicator = deptPlan.outputIndicator,
                        baseline = deptPlan.baseline,
                        budgetCode = deptPlan.budgetCode,
                        unitCost = deptPlan.unitCost,
                        Q1Target = deptPlan.Q1Target,
                        Q1Budget = deptPlan.Q1Budget,
                        Q2Target = deptPlan.Q2Target,
                        Q2Budget = deptPlan.Q2Budget,
                        Q3Target = deptPlan.Q3Target,
                        Q3Budget = deptPlan.Q3Budget,
                        Q4Target = deptPlan.Q4Target,
                        Q4Budget = deptPlan.Q4Budget,
                        comparativeTarget = deptPlan.comparativeTarget,
                        justification = deptPlan.justification,
                        budgetAmount = deptPlan.budgetAmount,
                        DepartmentId = deptPlan.DepartmentId,
                        intActivity= deptPlan.intActivity,
                    };
                    _context.Update(rd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RiskIdentificationExists(deptPlan.intActivity))
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
            ViewBag.StrategicPlanList = _context.StrategicPlan == null ? new List<StrategicPlan>() : await _context.StrategicPlan.ToListAsync();
            ViewBag.DeptList = _context.Departments == null ? new List<Department>() : await _context.Departments.ToListAsync();
            return View(deptPlan);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DeptPlans == null)
            {
                return NotFound();
            }

            var deptPlan = await _context.DeptPlans.Include(m => m.StrategicPlanFk).Include(m => m.StrategicInterventionFk).Include(m=>m.StrategicActionFk).Include(s => s.DepartmentFk)
                .FirstOrDefaultAsync(m => m.intActivity == id);
            if (deptPlan == null)
            {
                return NotFound();
            }
            ViewBag.Users = _userManager;
            return View(deptPlan);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DeptPlans == null)
            {
                return Problem("Entity set 'AppDbContext.DeptPlans'  is null.");
            }
            var deptPlan = await _context.DeptPlans.FindAsync(id);
            if (deptPlan != null)
            {
                _context.DeptPlans.Remove(deptPlan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RiskIdentificationExists(int id)
        {
            return (_context.DeptPlans?.Any(e => e.intActivity == id)).GetValueOrDefault();
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
            var res = await _context.DeptPlans.FirstOrDefaultAsync(m => m.intActivity == id);
            if (res != null)
            {
                res.IsVerified = IsVerified;
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Verify");
        }
    }
}
