using MEMIS.Data;
using MEMIS.Helpers.ExcelReports;
using MEMIS.Helpers.PdfReports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MEMIS.Controllers.Reports
{
  public class RiskReportController : Controller
  {
    private readonly AppDbContext _context;

    public RiskReportController(AppDbContext context)
    {
      _context = context;
    }
    public async Task<IActionResult> Index()
    {
      var list = await _context.RiskTreatmentPlans.Include(x => x.QuarterlyRiskActions).ToListAsync();
      return View(list);
    }

    public async Task<IActionResult> QuarterlyReportExcel()
    {
      try
      {
        var list = await _context.RiskTreatmentPlans.Include(x => x.QuarterlyRiskActions).ToListAsync();
        var stream = ExportHandler.QuarterlyReport(list);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Quarterly Report.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }
    public async Task<IActionResult> QuarterlyReportPdf()
    {
      try
      {
        var list = await _context.RiskTreatmentPlans.Include(x => x.QuarterlyRiskActions).ToListAsync();
        var stream = PdfHandler.QuarterlyReportToPdf(list);
        return File(stream, "application/pdf", "Quarterly Report.pdf");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public async Task<IActionResult> AnnualReport()
    {
      try
      {
        var list = await _context.RiskRegister.ToListAsync();
        return View(list);
      }
      catch (Exception)
      {

        throw;
      }
    }

    public async Task<IActionResult> AnnualReportExcel()
    {
      try
      {
        var list = await _context.RiskRegister.ToListAsync();
        var stream = ExportHandler.AnnualReport(list);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Annual Report.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }
    public async Task<IActionResult> AnnualReportPdf()
    {
      try
      {
        var list = await _context.RiskRegister.ToListAsync();
        var stream = PdfHandler.AnnualReportPdf(list);
        return File(stream, "application/pdf", "Annual Report.pdf");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public async Task<IActionResult> RiskRegister()
    {
      try
      {
        var list = await _context.RiskRegister.ToListAsync();
        return View(list);
      }
      catch (Exception)
      {

        throw;
      }
    }

    public async Task<IActionResult> RiskRegisterExcel()
    {
      try
      {
        var list = await _context.RiskRegister.ToListAsync();
        var stream = ExportHandler.RiskRegisterReport(list);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Risk Register.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }
    public async Task<IActionResult> RiskRegisterPdf()
    {
      try
      {
        var list = await _context.RiskRegister.ToListAsync();
        var stream = PdfHandler.RiskRegisterReportPdf(list);
        return File(stream, "application/pdf", "Risk Register.pdf");
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public async Task<IActionResult> RiskTreatmentPlan()
    {
      try
      {
        var list = await _context.RiskTreatmentPlans.ToListAsync();
        return View(list);
      }
      catch (Exception)
      {

        throw;
      }
    }

    public async Task<IActionResult> RiskTreatmentPlanExcel()
    {
      try
      {
        var list = await _context.RiskTreatmentPlans.ToListAsync();
        var stream = ExportHandler.RiskTreatmentReport(list);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Risk Treatment Plan.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }
    public async Task<IActionResult> RiskTreatmentPlanPdf()
    {
      try
      {
        var list = await _context.RiskTreatmentPlans.ToListAsync();
        var stream = PdfHandler.RiskTreatmentReportPdf(list);
        return File(stream, "application/pdf", "Risk Treatment Plan.pdf");
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    public async Task<IActionResult> RiskMonitoringReport()
    {
      try
      {
        var list = await _context.RiskTreatmentPlans.ToListAsync();
        return View(list);
      }
      catch (Exception)
      {

        throw;
      }
    }

    public async Task<IActionResult> RiskMonitoringReportExcel()
    {
      try
      {
        var list = await _context.RiskTreatmentPlans.Include(x => x.RiskRegister)
          .ThenInclude(x => x.RiskIdentificationFk)
          .Include(x => x.QuarterlyRiskActions).ToListAsync();
        var stream = ExportHandler.RiskMonitoringReport(list);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Risk Monitoring Plan.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }
    public async Task<IActionResult> RiskMonitoringReportPdf()
    {
      try
      {
        var list = await _context.RiskTreatmentPlans.Include(x => x.RiskRegister)
          .ThenInclude(x => x.RiskIdentificationFk)
          .Include(x => x.QuarterlyRiskActions).ToListAsync();
        var stream = PdfHandler.RiskMonitoringReportPdf(list);
        return File(stream, "application/pdf", "Risk Monitoring Plan.pdf");
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public async Task<IActionResult> IncidentReport()
    {
      try
      {
        var list = await _context.QuarterlyRiskActions.Include(x => x.Incidents).Include(x => x.RiskTreatmentPlan).ToListAsync();
        return View(list);
      }
      catch (Exception)
      {

        throw;
      }
    }

    public async Task<IActionResult> IncidentReportExcel()
    {
      try
      {
        var list = await _context.QuarterlyRiskActions.Include(x => x.Incidents).Include(x => x.RiskTreatmentPlan).ToListAsync();
        var stream = ExportHandler.IncidentReport(list);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Incident Report.xlsx");
      }
      catch (Exception ex)
      {

        throw;
      }
    }
    public async Task<IActionResult> IncidentReportPdf()
    {
      try
      {
        var list = await _context.QuarterlyRiskActions.Include(x => x.Incidents).Include(x => x.RiskTreatmentPlan).ToListAsync();
        var stream = PdfHandler.IncidentReportPdf(list);
        return File(stream, "application/pdf", "Incident Report.pdf");
      }
      catch (Exception ex)
      {
        throw;
      }
    }
  }
}
