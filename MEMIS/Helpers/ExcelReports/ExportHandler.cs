using ClosedXML.Excel;
using MEMIS.Data;
using MEMIS.Data.Project;
using MEMIS.Data.Risk;
using MEMIS.Models.Report;

namespace MEMIS.Helpers.ExcelReports
{
  public class ExportHandler
  {
    public static MemoryStream StrategicImplementationPlanReport(List<ProgramImplementationPlan> plans)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Strategic Implementation Plan");

        // Style the headers
        var headerRange = worksheet.Range("A1:M1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.FontColor = XLColor.White;
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Strategic Objective";
        worksheet.Cell(1, 2).Value = "Strategic Intervention";
        worksheet.Cell(1, 3).Value = "Strategic Actions";
        worksheet.Cell(1, 4).Value = "Activities";
        worksheet.Cell(1, 5).Value = "Output Indicators";
        worksheet.Cell(1, 6).Value = "Output Targets";
        worksheet.Cell(1, 7).Value = "FY 1";
        worksheet.Cell(1, 8).Value = "FY 2";
        worksheet.Cell(1, 9).Value = "FY 3";
        worksheet.Cell(1, 10).Value = "FY 4";
        worksheet.Cell(1, 11).Value = "FY 5";
        worksheet.Cell(1, 12).Value = "Means of Verification";
        worksheet.Cell(1, 13).Value = "Responsible Party";

        //// Merging cells for the rest of the headers as per the layout
        //worksheet.Range(1, 1, 2, 1).Merge();
        //worksheet.Range(1, 2, 2, 2).Merge();
        //worksheet.Range(1, 3, 2, 3).Merge();
        //worksheet.Range(1, 4, 2, 4).Merge();
        //worksheet.Range(1, 5, 2, 5).Merge();
        //worksheet.Range(1, 6, 2, 6).Merge();
        //worksheet.Range(1, 12, 2, 12).Merge();
        //worksheet.Range(1, 13, 2, 13).Merge();

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var plan in plans)
        {
          worksheet.Cell(row, 1).Value = "";
          worksheet.Cell(row, 2).Value = "";
          worksheet.Cell(row, 3).Value = "";
          worksheet.Cell(row, 4).Value = plan?.intActivity;
          worksheet.Cell(row, 5).Value = "";
          worksheet.Cell(row, 6).Value = "";
          worksheet.Cell(row, 7).Value = plan?.FY1;
          worksheet.Cell(row, 8).Value = plan?.FY2;
          worksheet.Cell(row, 9).Value = plan?.FY3;
          worksheet.Cell(row, 10).Value = plan?.FY4;
          worksheet.Cell(row, 11).Value = plan?.FY5;
          worksheet.Cell(row, 12).Value = "";
          worksheet.Cell(row, 13).Value = plan?.ResponsibleParty;

          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 13);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static MemoryStream ProgrammeImplementationPlanReport(List<ProgramImplementationPlan> plans)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Strategic Implementation Plan");

        // Style the headers
        var headerRange = worksheet.Range("A1:M2");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.FontColor = XLColor.White;
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Strategic Objective";
        worksheet.Cell(1, 2).Value = "Strategic Intervention";
        worksheet.Cell(1, 3).Value = "Strategic Actions";
        worksheet.Cell(1, 4).Value = "Activities";
        worksheet.Cell(1, 5).Value = "Output Indicators";
        worksheet.Cell(1, 6).Value = "Output Targets";
        worksheet.Cell(1, 12).Value = "Means of Verification";
        worksheet.Cell(1, 13).Value = "Responsible Party";

        // Setting sub-headers under Annual Targets
        worksheet.Cell(2, 7).Value = "FY 1";
        worksheet.Cell(2, 8).Value = "FY 2";
        worksheet.Cell(2, 9).Value = "FY 3";
        worksheet.Cell(2, 10).Value = "FY 4";
        worksheet.Cell(2, 11).Value = "FY 5";

        // Merging the cells for Annual Targets as per the image
        var annualTargetsHeader = worksheet.Range(1, 7, 1, 11).Merge();
        annualTargetsHeader.Value = "Annual Targets";

        // Merging cells for the rest of the headers as per the layout
        worksheet.Range(1, 1, 2, 1).Merge();
        worksheet.Range(1, 2, 2, 2).Merge();
        worksheet.Range(1, 3, 2, 3).Merge();
        worksheet.Range(1, 4, 2, 4).Merge();
        worksheet.Range(1, 5, 2, 5).Merge();
        worksheet.Range(1, 6, 2, 6).Merge();
        worksheet.Range(1, 12, 2, 12).Merge();
        worksheet.Range(1, 13, 2, 13).Merge();

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 3;
        foreach (var plan in plans)
        {
          worksheet.Cell(row, 1).Value = "";
          worksheet.Cell(row, 2).Value = "";
          worksheet.Cell(row, 3).Value = "";
          worksheet.Cell(row, 4).Value = plan?.intActivity;
          worksheet.Cell(row, 5).Value = "";
          worksheet.Cell(row, 6).Value = "";
          worksheet.Cell(row, 7).Value = plan?.FY1;
          worksheet.Cell(row, 8).Value = plan?.FY2;
          worksheet.Cell(row, 9).Value = plan?.FY3;
          worksheet.Cell(row, 10).Value = plan?.FY4;
          worksheet.Cell(row, 11).Value = plan?.FY5;
          worksheet.Cell(row, 12).Value = "";
          worksheet.Cell(row, 13).Value = plan?.ResponsibleParty;

          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 13);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }


