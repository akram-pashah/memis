using MEMIS.Data;
using MEMIS.Helpers.ExcelReports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MEMIS.Controllers.Reports
{
  public class ProjectManagementReportsController : Controller
  {
    private readonly AppDbContext _context;
    public ProjectManagementReportsController(AppDbContext context)
    {
      _context = context;
    }

    public async Task<IActionResult> Index()
    {
      var list = await _context.ProjectInitiations.Include(x => x.Department).ToListAsync();
      return View(list);
    }

    public async Task<IActionResult> ProjectListReportExcel()
    {
      try
      {
        var list = await _context.ProjectInitiations.Include(x => x.Department).ToListAsync();
        var stream = ExportHandler.ProjectListReport(list);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Project List Report.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public async Task<IActionResult> ProjectActivitySchedule()
    {
      var list = await _context.ActivityPlans.ToListAsync();
      return View(list);
    }

    public async Task<IActionResult> ProjectActivityScheduleExcel()
    {
      try
      {
        var list = await _context.ActivityPlans.ToListAsync();
        var stream = ExportHandler.ProjectActivityScheduleReport(list);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Project Activity Schedule Report.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public async Task<IActionResult> ProjectRiskManagementReport()
    {
      var list = await _context.ProjectRiskIdentifications.ToListAsync();
      return View(list);
    }

    public async Task<IActionResult> ProjectRiskManagementReportExcel()
    {
      try
      {
        var list = await _context.ProjectRiskIdentifications.ToListAsync();
        var stream = ExportHandler.ProjectRiskManagementReport(list);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Project Risk Management Report.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public async Task<IActionResult> StakeholderManagementReport()
    {
      var list = await _context.StakeHolder.ToListAsync();
      return View(list);
    }

    public async Task<IActionResult> StakeholderManagementReportExcel()
    {
      try
      {
        var list = await _context.StakeHolder.ToListAsync();
        var stream = ExportHandler.StakeholderManagementReport(list);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Stake Holder Management Report.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }
  }
}
