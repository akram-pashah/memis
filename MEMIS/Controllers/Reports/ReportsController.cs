using MEMIS.Data;
using MEMIS.Helpers.ExcelReports;
using MEMIS.Models.Report;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Numerics;

namespace MEMIS.Controllers.Reports
{
  public class ReportsController : Controller
  {
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
      _context = context;
    }

    public IActionResult Index()
    {
      return View();
    }

    public async Task<IActionResult> planningIndex()
    {
      var list = await _context.ProgramImplementationPlan.ToListAsync();
      return View(list);
    }

    public async Task<IActionResult> ExportToExcel()
    {
      try
      {
        var list = await _context.ProgramImplementationPlan.ToListAsync();
        var stream = ExportHandler.StrategicImplementationPlanReport(list);
        stream.Position = 0;
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Strategic Implementation Plan.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public async Task<IActionResult> ConsolDeptPlan(Guid? selectedDeptId, string? quarter)
    {
      try
      {
        ViewBag.Departments = new SelectList(await _context.Departments.ToListAsync(), "intDept", "deptName", selectedDeptId);

        var list = _context.ActivityAssess.Include(m => m.StrategicAction).Include(m => m.StrategicIntervention).ThenInclude(x => x.StrategicObjective).Include(m => m.ActivityFk).Include(x => x.QuaterlyPlans).AsQueryable();

        if (selectedDeptId.HasValue)
        {
          list = list.Where(a => a.intDept == selectedDeptId.Value);
        }
        if (!string.IsNullOrEmpty(quarter))
        {
          list = list.Where(x => x.QuaterlyPlans.Where(x => x.Quarter == quarter).Any());
        }

        ViewData["SelectedDeptId"] = selectedDeptId;
        ViewData["SelectedQuarter"] = quarter;

        return View(await list.ToListAsync());
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    [HttpGet]
    public IActionResult GetActivityAssessDetails(int id)
    {
      var activityAssess = _context.ActivityAssess.Include(m => m.StrategicAction).Include(m => m.StrategicIntervention).ThenInclude(x => x.StrategicObjective).Include(m => m.ActivityFk).Include(x => x.QuaterlyPlans).Include(x => x.DepartmentFk).FirstOrDefault(x => x.intAssess == id);
      if (activityAssess == null)
      {
        return NotFound();
      }

      return PartialView("_ActivityAssessDetails", activityAssess);
    }
    public async Task<IActionResult> ConsolidatedExportToExcel(Guid? selectedDeptId, string? quarter)
    {
      try
      {
        var list = _context.ActivityAssess.Include(m => m.StrategicAction).Include(m => m.StrategicIntervention).ThenInclude(x => x.StrategicObjective).Include(m => m.ActivityFk).Include(x => x.QuaterlyPlans).AsQueryable();

        if (selectedDeptId.HasValue)
        {
          list = list.Where(a => a.intDept == selectedDeptId.Value);
        }
        if (!string.IsNullOrEmpty(quarter))
        {
          list = list.Where(x => x.QuaterlyPlans.Where(x => x.Quarter == quarter).Any());
        }

        var stream = ExportHandler.AnnualDetailedResultsFrameworkReport(await list.ToListAsync());
        stream.Position = 0;
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Annual Detailed Results Framework.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    //, string? quarter
    public async Task<IActionResult> ActivityImplementationStatus(Guid? selectedDeptId)
    {
      try
      {
        ViewBag.Departments = new SelectList(await _context.Departments.ToListAsync(), "intDept", "deptName", selectedDeptId);

        var list = _context.ActivityAssessment.Include(m => m.ImplementationStatus)
          .Include(m => m.QuaterlyPlans)
          .ThenInclude(x => x.ActivityAssess)
          .ThenInclude(m => m.DepartmentFk).AsQueryable();

        if (selectedDeptId.HasValue)
        {
          list = list.Where(a => a.QuaterlyPlans.Any() && a.QuaterlyPlans.FirstOrDefault().ActivityAssess != null && a.QuaterlyPlans.FirstOrDefault().ActivityAssess.DepartmentFk != null && a.QuaterlyPlans.FirstOrDefault().ActivityAssess.DepartmentFk.intDept == selectedDeptId.Value);
        }
        //if (!string.IsNullOrEmpty(quarter))
        //{
        //  list = list.Where(x => x.QuaterlyPlans.Where(x => x.Quarter == quarter).Any());
        //}

        ViewData["SelectedDeptId"] = selectedDeptId;
        //ViewData["SelectedQuarter"] = quarter;

        return View(await list.ToListAsync());
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    [HttpGet]
    public IActionResult GetActivityImplementationDetails(int id)
    {
      var activityAssess = _context.ActivityAssessment.Include(x => x.QuaterlyPlans).ThenInclude(x => x.ActivityAssess).ThenInclude(x => x.DepartmentFk).FirstOrDefault(x => x.intDeptPlan == id);
      if (activityAssess == null)
      {
        return NotFound();
      }

      return PartialView("_ActivityImplementationDetails", activityAssess);
    }
    public async Task<IActionResult> ActivityImplementationStatusExportToExcel(Guid? selectedDeptId, string? quarter)
    {
      try
      {
        var list = _context.ActivityAssessment.Include(x => x.QuaterlyPlans).ThenInclude(x => x.ActivityAssess).ThenInclude(x => x.DepartmentFk).AsQueryable();

        if (selectedDeptId.HasValue)
        {
          list = list.Where(a => a.QuaterlyPlans.Any() && a.QuaterlyPlans.FirstOrDefault().ActivityAssess.intDept == selectedDeptId.Value);
        }
        if (!string.IsNullOrEmpty(quarter))
        {
          list = list.Where(x => x.QuaterlyPlans.Where(x => x.Quarter == quarter).Any());
        }

        var stream = ExportHandler.ActivityImplementationStatusExport(await list.ToListAsync());
        stream.Position = 0;
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Activity Implementation Status Report.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public async Task<IActionResult> StrategicPlanActivityImplementationTracker()
    {
      try
      {
        List<StrategicObjectiveReport> list = await GetStrategicPlanReportAsync();

        return View(list);
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    [HttpGet]
    public IActionResult GetStrategicPlanActivityImplementationTrackerDetails(int id)
    {
      var activityAssess = _context.ActivityAssessment.Include(x => x.QuaterlyPlans).ThenInclude(x => x.ActivityAssess).ThenInclude(x => x.DepartmentFk).FirstOrDefault(x => x.intDeptPlan == id);
      if (activityAssess == null)
      {
        return NotFound();
      }

      return PartialView("_ActivityImplementationDetails", activityAssess);
    }

    public async Task<IActionResult> StrategicPlanActivityImplementationTrackerExportToExcel(Guid? selectedDeptId, string? quarter)
    {
      try
      {
        List<StrategicObjectiveReport> list = await GetStrategicPlanReportAsync();

        var stream = ExportHandler.StrategicPlanActivityImplementationTrackerExport(list);
        stream.Position = 0;
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Activity Implementation Tracker.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public async Task<List<StrategicObjectiveReport>> GetStrategicPlanReportAsync()
    {
      var currentYear = DateTime.Now.Year;
      var years = new[] { currentYear, currentYear - 1, currentYear - 2, currentYear - 3, currentYear - 4 };

      var data = await _context.ActivityAssessment
          .Include(a => a.ImplementationStatus)
          .ToListAsync();

      if (data == null || !data.Any())
      {
        return new List<StrategicObjectiveReport>(); // Return an empty list if no data is found
      }

      //var groupedData = await _context.ActivityAssessment
      //      .Include(a => a.ImplementationStatus)
      //      .GroupBy(a => a.strategicObjective)
      //      .ToDictionaryAsync(
      //          g => g.Key,
      //          g => g
      //              .GroupBy(a => a.strategicIntervention)
      //              .ToDictionary(
      //                  ig => ig.Key,
      //                  ig => ig
      //                      .GroupBy(a => a.StrategicAction)
      //                      .ToDictionary(
      //                          ag => ag.Key,
      //                          ag => new double[]
      //                          {
      //                              CalculateAverageImplementationStatus(ag, years[0]),
      //                              CalculateAverageImplementationStatus(ag, years[1]),
      //                              CalculateAverageImplementationStatus(ag, years[2]),
      //                              CalculateAverageImplementationStatus(ag, years[3]),
      //                              CalculateAverageImplementationStatus(ag, years[4])
      //                          }
      //                      )
      //              )
      //      );

      var report = data
          .GroupBy(a => a.strategicObjective)
          .Select(g => new StrategicObjectiveReport
          {
            StrategicObjective = g.Key,
            StrategicInterventions = g
                  .GroupBy(a => a.strategicIntervention)
                  .Select(ig => new StrategicInterventionReport
                  {
                    StrategicIntervention = ig.Key,
                    StrategicActions = ig
                          .GroupBy(a => a.StrategicAction)
                          .Select(ag => new StrategicActionReport
                          {
                            StrategicAction = ag.Key,
                            FiscalYearData = years.Select(year => CalculateAverageImplementationStatus(ag, year)).ToArray()
                          }).ToList()
                  }).ToList()
          }).ToList();

      return report;
    }

    private double CalculateAverageImplementationStatus(IEnumerable<ActivityAssessment> assessments, int year)
    {
      var filteredAssessments = assessments.Where(a => a.Fyear == year);
      if (!filteredAssessments.Any())
      {
        return 0;
      }
      return filteredAssessments.Average(a => a.ImplementationStatus.ImpStatusName == "Fully Implemented" ? 1 : a.ImplementationStatus.ImpStatusName == "Partially Implemented" ? 0.5 : 0);
    }

    public async Task<IActionResult> StrategicPlanOutputMonitoringTracker()
    {
      try
      {
        List<StrategicObjectiveReport> list = await GetStrategicPlanOutputReportAsync();

        return View(list);
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    [HttpGet]
    public IActionResult GetStrategicPlanOutputMonitoringTrackerDetails(int id)
    {
      var activityAssess = _context.KPIAssessment.Include(x => x.KPIMasterFk).ThenInclude(x => x.StrategicPlanFk).FirstOrDefault(x => x.Id == id);
      if (activityAssess == null)
      {
        return NotFound();
      }

      return PartialView("_AchievementImplementationDetails", activityAssess);
    }

    public async Task<IActionResult> StrategicPlanOutputMonitoringTrackerExportToExcel(Guid? selectedDeptId, string? quarter)
    {
      try
      {
        List<StrategicObjectiveReport> list = await GetStrategicPlanOutputReportAsync();

        var stream = ExportHandler.StrategicPlanOutputMonitoringTrackerExport(list);
        stream.Position = 0;
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Strategic Plan Output Monitoring Tracker.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }


    public async Task<List<StrategicObjectiveReport>> GetStrategicPlanOutputReportAsync()
    {
      var currentYear = DateTime.Now.Year;
      var years = new[] { currentYear, currentYear - 1, currentYear - 2, currentYear - 3, currentYear - 4 };

      var data = await _context.KPIAssessment
          .Include(a => a.KPIMasterFk)
          .ThenInclude(x => x.StrategicPlanFk)
          .ToListAsync();

      if (data == null || !data.Any())
      {
        return new List<StrategicObjectiveReport>(); // Return an empty list if no data is found
      }

      //var groupedData = await _context.ActivityAssessment
      //      .Include(a => a.ImplementationStatus)
      //      .GroupBy(a => a.strategicObjective)
      //      .ToDictionaryAsync(
      //          g => g.Key,
      //          g => g
      //              .GroupBy(a => a.strategicIntervention)
      //              .ToDictionary(
      //                  ig => ig.Key,
      //                  ig => ig
      //                      .GroupBy(a => a.StrategicAction)
      //                      .ToDictionary(
      //                          ag => ag.Key,
      //                          ag => new double[]
      //                          {
      //                              CalculateAverageImplementationStatus(ag, years[0]),
      //                              CalculateAverageImplementationStatus(ag, years[1]),
      //                              CalculateAverageImplementationStatus(ag, years[2]),
      //                              CalculateAverageImplementationStatus(ag, years[3]),
      //                              CalculateAverageImplementationStatus(ag, years[4])
      //                          }
      //                      )
      //              )
      //      );

      var report = data
          .GroupBy(a => a.KPIMasterFk.StrategicPlanFk.strategicObjective)
          .Select(g => new StrategicObjectiveReport
          {
            StrategicObjective = g.Key,
            StrategicInterventions = g
                  .GroupBy(a => a.KPIMasterFk.StrategicPlanFk.strategicIntervention)
                  .Select(ig => new StrategicInterventionReport
                  {
                    StrategicIntervention = ig.Key,
                    StrategicActions = ig
                          .GroupBy(a => a.KPIMasterFk.StrategicPlanFk.StrategicAction)
                          .Select(ag => new StrategicActionReport
                          {
                            StrategicAction = ag.Key,
                            FiscalYearData = years.Select(year => CalculateAverageAchievementStatus(ag, year)).ToArray()
                          }).ToList()
                  }).ToList()
          }).ToList();

      return report;
    }

    private double CalculateAverageAchievementStatus(IEnumerable<KPIAssessment> assessments, int year)
    {
      var filteredAssessments = assessments.Where(a => a.FY == year);
      if (!filteredAssessments.Any())
      {
        return 0;
      }
      return filteredAssessments.Average(a => !string.IsNullOrEmpty(a.Achieved) && double.Parse(a.Achieved) == 1 ? 1 : !string.IsNullOrEmpty(a.Achieved) && double.Parse(a.Achieved) == 0.5 ? 0.5 : !string.IsNullOrEmpty(a.Achieved) && double.Parse(a.Achieved) == 0.25 ? 0.25 : 0);
    }


    public async Task<IActionResult> MandEIndex()
    {

      var list = await _context.SDTAssessment.Include(s => s.SDTMasterFk).Include(s => s.SDTMasterFk.DepartmentFk).ToListAsync();
      return View(list);
    }

    public async Task<IActionResult> SDTExportToExcel()
    {
      try
      {
        var list = await _context.SDTAssessment.Include(s => s.SDTMasterFk).Include(s => s.SDTMasterFk.DepartmentFk).ToListAsync();
        var stream = ExportHandler.SDTQuarterlyPerformanceReport(list);
        stream.Position = 0;
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SDT Quarterly Performances.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public async Task<IActionResult> KpiAssessment()
    {
      var list = await _context.KPIAssessment.ToListAsync();

      return View(list);
    }

    public async Task<IActionResult> KPIExportToExcel()
    {
      try
      {
        var list = await _context.KPIAssessment.ToListAsync();
        var stream = ExportHandler.KPIMandEReport(list);
        stream.Position = 0;
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "KPI M&E.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public async Task<IActionResult> KPIMandEFramework()
    {
      var list = await _context.KPIMasters.ToListAsync();

      return View(list);
    }

    public async Task<IActionResult> KPIMandEFrameworkReportExcel()
    {
      try
      {
        var list = await _context.KPIMasters.ToListAsync();
        var stream = ExportHandler.KPIMandEFrameworkReport(list);
        stream.Position = 0;
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "KPI M&E Framework Report.xlsx");
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    //public async Task<IActionResult> StrategicPlanOutputMonitoringTracker()
    //{
    //  var list = await _context.KPIAssessment.Include(x => x.KPIMasterFk).ToListAsync();

    //  return View(list);
    //}

    //public async Task<IActionResult> StrategicPlanOutputMonitoringTrackerExcel()
    //{
    //  try
    //  {
    //    var list = await _context.KPIMasters.ToListAsync();
    //    var stream = ExportHandler.KPIMandEFrameworkReport(list);
    //    stream.Position = 0;
    //    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "KPI M&E Framework Report.xlsx");
    //  }
    //  catch (Exception ex)
    //  {

    //    throw;
    //  }
    //}

    public async Task<IActionResult> OpTargetPerfAchievReport()
    {
      List<ActivityAssessment> list = await _context.ActivityAssessment
        .Include(x => x.ImplementationStatus)
        .Include(x => x.DepartmentFk)
        .ToListAsync();

      return View(list);
    }

    public async Task<IActionResult> OpTargetPerfAchievReportExcel()
    {
      List<ActivityAssessment> list = await _context.ActivityAssessment
        .Include(x => x.ImplementationStatus)
        .Include(x => x.DepartmentFk)
        .ToListAsync();

      var stream = ExportHandler.OpTargetPerfAcheivReport(list);
      stream.Position = 0;
      return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Output/Target Performance Achievement Report.xlsx");
    }

    public async Task<IActionResult> StrategicActionPerfAchievReport()
    {
      List<StrategicActionPerformanceAchievementDto> list = await _context.ActivityAssessment
        .Include(x => x.ImplementationStatus)
        .GroupBy(x => x.StrategicAction)
        .Select(x => new StrategicActionPerformanceAchievementDto()
        {
          StrategicAction = x.Key,
          PerformanceAchievementStatus = x.Average(a =>
                a.ImplementationStatus != null
                ? a.ImplementationStatus.ImpStatusName == "Fully Implemented" ? 1
                : a.ImplementationStatus.ImpStatusName == "Partially Implemented" ? 0.5
                : 0
                : 0
            ) * 100
        })
        .ToListAsync();

      return View(list);
    }

    public async Task<IActionResult> StrategicActionPerfAchievReportExcel()
    {
      List<StrategicActionPerformanceAchievementDto> list = await _context.ActivityAssessment
        .Include(x => x.ImplementationStatus)
        .GroupBy(x => x.StrategicAction)
        .Select(x => new StrategicActionPerformanceAchievementDto()
        {
          StrategicAction = x.Key,
          PerformanceAchievementStatus = x.Average(a =>
                a.ImplementationStatus != null
                ? a.ImplementationStatus.ImpStatusName == "Fully Implemented" ? 1
                : a.ImplementationStatus.ImpStatusName == "Partially Implemented" ? 0.5
                : 0
                : 0
            ) * 100
        })
        .ToListAsync();

      var stream = ExportHandler.StrategicActionPerfAchievReport(list);
      stream.Position = 0;
      return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Strategic Action Performance Achievement.xlsx");
    }

    public async Task<IActionResult> StrategicInterventionPerfAchievReport()
    {
      List<StrategicInterventionPerformanceAchievementDto> list = await _context.ActivityAssessment
        .Include(x => x.ImplementationStatus)
        .GroupBy(x => x.strategicIntervention)
        .Select(x => new StrategicInterventionPerformanceAchievementDto()
        {
          StrategicIntervention = x.Key,
          PerformanceAchievementStatus = x.Average(a =>
                a.ImplementationStatus != null
                ? a.ImplementationStatus.ImpStatusName == "Fully Implemented" ? 1
                : a.ImplementationStatus.ImpStatusName == "Partially Implemented" ? 0.5
                : 0
                : 0
            ) * 100
        })
        .ToListAsync();

      return View(list);
    }

    public async Task<IActionResult> StrategicInterventionPerfAchievReportExcel()
    {
      List<StrategicInterventionPerformanceAchievementDto> list = await _context.ActivityAssessment
        .Include(x => x.ImplementationStatus)
        .GroupBy(x => x.strategicIntervention)
        .Select(x => new StrategicInterventionPerformanceAchievementDto()
        {
          StrategicIntervention = x.Key,
          PerformanceAchievementStatus = x.Average(a =>
                a.ImplementationStatus != null
                ? a.ImplementationStatus.ImpStatusName == "Fully Implemented" ? 1
                : a.ImplementationStatus.ImpStatusName == "Partially Implemented" ? 0.5
                : 0
                : 0
            ) * 100
        })
        .ToListAsync();

      var stream = ExportHandler.StrategicInterventionPerfAchievReport(list);
      stream.Position = 0;
      return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Strategic Intervention Performance Achievement.xlsx");
    }

    public async Task<IActionResult> StrategicObjectivePerfAchievReport()
    {
      List<StrategicObjectivePerformanceAchievementDto> list = await _context.ActivityAssessment
        .Include(x => x.ImplementationStatus)
        .GroupBy(x => x.strategicObjective)
        .Select(x => new StrategicObjectivePerformanceAchievementDto()
        {
          StrategicObjective = x.Key,
          PerformanceAchievementStatus = x.Average(a =>
                a.ImplementationStatus != null
                ? a.ImplementationStatus.ImpStatusName == "Fully Implemented" ? 1
                : a.ImplementationStatus.ImpStatusName == "Partially Implemented" ? 0.5
                : 0
                : 0
            ) * 100
        })
        .ToListAsync();

      return View(list);
    }

    public async Task<IActionResult> StrategicObjectivePerfAchievReportExcel()
    {
      List<StrategicObjectivePerformanceAchievementDto> list = await _context.ActivityAssessment
        .Include(x => x.ImplementationStatus)
        .GroupBy(x => x.strategicObjective)
        .Select(x => new StrategicObjectivePerformanceAchievementDto()
        {
          StrategicObjective = x.Key,
          PerformanceAchievementStatus = x.Average(a =>
                a.ImplementationStatus != null
                ? a.ImplementationStatus.ImpStatusName == "Fully Implemented" ? 1
                : a.ImplementationStatus.ImpStatusName == "Partially Implemented" ? 0.5
                : 0
                : 0
            ) * 100
        })
        .ToListAsync();

      var stream = ExportHandler.StrategicObjectivePerfAchievReport(list);
      stream.Position = 0;
      return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Strategic Intervention Performance Achievement.xlsx");
    }

    public async Task<IActionResult> OutcomePerformanceAchievementReport()
    {
      List<ActivityAssessment> list = await _context.ActivityAssessment
        .Include(x => x.ImplementationStatus)
        .Include(x => x.DepartmentFk)
        .ToListAsync();

      return View(list);
    }

    public async Task<IActionResult> OutcomePerformanceAchievementReportExcel()
    {
      List<ActivityAssessment> list = await _context.ActivityAssessment
       .Include(x => x.ImplementationStatus)
       .Include(x => x.DepartmentFk)
       .ToListAsync();

      var stream = ExportHandler.OutcomePerfAchievReport(list);
      stream.Position = 0;
      return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Outcome Performance Achievement Report.xlsx");
    }

    public async Task<IActionResult> ImpactPerformanceAchievementReport()
    {
      List<ActivityAssessment> list = await _context.ActivityAssessment
        .Include(x => x.ImplementationStatus)
        .Include(x => x.DepartmentFk)
        .ToListAsync();

      return View(list);
    }

    public async Task<IActionResult> ImpactPerformanceAchievementReportExcel()
    {
      List<ActivityAssessment> list = await _context.ActivityAssessment
       .Include(x => x.ImplementationStatus)
       .Include(x => x.DepartmentFk)
       .ToListAsync();

      var stream = ExportHandler.ImpactPerfAchievReport(list);
      stream.Position = 0;
      return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Impact Performance Achievement Report.xlsx");
    }

    public async Task<IActionResult> SDTMAndEFrameworkReport()
    {
      List<SDTMaster> list = await _context.SDTMasters
      .Include(x => x.DepartmentFk)
      .ToListAsync();

      return View(list);
    }

    public async Task<IActionResult> SDTMAndEFrameworkReportExcel()
    {
      List<SDTMaster> list = await _context.SDTMasters
       .Include(x => x.DepartmentFk)
       .ToListAsync();

      var stream = ExportHandler.SdtMAndEFrameworkReport(list);
      stream.Position = 0;
      return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SDT M&E Framework Report.xlsx");
    }

  }
}
