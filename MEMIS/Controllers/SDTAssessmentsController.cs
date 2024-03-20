using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data;
using cloudscribe.Pagination.Models;
using MEMIS.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace MEMIS.Controllers
{
	public class SDTAssessmentsController : Controller
	{
		private readonly Data.AppDbContext _context;

		public SDTAssessmentsController(Data.AppDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index(int pageNumber = 1)
		{
			int pageSize = 10;
			var offset = (pageSize * pageNumber) - pageSize;
			var appDbContext = _context.SDTAssessment.Include(s => s.SDTMasterFk).Include(s => s.SDTMasterFk.DepartmentFk).Skip(offset).Take(pageSize);
			var result = new PagedResult<SDTAssessment>
			{
				Data = await appDbContext.AsNoTracking().ToListAsync(),
				TotalItems = _context.SDTAssessment.Count(),
				PageNumber = pageNumber,
				PageSize = pageSize
			};
			ViewData["Months"] = ListHelper.Months();
			ViewData["Departments"] = new SelectList(_context.Departments.OrderBy(d => d.deptName), "deptCode", "deptName");
			return View(result);
		}

		[HttpPost]
		public async Task<IActionResult> Index(int pageNumber = 1, int? month = null, string? deptCode = null)
		{
			int pageSize = 10;
			var offset = (pageSize * pageNumber) - pageSize;
			var appDbContext = _context.SDTAssessment.Include(s => s.SDTMasterFk).Include(s => s.SDTMasterFk.DepartmentFk).Where(s => (month != null ? s.Month == month : true) && (deptCode != null ? s.SDTMasterFk.DepartmentFk.deptCode == deptCode : true)).Skip(offset).Take(pageSize);
			var result = new PagedResult<SDTAssessment>
			{
				Data = await appDbContext.AsNoTracking().ToListAsync(),
				TotalItems = _context.SDTAssessment.Count(),
				PageNumber = pageNumber,
				PageSize = pageSize
			};

			var months = ListHelper.Months();
			if (month != null)
			{
				var selectedMonth = months.Where(x => (x.Value == month.ToString() || x.Value == null)).First();
				selectedMonth.Selected = true;
			} 
			ViewData["Months"] = months;

			ViewData["Departments"] = new SelectList(_context.Departments.OrderBy(d => d.deptName), "deptCode", "deptName", deptCode);
			return View(result);
		}

		public async Task<IActionResult> SDTList(int pageNumber = 1)
		{
			int pageSize = 10;
			var offset = (pageSize * pageNumber) - pageSize;
			var appDbContext = _context.SDTMasters.Include(s => s.DepartmentFk).Skip(offset).Take(pageSize);
			var result = new PagedResult<SDTMaster>
			{
				Data = await appDbContext.AsNoTracking().ToListAsync(),
				TotalItems = _context.SDTMasters.Count(),
				PageNumber = pageNumber,
				PageSize = pageSize
			};
			ViewData["Departments"] = new SelectList(_context.Departments.OrderBy(d => d.deptName), "deptCode", "deptName");
			return View(result);
		}

		[HttpPost]
		public async Task<IActionResult> SDTList(int pageNumber = 1, string? deptCode = null)
		{
			int pageSize = 10;
			var offset = (pageSize * pageNumber) - pageSize;
			var appDbContext = _context.SDTMasters.Include(s => s.DepartmentFk).Where(s => deptCode != null ? (s.DepartmentFk.deptCode == deptCode) : true).Skip(offset).Take(pageSize);
			var result = new PagedResult<SDTMaster>
			{
				Data = await appDbContext.AsNoTracking().ToListAsync(),
				TotalItems = _context.SDTMasters.Count(),
				PageNumber = pageNumber,
				PageSize = pageSize
			};

			ViewData["Departments"] = new SelectList(_context.Departments.OrderBy(d => d.deptName), "deptCode", "deptName", deptCode);
			return View(result);
		}


		public async Task<IActionResult> Create(int? id)
		{
			if (id == null || _context.SDTMasters == null)
			{
				return NotFound();
			}

			var sdtMaster = await _context.SDTMasters.Include(s => s.DepartmentFk).FirstOrDefaultAsync(m => m.Id == id);
			if (sdtMaster == null)
			{
				return NotFound();
			}
			ViewData["Months"] = ListHelper.Months();
			ViewData["AchievementStatus"] = ListHelper.AchievementStatus();
			//ViewData["SDTAsessmentAction"] = ListHelper.SDTAsessmentAction();

			return View(new SDTAssessmentDto { SDTMasterFk = sdtMaster });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(SDTAssessmentDto assessment)
		{
			if (ModelState.IsValid)
			{
				if (_context.SDTAssessment == null)
				{
					return NotFound();
				}

				SDTAssessment _data = new()
				{
					Month = assessment.Month,
					Numerator = assessment.Numerator,
					Denominator = assessment.Denominator,
					ImplementedTimeline = assessment.ImplementedTimeline,
					Rate = assessment.Rate,
					ProportionTimeline = assessment.ProportionTimeline,
					Target = assessment.Target,
					AchivementStatus = assessment.AchivementStatus,
					Variance = assessment.Variance,
					Justification = assessment.Justification,
					Rating = assessment.Rating,
					SDTMasterId = assessment.SDTMasterId,
					HODActionDate = null,
					DirectorActionDate = null,
					ApprovalStatusDirector = 0,
					ApprovalStatusHOD = 0,
					ApprovalStatusOfficer = 0,
					ApprovalStatusMEOFfficer = 0,
					ApprovalStatusHBPD = 0,
					ApprovalStatusDDCS = 0
				};

				var assessmentDetails = await _context.SDTAssessment.FirstOrDefaultAsync(m => m.SDTMasterId == assessment.SDTMasterId && m.Month == assessment.Month);

				if (assessmentDetails == null)
					_context.Add(_data);
				else
					_context.Update(_data);

				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(assessment);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.SDTAssessment == null)
			{
				return NotFound();
			}

			var assessment = await _context.SDTAssessment.Include(a => a.SDTMasterFk).FirstOrDefaultAsync(a => a.Id == id);
			if (assessment == null)
			{
				return NotFound();
			}
			SDTAssessmentDto dto = new()
			{
				Id = assessment.Id,
				Month = assessment.Month,
				Numerator = assessment.Numerator,
				Denominator = assessment.Denominator,
				ImplementedTimeline = assessment.ImplementedTimeline,
				Rate = assessment.Rate,
				ProportionTimeline = assessment.ProportionTimeline,
				Target = assessment.Target,
				AchivementStatus = assessment.AchivementStatus,
				Variance = assessment.Variance,
				Justification = assessment.Justification,
				Rating = assessment.Rating,
				SDTMasterId = assessment.SDTMasterId,
				SDTMasterFk = assessment.SDTMasterFk
			};
			if (assessment.Month != null)
			{
				var selectedMonth = ListHelper.Months().Where(x => x.Value == assessment.Month.ToString()).First();
				selectedMonth.Selected = true;		 
			}

			var months = ListHelper.Months();
			ViewData["Months"] = months;
			ViewData["AchievementStatus"] = ListHelper.AchievementStatus();
			var selectedAchievementStatus = ListHelper.AchievementStatus().Where(x => x.Value == assessment.AchivementStatus).First();
			selectedAchievementStatus.Selected = true; 
			return View(dto);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(SDTAssessmentDto assessment)
		{
			if (ModelState.IsValid)
			{
				try
				{
					SDTAssessment _data = new()
					{
						Id = assessment.Id,
						Month = assessment.Month,
						Numerator = assessment.Numerator,
						Denominator = assessment.Denominator,
						ImplementedTimeline = assessment.ImplementedTimeline,
						Rate = assessment.Rate,
						ProportionTimeline = assessment.ProportionTimeline,
						Target = assessment.Target,
						AchivementStatus = assessment.AchivementStatus,
						Variance = assessment.Variance,
						Justification = assessment.Justification,
						Rating = assessment.Rating,
						SDTMasterId = assessment.SDTMasterId,
						ApprovalStatusHOD = 0,
						ApprovalStatusDirector = 0,
						ApprovalStatusOfficer = 0,
						ApprovalStatusMEOFfficer = 0,
						ApprovalStatusHBPD = 0,
						ApprovalStatusDDCS = 0,
					};
					_context.Update(_data);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!SDTMasterExists(assessment.Id))
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

			return View(assessment);
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.SDTAssessment == null)
			{
				return NotFound();
			}

			var assessment = await _context.SDTAssessment
				.Include(s => s.SDTMasterFk)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (assessment == null)
			{
				return NotFound();
			}

			return View(assessment);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.SDTAssessment == null)
			{
				return NotFound();
			}

			var assessment = await _context.SDTAssessment.Include(s => s.SDTMasterFk).FirstOrDefaultAsync(m => m.Id == id);
			if (assessment == null)
			{
				return NotFound();
			}

			return View(assessment);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.SDTAssessment == null)
			{
				return Problem("Entity set 'AppDbContext.SDTAssessment'  is null.");
			}
			var assessment = await _context.SDTAssessment.FindAsync(id);
			if (assessment != null)
			{
				_context.SDTAssessment.Remove(assessment);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool SDTMasterExists(int id)
		{
			return (_context.SDTAssessment?.Any(e => e.Id == id)).GetValueOrDefault();
		}

		public async Task<IActionResult> VerifyByHOD(int pageNumber = 1, int? month = null, string? deptCode = null)
		{
			int pageSize = 10;
			var offset = (pageSize * pageNumber) - pageSize;
			var appDbContext = _context.SDTAssessment.Include(s => s.SDTMasterFk).Include(s => s.SDTMasterFk.DepartmentFk)
				.Where(e => e.ApprovalStatusHOD == 0)
				.Where(s => (month != null ? s.Month == month : true) && (deptCode != null ? s.SDTMasterFk.DepartmentFk.deptCode == deptCode : true)).Skip(offset).Take(pageSize);
			var result = new PagedResult<SDTAssessment>
			{
				Data = await appDbContext.AsNoTracking().ToListAsync(),
				TotalItems = _context.SDTAssessment.Count(),
				PageNumber = pageNumber,
				PageSize = pageSize
			};

			var months = ListHelper.Months();
			if (month != null)
			{
				var selectedMonth = months.Where(x => (x.Value == month.ToString() || x.Value == null)).First();
				selectedMonth.Selected = true;
			}
			ViewData["Months"] = months;
			ViewData["Departments"] = new SelectList(_context.Departments.OrderBy(d => d.deptName), "deptCode", "deptName", deptCode);
			return View(result);
		}
		public async Task<IActionResult> VerifyDetailsHOD(int? id)
		{
			if (id == null || _context.SDTAssessment == null)
			{
				return NotFound();
			}

			var assessment = await _context.SDTAssessment
				.Include(s => s.SDTMasterFk)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (assessment == null)
			{
				return NotFound();
			}

			ViewData["Approval"] = ListHelper.ApprovalStatus();
			return View(assessment); ;
		}
		public async Task<IActionResult> ChangeStatusHOD(SDTAssessment dto)
		{
			if (dto.Id != null && dto.Id != 0)
			{
				try
				{
					var pp = await _context.SDTAssessment.FindAsync(dto.Id);
					if (pp == null)
					{
						return NotFound();
					}
					pp.HODAction = User.FindFirstValue(ClaimTypes.NameIdentifier);
					pp.HODActionDate = DateTime.Now;
					pp.HODComment = dto.HODComment;
					pp.ApprovalStatusHOD = dto.ApprovalStatusHOD;
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!SDTMasterExists(dto.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(VerifyByHOD));
			}
			return RedirectToAction(nameof(VerifyByHOD));


		}



		public async Task<IActionResult> VerifyByDirector(int pageNumber = 1, int? month = null, string? deptCode = null)
		{
			int pageSize = 10;
			var offset = (pageSize * pageNumber) - pageSize;
			var appDbContext = _context.SDTAssessment.Include(s => s.SDTMasterFk).Include(s => s.SDTMasterFk.DepartmentFk)
				.Where(e => e.ApprovalStatusDirector == 0)
				.Where(s => (month != null ? s.Month == month : true) && (deptCode != null ? s.SDTMasterFk.DepartmentFk.deptCode == deptCode : true)).Skip(offset).Take(pageSize);
			var result = new PagedResult<SDTAssessment>
			{
				Data = await appDbContext.AsNoTracking().ToListAsync(),
				TotalItems = _context.SDTAssessment.Count(),
				PageNumber = pageNumber,
				PageSize = pageSize
			};

			var months = ListHelper.Months();
			if (month != null)
			{
				var selectedMonth = months.Where(x => (x.Value == month.ToString() || x.Value == null)).First();
				selectedMonth.Selected = true;
			}
			ViewData["Months"] = months;
			ViewData["Departments"] = new SelectList(_context.Departments.OrderBy(d => d.deptName), "deptCode", "deptName", deptCode);
			return View(result);
		}
		public async Task<IActionResult> VerifyDetailsDirector(int? id)
		{
			if (id == null || _context.SDTAssessment == null)
			{
				return NotFound();
			}

			var assessment = await _context.SDTAssessment
				.Include(s => s.SDTMasterFk)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (assessment == null)
			{
				return NotFound();
			}
			ViewData["Approval"] = ListHelper.ApprovalStatus();
			return View(assessment);
		}
		public async Task<IActionResult> ChangeStatusDirector(SDTAssessment dto)
		{
			if (dto.Id != null && dto.Id != 0)
			{
				try
				{
					var pp = await _context.SDTAssessment.FindAsync(dto.Id);
					if (pp == null)
					{
						return NotFound();
					}
					pp.DirectorAction = User.FindFirstValue(ClaimTypes.NameIdentifier);
					pp.DirectorActionDate = DateTime.Now;
					pp.DirectorComment = dto.DirectorComment;
					pp.ApprovalStatusDirector = dto.ApprovalStatusDirector;
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!SDTMasterExists(dto.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(VerifyByDirector));
			}
			return RedirectToAction(nameof(VerifyByDirector));


		}
		public async Task<IActionResult> VerifyByMEOfficer(int pageNumber = 1, int? month = null, string? deptCode = null)
		{
			int pageSize = 10;
			var offset = (pageSize * pageNumber) - pageSize;
			var appDbContext = _context.SDTAssessment.Include(s => s.SDTMasterFk).Include(s => s.SDTMasterFk.DepartmentFk)
				.Where(e => e.ApprovalStatusMEOFfficer == 0)
				.Where(s => (month != null ? s.Month == month : true) && (deptCode != null ? s.SDTMasterFk.DepartmentFk.deptCode == deptCode : true)).Skip(offset).Take(pageSize);
			var result = new PagedResult<SDTAssessment>
			{
				Data = await appDbContext.AsNoTracking().ToListAsync(),
				TotalItems = _context.SDTAssessment.Count(),
				PageNumber = pageNumber,
				PageSize = pageSize
			};

			var months = ListHelper.Months();
			if (month != null)
			{
				var selectedMonth = months.Where(x => (x.Value == month.ToString() || x.Value == null)).First();
				selectedMonth.Selected = true;
			}
			ViewData["Months"] = months;
			ViewData["Departments"] = new SelectList(_context.Departments.OrderBy(d => d.deptName), "deptCode", "deptName", deptCode);
			return View(result);
		}
		public async Task<IActionResult> VerifyDetailsMEOfficer(int? id)
		{
			if (id == null || _context.SDTAssessment == null)
			{
				return NotFound();
			}

			var assessment = await _context.SDTAssessment
				.Include(s => s.SDTMasterFk)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (assessment == null)
			{
				return NotFound();
			}
			ViewData["Approval"] = ListHelper.ApprovalStatus();
			return View(assessment);
		}
		public async Task<IActionResult> ChangeStatusMEOfficer(SDTAssessment dto)
		{
			if (dto.Id != null && dto.Id != 0)
			{
				try
				{
					var pp = await _context.SDTAssessment.FindAsync(dto.Id);
					if (pp == null)
					{
						return NotFound();
					}
					pp.MEOfficerAction = User.FindFirstValue(ClaimTypes.NameIdentifier);
					pp.MEOfficerActionDate = DateTime.Now;
					pp.MEOfficerComment = dto.DirectorComment;
					pp.ApprovalStatusMEOFfficer = dto.ApprovalStatusMEOFfficer;
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!SDTMasterExists(dto.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(VerifyByMEOfficer));
			}
			return RedirectToAction(nameof(VerifyByMEOfficer));


		}
		public async Task<IActionResult> VerifyByHBPD(int pageNumber = 1, int? month = null, string? deptCode = null)
		{
			int pageSize = 10;
			var offset = (pageSize * pageNumber) - pageSize;
			var appDbContext = _context.SDTAssessment.Include(s => s.SDTMasterFk).Include(s => s.SDTMasterFk.DepartmentFk)
				.Where(e => e.ApprovalStatusHBPD == 0)
				.Where(s => (month != null ? s.Month == month : true) && (deptCode != null ? s.SDTMasterFk.DepartmentFk.deptCode == deptCode : true)).Skip(offset).Take(pageSize);
			var result = new PagedResult<SDTAssessment>
			{
				Data = await appDbContext.AsNoTracking().ToListAsync(),
				TotalItems = _context.SDTAssessment.Count(),
				PageNumber = pageNumber,
				PageSize = pageSize
			};

			var months = ListHelper.Months();
			if (month != null)
			{
				var selectedMonth = months.Where(x => (x.Value == month.ToString() || x.Value == null)).First();
				selectedMonth.Selected = true;
			}
			ViewData["Months"] = months;
			ViewData["Departments"] = new SelectList(_context.Departments.OrderBy(d => d.deptName), "deptCode", "deptName", deptCode);
			return View(result);
		}
		public async Task<IActionResult> VerifyDetailsHBPD(int? id)
		{
			if (id == null || _context.SDTAssessment == null)
			{
				return NotFound();
			}

			var assessment = await _context.SDTAssessment
				.Include(s => s.SDTMasterFk)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (assessment == null)
			{
				return NotFound();
			}
			ViewData["Approval"] = ListHelper.ApprovalStatus();
			return View(assessment);
		}
		public async Task<IActionResult> ChangeStatusHBPD(SDTAssessment dto)
		{
			if (dto.Id != null && dto.Id != 0)
			{
				try
				{
					var pp = await _context.SDTAssessment.FindAsync(dto.Id);
					if (pp == null)
					{
						return NotFound();
					}
					pp.HBPDAction = User.FindFirstValue(ClaimTypes.NameIdentifier);
					pp.HBPDActionDate = DateTime.Now;
					pp.HBPDComment = dto.DirectorComment;
					pp.ApprovalStatusHBPD = dto.ApprovalStatusHBPD;
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!SDTMasterExists(dto.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(VerifyByHBPD));
			}
			return RedirectToAction(nameof(VerifyByHBPD));


		}
		public async Task<IActionResult> VerifyByDDCS(int pageNumber = 1, int? month = null, string? deptCode = null)
		{
			int pageSize = 10;
			var offset = (pageSize * pageNumber) - pageSize;
			var appDbContext = _context.SDTAssessment.Include(s => s.SDTMasterFk).Include(s => s.SDTMasterFk.DepartmentFk)
				.Where(e => e.ApprovalStatusDDCS == 0)
				.Where(s => (month != null ? s.Month == month : true) && (deptCode != null ? s.SDTMasterFk.DepartmentFk.deptCode == deptCode : true)).Skip(offset).Take(pageSize);
			var result = new PagedResult<SDTAssessment>
			{
				Data = await appDbContext.AsNoTracking().ToListAsync(),
				TotalItems = _context.SDTAssessment.Count(),
				PageNumber = pageNumber,
				PageSize = pageSize
			};

			var months = ListHelper.Months();
			if (month != null)
			{
				var selectedMonth = months.Where(x => (x.Value == month.ToString() || x.Value == null)).First();
				selectedMonth.Selected = true;
			}
			ViewData["Months"] = months;
			ViewData["Departments"] = new SelectList(_context.Departments.OrderBy(d => d.deptName), "deptCode", "deptName", deptCode);
			return View(result);
		}
		public async Task<IActionResult> VerifyDetailsByDDCS(int? id)
		{
			if (id == null || _context.SDTAssessment == null)
			{
				return NotFound();
			}

			var assessment = await _context.SDTAssessment
				.Include(s => s.SDTMasterFk)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (assessment == null)
			{
				return NotFound();
			}
			ViewData["Approval"] = ListHelper.ApprovalStatus();
			return View(assessment);
		}
		public async Task<IActionResult> ChangeStatusByDDCS(SDTAssessment dto)
		{
			if (dto.Id != null && dto.Id != 0)
			{
				try
				{
					var pp = await _context.SDTAssessment.FindAsync(dto.Id);
					if (pp == null)
					{
						return NotFound();
					}
					pp.DDCSAction = User.FindFirstValue(ClaimTypes.NameIdentifier);
					pp.DDCSActionDate = DateTime.Now;
					pp.DDCSComment = dto.DirectorComment;
					pp.ApprovalStatusDDCS = dto.ApprovalStatusDDCS;
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!SDTMasterExists(dto.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(VerifyByDDCS));
			}
			return RedirectToAction(nameof(VerifyByDDCS));


		}

	}
}