    public static MemoryStream AnnualDetailedResultsFrameworkReport(List<ActivityAssess> activityAssesses)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Annual Detailed Results");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Strategic Objective";
        worksheet.Cell(1, 2).Value = "Strategic Intervention";
        worksheet.Cell(1, 3).Value = "Strategic Actions";
        worksheet.Cell(1, 4).Value = "Activities/Initiatives";
        worksheet.Cell(1, 5).Value = "Output Indicators";
        worksheet.Cell(1, 6).Value = "Baseline";
        worksheet.Cell(1, 7).Value = "Annual Target";
        worksheet.Cell(1, 8).Value = "Budget Code";
        worksheet.Cell(1, 9).Value = "Amount Allocated";
        worksheet.Cell(1, 10).Value = "Quarter 1";
        worksheet.Cell(1, 11).Value = "Quarter 2";
        worksheet.Cell(1, 12).Value = "Quarter 3";
        worksheet.Cell(1, 13).Value = "Quarter 4";
        worksheet.Cell(1, 14).Value = "Identified Risks";
        worksheet.Cell(1, 15).Value = "Responsible Party";

        // Style the headers
        var headerRange = worksheet.Range("A1:O1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var activityAssess in activityAssesses)
        {
          worksheet.Cell(row, 1).Value = activityAssess?.StrategicIntervention?.StrategicObjective?.ObjectiveName;
          worksheet.Cell(row, 2).Value = activityAssess?.StrategicIntervention?.InterventionName;
          worksheet.Cell(row, 3).Value = activityAssess?.StrategicAction?.actionName;
          worksheet.Cell(row, 4).Value = activityAssess?.ActivityFk?.activityName;
          worksheet.Cell(row, 5).Value = activityAssess?.outputIndicator;
          worksheet.Cell(row, 6).Value = activityAssess?.baseline;
          worksheet.Cell(row, 7).Value = activityAssess?.QTarget;
          worksheet.Cell(row, 8).Value = activityAssess?.budgetCode;
          worksheet.Cell(row, 9).Value = activityAssess?.budgetAmount;
          worksheet.Cell(row, 10).Value = activityAssess?.QuaterlyPlans.FirstOrDefault(x => x.Quarter == "1")?.QTarget;
          worksheet.Cell(row, 11).Value = activityAssess?.QuaterlyPlans.FirstOrDefault(x => x.Quarter == "2")?.QTarget;
          worksheet.Cell(row, 12).Value = activityAssess?.QuaterlyPlans.FirstOrDefault(x => x.Quarter == "3")?.QTarget;
          worksheet.Cell(row, 13).Value = activityAssess?.QuaterlyPlans.FirstOrDefault(x => x.Quarter == "4")?.QTarget;
          worksheet.Cell(row, 14).Value = activityAssess?.IdentifiedRisks;
          worksheet.Cell(row, 15).Value = activityAssess?.DepartmentFk?.deptName;

          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 15);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static MemoryStream ActivityImplementationStatusExport(List<ActivityAssessment> activityAssesses)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Activity Implementation Status");

        // Setting the header row
        //worksheet.Cell(1, 1).Value = "Strategic Objective";
        //worksheet.Cell(1, 2).Value = "Strategic Intervention";
        //worksheet.Cell(1, 3).Value = "Strategic Actions";
        //worksheet.Cell(1, 4).Value = "Activities/Initiatives";
        //worksheet.Cell(1, 5).Value = "Output Indicators";
        //worksheet.Cell(1, 6).Value = "Baseline";
        //worksheet.Cell(1, 7).Value = "Annual Target";
        //worksheet.Cell(1, 8).Value = "Budget Code";
        //worksheet.Cell(1, 9).Value = "Amount Allocated";
        //worksheet.Cell(1, 10).Value = "Quarter 1";
        //worksheet.Cell(1, 11).Value = "Quarter 2";
        //worksheet.Cell(1, 12).Value = "Quarter 3";
        //worksheet.Cell(1, 13).Value = "Quarter 4";
        //worksheet.Cell(1, 14).Value = "Implementation Status";
        //worksheet.Cell(1, 15).Value = "Responsible Party";
        worksheet.Cell(1, 1).Value = "Strategic Intervention";
        worksheet.Cell(1, 2).Value = "Strategic Action";
        worksheet.Cell(1, 3).Value = "Activities/Initiatives";
        worksheet.Cell(1, 4).Value = "Implementation Status";
        worksheet.Cell(1, 5).Value = "Department";

        // Style the headers
        var headerRange = worksheet.Range("A1:E1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var activityAssess in activityAssesses)
        {
          worksheet.Cell(row, 1).Value = activityAssess?.strategicIntervention;
          worksheet.Cell(row, 2).Value = activityAssess?.StrategicAction;
          worksheet.Cell(row, 3).Value = activityAssess?.activity;
          worksheet.Cell(row, 4).Value = activityAssess?.ImplementationStatus?.ImpStatusName;
          worksheet.Cell(row, 5).Value = activityAssess?.QuaterlyPlans?.FirstOrDefault()?.ActivityAssess?.DepartmentFk?.deptName;

          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 5);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static MemoryStream StrategicPlanActivityImplementationTrackerExport(List<StrategicObjectiveReport> activityAssesses)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Strategic Plan Activity Tracker");

        worksheet.Cell(1, 1).Value = "Strategic Objectives";
        worksheet.Cell(1, 2).Value = "Strategic Interventions";
        worksheet.Cell(1, 3).Value = "Strategic Actions";
        worksheet.Cell(1, 4).Value = "FY 1";
        worksheet.Cell(1, 5).Value = "FY 2";
        worksheet.Cell(1, 6).Value = "FY 3";
        worksheet.Cell(1, 7).Value = "FY 4";
        worksheet.Cell(1, 8).Value = "FY 5";
        worksheet.Cell(1, 9).Value = "Cumulative Overall Performance on Strategic Intervention";
        worksheet.Cell(1, 10).Value = "Cumulative Overall Performance on Strategic Objective 1";

        // Style the headers
        var headerRange = worksheet.Range("A1:J1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var objective in activityAssesses)
        {
          worksheet.Cell(row, 1).Value = objective.StrategicObjective;

          foreach (var intervention in objective.StrategicInterventions)
          {
            worksheet.Cell(row, 2).Value = intervention.StrategicIntervention;
            foreach (var action in intervention.StrategicActions)
            {
              worksheet.Cell(row, 3).Value = action.StrategicAction;
              worksheet.Cell(row, 4).Value = action.FiscalYearData[0];
              worksheet.Cell(row, 5).Value = action.FiscalYearData[1];
              worksheet.Cell(row, 6).Value = action.FiscalYearData[2];
              worksheet.Cell(row, 7).Value = action.FiscalYearData[3];
              worksheet.Cell(row, 8).Value = action.FiscalYearData[4];
              row++;
            }
            worksheet.Cell(row, 9).Value = Math.Round(intervention.AverageFiscalYearData.Average(), 2);
          }
          worksheet.Cell(row, 10).Value = Math.Round(objective.AverageFiscalYearData.Average(), 2);
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 10);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static MemoryStream StrategicPlanOutputMonitoringTrackerExport(List<StrategicObjectiveReport> activityAssesses)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Strategic Plan Activity Tracker");

        worksheet.Cell(1, 1).Value = "Strategic Objectives";
        worksheet.Cell(1, 2).Value = "Strategic Interventions";
        worksheet.Cell(1, 3).Value = "Strategic Actions";
        worksheet.Cell(1, 4).Value = "FY 1";
        worksheet.Cell(1, 5).Value = "FY 2";
        worksheet.Cell(1, 6).Value = "FY 3";
        worksheet.Cell(1, 7).Value = "FY 4";
        worksheet.Cell(1, 8).Value = "FY 5";
        worksheet.Cell(1, 9).Value = "Cumulative Overall Performance on Strategic Intervention";
        worksheet.Cell(1, 10).Value = "Cumulative Overall Performance on Strategic Objective 1";

        // Style the headers
        var headerRange = worksheet.Range("A1:J1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var objective in activityAssesses)
        {
          worksheet.Cell(row, 1).Value = objective.StrategicObjective;

          foreach (var intervention in objective.StrategicInterventions)
          {
            worksheet.Cell(row, 2).Value = intervention.StrategicIntervention;
            foreach (var action in intervention.StrategicActions)
            {
              worksheet.Cell(row, 3).Value = action.StrategicAction;
              worksheet.Cell(row, 4).Value = action.FiscalYearData[0];
              worksheet.Cell(row, 5).Value = action.FiscalYearData[1];
              worksheet.Cell(row, 6).Value = action.FiscalYearData[2];
              worksheet.Cell(row, 7).Value = action.FiscalYearData[3];
              worksheet.Cell(row, 8).Value = action.FiscalYearData[4];
              row++;
            }
            worksheet.Cell(row, 9).Value = Math.Round(intervention.AverageFiscalYearData.Average(), 2);
          }
          worksheet.Cell(row, 10).Value = Math.Round(objective.AverageFiscalYearData.Average(), 2);
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 10);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static MemoryStream SDTQuarterlyPerformanceReport(List<SDTAssessment> assessments)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("SDT Quarterly Performances");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Month";
        worksheet.Cell(1, 2).Value = "SDT";
        worksheet.Cell(1, 3).Value = "SDT Numerator";
        worksheet.Cell(1, 4).Value = "SDT Denominator";
        worksheet.Cell(1, 5).Value = "Numerator Perf";
        worksheet.Cell(1, 6).Value = "Denominator Perf";
        worksheet.Cell(1, 7).Value = "Implemented Within Timeline";
        worksheet.Cell(1, 8).Value = "Average Working Days";
        worksheet.Cell(1, 9).Value = "% Implemented Within Timeline";
        worksheet.Cell(1, 10).Value = "Target";
        worksheet.Cell(1, 11).Value = "Achievement Status";
        worksheet.Cell(1, 12).Value = "% Variance";
        worksheet.Cell(1, 13).Value = "Justification";
        worksheet.Cell(1, 14).Value = "Rating";
        worksheet.Cell(1, 15).Value = "Department";

        // Style the headers
        var headerRange = worksheet.Range("A1:O1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var assessment in assessments)
        {
          worksheet.Cell(row, 1).Value = assessment?.Month;
          worksheet.Cell(row, 2).Value = assessment?.SDTMasterFk?.ServiceDeliveryTimeline;
          worksheet.Cell(row, 3).Value = assessment?.SDTMasterFk?.Numerator;
          worksheet.Cell(row, 4).Value = assessment?.SDTMasterFk?.Denominator;
          worksheet.Cell(row, 5).Value = assessment?.Numerator;
          worksheet.Cell(row, 6).Value = assessment?.Denominator;
          worksheet.Cell(row, 7).Value = assessment?.ImplementedTimeline;
          worksheet.Cell(row, 8).Value = assessment?.Rate;
          worksheet.Cell(row, 9).Value = assessment?.ProportionTimeline;
          worksheet.Cell(row, 10).Value = assessment?.Target;
          worksheet.Cell(row, 11).Value = getAchievement(assessment?.AchivementStatus ?? "");
          worksheet.Cell(row, 12).Value = assessment?.Variance;
          worksheet.Cell(row, 13).Value = assessment?.Justification;
          worksheet.Cell(row, 14).Value = assessment?.Rating;
          worksheet.Cell(row, 15).Value = assessment?.SDTMasterFk?.DepartmentFk?.deptName;

          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 15);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }
    public static MemoryStream KPIMandEFrameworkReport(List<KPIMaster> assessments)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("KPI M&E Framework Report");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Performance Indicator";
        worksheet.Cell(1, 2).Value = "Type of Indicator";
        worksheet.Cell(1, 3).Value = "Indicator Formulae";
        worksheet.Cell(1, 4).Value = "Indicator Definition";
        worksheet.Cell(1, 5).Value = "Original Baseline";
        worksheet.Cell(1, 6).Value = "Indicator Classification";
        worksheet.Cell(1, 7).Value = "Data Type";
        worksheet.Cell(1, 8).Value = "Unit of Measure";
        worksheet.Cell(1, 9).Value = "Frequency of Reporting";
        worksheet.Cell(1, 10).Value = "Target for FY 1";
        worksheet.Cell(1, 11).Value = "Target for FY 2";
        worksheet.Cell(1, 12).Value = "Target for FY 3";
        worksheet.Cell(1, 13).Value = "Target for FY 4";
        worksheet.Cell(1, 14).Value = "Target for FY 5";
        worksheet.Cell(1, 15).Value = "Means of Verification";
        worksheet.Cell(1, 16).Value = "Department";

        // Style the headers
        var headerRange = worksheet.Range("A1:P1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var assessment in assessments)
        {
          worksheet.Cell(row, 1).Value = assessment?.PerformanceIndicator;
          worksheet.Cell(row, 2).Value = assessment?.TypeofIndicator;
          worksheet.Cell(row, 3).Value = assessment?.IndicatorFormulae;
          worksheet.Cell(row, 4).Value = assessment?.IndicatorDefinition;
          worksheet.Cell(row, 5).Value = assessment?.OriginalBaseline;
          worksheet.Cell(row, 6).Value = assessment?.Indicatorclassification;
          worksheet.Cell(row, 7).Value = assessment?.DataType;
          worksheet.Cell(row, 8).Value = assessment?.Unitofmeasure;
          worksheet.Cell(row, 9).Value = assessment?.FrequencyofReporting;
          //worksheet.Cell(row, 10).Value = getAchievement(assessment?.Achieved ?? "");
          //worksheet.Cell(row, 10).Style.Font.FontColor = assessment?.Achieved == "1.0" ? XLColor.FromHtml("#00b050") : assessment?.Achieved == "0.5" ? XLColor.FromHtml("#fefe00") : XLColor.FromHtml("#fd0000");
          worksheet.Cell(row, 10).Value = assessment?.FY1;
          worksheet.Cell(row, 11).Value = assessment?.FY2;
          worksheet.Cell(row, 12).Value = assessment?.FY3;
          worksheet.Cell(row, 13).Value = assessment?.FY4;
          worksheet.Cell(row, 14).Value = assessment?.FY5;
          worksheet.Cell(row, 15).Value = assessment?.MeansofVerification;
          worksheet.Cell(row, 16).Value = assessment?.ResponsibleParty;

          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 16);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static MemoryStream KPIMandEReport(List<KPIAssessment> assessments)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("KPI M&E Report");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Performance Indicator";
        worksheet.Cell(1, 2).Value = "Frequency Of Reporting";
        worksheet.Cell(1, 3).Value = "Indicator Formulae";
        worksheet.Cell(1, 4).Value = "Indicator Definition";
        worksheet.Cell(1, 5).Value = "FY";
        worksheet.Cell(1, 6).Value = "Target";
        worksheet.Cell(1, 7).Value = "Numerator";
        worksheet.Cell(1, 8).Value = "Denominator";
        worksheet.Cell(1, 9).Value = "Rate";
        worksheet.Cell(1, 10).Value = "Achieved";
        worksheet.Cell(1, 11).Value = "Justification";

        // Style the headers
        var headerRange = worksheet.Range("A1:K1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var assessment in assessments)
        {
          worksheet.Cell(row, 1).Value = assessment?.PerformanceIndicator;
          worksheet.Cell(row, 2).Value = assessment?.FrequencyofReporting;
          worksheet.Cell(row, 3).Value = assessment?.IndicatorFormulae;
          worksheet.Cell(row, 4).Value = assessment?.IndicatorDefinition;
          worksheet.Cell(row, 5).Value = assessment?.FY;
          worksheet.Cell(row, 6).Value = assessment?.Target;
          worksheet.Cell(row, 7).Value = assessment?.Numerator;
          worksheet.Cell(row, 8).Value = assessment?.Denominator;
          worksheet.Cell(row, 9).Value = assessment?.Rate;
          //worksheet.Cell(row, 10).Value = getAchievement(assessment?.Achieved ?? "");
          //worksheet.Cell(row, 10).Style.Font.FontColor = assessment?.Achieved == "1.0" ? XLColor.FromHtml("#00b050") : assessment?.Achieved == "0.5" ? XLColor.FromHtml("#fefe00") : XLColor.FromHtml("#fd0000");
          worksheet.Cell(row, 10).Value = assessment?.Achieved;
          worksheet.Cell(row, 11).Value = assessment?.Justification;

          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 11);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static MemoryStream OpTargetPerfAcheivReport(List<ActivityAssessment> assessments)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Performance Achievement");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Strategic Action";
        worksheet.Cell(1, 2).Value = "Activity";
        worksheet.Cell(1, 3).Value = "Output/Target";
        worksheet.Cell(1, 4).Value = "Performance Achievement Status";
        worksheet.Cell(1, 5).Value = "Department";

        // Style the headers
        var headerRange = worksheet.Range("A1:E1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var assessment in assessments)
        {
          worksheet.Cell(row, 1).Value = assessment?.StrategicAction;
          worksheet.Cell(row, 2).Value = assessment?.activity;
          worksheet.Cell(row, 3).Value = assessment?.comparativeTarget;
          worksheet.Cell(row, 4).Value = assessment?.ImplementationStatus?.ImpStatusName;
          worksheet.Cell(row, 5).Value = assessment?.DepartmentFk?.deptName;
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 5);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static MemoryStream StrategicActionPerfAchievReport(List<StrategicActionPerformanceAchievementDto> actions)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Strategic Action Performance");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Strategic Action";
        worksheet.Cell(1, 2).Value = "Performance Achievement Status";

        // Style the headers
        var headerRange = worksheet.Range("A1:B1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var action in actions)
        {
          worksheet.Cell(row, 1).Value = action?.StrategicAction;
          worksheet.Cell(row, 2).Value = action?.PerformanceAchievementStatus;
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 2);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static MemoryStream StrategicInterventionPerfAchievReport(List<StrategicInterventionPerformanceAchievementDto> actions)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Strategic Intervention Performance ");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Strategic Intervention";
        worksheet.Cell(1, 2).Value = "Performance Achievement Status";

        // Style the headers
        var headerRange = worksheet.Range("A1:B1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var action in actions)
        {
          worksheet.Cell(row, 1).Value = action?.StrategicIntervention;
          worksheet.Cell(row, 2).Value = action?.PerformanceAchievementStatus;
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 2);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static MemoryStream StrategicObjectivePerfAchievReport(List<StrategicObjectivePerformanceAchievementDto> actions)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Strategic Objective Performance ");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Strategic Objective";
        worksheet.Cell(1, 2).Value = "Performance Achievement Status";

        // Style the headers
        var headerRange = worksheet.Range("A1:B1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var action in actions)
        {
          worksheet.Cell(row, 1).Value = action?.StrategicObjective;
          worksheet.Cell(row, 2).Value = action?.PerformanceAchievementStatus;
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 2);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static MemoryStream OutcomePerfAchievReport(List<ActivityAssessment> activities)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Outcome Performance");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Strategic Objective";
        worksheet.Cell(1, 2).Value = "Performance Achievement Status";
        worksheet.Cell(1, 3).Value = "Department";

        // Style the headers
        var headerRange = worksheet.Range("A1:C1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var activity in activities)
        {
          worksheet.Cell(row, 1).Value = activity?.strategicObjective;
          worksheet.Cell(row, 2).Value = activity?.ImplementationStatus?.ImpStatusName;
          worksheet.Cell(row, 3).Value = activity?.DepartmentFk?.deptName;
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 3);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static MemoryStream ImpactPerfAchievReport(List<ActivityAssessment> activities)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Impact Performance");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Strategic Objective";
        worksheet.Cell(1, 2).Value = "Performance Achievement Status";
        worksheet.Cell(1, 3).Value = "Department";

        // Style the headers
        var headerRange = worksheet.Range("A1:C1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var activity in activities)
        {
          worksheet.Cell(row, 1).Value = activity?.strategicObjective;
          worksheet.Cell(row, 2).Value = activity?.ImplementationStatus?.ImpStatusName;
          worksheet.Cell(row, 3).Value = activity?.DepartmentFk?.deptName;
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 3);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static MemoryStream SdtMAndEFrameworkReport(List<SDTMaster> sDTMasters)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("SDT M&E Framework Report");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "SDT Indicator";
        worksheet.Cell(1, 2).Value = "Measure";
        worksheet.Cell(1, 3).Value = "Evaluation Period";
        worksheet.Cell(1, 4).Value = "Target";
        worksheet.Cell(1, 5).Value = "Numerator";
        worksheet.Cell(1, 6).Value = "Denominator";
        worksheet.Cell(1, 7).Value = "Department";

        // Style the headers
        var headerRange = worksheet.Range("A1:G1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var activity in sDTMasters)
        {
          worksheet.Cell(row, 1).Value = activity?.ServiceDeliveryTimeline;
          worksheet.Cell(row, 2).Value = activity?.Measure;
          worksheet.Cell(row, 3).Value = activity?.EvaluationPeriod;
          worksheet.Cell(row, 4).Value = activity?.Target;
          worksheet.Cell(row, 5).Value = activity?.Numerator;
          worksheet.Cell(row, 6).Value = activity?.Denominator;
          worksheet.Cell(row, 7).Value = activity?.DepartmentFk?.deptName;
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 7);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    #region risk management reports

    public static MemoryStream QuarterlyReport(List<RiskTreatmentPlan> plans)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Quarterly Report");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Action";
        worksheet.Cell(1, 2).Value = "Indicator Description";
        worksheet.Cell(1, 3).Value = "Cumulative Target";
        worksheet.Cell(1, 4).Value = "Q1";
        worksheet.Cell(1, 5).Value = "Q2";
        worksheet.Cell(1, 6).Value = "Q3";
        worksheet.Cell(1, 7).Value = "Q4";
        worksheet.Cell(1, 8).Value = "Status";
        worksheet.Cell(1, 9).Value = "Data Collection Instrument & Methods";
        worksheet.Cell(1, 10).Value = "Means Of Verification";
        worksheet.Cell(1, 11).Value = "Responsible Person(s)";

        // Style the headers
        var headerRange = worksheet.Range("A1:K1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var plan in plans)
        {
          worksheet.Cell(row, 1).Value = plan?.TreatmentAction;
          worksheet.Cell(row, 2).Value = plan?.IndicatorDescription;
          worksheet.Cell(row, 3).Value = plan?.CumulativeTarget;
          worksheet.Cell(row, 4).Value = plan?.QuarterlyRiskActions?.Where(x => x.Quarter == 1).FirstOrDefault()?.IncidentValue;
          worksheet.Cell(row, 5).Value = plan?.QuarterlyRiskActions?.Where(x => x.Quarter == 2).FirstOrDefault()?.IncidentValue;
          worksheet.Cell(row, 6).Value = plan?.QuarterlyRiskActions?.Where(x => x.Quarter == 3).FirstOrDefault()?.IncidentValue;
          worksheet.Cell(row, 7).Value = plan?.QuarterlyRiskActions?.Where(x => x.Quarter == 4).FirstOrDefault()?.IncidentValue;

          string Status = "";
          var averageImpStatus = plan.QuarterlyRiskActions.Count > 0 ? plan.QuarterlyRiskActions?.Average(x => x.ImpStatusId) : 0;

          if (averageImpStatus < 1)
          {
            Status = "Not Implemented";
          }
          else if (averageImpStatus < 3)
          {
            Status = "Partially Implemented";
          }
          else if (averageImpStatus == 3)
          {
            Status = "Fully Implemented";
          }
          else
          {
            Status = "NA";
          }

          worksheet.Cell(row, 8).Value = Status;
          worksheet.Cell(row, 9).Value = plan?.DataCollectionInstrumentMethods;
          worksheet.Cell(row, 10).Value = plan?.MeansOfVerification;
          worksheet.Cell(row, 11).Value = plan?.ResponsiblePersons;
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 11);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception)
      {

        throw;
      }
    }

    public static MemoryStream AnnualReport(List<RiskRegister> risks)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Annual Report");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Risk ID";
        worksheet.Cell(1, 2).Value = "Risk Title";
        worksheet.Cell(1, 3).Value = "Inherent Rating";
        worksheet.Cell(1, 4).Value = "Residual Rating";
        worksheet.Cell(1, 5).Value = "Risk Movement";
        worksheet.Cell(1, 6).Value = "Control Effectiveness";

        // Style the headers
        var headerRange = worksheet.Range("A1:F1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var risk in risks)
        {
          worksheet.Cell(row, 1).Value = risk?.RiskRefID;
          worksheet.Cell(row, 2).Value = risk?.RiskDescription;
          worksheet.Cell(row, 3).Value = risk?.RiskRatingCategory;
          worksheet.Cell(row, 4).Value = risk?.RiskResidualRank;

          int inheringRating = risk?.RiskConsequenceId ?? 0 * risk?.RiskLikelihoodId ?? 0;
          int residualRating = (risk?.RiskResidualConsequenceId ?? 0) * (risk?.RiskResidualLikelihoodId ?? 0);
          (string movement, string icon) = inheringRating == residualRating ? ("Same", "bx bx-right-arrow-alt") : inheringRating
          < residualRating ? ("Reduced", "bx bx-down-arrow-alt") : ("Increased", "bx bx-up-arrow-alt");

          worksheet.Cell(row, 5).Value = movement;
          worksheet.Cell(row, 6).Value = risk?.ControlEffectiveness;
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 6);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception)
      {

        throw;
      }
    }

    public static MemoryStream RiskRegisterReport(List<RiskRegister> risks)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Risk Register");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Objective";
        worksheet.Cell(1, 2).Value = "Risk Reference Code/Number";
        worksheet.Cell(1, 3).Value = "Risk Description";
        worksheet.Cell(1, 4).Value = "Risk Category";
        worksheet.Cell(1, 5).Value = "Risk Driver/Root Cause";
        worksheet.Cell(1, 6).Value = "Consequence/Impact Definition";
        worksheet.Cell(1, 7).Value = "Existing Mitigation";
        worksheet.Cell(1, 8).Value = "Likelihood";
        worksheet.Cell(1, 9).Value = "Consequence";
        worksheet.Cell(1, 10).Value = "Inherent Risk Rating";
        worksheet.Cell(1, 11).Value = "Further Action Required/Additional Mitigation Strategies";
        worksheet.Cell(1, 12).Value = "Opportunity";
        worksheet.Cell(1, 13).Value = "Review/Implementation Date";
        worksheet.Cell(1, 14).Value = "Risk Owner";

        // Style the headers
        var headerRange = worksheet.Range("A1:N1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var risk in risks)
        {
          worksheet.Cell(row, 1).Value = risk?.StrategicPlanFk?.ObjectiveName;
          worksheet.Cell(row, 2).Value = "";
          worksheet.Cell(row, 3).Value = risk?.RiskDescription;
          worksheet.Cell(row, 4).Value = risk?.RiskRatingCategory;
          worksheet.Cell(row, 5).Value = risk?.RiskCause;
          worksheet.Cell(row, 6).Value = risk?.RiskConsequence;
          worksheet.Cell(row, 7).Value = "";
          worksheet.Cell(row, 8).Value = risk?.RiskLikelihoodId;
          worksheet.Cell(row, 9).Value = risk?.RiskConsequence;
          worksheet.Cell(row, 10).Value = risk?.RiskRatingId;
          worksheet.Cell(row, 11).Value = risk?.ActionTaken;
          worksheet.Cell(row, 12).Value = risk?.Opportunity;
          worksheet.Cell(row, 13).Value = risk?.ReviewDate;
          worksheet.Cell(row, 14).Value = risk?.RiskOwner;
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 14);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception)
      {

        throw;
      }
    }

    public static MemoryStream RiskTreatmentReport(List<RiskTreatmentPlan> plans)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Risk Treatment Plan");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Treatment Action";
        worksheet.Cell(1, 2).Value = "Indicator Description";
        worksheet.Cell(1, 3).Value = "Baseline";
        worksheet.Cell(1, 4).Value = "Cumulative Target";
        worksheet.Cell(1, 5).Value = "Q1";
        worksheet.Cell(1, 6).Value = "Q2";
        worksheet.Cell(1, 7).Value = "Q3";
        worksheet.Cell(1, 8).Value = "Q4";
        worksheet.Cell(1, 9).Value = "Data Collection Instrument & Methods";
        worksheet.Cell(1, 10).Value = "Frequency Of Reporting";
        worksheet.Cell(1, 11).Value = "Means Of Verification";
        worksheet.Cell(1, 12).Value = "Responsible Person(s)";

        // Style the headers
        var headerRange = worksheet.Range("A1:L1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var plan in plans)
        {
          worksheet.Cell(row, 1).Value = plan?.TreatmentAction;
          worksheet.Cell(row, 2).Value = plan?.IndicatorDescription;
          worksheet.Cell(row, 3).Value = plan?.Baseline;
          worksheet.Cell(row, 4).Value = plan?.CumulativeTarget;
          worksheet.Cell(row, 5).Value = plan?.QuarterlyRiskActions?.Where(x => x.Quarter == 1).FirstOrDefault()?.IncidentValue;
          worksheet.Cell(row, 6).Value = plan?.QuarterlyRiskActions?.Where(x => x.Quarter == 2).FirstOrDefault()?.IncidentValue;
          worksheet.Cell(row, 7).Value = plan?.QuarterlyRiskActions?.Where(x => x.Quarter == 3).FirstOrDefault()?.IncidentValue;
          worksheet.Cell(row, 8).Value = plan?.QuarterlyRiskActions?.Where(x => x.Quarter == 4).FirstOrDefault()?.IncidentValue;
          worksheet.Cell(row, 9).Value = plan?.DataCollectionInstrumentMethods;
          worksheet.Cell(row, 10).Value = plan?.FrequencyOfReporting;
          worksheet.Cell(row, 11).Value = plan?.MeansOfVerification;
          worksheet.Cell(row, 12).Value = plan?.ResponsiblePersons;
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 12);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception)
      {

        throw;
      }
    }

    public static MemoryStream RiskMonitoringReport(List<RiskTreatmentPlan> plans)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Risk Monitoring Report");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Risk Code";
        worksheet.Cell(1, 2).Value = "Risk Description";
        worksheet.Cell(1, 3).Value = "Proposed mitigation actions";
        worksheet.Cell(1, 4).Value = " Actions Undertaken";
        worksheet.Cell(1, 5).Value = "Status";
        worksheet.Cell(1, 6).Value = "Risk Incidents/indicators in the period";
        worksheet.Cell(1, 7).Value = "Incident Value";
        worksheet.Cell(1, 8).Value = "Inherent  Likelihood";
        worksheet.Cell(1, 9).Value = "Inherent  Consequence";
        worksheet.Cell(1, 10).Value = "Inherent  Risk Rating";
        worksheet.Cell(1, 11).Value = "Residual  Likelihood";
        worksheet.Cell(1, 12).Value = "Residual  Consequence";
        worksheet.Cell(1, 13).Value = "Residual  Risk Rating";

        // Style the headers
        var headerRange = worksheet.Range("A1:M1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var plan in plans)
        {
          worksheet.Cell(row, 1).Value = plan?.RiskRegister?.RiskCode;
          worksheet.Cell(row, 2).Value = plan?.RiskRegister?.RiskDescription;
          worksheet.Cell(row, 3).Value = plan?.TreatmentAction;
          worksheet.Cell(row, 4).Value = plan?.RiskRegister?.ActionTaken;

          string Status = "";
          var averageImpStatus = plan.QuarterlyRiskActions?.Average(x => x.ImpStatusId);

          if (averageImpStatus < 1)
          {
            Status = "Not Implemented";
          }
          else if (averageImpStatus < 3)
          {
            Status = "Partially Implemented";
          }
          else if (averageImpStatus == 3)
          {
            Status = "Fully Implemented";
          }
          else
          {
            Status = "NA";
          }

          worksheet.Cell(row, 5).Value = Status;
          worksheet.Cell(row, 6).Value = plan?.IndicatorDescription;
          worksheet.Cell(row, 7).Value = plan?.QuarterlyRiskActions.Count > 0 ? plan?.QuarterlyRiskActions?.Sum(x => x.IncidentValue) : 0;
          worksheet.Cell(row, 8).Value = plan?.RiskRegister?.RiskIdentificationFk?.RiskLikelihoodId;
          worksheet.Cell(row, 9).Value = plan?.RiskRegister?.RiskIdentificationFk?.RiskConsequenceId;
          worksheet.Cell(row, 10).Value = "";
          worksheet.Cell(row, 11).Value = plan?.RiskRegister?.RiskLikelihoodId;
          worksheet.Cell(row, 12).Value = plan?.RiskRegister?.RiskConsequenceId;
          worksheet.Cell(row, 13).Value = plan?.RiskRegister?.RiskRatingId;
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 13);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception)
      {

        throw;
      }
    }

    #endregion

    #region project management reports

    public static MemoryStream ProjectListReport(List<ProjectInitiation> projects)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Project List");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Code";
        worksheet.Cell(1, 2).Value = "Project Name";
        worksheet.Cell(1, 3).Value = "Project Type";
        worksheet.Cell(1, 4).Value = "Project Section";
        worksheet.Cell(1, 5).Value = "Program";
        worksheet.Cell(1, 6).Value = "Sub Program";
        worksheet.Cell(1, 7).Value = "Start Date";
        worksheet.Cell(1, 8).Value = "End Date";
        worksheet.Cell(1, 9).Value = "Project Manager";
        worksheet.Cell(1, 10).Value = "Department";

        // Style the headers
        var headerRange = worksheet.Range("A1:J1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var project in projects)
        {
          worksheet.Cell(row, 1).Value = project?.Code;
          worksheet.Cell(row, 2).Value = project?.Name;
          worksheet.Cell(row, 3).Value = project?.Type;
          worksheet.Cell(row, 4).Value = project?.Section;
          worksheet.Cell(row, 5).Value = project?.Program;
          worksheet.Cell(row, 6).Value = project?.SubProgram;
          worksheet.Cell(row, 7).Value = project?.StartDate;
          worksheet.Cell(row, 8).Value = project.EndDate;
          worksheet.Cell(row, 9).Value = project?.Manager;
          worksheet.Cell(row, 10).Value = project?.Department.deptName;
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 10);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception)
      {

        throw;
      }
    }

    public static MemoryStream ProjectActivityScheduleReport(List<ActivityPlan> activities)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Project Activity Schedule");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Task";
        worksheet.Cell(1, 2).Value = "Task Owner";
        worksheet.Cell(1, 3).Value = "Start Date";
        worksheet.Cell(1, 4).Value = "End Date";
        worksheet.Cell(1, 5).Value = "Status";

        // Style the headers
        var headerRange = worksheet.Range("A1:E1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var project in activities)
        {
          worksheet.Cell(row, 1).Value = project?.Activity;
          worksheet.Cell(row, 2).Value = project?.Person;
          worksheet.Cell(row, 3).Value = project?.StartDate;
          worksheet.Cell(row, 4).Value = project?.EndDate;
          worksheet.Cell(row, 5).Value = ListHelper.GetActivityPlanStatus(project?.Status);
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 5);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception)
      {

        throw;
      }
    }

    public static MemoryStream ProjectRiskManagementReport(List<ProjectRiskIdentification> riskIdentifications)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Project Risk Management");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Risk";
        worksheet.Cell(1, 2).Value = "Rank";

        // Style the headers
        var headerRange = worksheet.Range("A1:B1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var riskIdentification in riskIdentifications)
        {
          worksheet.Cell(row, 1).Value = riskIdentification?.Risk;
          worksheet.Cell(row, 2).Value = riskIdentification?.Rank;
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 2);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception)
      {

        throw;
      }
    }

    public static MemoryStream StakeholderManagementReport(List<StakeHolder> stakeHolders)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Stake Holder Management");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Stakeholder Name";
        worksheet.Cell(1, 2).Value = "Contact Person Name";
        worksheet.Cell(1, 3).Value = "Contact Person Email";
        worksheet.Cell(1, 4).Value = "Contact Person Phone";
        worksheet.Cell(1, 5).Value = "Contact Person Address";
        worksheet.Cell(1, 6).Value = "Contact Person Website";
        worksheet.Cell(1, 7).Value = "Impact";
        worksheet.Cell(1, 8).Value = "Influence";
        worksheet.Cell(1, 9).Value = "What is important to the stakeholder?";
        worksheet.Cell(1, 10).Value = "How could the stakeholder contribute to the project?";
        worksheet.Cell(1, 11).Value = "How could the stakeholder block the project?";
        worksheet.Cell(1, 12).Value = "Strategy for engaging the stakeholder";

        // Style the headers
        var headerRange = worksheet.Range("A1:L1");
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#063241");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Auto fit columns to content
        worksheet.Columns().AdjustToContents();
        AdjustColumnWidths(worksheet, 25, 65);

        int row = 2;
        foreach (var stakeHolder in stakeHolders)
        {
          worksheet.Cell(row, 1).Value = stakeHolder?.StakeholderName;
          worksheet.Cell(row, 2).Value = stakeHolder?.ContactPersonName;
          worksheet.Cell(row, 3).Value = stakeHolder?.ContactPersonEmail;
          worksheet.Cell(row, 4).Value = stakeHolder?.ContactPersonPhone;
          worksheet.Cell(row, 5).Value = stakeHolder?.ContactPersonAddress;
          worksheet.Cell(row, 6).Value = stakeHolder?.ContactPersonWebsite;
          worksheet.Cell(row, 7).Value = ListHelper.GetImpact(stakeHolder?.Impact);
          worksheet.Cell(row, 8).Value = ListHelper.GetImpact(stakeHolder?.Influence);
          worksheet.Cell(row, 9).Value = stakeHolder?.StakeHolderImportant;
          worksheet.Cell(row, 10).Value = stakeHolder?.StakeholderContribution;
          worksheet.Cell(row, 11).Value = stakeHolder?.Stakeholderblock;
          worksheet.Cell(row, 12).Value = stakeHolder?.StakeholderStrategy;
          row++;
        }

        var tableRange = worksheet.Range(1, 1, row - 1, 12);
        var table = tableRange.CreateTable();
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        table.ShowAutoFilter = true;
        table.AutoFilter.IsEnabled = true;
        table.AutoFilter.Sort(1, XLSortOrder.Ascending);

        // Save the workbook to a memory stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
      }
      catch (Exception)
      {

        throw;
      }
    }

    #endregion
    public static string getAchievement(string achieve)
    {
      foreach (var nachieve in ListHelper.AchievementStatus())
      {
        if (nachieve.Value == achieve)
        {
          return nachieve.Text;
        }
      }
      return "NA";
    }

    private static void StyleHeader(IXLRange range)
    {
      var headerStyle = range.Style;
      headerStyle.Fill.BackgroundColor = XLColor.FromHtml("#063241");
      headerStyle.Font.FontColor = XLColor.White;
      headerStyle.Font.Bold = true;
    }

    private static void AdjustColumnWidths(IXLWorksheet worksheet, double minWidth, double maxWidth)
    {
      foreach (var column in worksheet.ColumnsUsed())
      {
        column.AdjustToContents();
        if (column.Width > maxWidth)
        {
          column.Width = maxWidth;
        }
        else if (column.Width < minWidth)
        {
          column.Width = minWidth;
        }
        //column.Style.Alignment.WrapText = true;
      }
    }
  }
}
