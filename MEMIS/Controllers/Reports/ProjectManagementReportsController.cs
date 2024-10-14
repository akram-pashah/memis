using DocumentFormat.OpenXml.Office2010.Excel;
using MEMIS.Data;
using MEMIS.Helpers.ExcelReports;
using MEMIS.Helpers.PdfReports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public async Task<IActionResult> ProjectListReportPdf()
    {
      try
      {
        var list = await _context.ProjectInitiations.Include(x => x.Department).ToListAsync();
        var stream = PdfHandler.ProjectListReportPdf(list);
        return File(stream, "application/pdf", "Project List Report.pdf");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public async Task<IActionResult> ProjectActivitySchedule(int? ProjectInitiationId)
    {
      var query = _context.ActivityPlans.AsQueryable();
      if (ProjectInitiationId != null && ProjectInitiationId > 0)
      {
        query = query.Where(x => x.ProjectInitiationId == ProjectInitiationId);
      }
      var list = await query.ToListAsync();
      ViewBag.SelectedProjectInitiationId = ProjectInitiationId ?? 0;
      ViewData["ProjectInitiations"] = new SelectList(_context.ProjectInitiations.OrderBy(d => d.Name), "Id", "Name");
      return View(list);
    }

    public async Task<IActionResult> ProjectActivityScheduleExcel(int? ProjectInitiationId)
    {
      try
      {
        var query = _context.ActivityPlans.AsQueryable();
        if (ProjectInitiationId != null && ProjectInitiationId > 0)
        {
          query = query.Where(x => x.ProjectInitiationId == ProjectInitiationId);
        }
        var list = await query.ToListAsync();
        var stream = ExportHandler.ProjectActivityScheduleReport(list);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Project Activity Schedule Report.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }
    public async Task<IActionResult> ProjectActivitySchedulePdf(int? ProjectInitiationId)
    {
      try
      {
        var query = _context.ActivityPlans.AsQueryable();
        if (ProjectInitiationId != null && ProjectInitiationId > 0)
        {
          query = query.Where(x => x.ProjectInitiationId == ProjectInitiationId);
        }
        var list = await query.ToListAsync();
        var stream = PdfHandler.ProjectActivityScheduleReportPdf(list);
        return File(stream, "application/pdf", "Project Activity Schedule Report.pdf");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public async Task<IActionResult> ProjectRiskManagementReport(int? ProjectInitiationId)
    {
      var query = _context.ProjectRiskIdentifications.AsQueryable();
      if (ProjectInitiationId != null && ProjectInitiationId > 0)
      {
        query = query.Where(x => x.ProjectInitiationId == ProjectInitiationId);
      }

      var list = await query.ToListAsync();
      ViewBag.SelectedProjectInitiationId = ProjectInitiationId ?? 0;
      ViewData["ProjectInitiations"] = new SelectList(_context.ProjectInitiations.OrderBy(d => d.Name), "Id", "Name");
      return View(list);
    }

    public async Task<IActionResult> ProjectRiskManagementReportExcel(int? ProjectInitiationId)
    {
      try
      {
        var query = _context.ProjectRiskIdentifications.AsQueryable();
        if (ProjectInitiationId != null && ProjectInitiationId > 0)
        {
          query = query.Where(x => x.ProjectInitiationId == ProjectInitiationId);
        }

        var list = await query.ToListAsync();
        var stream = ExportHandler.ProjectRiskManagementReport(list);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Project Risk Management Report.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }
    public async Task<IActionResult> ProjectRiskManagementReportPdf(int? ProjectInitiationId)
    {
      try
      {
        var query = _context.ProjectRiskIdentifications.AsQueryable();
        if (ProjectInitiationId != null && ProjectInitiationId > 0)
        {
          query = query.Where(x => x.ProjectInitiationId == ProjectInitiationId);
        }

        var list = await query.ToListAsync();
        var stream = PdfHandler.ProjectRiskManagementReportPdf(list);
        return File(stream, "application/pdf", "Project Risk Management Report.pdf");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public async Task<IActionResult> StakeholderManagementReport(int? ProjectInitiationId)
    {
      var query = _context.StakeHolder.AsQueryable();
      if (ProjectInitiationId != null && ProjectInitiationId > 0)
      {
        query = query.Where(x => x.ProjectInitiationId == ProjectInitiationId);
      }

      var list = await query.ToListAsync();
      ViewBag.SelectedProjectInitiationId = ProjectInitiationId ?? 0;
      ViewData["ProjectInitiations"] = new SelectList(_context.ProjectInitiations.OrderBy(d => d.Name), "Id", "Name");
      return View(list);
    }

    public async Task<IActionResult> StakeholderManagementReportExcel(int? ProjectInitiationId)
    {
      try
      {
        var query = _context.StakeHolder.AsQueryable();
        if (ProjectInitiationId != null && ProjectInitiationId > 0)
        {
          query = query.Where(x => x.ProjectInitiationId == ProjectInitiationId);
        }

        var list = await query.ToListAsync();
        var stream = ExportHandler.StakeholderManagementReport(list);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Stake Holder Management Report.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }
    public async Task<IActionResult> StakeholderManagementReportPdf(int? ProjectInitiationId)
    {
      try
      {
        var query = _context.StakeHolder.AsQueryable();
        if (ProjectInitiationId != null && ProjectInitiationId > 0)
        {
          query = query.Where(x => x.ProjectInitiationId == ProjectInitiationId);
        }

        var list = await query.ToListAsync();
        var stream = PdfHandler.StakeholderManagementReportPdf(list);
        return File(stream, "application/pdf", "Stake Holder Management Report.pdf");
      }
      catch (Exception ex)
      {

        throw;
      }
    }
  }
}
