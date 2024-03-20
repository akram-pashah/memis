using cloudscribe.Pagination.Models;
using MEMIS.Data;
using MEMIS.Models.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Controllers.Reports
{
    public class SDTReportController : Controller
    {
        private readonly Data.AppDbContext _context;

        public SDTReportController(Data.AppDbContext context)
        {
            _context = context;
        }

        // GET: SDTReportController
        public ActionResult Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var offset = (pageSize * pageNumber) - pageSize;
            var model = GetData();
            model.Skip(offset).Take(pageSize);
            var result = new PagedResult<SDTReportModel>
            {
                Data = model,
                TotalItems = model.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            
            return View(result);
        }

        private List<SDTReportModel> GetData()
        {
            var dt = _context.SDTAssessment.GroupBy(x => x.SDTMasterId)
                     .Select(r => new
                     {
                         SDTMasterId = r.Key,
                         Jul = r.Where(x => x.Month == 7).Sum(x => x.Rate),
                         Aug = r.Where(x => x.Month == 8).Sum(x => x.Rate),
                         Sep = r.Where(x => x.Month == 9).Sum(x => x.Rate),
                         Oct = r.Where(x => x.Month == 10).Sum(x => x.Rate),
                         Nov = r.Where(x => x.Month == 11).Sum(x => x.Rate),
                         Dec = r.Where(x => x.Month == 12).Sum(x => x.Rate),
                         Jan = r.Where(x => x.Month == 1).Sum(x => x.Rate),
                         Feb = r.Where(x => x.Month == 2).Sum(x => x.Rate),
                         Mar = r.Where(x => x.Month == 3).Sum(x => x.Rate),
                         Apr = r.Where(x => x.Month == 4).Sum(x => x.Rate),
                         May = r.Where(x => x.Month == 5).Sum(x => x.Rate),
                         Jun = r.Where(x => x.Month == 6).Sum(x => x.Rate),
                         Target = r.Where(x => x.Target > 0).Average(x => x.Target),
                         AverageQ1 = (r.Where(x => x.Month == 7).Sum(x => x.Rate) + r.Where(x => x.Month == 8).Sum(x => x.Rate) + r.Where(x => x.Month == 9).Sum(x => x.Rate)) / 3,
                         AverageQ2 = (r.Where(x => x.Month == 10).Sum(x => x.Rate) + r.Where(x => x.Month == 11).Sum(x => x.Rate) + r.Where(x => x.Month == 12).Sum(x => x.Rate)) / 3,
                         AverageQ3 = (r.Where(x => x.Month == 1).Sum(x => x.Rate) + r.Where(x => x.Month == 2).Sum(x => x.Rate) + r.Where(x => x.Month == 3).Sum(x => x.Rate)) / 3,
                         AverageQ4 = (r.Where(x => x.Month == 4).Sum(x => x.Rate) + r.Where(x => x.Month == 5).Sum(x => x.Rate) + r.Where(x => x.Month == 6).Sum(x => x.Rate)) / 3,
                         AverageAnnual = (r.Where(x => x.Month == 7).Sum(x => x.Rate) + r.Where(x => x.Month == 8).Sum(x => x.Rate) + r.Where(x => x.Month == 9).Sum(x => x.Rate) +
                                          r.Where(x => x.Month == 10).Sum(x => x.Rate) + r.Where(x => x.Month == 11).Sum(x => x.Rate) + r.Where(x => x.Month == 12).Sum(x => x.Rate) +
                                          r.Where(x => x.Month == 1).Sum(x => x.Rate) + r.Where(x => x.Month == 2).Sum(x => x.Rate) + r.Where(x => x.Month == 3).Sum(x => x.Rate) +
                                          r.Where(x => x.Month == 4).Sum(x => x.Rate) + r.Where(x => x.Month == 5).Sum(x => x.Rate) + r.Where(x => x.Month == 6).Sum(x => x.Rate)) / 12,
                         AchievedJul = r.Where(x => x.Month == 7).Sum(x => Convert.ToDecimal(x.AchivementStatus)),
                         AchievedAug = r.Where(x => x.Month == 8).Sum(x => Convert.ToDecimal(x.AchivementStatus)),
                         AchievedSep = r.Where(x => x.Month == 9).Sum(x => Convert.ToDecimal(x.AchivementStatus)),
                         AchievedOct = r.Where(x => x.Month == 10).Sum(x => Convert.ToDecimal(x.AchivementStatus)),
                         AchievedNov = r.Where(x => x.Month == 11).Sum(x => Convert.ToDecimal(x.AchivementStatus)),
                         AchievedDec = r.Where(x => x.Month == 12).Sum(x => Convert.ToDecimal(x.AchivementStatus)),
                         AchievedJan = r.Where(x => x.Month == 1).Sum(x => Convert.ToDecimal(x.AchivementStatus)),
                         AchievedFeb = r.Where(x => x.Month == 2).Sum(x => Convert.ToDecimal(x.AchivementStatus)),
                         AchievedMar = r.Where(x => x.Month == 3).Sum(x => Convert.ToDecimal(x.AchivementStatus)),
                         AchievedApr = r.Where(x => x.Month == 4).Sum(x => Convert.ToDecimal(x.AchivementStatus)),
                         AchievedMay = r.Where(x => x.Month == 5).Sum(x => Convert.ToDecimal(x.AchivementStatus)),
                         AchievedJun = r.Where(x => x.Month == 6).Sum(x => Convert.ToDecimal(x.AchivementStatus)),
                     }).AsQueryable();

            var dat = from sa in dt
                      join sm in _context.SDTMasters on sa.SDTMasterId equals sm.Id
                      select new SDTReportModel
                      {
                          ServiceDeliveryTimeline = sm.ServiceDeliveryTimeline,
                          Measure = sm.Measure,
                          ReportingInterval = sm.EvaluationPeriod,
                          Jul = sa.Jul,
                          Aug = sa.Aug,
                          Sep = sa.Sep,
                          Oct = sa.Oct,
                          Nov = sa.Nov,
                          Dec = sa.Dec,
                          Jan = sa.Jan,
                          Feb = sa.Feb,
                          Mar = sa.Mar,
                          Apr = sa.Apr,
                          May = sa.May,
                          Jun = sa.Jun,
                          Target = sa.Target,
                          AvgQ1 = sa.AverageQ1,
                          AvgQ2 = sa.AverageQ2,
                          AvgQ3 = sa.AverageQ3,
                          AvgQ4 = sa.AverageQ4,
                          AnnualAverage = sa.AverageAnnual,
                          AchievedJul=sa.AchievedJul,
                          AchievedAug = sa.AchievedAug,
                          AchievedSep = sa.AchievedSep,
                          AchievedOct = sa.AchievedOct,
                          AchievedNov = sa.AchievedNov,
                          AchievedDec = sa.AchievedDec,
                          AchievedJan = sa.AchievedJan,
                          AchievedFeb = sa.AchievedFeb,
                          AchievedMar = sa.AchievedMar,
                          AchievedApr = sa.AchievedApr,
                          AchievedMay = sa.AchievedMay,
                          AchievedJun = sa.AchievedJun,
                      };

            return dat.ToList();
        }











        // GET: SDTReportController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SDTReportController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SDTReportController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SDTReportController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SDTReportController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SDTReportController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SDTReportController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
