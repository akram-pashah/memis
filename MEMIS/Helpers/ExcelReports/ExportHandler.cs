using ClosedXML.Excel;
using MEMIS.Data;
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
          worksheet.Cell(row, 4).Value = plan?.Activity;
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
          worksheet.Cell(row, 4).Value = plan?.Activity;
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
        worksheet.Cell(1, 1).Value = "Strategic Objectives";
        worksheet.Cell(1, 2).Value = "Strategic Interventions";
        worksheet.Cell(1, 3).Value = "Strategic Actions";
        worksheet.Cell(1, 4).Value = "FY1";
        worksheet.Cell(1, 5).Value = "FY2";
        worksheet.Cell(1, 6).Value = "FY3";
        worksheet.Cell(1, 7).Value = "FY4";
        worksheet.Cell(1, 8).Value = "FY5";
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

          foreach(var intervention in objective.StrategicInterventions)
          {
            worksheet.Cell(row, 2).Value = intervention.StrategicIntervention;
            foreach(var action in intervention.StrategicActions)
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
    public static MemoryStream KPIMandEReport(List<KPIAssessment> assessments)
    {
      try
      {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("KPI M&E");

        // Setting the header row
        worksheet.Cell(1, 1).Value = "Financial Year";
        worksheet.Cell(1, 2).Value = "Performance Indicator";
        worksheet.Cell(1, 3).Value = "Frequency Of Reporting";
        worksheet.Cell(1, 4).Value = "Numerator Description";
        worksheet.Cell(1, 5).Value = "Denominator Description";
        worksheet.Cell(1, 6).Value = "Target";
        worksheet.Cell(1, 7).Value = "Numerator";
        worksheet.Cell(1, 8).Value = "Denominator";
        worksheet.Cell(1, 9).Value = "Performance";
        worksheet.Cell(1, 10).Value = "Rating";
        worksheet.Cell(1, 11).Value = "Justification";
        worksheet.Cell(1, 12).Value = "Department";

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
        foreach (var assessment in assessments)
        {
          worksheet.Cell(row, 1).Value = assessment?.FY;
          worksheet.Cell(row, 2).Value = assessment?.PerformanceIndicator;
          worksheet.Cell(row, 3).Value = assessment?.FrequencyofReporting;
          worksheet.Cell(row, 4).Value = assessment?.IndicatorFormulae;
          worksheet.Cell(row, 5).Value = assessment?.IndicatorDefinition;
          worksheet.Cell(row, 6).Value = assessment?.Target;
          worksheet.Cell(row, 7).Value = assessment?.Numerator;
          worksheet.Cell(row, 8).Value = assessment?.Denominator;
          worksheet.Cell(row, 9).Value = assessment?.Rate;
          worksheet.Cell(row, 10).Value = getAchievement(assessment?.Achieved ?? "");
          worksheet.Cell(row, 10).Style.Font.FontColor = assessment?.Achieved == "1.0" ? XLColor.FromHtml("#00b050") : assessment?.Achieved == "0.5" ? XLColor.FromHtml("#fefe00") : XLColor.FromHtml("#fd0000");
          worksheet.Cell(row, 11).Value = assessment?.Justification;
          worksheet.Cell(row, 12).Value = assessment?.KPIMasterFk?.ResponsibleParty;

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
      catch (Exception ex)
      {
        throw;
      }
    }

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
