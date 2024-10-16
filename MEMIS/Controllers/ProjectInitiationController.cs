﻿using Microsoft.AspNetCore.Mvc;
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
			ViewData["RiskRank"] = ListHelper.RiskRank();
			ViewData["Impact"] = ListHelper.Impact();
			ViewData["Influence"] = ListHelper.Influence();
			ViewData["ProjectInitiations"] = new SelectList(_context.ProjectInitiations.OrderBy(d => d.Name), "Id", "Name", id);

			return View("Create",new ProjectInitiationDetailsDto { ProjectInitId = id, ActivityPlans = activities, ProjectPayments = payments, ProjectRiskIdentifications = ristIdentifications, ProjectOthersTabs= others,StakeHolders=stakeholder });
		}

		[HttpPost]
		public async Task<IActionResult> Create(int ProjectInitiationId = 0)
		{
			var others = _context.ProjectOthersTab.Where(m => m.ProjectInitiationId == ProjectInitiationId).ToList();
			var activities = _context.ActivityPlans.Where(m => m.ProjectInitiationId == ProjectInitiationId).ToList();
			var payments = _context.ProjectPayments.Where(m => m.ProjectInitiationId == ProjectInitiationId).ToList();
			var ristIdentifications = _context.ProjectRiskIdentifications.Where(m => m.ProjectInitiationId == ProjectInitiationId).ToList();
			var stakeholder = _context.StakeHolder.Where(m => m.ProjectInitiationId == ProjectInitiationId).ToList();

			ViewData["RiskRank"] = ListHelper.RiskRank();
			ViewData["Impact"] = ListHelper.Impact();
			ViewData["Influence"] = ListHelper.Influence();
			ViewData["ProjectInitiations"] = new SelectList(_context.ProjectInitiations.OrderBy(d => d.Name), "Id", "Name", ProjectInitiationId);
			var x = new ProjectInitiationDetailsDto { ProjectInitId = ProjectInitiationId, ActivityPlans = activities, ProjectPayments = payments, ProjectRiskIdentifications = ristIdentifications , ProjectOthersTabs= others,StakeHolders=stakeholder};
			//x.ActivityPlan = new ActivityPlanDto();
			//x.ProjectRiskIdentification = new ProjectRiskIdentificationDto();
			//x.ProjectPayment = new ProjectPaymentDto();
			return View(x);
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
					Risk = riskIdentification.Risk,
					Rank = riskIdentification.Rank,
					
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
		public async Task<IActionResult> AddprojectOthersTab(ProjectOthersTabDto projectOthersTab)
		{
			if (ModelState.IsValid)
			{
				ProjectOthersTab _data = new()
				{
					ProjectInitiationId = projectOthersTab.ProjectInitiationId,
					Attachment = projectOthersTab.Attachment,
					Resourses = projectOthersTab.Resourses,
				};
				_context.Add(_data);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Add), new { id = projectOthersTab.ProjectInitiationId });
			}
			return RedirectToAction(nameof(Add));
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
		public async Task<IActionResult> AddStakeholder(StakeholderDto stakeholderdto)
		{
			if (ModelState.IsValid)
			{
				StakeHolder _data = new()
				{
					ProjectInitiationId = stakeholderdto.ProjectInitiationId,
					StakeholderName = stakeholderdto.StakeholderName,
					ContactPersonName = stakeholderdto.ContactPersonName,
					ContactPersonEmail = stakeholderdto.ContactPersonEmail,
					ContactPersonAddress = stakeholderdto.ContactPersonAddress,
					ContactPersonPhone = stakeholderdto.ContactPersonPhone,
					ContactPersonWebsite = stakeholderdto.ContactPersonWebsite,
					Impact=stakeholderdto.Impact,
					Influence=stakeholderdto.Influence,
					StakeHolderImportant=stakeholderdto.StakeHolderImportant,
					StakeholderContribution=stakeholderdto.StakeholderContribution,
					Stakeholderblock=stakeholderdto.Stakeholderblock,
					StakeholderStrategy=stakeholderdto.StakeholderStrategy,

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
					Code=dto.Code,
					Name=dto.Name,
					Type=dto.Type,
					Section=dto.Section,
					Program=dto.Program,
					SubProgram=dto.SubProgram,
					StartDate=dto.StartDate,
					EndDate=dto.EndDate,
					Members=dto.Members,
					BudgetCode=dto.BudgetCode,
					DepartmentId=dto.DepartmentId,
					Desc=dto.Desc,
					Manager=dto.Manager,
                };
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(dto);
        }
    }
    
}
