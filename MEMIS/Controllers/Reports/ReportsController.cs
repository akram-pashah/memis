using MEMIS.Data;
using MEMIS.Helpers.ExcelReports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    public IActionResult planningIndex()
    {
      return View();
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
    public async Task<IActionResult> ConsolidatedExportToExcel()
    {
      try
      {
        var list = await _context.ActivityAssess.Include(m => m.StrategicAction).Include(m => m.StrategicIntervention).Include(m => m.ActivityFk).ToListAsync();
        var stream = ExportHandler.AnnualDetailedResultsFrameworkReport(list);
        stream.Position = 0;
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Annual Detailed Results Framework.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public IActionResult MandEIndex()
    {
      return View();
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

    public async Task<IActionResult> KPIExportToExcel()
    {
      try
      {
        var list = await _context.KPIAssessment.Include(s => s.KPIMasterFk).ToListAsync();
        var stream = ExportHandler.KPIMandEReport(list);
        stream.Position = 0;
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "KPI M&E.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }
  }
}
