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
    public class KPIAssessmentController : Controller
    {
        private readonly Data.AppDbContext _context;

        public KPIAssessmentController(Data.AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.KPIAssessment.Include(s => s.KPIMasterFk).Skip(offset).Take(pageSize);
            var result = new PagedResult<KPIAssessment>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.KPIAssessment.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewData["FYear"] = ListHelper.FYear();
          //  ViewData["Departments"] = new SelectList(_context.Departments.OrderBy(d => d.deptName), "deptCode", "deptName");
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Index(int pageNumber = 1, int? fy = null, string? deptCode = null)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.KPIAssessment.Include(s => s.KPIMasterFk).Where(s => (s != null ? s.FY == fy : true)).Skip(offset).Take(pageSize);
            var result = new PagedResult<KPIAssessment>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.KPIAssessment.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var fyear = ListHelper.FYear();
            if (fy != null)
            {
                var selectFyear = fyear.Where(x => (x.Value == fy.ToString() || x.Value == null)).First();
                selectFyear.Selected = true;
            }
            ViewData["FYear"] = fyear;

           // ViewData["Departments"] = new SelectList(_context.Departments.OrderBy(d => d.deptName), "deptCode", "deptName", deptCode);
            return View(result);
        }

        public async Task<IActionResult> KPIList(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.KPIMasters.Skip(offset).Take(pageSize);
            var result = new PagedResult<KPIMaster>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.KPIMasters.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
			ViewData["TypeofIndicator"] = ListHelper.TypeofIndicator();
			ViewData["Indicatorclassification"] = ListHelper.Indicatorclassification();
			ViewData["FrequencyofReporting"] = ListHelper.FrequencyofReporting();
			//  ViewData["Departments"] = new SelectList(_context.Departments.OrderBy(d => d.deptName), "deptCode", "deptName");
			return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> KPIList(int pageNumber = 1, string? deptCode = null)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.KPIMasters.Skip(offset).Take(pageSize);
            var result = new PagedResult<KPIMaster>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.KPIMasters.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
			ViewData["TypeofIndicator"] = ListHelper.TypeofIndicator();
			ViewData["Indicatorclassification"] = ListHelper.Indicatorclassification();
			ViewData["FrequencyofReporting"] = ListHelper.FrequencyofReporting();
			// ViewData["Departments"] = new SelectList(_context.Departments.OrderBy(d => d.deptName), "deptCode", "deptName", deptCode);
			return View(result);
        }


        public async Task<IActionResult> Create(int? id)
        {
            if (id == null || _context.KPIMasters == null)
            {
                return NotFound();
            }

            var kpiMaster = await _context.KPIMasters.Include(s => s.StrategicPlanFk).FirstOrDefaultAsync(m => m.Id == id);
            if (kpiMaster == null)
            {
                return NotFound();
            }
            ViewData["FYear"] = ListHelper.FYear();
            ViewData["AchievementStatus"] = ListHelper.AchievementStatus();
			ViewData["FrequencyofReporting"] = ListHelper.FrequencyofReporting();
			//ViewData["SDTAsessmentAction"] = ListHelper.SDTAsessmentAction();

			return View(new KPIAssessmentDto { KPIMasterFk = kpiMaster });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KPIAssessmentDto assessment)
        {
            //if (ModelState.IsValid)
            //{
                if (_context.KPIAssessment == null)
                {
                    return NotFound();
                }

                KPIAssessment _data = new()
                { 
                    KPIMasterId = assessment.KPIMasterId,
                    AssessmentDate=DateTime.Now,
                    PerformanceIndicator = assessment.KPIMasterFk.PerformanceIndicator, 
                    FrequencyofReporting = assessment.KPIMasterFk.FrequencyofReporting,
                    IndicatorFormulae= assessment.KPIMasterFk.IndicatorFormulae,
                    IndicatorDefinition= assessment.KPIMasterFk.IndicatorDefinition,
                    FY=assessment.FY,
                    Target=assessment.Target,
                    Numerator=assessment.Numerator,
                    Denominator=assessment.Denominator,
                    Rate=assessment.Rate,  
                    Achieved =assessment.Achieved,
                    ApprovalStatus=0,
                    Justification=assessment.Justification,

                };

                var assessmentDetails = await _context.KPIAssessment.FirstOrDefaultAsync(m => m.KPIMasterId == assessment.KPIMasterId && m.FY == assessment.FY);

                if (assessmentDetails == null)
                    _context.Add(_data);
                else
                    _context.Update(_data);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            return View(assessment);
        }

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.KPIAssessment == null)
        //    {
        //        return NotFound();
        //    }

        //    var assessment = await _context.KPIAssessment.Include(a => a.KPIMasterFk).FirstOrDefaultAsync(a => a.Id == id);
        //    if (assessment == null)
        //    {
        //        return NotFound();
        //    }
        //    SDTAssessmentDto dto = new()
        //    {
        //        Id = assessment.Id,
        //        Month = assessment.Month,
        //        Numerator = assessment.Numerator,
        //        Denominator = assessment.Denominator,
        //        ImplementedTimeline = assessment.ImplementedTimeline,
        //        Rate = assessment.Rate,
        //        ProportionTimeline = assessment.ProportionTimeline,
        //        Target = assessment.Target,
        //        AchivementStatus = assessment.AchivementStatus,
        //        Variance = assessment.Variance,
        //        Justification = assessment.Justification,
        //        Rating = assessment.Rating,
        //        SDTMasterId = assessment.SDTMasterId,
        //        KPIMasterFk = assessment.KPIMasterFk
        //    };
        //    if (assessment.Month != null)
        //    {
        //        var selectFyear = ListHelper.FYear().Where(x => x.Value == assessment.Month.ToString()).First();
        //        selectFyear.Selected = true;
        //    }

        //    var fyear = ListHelper.FYear();
        //    ViewData["FYear"] = fyear;
        //    ViewData["AchievementStatus"] = ListHelper.AchievementStatus();
        //    var selectedAchievementStatus = ListHelper.AchievementStatus().Where(x => x.Value == assessment.AchivementStatus).First();
        //    selectedAchievementStatus.Selected = true;


        //    return View(dto);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(SDTAssessmentDto assessment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            KPIAssessment _data = new()
        //            {
        //                Id = assessment.Id,
        //                Month = assessment.Month,
        //                Numerator = assessment.Numerator,
        //                Denominator = assessment.Denominator,
        //                ImplementedTimeline = assessment.ImplementedTimeline,
        //                Rate = assessment.Rate,
        //                ProportionTimeline = assessment.ProportionTimeline,
        //                Target = assessment.Target,
        //                AchivementStatus = assessment.AchivementStatus,
        //                Variance = assessment.Variance,
        //                Justification = assessment.Justification,
        //                Rating = assessment.Rating,
        //                SDTMasterId = assessment.SDTMasterId,
        //                ApprovalStatusHOD = 0,
        //                ApprovalStatusDirector = 0,
        //                ApprovalStatusOfficer = 0,
        //                ApprovalStatusMEOFfficer = 0,
        //                ApprovalStatusHBPD = 0,
        //                ApprovalStatusDDCS = 0,
        //            };
        //            _context.Update(_data);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!SDTMasterExists(assessment.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(assessment);
        //}

        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.KPIAssessment == null)
        //    {
        //        return NotFound();
        //    }

        //    var assessment = await _context.KPIAssessment
        //        .Include(s => s.KPIMasterFk)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (assessment == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(assessment);
        //}

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KPIAssessment == null)
            {
                return NotFound();
            }

            var assessment = await _context.KPIAssessment.Include(s => s.KPIMasterFk).FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.KPIAssessment == null)
            {
                return Problem("Entity set 'AppDbContext.KPIAssessment'  is null.");
            }
            var assessment = await _context.KPIAssessment.FindAsync(id);
            if (assessment != null)
            {
                _context.KPIAssessment.Remove(assessment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SDTMasterExists(int id)
        {
            return (_context.KPIAssessment?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> VerifyByHOD(int pageNumber = 1, int? fy = null, string? deptCode = null)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.KPIAssessment.Include(s => s.KPIMasterFk).Include(s => s.KPIMasterFk.StrategicPlanFk)
                .Where(e => e.ApprovalStatus == 0)
                .Where(s => (fy != null ? s.FY == fy : true) ).Skip(offset).Take(pageSize);
            var result = new PagedResult<KPIAssessment>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.KPIAssessment.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var fyear = ListHelper.FYear();
            if (fy != null)
            {
                var selectFyear = fyear.Where(x => (x.Value == fy.ToString() || x.Value == null)).First();
                selectFyear.Selected = true;
            }
            ViewData["FYear"] = ListHelper.FYear();
            ViewData["AchievementStatus"] = ListHelper.AchievementStatus();
            ViewData["FrequencyofReporting"] = ListHelper.FrequencyofReporting();
            return View(result);
        }
        public async Task<IActionResult> VerifyDetailsHOD(int? id)
        {
            if (id == null || _context.KPIAssessment == null)
            {
                return NotFound();
            }

            var assessment = await _context.KPIAssessment
                .Include(s => s.KPIMasterFk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assessment == null)
            {
                return NotFound();
            }
            ViewData["FYear"] = ListHelper.FYear();
            ViewData["AchievementStatus"] = ListHelper.AchievementStatus();
            ViewData["FrequencyofReporting"] = ListHelper.FrequencyofReporting();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(assessment); ;
        }
        public async Task<IActionResult> ChangeStatusHOD(KPIAssessment dto)
        {
            if (dto.Id != null && dto.Id != 0)
            {
                try
                {
                    var pp = await _context.KPIAssessment.FindAsync(dto.Id);
                    if (pp == null)
                    {
                        return NotFound();
                    } 
                    pp.ApprovalStatus = dto.ApprovalStatus;
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



        public async Task<IActionResult> VerifyByDirector(int pageNumber = 1, int? fy = null, string? deptCode = null)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.KPIAssessment.Include(s => s.KPIMasterFk).Include(s => s.KPIMasterFk.StrategicPlanFk)
                .Where(e => e.ApprovalStatus == 1)
                .Where(s => (fy != null ? s.FY == fy : true) ).Skip(offset).Take(pageSize);
            var result = new PagedResult<KPIAssessment>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.KPIAssessment.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var fyear = ListHelper.FYear();
            if (fy != null)
            {
                var selectFyear = fyear.Where(x => (x.Value == fy.ToString() || x.Value == null)).First();
                selectFyear.Selected = true;
            }
            ViewData["FYear"] = ListHelper.FYear();
            ViewData["AchievementStatus"] = ListHelper.AchievementStatus();
            ViewData["FrequencyofReporting"] = ListHelper.FrequencyofReporting();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(result);
        }
        public async Task<IActionResult> VerifyDetailsDirector(int? id)
        {
            if (id == null || _context.KPIAssessment == null)
            {
                return NotFound();
            }

            var assessment = await _context.KPIAssessment
                .Include(s => s.KPIMasterFk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assessment == null)
            {
                return NotFound();
            }
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(assessment);
        }
        public async Task<IActionResult> ChangeStatusDirector(KPIAssessment dto)
        {
            if (dto.Id != null && dto.Id != 0)
            {
                try
                {
                    var pp = await _context.KPIAssessment.FindAsync(dto.Id);
                    if (pp == null)
                    {
                        return NotFound();
                    }
                    if(dto.ApprovalStatus==1)
                    {
                        pp.ApprovalStatus =3;
                    }
                    else
                    {
                        pp.ApprovalStatus =4;
                    }
                    
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
        public async Task<IActionResult> VerifyByMEOfficer(int pageNumber = 1, int? fy = null, string? deptCode = null)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.KPIAssessment.Include(s => s.KPIMasterFk).Include(s => s.KPIMasterFk.StrategicPlanFk)
                .Where(e => e.ApprovalStatus == 3)
                .Where(s => (fy != null ? s.FY == fy : true) ).Skip(offset).Take(pageSize);
            var result = new PagedResult<KPIAssessment>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.KPIAssessment.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var fyear = ListHelper.FYear();
            if (fy != null)
            {
                var selectFyear = fyear.Where(x => (x.Value == fy.ToString() || x.Value == null)).First();
                selectFyear.Selected = true;
            }
            ViewData["FYear"] = ListHelper.FYear();
            ViewData["AchievementStatus"] = ListHelper.AchievementStatus();
            ViewData["FrequencyofReporting"] = ListHelper.FrequencyofReporting();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(result);
        }
        public async Task<IActionResult> VerifyDetailsMEOfficer(int? id)
        {
            if (id == null || _context.KPIAssessment == null)
            {
                return NotFound();
            }

            var assessment = await _context.KPIAssessment
                .Include(s => s.KPIMasterFk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assessment == null)
            {
                return NotFound();
            }
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(assessment);
        }
        public async Task<IActionResult> ChangeStatusMEOfficer(KPIAssessment dto)
        {
            if (dto.Id != null && dto.Id != 0)
            {
                try
                {
                    var pp = await _context.KPIAssessment.FindAsync(dto.Id);
                    if (pp == null)
                    {
                        return NotFound();
                    }
                    if (dto.ApprovalStatus == 1)
                    {
                        pp.ApprovalStatus = 5;
                    }
                    else
                    {
                        pp.ApprovalStatus = 6;
                    }
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
        public async Task<IActionResult> VerifyByHBPD(int pageNumber = 1, int? fy = null, string? deptCode = null)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.KPIAssessment.Include(s => s.KPIMasterFk).Include(s => s.KPIMasterFk.StrategicPlanFk)
                .Where(e => e.ApprovalStatus == 5)
                .Where(s => (fy != null ? s.FY == fy : true) ).Skip(offset).Take(pageSize);
            var result = new PagedResult<KPIAssessment>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.KPIAssessment.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var fyear = ListHelper.FYear();
            if (fy != null)
            {
                var selectFyear = fyear.Where(x => (x.Value == fy.ToString() || x.Value == null)).First();
                selectFyear.Selected = true;
            }
            ViewData["FYear"] = ListHelper.FYear();
            ViewData["AchievementStatus"] = ListHelper.AchievementStatus();
            ViewData["FrequencyofReporting"] = ListHelper.FrequencyofReporting();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(result);
        }
        public async Task<IActionResult> VerifyDetailsHBPD(int? id)
        {
            if (id == null || _context.KPIAssessment == null)
            {
                return NotFound();
            }

            var assessment = await _context.KPIAssessment
                .Include(s => s.KPIMasterFk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assessment == null)
            {
                return NotFound();
            }
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(assessment);
        }
        public async Task<IActionResult> ChangeStatusHBPD(KPIAssessment dto)
        {
            if (dto.Id != null && dto.Id != 0)
            {
                try
                {
                    var pp = await _context.KPIAssessment.FindAsync(dto.Id);
                    if (pp == null)
                    {
                        return NotFound();
                    }
                    if (dto.ApprovalStatus == 1)
                    {
                        pp.ApprovalStatus = 7;
                    }
                    else
                    {
                        pp.ApprovalStatus = 8;
                    }
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
        public async Task<IActionResult> VerifyByDDCS(int pageNumber = 1, int? fy = null, string? deptCode = null)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var appDbContext = _context.KPIAssessment.Include(s => s.KPIMasterFk).Include(s => s.KPIMasterFk.StrategicPlanFk)
                .Where(e => e.ApprovalStatus == 7)
                .Where(s => (fy != null ? s.FY == fy : true)).Skip(offset).Take(pageSize);
            var result = new PagedResult<KPIAssessment>
            {
                Data = await appDbContext.AsNoTracking().ToListAsync(),
                TotalItems = _context.KPIAssessment.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var fyear = ListHelper.FYear();
            if (fy != null)
            {
                var selectFyear = fyear.Where(x => (x.Value == fy.ToString() || x.Value == null)).First();
                selectFyear.Selected = true;
            }
            ViewData["FYear"] = ListHelper.FYear();
            ViewData["AchievementStatus"] = ListHelper.AchievementStatus();
            ViewData["FrequencyofReporting"] = ListHelper.FrequencyofReporting();
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(result);
        }
        public async Task<IActionResult> VerifyDetailsByDDCS(int? id)
        {
            if (id == null || _context.KPIAssessment == null)
            {
                return NotFound();
            }

            var assessment = await _context.KPIAssessment
                .Include(s => s.KPIMasterFk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assessment == null)
            {
                return NotFound();
            }
            ViewData["Approval"] = ListHelper.ApprovalStatus();
            return View(assessment);
        }
        public async Task<IActionResult> ChangeStatusByDDCS(KPIAssessment dto)
        {
            if (dto.Id != null && dto.Id != 0)
            {
                try
                {
                    var pp = await _context.KPIAssessment.FindAsync(dto.Id);
                    if (pp == null)
                    {
                        return NotFound();
                    }
                    if (dto.ApprovalStatus == 1)
                    {
                        pp.ApprovalStatus = 9;
                    }
                    else
                    {
                        pp.ApprovalStatus = 10;
                    }
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
