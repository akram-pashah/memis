using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections.Generic;
using System.IO;
using MEMIS.Data;
using MEMIS.Data.Project;
using MEMIS.Data.Risk;
using MEMIS.Models.Report;
using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;


namespace MEMIS.Helpers.PdfReports
{
  public class PdfHandler
  {
    public static MemoryStream StrategicImplementationPlanReportToPdf(List<ProgramImplementationPlan> plans)
    {
      var stream = new MemoryStream();

      PdfWriter writer = new PdfWriter(stream);
      PdfDocument pdf = new PdfDocument(writer);
      Document document = new Document(pdf);
      Table table = new Table(13);

      var headerColor = new DeviceRgb(6, 50, 65);
      var headerFontColor = ColorConstants.WHITE;

      string[] headers = {
            "Strategic Objective",
            "Strategic Intervention",
            "Strategic Actions",
            "Activities",
            "Output Indicators",
            "Output Targets",
            "FY 1",
            "FY 2",
            "FY 3",
            "FY 4",
            "FY 5",
            "Means of Verification",
            "Responsible Party"
        };

      foreach (var header in headers)
      {
        Cell cell = new Cell().Add(new Paragraph(header))
            .SetBackgroundColor(headerColor)
            .SetFontColor(headerFontColor)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBold()
            .SetFontSize(5)
            .SetPadding(5);

        table.AddHeaderCell(cell);
      }

      foreach (var plan in plans)
      {
        table.AddCell(new Cell()
            .Add(new Paragraph(plan.StrategicObjectiveFK?.ObjectiveName ?? string.Empty))
            .SetFontSize(5)
            .SetPadding(5));

        table.AddCell(new Cell()
            .Add(new Paragraph(plan.StrategicInterventionFK?.InterventionName ?? string.Empty))
            .SetFontSize(5)
            .SetPadding(5));

        table.AddCell(new Cell()
            .Add(new Paragraph(plan.StrategicActionFK?.actionName ?? string.Empty))
            .SetFontSize(5)
            .SetPadding(5));

        table.AddCell(new Cell()
            .Add(new Paragraph(plan?.ActivityFK?.activityName ?? string.Empty))
            .SetFontSize(5)
            .SetPadding(5));

        table.AddCell(new Cell()
            .Add(new Paragraph(plan.Output ?? string.Empty))
            .SetFontSize(5)
            .SetPadding(5));

        table.AddCell(new Cell()
            .Add(new Paragraph(plan.OutputTarget ?? string.Empty))
            .SetFontSize(5)
            .SetPadding(5));

        table.AddCell(new Cell()
            .Add(new Paragraph(plan?.FY1?.ToString() ?? string.Empty))
            .SetFontSize(5)
            .SetPadding(5));

        table.AddCell(new Cell()
            .Add(new Paragraph(plan?.FY2?.ToString() ?? string.Empty))
            .SetFontSize(5)
            .SetPadding(5));

        table.AddCell(new Cell()
            .Add(new Paragraph(plan?.FY3?.ToString() ?? string.Empty))
            .SetFontSize(5)
            .SetPadding(5));

        table.AddCell(new Cell()
            .Add(new Paragraph(plan?.FY4?.ToString() ?? string.Empty))
            .SetFontSize(5)
            .SetPadding(5));

        table.AddCell(new Cell()
            .Add(new Paragraph(plan?.FY5?.ToString() ?? string.Empty))
            .SetFontSize(5)
            .SetPadding(5));

        table.AddCell(new Cell()
            .Add(new Paragraph(plan?.MeansofVerification ?? string.Empty))
            .SetFontSize(5)
            .SetPadding(5));

        table.AddCell(new Cell()
            .Add(new Paragraph(plan?.ResponsibleParty ?? string.Empty))
            .SetFontSize(5)
            .SetPadding(5));
      }

      document.Add(table);
      document.Close();
      return new MemoryStream(stream.ToArray());
    }
    public static MemoryStream AnnualDetailedResultsFrameworkReportPdf(List<ActivityAssess> activityAssesses)
    {
      var stream = new MemoryStream();

      var pdfWriter = new PdfWriter(stream);
      var pdfDocument = new PdfDocument(pdfWriter);
      var document = new Document(pdfDocument);

      PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
      PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

      Table table = new Table(15);

      string[] headers = {
        "Strategic Objective", "Strategic Intervention", "Strategic Actions", "Activities/Initiatives",
        "Output Indicators", "Baseline", "Annual Target", "Budget Code", "Amount Allocated",
        "Quarter 1", "Quarter 2", "Quarter 3", "Quarter 4", "Identified Risks", "Responsible Party"
      };

      foreach (var header in headers)
      {
        Cell headerCell = new Cell().Add(new Paragraph(header).SetFont(font).SetFontColor(ColorConstants.WHITE))
                                    .SetBackgroundColor(new DeviceRgb(6, 50, 65))
                                    .SetTextAlignment(TextAlignment.CENTER)
                                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                                    .SetFontSize(4)
                                    .SetPadding(5);

        table.AddHeaderCell(headerCell);
      }

      foreach (var activityAssess in activityAssesses)
      {
        table.AddCell(new Cell().Add(new Paragraph(activityAssess?.StrategicIntervention?.StrategicObjective?.ObjectiveName ?? "")
                                     .SetFontSize(4).SetPadding(5)));
        table.AddCell(new Cell().Add(new Paragraph(activityAssess?.StrategicIntervention?.InterventionName ?? "")
                                     .SetFontSize(4).SetPadding(5)));
        table.AddCell(new Cell().Add(new Paragraph(activityAssess?.StrategicAction?.actionName ?? "")
                                     .SetFontSize(4).SetPadding(5)));
        table.AddCell(new Cell().Add(new Paragraph(activityAssess?.ActivityFk?.activityName ?? "")
                                     .SetFontSize(4).SetPadding(5)));
        table.AddCell(new Cell().Add(new Paragraph(activityAssess?.outputIndicator ?? "")
                                     .SetFontSize(4).SetPadding(5)));
        table.AddCell(new Cell().Add(new Paragraph(activityAssess?.baseline?.ToString() ?? "")
                                      .SetFontSize(4).SetPadding(5)));
        table.AddCell(new Cell().Add(new Paragraph(activityAssess?.comparativeTarget?.ToString() ?? "")
                                      .SetFontSize(4).SetPadding(5)));
        table.AddCell(new Cell().Add(new Paragraph(activityAssess?.budgetCode.ToString() ?? "")
                                     .SetFontSize(4).SetPadding(5)));
        table.AddCell(new Cell().Add(new Paragraph(activityAssess?.budgetAmount.ToString() ?? "")
                                     .SetFontSize(4).SetPadding(5)));
        table.AddCell(new Cell().Add(new Paragraph(activityAssess?.QuaterlyPlans.FirstOrDefault(x => x.Quarter == "1")?.QTarget?.ToString() ?? "")
                                      .SetFontSize(4).SetPadding(5)));
        table.AddCell(new Cell().Add(new Paragraph(activityAssess?.QuaterlyPlans.FirstOrDefault(x => x.Quarter == "2")?.QTarget?.ToString() ?? "")
                                      .SetFontSize(4).SetPadding(5)));
        table.AddCell(new Cell().Add(new Paragraph(activityAssess?.QuaterlyPlans.FirstOrDefault(x => x.Quarter == "3")?.QTarget?.ToString() ?? "")
                                     .SetFontSize(4).SetPadding(5)));
        table.AddCell(new Cell().Add(new Paragraph(activityAssess?.QuaterlyPlans.FirstOrDefault(x => x.Quarter == "4")?.QTarget?.ToString() ?? "")
                                      .SetFontSize(4).SetPadding(5)));
        table.AddCell(new Cell().Add(new Paragraph(activityAssess?.IdentifiedRisks ?? "")
                                      .SetFontSize(4).SetPadding(5)));
        table.AddCell(new Cell().Add(new Paragraph(activityAssess?.DepartmentFk?.deptName ?? "")
                                      .SetFontSize(4).SetPadding(5)));
      }

      document.Add(table);
      document.Close();

      return new MemoryStream(stream.ToArray());
    }

    public static MemoryStream ActivityImplementationStatusExportPdf(List<ActivityAssessment> activityAssesses)
    {
      using (var stream = new MemoryStream())
      {
        PdfWriter pdfWriter = new PdfWriter(stream);
        PdfDocument pdfDocument = new PdfDocument(pdfWriter);
        Document document = new Document(pdfDocument);

        Table table = new Table(5);
        table.AddHeaderCell(new Cell().Add(new Paragraph("Strategic Intervention"))
            .SetFontColor(ColorConstants.WHITE)
            .SetBackgroundColor(new DeviceRgb(6, 50, 65))
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER));
        table.AddHeaderCell(new Cell().Add(new Paragraph("Strategic Action"))
            .SetFontColor(ColorConstants.WHITE)
            .SetBackgroundColor(new DeviceRgb(6, 50, 65))
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER));
        table.AddHeaderCell(new Cell().Add(new Paragraph("Activities/Initiatives"))
            .SetFontColor(ColorConstants.WHITE)
            .SetBackgroundColor(new DeviceRgb(6, 50, 65))
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER));
        table.AddHeaderCell(new Cell().Add(new Paragraph("Implementation Status"))
            .SetFontColor(ColorConstants.WHITE)
            .SetBackgroundColor(new DeviceRgb(6, 50, 65))
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER));
        table.AddHeaderCell(new Cell().Add(new Paragraph("Department"))
            .SetFontColor(ColorConstants.WHITE)
            .SetBackgroundColor(new DeviceRgb(6, 50, 65))
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER));

        foreach (var activityAssess in activityAssesses)
        {
          table.AddCell(new Cell().Add(new Paragraph(activityAssess?.strategicIntervention ?? "N/A")));
          table.AddCell(new Cell().Add(new Paragraph(activityAssess?.StrategicAction ?? "N/A")));
          table.AddCell(new Cell().Add(new Paragraph(activityAssess?.activity ?? "N/A")));
          table.AddCell(new Cell().Add(new Paragraph(activityAssess?.ImplementationStatus?.ImpStatusName ?? "N/A")));
          table.AddCell(new Cell().Add(new Paragraph(activityAssess?.DepartmentFk?.deptName ?? "N/A")));
        }

        document.Add(table);

        document.Close();

        return new MemoryStream(stream.ToArray());
      }
    }
    public static MemoryStream QuarterlyReportToPdf(List<RiskTreatmentPlan> plans)
    {
      using (var stream = new MemoryStream())
      {
        PdfWriter writer = new PdfWriter(stream);
        PdfDocument pdf = new PdfDocument(writer);
        Document document = new Document(pdf);

        var bold = iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
        var regular = iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);
        Color headerColor = new DeviceRgb(6, 50, 65); // #063241

        Table table = new Table(11, true);
        table.SetWidth(UnitValue.CreatePercentValue(100));

        string[] headers = { "Action", "Indicator Description", "Cumulative Target", "Q1", "Q2", "Q3", "Q4", "Status", "Data Collection Instrument & Methods", "Means Of Verification", "Responsible Person(s)" };

        foreach (var header in headers)
        {
          Cell headerCell = new Cell()
              .Add(new Paragraph(header).SetFont(bold).SetFontColor(ColorConstants.WHITE))
              .SetBackgroundColor(headerColor)
              .SetTextAlignment(TextAlignment.CENTER);
          table.AddHeaderCell(headerCell);
        }

        foreach (var plan in plans)
        {
          table.AddCell(new Cell().Add(new Paragraph(plan?.TreatmentAction ?? "").SetFont(regular)));
          table.AddCell(new Cell().Add(new Paragraph(plan?.IndicatorDescription ?? "").SetFont(regular)));
          table.AddCell(new Cell().Add(new Paragraph(plan?.CumulativeTarget.ToString() ?? "").SetFont(regular)));

          for (int quarter = 1; quarter <= 4; quarter++)
          {
            string quarterlyValue = plan?.QuarterlyRiskActions?.Where(x => x.Quarter == quarter).FirstOrDefault()?.IncidentValue.ToString() ?? "";
            table.AddCell(new Cell().Add(new Paragraph(quarterlyValue).SetFont(regular)));
          }
          string status = "";
          var averageImpStatus = plan.QuarterlyRiskActions.Count > 0 ? plan.QuarterlyRiskActions?.Average(x => x.ImpStatusId) : 0;

          if (averageImpStatus < 1)
          {
            status = "Not Implemented";
          }
          else if (averageImpStatus < 3)
          {
            status = "Partially Implemented";
          }
          else if (averageImpStatus == 3)
          {
            status = "Fully Implemented";
          }
          else
          {
            status = "NA";
          }
          table.AddCell(new Cell().Add(new Paragraph(status).SetFont(regular)));
          table.AddCell(new Cell().Add(new Paragraph(plan?.DataCollectionInstrumentMethods ?? "").SetFont(regular)));
          table.AddCell(new Cell().Add(new Paragraph(plan?.MeansOfVerification ?? "").SetFont(regular)));
          table.AddCell(new Cell().Add(new Paragraph(plan?.ResponsiblePersons ?? "").SetFont(regular)));
        }
        document.Add(table);
        document.Close();
        return new MemoryStream(stream.ToArray());
      }
    }
    public static MemoryStream AnnualReportPdf(List<RiskRegister> risks)
    {
      using (var stream = new MemoryStream())
      {
        PdfWriter pdfWriter = new PdfWriter(stream);
        PdfDocument pdfDocument = new PdfDocument(pdfWriter);
        Document document = new Document(pdfDocument);

        // Create a table with 6 columns
        Table table = new Table(6);

        // Adding headers to the table
        table.AddHeaderCell(new Cell().Add(new Paragraph("Risk ID"))
            .SetFontColor(ColorConstants.WHITE)
            .SetBackgroundColor(new DeviceRgb(6, 50, 65))
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER));
        table.AddHeaderCell(new Cell().Add(new Paragraph("Risk Title"))
            .SetFontColor(ColorConstants.WHITE)
            .SetBackgroundColor(new DeviceRgb(6, 50, 65))
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER));
        table.AddHeaderCell(new Cell().Add(new Paragraph("Inherent Rating"))
            .SetFontColor(ColorConstants.WHITE)
            .SetBackgroundColor(new DeviceRgb(6, 50, 65))
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER));
        table.AddHeaderCell(new Cell().Add(new Paragraph("Residual Rating"))
            .SetFontColor(ColorConstants.WHITE)
            .SetBackgroundColor(new DeviceRgb(6, 50, 65))
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER));
        table.AddHeaderCell(new Cell().Add(new Paragraph("Risk Movement"))
            .SetFontColor(ColorConstants.WHITE)
            .SetBackgroundColor(new DeviceRgb(6, 50, 65))
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER));
        table.AddHeaderCell(new Cell().Add(new Paragraph("Control Effectiveness"))
            .SetFontColor(ColorConstants.WHITE)
            .SetBackgroundColor(new DeviceRgb(6, 50, 65))
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER));

        foreach (var risk in risks)
        {
          table.AddCell(new Cell().Add(new Paragraph(risk?.RiskRefID.ToString() ?? "N/A")));
          table.AddCell(new Cell().Add(new Paragraph(risk?.RiskDescription ?? "N/A")));
          table.AddCell(new Cell().Add(new Paragraph(risk?.RiskRatingCategory ?? "N/A")));
          table.AddCell(new Cell().Add(new Paragraph(risk?.RiskResidualRank ?? "N/A")));

          int inheringRating = (risk?.RiskConsequenceId ?? 0) * (risk?.RiskLikelihoodId ?? 0);
          int residualRating = (risk?.RiskResidualConsequenceId ?? 0) * (risk?.RiskResidualLikelihoodId ?? 0);
          string movement = inheringRating == residualRating ? "Same" : inheringRating < residualRating ? "Reduced" : "Increased";

          table.AddCell(new Cell().Add(new Paragraph(movement)));
          table.AddCell(new Cell().Add(new Paragraph(risk?.ControlEffectiveness ?? "N/A")));
        }

        document.Add(table);

        document.Close();
        return new MemoryStream(stream.ToArray());
      }
    }
    public static MemoryStream RiskRegisterReportPdf(List<RiskRegister> risks)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        Table table = new Table(14);

        var headerCells = new[]
        {
            "Objective",
            "Risk Reference Code/Number",
            "Risk Description",
            "Risk Category",
            "Risk Driver/Root Cause",
            "Consequence/Impact Definition",
            "Existing Mitigation",
            "Likelihood",
            "Consequence",
            "Inherent Risk Rating",
            "Further Action Required/Additional Mitigation Strategies",
            "Opportunity",
            "Review/Implementation Date",
            "Risk Owner"
        };
        foreach (var header in headerCells)
        {
          table.AddHeaderCell(new Cell().Add(new Paragraph(header))
              .SetFontColor(ColorConstants.WHITE)
              .SetBackgroundColor(new DeviceRgb(6, 50, 65))
              .SetBold()
              .SetFontSize(3)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }

        foreach (var risk in risks)
        {
          table.AddCell(new Cell().Add(new Paragraph(risk?.StrategicPlanFk?.ObjectiveName ?? "N/A"))
              .SetFontSize(3)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph("")));
          table.AddCell(new Cell().Add(new Paragraph(risk?.RiskDescription ?? "N/A"))
              .SetFontSize(3)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(risk?.RiskRatingCategory ?? "N/A"))
              .SetFontSize(3)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph("")));
          table.AddCell(new Cell().Add(new Paragraph("")));
          table.AddCell(new Cell().Add(new Paragraph("")));
          table.AddCell(new Cell().Add(new Paragraph(risk?.RiskLikelihoodId.ToString() ?? "N/A"))
              .SetFontSize(3)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph("")));
          table.AddCell(new Cell().Add(new Paragraph(risk?.RiskRatingId.ToString() ?? "N/A"))
              .SetFontSize(3)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(risk?.ActionTaken ?? "N/A"))
              .SetFontSize(3)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(risk?.Opportunity ?? "N/A"))
              .SetFontSize(3)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(risk?.ReviewDate?.ToShortDateString() ?? "N/A"))
              .SetFontSize(3)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(risk?.RiskOwner ?? "N/A"))
              .SetFontSize(3)
              .SetPadding(5));
        }
        document.Add(table);
      }
      return new MemoryStream(stream.ToArray());
    }
    public static MemoryStream RiskTreatmentReportPdf(List<RiskTreatmentPlan> plans)
    {
      var stream = new MemoryStream();
      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        Table table = new Table(12);

        var headerCells = new[]
        {
            "Treatment Action",
            "Indicator Description",
            "Baseline",
            "Cumulative Target",
            "Q1",
            "Q2",
            "Q3",
            "Q4",
            "Data Collection Instrument & Methods",
            "Frequency Of Reporting",
            "Means Of Verification",
            "Responsible Person(s)"
        };
        foreach (var header in headerCells)
        {
          table.AddHeaderCell(new Cell().Add(new Paragraph(header))
              .SetFontColor(ColorConstants.WHITE)
              .SetBackgroundColor(new DeviceRgb(6, 50, 65))
              .SetBold()
              .SetFontSize(6)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }
        foreach (var plan in plans)
        {
          table.AddCell(new Cell().Add(new Paragraph(plan?.TreatmentAction ?? "N/A"))
              .SetFontSize(6)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(plan?.IndicatorDescription ?? "N/A"))
              .SetFontSize(6)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(plan?.Baseline.ToString() ?? "N/A"))
              .SetFontSize(6)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(plan?.CumulativeTarget.ToString() ?? "N/A"))
              .SetFontSize(6)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(plan?.QuarterlyRiskActions?.FirstOrDefault(x => x.Quarter == 1)?.IncidentValue?.ToString() ?? "N/A"))
              .SetFontSize(6)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(plan?.QuarterlyRiskActions?.FirstOrDefault(x => x.Quarter == 2)?.IncidentValue?.ToString() ?? "N/A"))
              .SetFontSize(6)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(plan?.QuarterlyRiskActions?.FirstOrDefault(x => x.Quarter == 3)?.IncidentValue?.ToString() ?? "N/A"))
              .SetFontSize(6)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(plan?.QuarterlyRiskActions?.FirstOrDefault(x => x.Quarter == 4)?.IncidentValue?.ToString() ?? "N/A"))
              .SetFontSize(6)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(plan?.DataCollectionInstrumentMethods ?? "N/A"))
              .SetFontSize(6)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(plan?.FrequencyOfReporting ?? "N/A"))
              .SetFontSize(6)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(plan?.MeansOfVerification ?? "N/A"))
              .SetFontSize(6)
              .SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(plan?.ResponsiblePersons ?? "N/A"))
              .SetFontSize(6)
              .SetPadding(5));
        }
        document.Add(table);
      }

      return new MemoryStream(stream.ToArray());
    }

    public static MemoryStream RiskMonitoringReportPdf(List<RiskTreatmentPlan> plans)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(13).UseAllAvailableWidth();

        string[] headers = {
        "Risk Code", "Risk Description", "Proposed mitigation actions", "Actions Undertaken",
        "Status", "Risk Incidents/indicators in the period", "Incident Value",
        "Inherent Likelihood", "Inherent Consequence", "Inherent Risk Rating",
        "Residual Likelihood", "Residual Consequence", "Residual Risk Rating"
      };

        foreach (var header in headers)
        {
          Cell headerCell = new Cell()
              .Add(new Paragraph(header)
              .SetFontColor(ColorConstants.WHITE))
              .SetBackgroundColor(new DeviceRgb(6, 50, 65))
              .SetTextAlignment(TextAlignment.CENTER)
              .SetVerticalAlignment(VerticalAlignment.MIDDLE)
              .SetBold()
              .SetFontSize(5)
              .SetPadding(5);
          table.AddHeaderCell(headerCell);
        }

        foreach (var plan in plans)
        {
          table.AddCell(new Cell()
              .Add(new Paragraph(plan?.RiskRegister?.RiskCode ?? "")
              .SetFontSize(5)).SetPadding(5));
          table.AddCell(new Cell()
              .Add(new Paragraph(plan?.RiskRegister?.RiskDescription ?? "")
              .SetFontSize(5)).SetPadding(5));

          table.AddCell(new Cell()
              .Add(new Paragraph(plan?.TreatmentAction ?? "")
              .SetFontSize(5)).SetPadding(5));

          table.AddCell(new Cell()
              .Add(new Paragraph(plan?.RiskRegister?.ActionTaken ?? "")
              .SetFontSize(5)).SetPadding(5));

          string status = "";
          if (plan.QuarterlyRiskActions != null && plan.QuarterlyRiskActions.Any())
          {
            var averageImpStatus = plan.QuarterlyRiskActions.Average(x => x.ImpStatusId);

            if (averageImpStatus < 1)
              status = "Not Implemented";
            else if (averageImpStatus < 3)
              status = "Partially Implemented";
            else if (averageImpStatus == 3)
              status = "Fully Implemented";
            else
              status = "N/A";
          }
          else
          {
            status = "N/A"; // Default status if no actions exist
          }

          table.AddCell(new Cell()
              .Add(new Paragraph(status)
              .SetFontSize(5)).SetPadding(5));

          table.AddCell(new Cell()
              .Add(new Paragraph(plan?.IndicatorDescription ?? "")
              .SetFontSize(5)).SetPadding(5));

          table.AddCell(new Cell()
              .Add(new Paragraph(plan?.QuarterlyRiskActions.Count > 0
                  ? plan?.QuarterlyRiskActions?.Sum(x => x.IncidentValue).ToString()
                  : "0")
              .SetFontSize(5)).SetPadding(5));

          table.AddCell(new Cell()
              .Add(new Paragraph(plan?.RiskRegister?.RiskIdentificationFk?.RiskLikelihoodId.ToString() ?? "")
              .SetFontSize(5)).SetPadding(5));

          table.AddCell(new Cell()
              .Add(new Paragraph(plan?.RiskRegister?.RiskIdentificationFk?.RiskConsequenceId.ToString() ?? "")
              .SetFontSize(5)).SetPadding(5));

          table.AddCell(new Cell()
              .Add(new Paragraph("")
              .SetFontSize(5)).SetPadding(5));

          table.AddCell(new Cell()
              .Add(new Paragraph(plan?.RiskRegister?.RiskLikelihoodId.ToString() ?? "")
              .SetFontSize(5)).SetPadding(5));

          table.AddCell(new Cell()
              .Add(new Paragraph(plan?.RiskRegister?.RiskConsequenceId.ToString() ?? "")
              .SetFontSize(5)).SetPadding(5));

          table.AddCell(new Cell()
              .Add(new Paragraph(plan?.RiskRegister?.RiskRatingId.ToString() ?? "")
              .SetFontSize(5)).SetPadding(5));
        }

        document.Add(table);
        document.Close();
      }
      return new MemoryStream(stream.ToArray());
    }

    public static MemoryStream ProjectListReportPdf(List<ProjectInitiation> projects)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(10).UseAllAvailableWidth();

        string[] headers = { "Code", "Project Name", "Project Type", "Project Section", "Program", "Sub Program", "Start Date", "End Date", "Project Manager", "Department" };
        foreach (var header in headers)
        {
          table.AddHeaderCell(new Cell()
              .Add(new Paragraph(header).SetBold().SetFontSize(10))
              .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }
        foreach (var project in projects)
        {
          table.AddCell(new Cell().Add(new Paragraph(project?.Code ?? "").SetFontSize(8)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(project?.Name ?? "").SetFontSize(8)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(project?.Type.ToString() ?? "").SetFontSize(8)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(project?.Section ?? "").SetFontSize(8)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(project?.Program ?? "").SetFontSize(8)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(project?.SubProgram ?? "").SetFontSize(8)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(project?.StartDate.ToString("dd-MM-yyyy") ?? "").SetFontSize(8)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(project?.EndDate.ToString("dd-MM-yyyy") ?? "").SetFontSize(8)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(project?.Manager ?? "").SetFontSize(8)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(project?.Department?.deptName ?? "").SetFontSize(8)).SetPadding(5));
        }
        document.Add(table);

        document.Close();
      }
      return new MemoryStream(stream.ToArray());
    }

    public static MemoryStream ProjectActivityScheduleReportPdf(List<ActivityPlan> activities)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(5).UseAllAvailableWidth();
        string[] headers = { "Task", "Task Owner", "Start Date", "End Date", "Status" };
        foreach (var header in headers)
        {
          table.AddHeaderCell(new Cell()
              .Add(new Paragraph(header).SetBold().SetFontSize(10))
              .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }
        foreach (var project in activities)
        {
          table.AddCell(new Cell().Add(new Paragraph(project?.Activity ?? "").SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(project?.Person ?? "").SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(project?.StartDate.ToString("yyyy-MM-dd") ?? "").SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(project?.EndDate.ToString("yyyy-MM-dd") ?? "").SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(ListHelper.GetActivityPlanStatus(project?.Status)).SetFontSize(10).SetPadding(5)));
        }
        document.Add(table);

        document.Close();
      }
      return new MemoryStream(stream.ToArray());
    }

    public static MemoryStream ProjectRiskManagementReportPdf(List<ProjectRiskIdentification> riskIdentifications)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(9).UseAllAvailableWidth();
        string[] headers = { "Stage", "Risk", "Likelihood", "Severity", "Rank", "Consequence", "Mitigation", "Cost Of Implementing The Risk", "Ownership" };
        foreach (var header in headers)
        {
          table.AddHeaderCell(new Cell()
              .Add(new Paragraph(header).SetBold().SetFontSize(10))
              .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }
        foreach (var riskIdentification in riskIdentifications)
        {
          table.AddCell(new Cell().Add(new Paragraph(riskIdentification?.Stage ?? "").SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(riskIdentification?.Risk ?? "").SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(Enum.GetName(typeof(MEMIS.EnumRiskRank), riskIdentification?.Likelihood) ?? "").SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(Enum.GetName(typeof(MEMIS.EnumRiskRank), riskIdentification?.Severity) ?? "").SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(riskIdentification?.Rank.ToString() ?? "").SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(riskIdentification?.Consequence ?? "").SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(riskIdentification?.Mitigation ?? "").SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(riskIdentification?.RiskImplementationCost.ToString() ?? "").SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(riskIdentification?.Ownership ?? "").SetFontSize(10).SetPadding(5)));
        }
        document.Add(table);

        document.Close();
      }
      return new MemoryStream(stream.ToArray());
    }

    public static MemoryStream StakeholderManagementReportPdf(List<StakeHolder> stakeHolders)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(12).UseAllAvailableWidth();
        string[] headers = { "Stakeholder Name", "Contact Person Name", "Contact Person Email", "Contact Person Phone", "Contact Person Address", "Contact Person Website", "Impact", "Influence", "What is important to the stakeholder?", "How could the stakeholder contribute to the project?", "How could the stakeholder block the project?", "Strategy for engaging the stakeholder" };
        foreach (var header in headers)
        {
          table.AddHeaderCell(new Cell()
              .Add(new Paragraph(header).SetBold().SetFontSize(8))
              .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }
        foreach (var stakeHolder in stakeHolders)
        {
          table.AddCell(new Cell().Add(new Paragraph(stakeHolder?.StakeholderName ?? "").SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(stakeHolder?.ContactPersonName ?? "").SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(stakeHolder?.ContactPersonEmail ?? "").SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(stakeHolder?.ContactPersonPhone ?? "").SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(stakeHolder?.ContactPersonAddress ?? "").SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(stakeHolder?.ContactPersonWebsite ?? "").SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(ListHelper.GetImpact(stakeHolder?.Impact)).SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(ListHelper.GetImpact(stakeHolder?.Influence)).SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(stakeHolder?.StakeHolderImportant ?? "").SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(stakeHolder?.StakeholderContribution ?? "").SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(stakeHolder?.Stakeholderblock ?? "").SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(stakeHolder?.StakeholderStrategy ?? "").SetFontSize(8).SetPadding(5)));
        }
        document.Add(table);

        document.Close();
      }
      return new MemoryStream(stream.ToArray());
    }

    public static MemoryStream SDTQuarterlyPerformanceReportPdf(List<SDTAssessment> assessments)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(15).UseAllAvailableWidth();
        string[] headers = { "Month", "SDT", "SDT Numerator", "SDT Denominator", "Numerator Perf", "Denominator Perf", "Implemented Within Timeline", "Average Working Days", "% Implemented Within Timeline", "Target", "Achievement Status", "% Variance", "Justification", "Rating", "Department" };
        foreach (var header in headers)
        {
          table.AddHeaderCell(new Cell()
              .Add(new Paragraph(header).SetBold().SetFontSize(5))
              .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }
        foreach (var assessment in assessments)
        {
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Month.ToString() ?? "N/A").SetFontSize(5)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.SDTMasterFk?.ServiceDeliveryTimeline ?? "N/A").SetFontSize(5)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.SDTMasterFk?.Numerator?.ToString()).SetFontSize(5)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.SDTMasterFk?.Denominator?.ToString()).SetFontSize(5)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Numerator.ToString()).SetFontSize(5)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Denominator.ToString()).SetFontSize(5)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.ImplementedTimeline.ToString() ?? "N/A").SetFontSize(5)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Rate.ToString()).SetFontSize(5)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.ProportionTimeline.ToString()).SetFontSize(5)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Target.ToString()).SetFontSize(5)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.AchivementStatus.ToString() ?? "N/A").SetFontSize(5)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Variance?.ToString()).SetFontSize(5)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Justification ?? "N/A").SetFontSize(5)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Rating?.ToString() ?? "N/A").SetFontSize(5)).SetPadding(5));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.SDTMasterFk?.DepartmentFk?.deptName ?? "N/A").SetFontSize(5)).SetPadding(5));
        }
        document.Add(table);

        document.Close();
      }
      return new MemoryStream(stream.ToArray());
    }

    public static MemoryStream KPIMandEReportPdf(List<KPIAssessment> assessments)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(11).UseAllAvailableWidth();
        string[] headers = {
                    "Performance Indicator", "Frequency Of Reporting", "Indicator Formulae",
                    "Indicator Definition", "FY", "Target", "Numerator",
                    "Denominator", "Rate", "Achieved", "Justification"
        };
        foreach (var header in headers)
        {
          table.AddHeaderCell(new Cell()
              .Add(new Paragraph(header).SetBold().SetFontSize(8))
              .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }
        foreach (var assessment in assessments)
        {
          table.AddCell(new Cell().Add(new Paragraph(assessment?.PerformanceIndicator ?? string.Empty).SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.FrequencyofReporting.ToString() ?? string.Empty).SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.IndicatorFormulae ?? string.Empty).SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.IndicatorDefinition ?? string.Empty).SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.FY.ToString() ?? string.Empty).SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Target?.ToString() ?? string.Empty).SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Numerator?.ToString() ?? string.Empty).SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Denominator?.ToString() ?? string.Empty).SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Rate?.ToString() ?? string.Empty).SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Achieved ?? string.Empty).SetFontSize(8).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Justification ?? string.Empty).SetFontSize(8).SetPadding(5)));
        }
        document.Add(table);

        document.Close();
      }
      return new MemoryStream(stream.ToArray());
    }
    public static MemoryStream KPIMandEFrameworkReportPdf(List<KPIMaster> assessments)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(16).UseAllAvailableWidth();
        string[] headers = {
                "Performance Indicator", "Type of Indicator", "Indicator Formulae",
                "Indicator Definition", "Original Baseline", "Indicator Classification",
                "Data Type", "Unit of Measure", "Frequency of Reporting",
                "Target for FY 1", "Target for FY 2", "Target for FY 3",
                "Target for FY 4", "Target for FY 5", "Means of Verification",
                "Department"
        };
        foreach (var header in headers)
        {
          table.AddHeaderCell(new Cell()
              .Add(new Paragraph(header).SetBold().SetFontSize(5))
              .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }
        foreach (var assessment in assessments)
        {
          table.AddCell(new Cell().Add(new Paragraph(assessment?.PerformanceIndicator ?? "").SetFontSize(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.TypeofIndicator.ToString() ?? "").SetFontSize(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.IndicatorFormulae ?? "").SetFontSize(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.IndicatorDefinition ?? "").SetFontSize(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.OriginalBaseline.ToString() ?? "").SetFontSize(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Indicatorclassification.ToString() ?? "").SetFontSize(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.DataType ?? "").SetFontSize(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.Unitofmeasure ?? "").SetFontSize(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.FrequencyofReporting.ToString() ?? "").SetFontSize(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.FY1?.ToString() ?? "").SetFontSize(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.FY2?.ToString() ?? "").SetFontSize(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.FY3?.ToString() ?? "").SetFontSize(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.FY4?.ToString() ?? "").SetFontSize(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.FY5?.ToString() ?? "").SetFontSize(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.MeansofVerification ?? "").SetFontSize(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.ResponsibleParty ?? "").SetFontSize(5)));
        }
        document.Add(table);

        document.Close();
      }
      return new MemoryStream(stream.ToArray());
    }

    public static MemoryStream OpTargetPerfAcheivReportPdf(List<ActivityAssessment> assessments)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(5).UseAllAvailableWidth();
        string[] headers = { "Strategic Action", "Activity", "Output/Target", "Performance Achievement Status", "Department" };
        foreach (var header in headers)
        {
          table.AddHeaderCell(new Cell()
              .Add(new Paragraph(header).SetBold().SetFontSize(10))
              .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }
        foreach (var assessment in assessments)
        {
          table.AddCell(new Cell().Add(new Paragraph(assessment?.StrategicAction ?? string.Empty).SetFont(regularFont).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.activity ?? string.Empty).SetFont(regularFont).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.comparativeTarget.ToString() ?? string.Empty).SetFont(regularFont).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.ImplementationStatus?.ImpStatusName ?? string.Empty).SetFont(regularFont).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(assessment?.DepartmentFk?.deptName ?? string.Empty).SetFont(regularFont).SetFontSize(10).SetPadding(5)));
        }

        document.Add(table);

        document.Close();
      }
      return new MemoryStream(stream.ToArray());

    }
    public static MemoryStream StrategicActionPerfAchievReportPdf(List<StrategicActionPerformanceAchievementDto> actions)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(2).UseAllAvailableWidth();
        string[] headers = { "Strategic Action", "Performance Achievement Status" };
        foreach (var header in headers)
        {
          table.AddHeaderCell(new Cell()
              .Add(new Paragraph(header).SetBold().SetFontSize(12))
              .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }
        foreach (var action in actions)
        {
          table.AddCell(new Cell().Add(new Paragraph(action?.StrategicAction ?? string.Empty).SetFont(regularFont).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(action?.PerformanceAchievementStatus.ToString() ?? string.Empty).SetFont(regularFont).SetFontSize(10).SetPadding(5)));
        }
        document.Add(table);

        document.Close();
      }
      return new MemoryStream(stream.ToArray());

    }
    public static MemoryStream StrategicInterventionPerfAchievReportPdf(List<StrategicInterventionPerformanceAchievementDto> actions)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(2).UseAllAvailableWidth();
        string[] headers = { "Strategic Intervention", "Performance Achievement Status" };
        foreach (var header in headers)
        {
          table.AddHeaderCell(new Cell()
              .Add(new Paragraph(header).SetFont(boldFont).SetFontSize(10))
              .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }

        foreach (var action in actions)
        {
          table.AddCell(new Cell().Add(new Paragraph(action?.StrategicIntervention ?? string.Empty).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(action?.PerformanceAchievementStatus.ToString() ?? string.Empty).SetFontSize(10).SetPadding(5)));
        }
        document.Add(table);

        document.Close();
      }
      return new MemoryStream(stream.ToArray());

    }

    public static MemoryStream StrategicObjectivePerfAchievReportPdf(List<StrategicObjectivePerformanceAchievementDto> actions)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(2).UseAllAvailableWidth();
        string[] headers = { "Strategic Objective", "Performance Achievement Status" };
        foreach (var header in headers)
        {
          table.AddHeaderCell(new Cell()
              .Add(new Paragraph(header).SetFont(boldFont).SetFontSize(10))
              .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }

        foreach (var action in actions)
        {
          table.AddCell(new Cell().Add(new Paragraph(action?.StrategicObjective ?? string.Empty).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(action?.PerformanceAchievementStatus.ToString() ?? string.Empty).SetFontSize(10).SetPadding(5)));
        }
        document.Add(table);

        document.Close();
      }
      return new MemoryStream(stream.ToArray());

    }
    public static MemoryStream OutcomePerfAchievReportPdf(List<ActivityAssessment> activities)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(3).UseAllAvailableWidth();
        string[] headers = { "Strategic Objective", "Performance Achievement Status", "Department" };
        foreach (var header in headers)
        {
          table.AddHeaderCell(new Cell()
              .Add(new Paragraph(header).SetFont(boldFont).SetFontSize(10))
              .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }

        foreach (var activity in activities)
        {
          table.AddCell(new Cell().Add(new Paragraph(activity?.strategicObjective ?? string.Empty).SetFont(regularFont).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(activity?.ImplementationStatus?.ImpStatusName ?? string.Empty).SetFont(regularFont).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(activity?.DepartmentFk?.deptName ?? string.Empty).SetFont(regularFont).SetFontSize(10).SetPadding(5)));
        }
        document.Add(table);

        document.Close();
      }
      return new MemoryStream(stream.ToArray());
    }
    public static MemoryStream ImpactPerfAchievReportPdf(List<ActivityAssessment> activities)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(3).UseAllAvailableWidth();
        string[] headers = { "Strategic Objective", "Performance Achievement Status", "Department" };
        foreach (var header in headers)
        {
          table.AddHeaderCell(new Cell()
              .Add(new Paragraph(header).SetFont(boldFont).SetFontSize(10))
              .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }

        foreach (var activity in activities)
        {
          table.AddCell(new Cell().Add(new Paragraph(activity?.strategicObjective ?? string.Empty).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(activity?.ImplementationStatus?.ImpStatusName ?? string.Empty).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(activity?.DepartmentFk?.deptName ?? string.Empty).SetFontSize(10).SetPadding(5)));
        }
        document.Add(table);

        document.Close();
      }
      return new MemoryStream(stream.ToArray());
    }
    public static MemoryStream SdtMAndEFrameworkReportPdf(List<SDTMaster> sDTMasters)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(7).UseAllAvailableWidth();
        string[] headers = {
                "SDT Indicator", "Measure", "Evaluation Period",
                "Target", "Numerator", "Denominator", "Department"
        };

        foreach (var header in headers)
        {
          table.AddHeaderCell(new Cell()
              .Add(new Paragraph(header).SetFont(boldFont).SetFontSize(10))
              .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }
        foreach (var activity in sDTMasters)
        {
          table.AddCell(new Cell().Add(new Paragraph(activity?.ServiceDeliveryTimeline ?? string.Empty).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(activity?.Measure ?? string.Empty).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(activity?.EvaluationPeriod ?? string.Empty).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(activity?.Target ?? string.Empty).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(activity?.Numerator?.ToString() ?? string.Empty).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(activity?.Denominator?.ToString() ?? string.Empty).SetFontSize(10).SetPadding(5)));
          table.AddCell(new Cell().Add(new Paragraph(activity?.DepartmentFk?.deptName ?? string.Empty).SetFontSize(10).SetPadding(5)));
        }
        document.Add(table);

        document.Close();
      }
      return new MemoryStream(stream.ToArray());
    }
    public static MemoryStream StrategicPlanOutputMonitoringTrackerExportPdf(List<StrategicObjectiveReport> activityAssesses)
    {
      var stream = new MemoryStream();

      using (var pdfWriter = new PdfWriter(stream))
      using (var pdfDocument = new PdfDocument(pdfWriter))
      using (var document = new Document(pdfDocument))
      {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        Table table = new Table(10).UseAllAvailableWidth();
        string[] headers = {
                "Strategic Objectives", "Strategic Interventions", "Strategic Actions",
                "FY 1", "FY 2", "FY 3", "FY 4", "FY 5",
                "Cumulative Overall Performance on Strategic Intervention",
                "Cumulative Overall Performance on Strategic Objective 1"
        };
        foreach (var header in headers)
        {
          table.AddHeaderCell(new Cell()
              .Add(new Paragraph(header).SetFont(boldFont).SetFontSize(10))
              .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetPadding(5));
        }
        foreach (var objective in activityAssesses)
        {
          table.AddCell(new Cell(1, 1).Add(new Paragraph(objective.StrategicObjective).SetFont(regularFont).SetFontSize(10).SetPadding(5)));

          foreach (var intervention in objective.StrategicInterventions)
          {
            table.AddCell(new Cell(1, 1).Add(new Paragraph(intervention.StrategicIntervention).SetFont(regularFont).SetFontSize(10).SetPadding(5)));

            foreach (var action in intervention.StrategicActions)
            {
              table.AddCell(new Cell(1, 1).Add(new Paragraph(action.StrategicAction).SetFont(regularFont).SetFontSize(10).SetPadding(5)));
              table.AddCell(new Cell(1, 1).Add(new Paragraph(action.FiscalYearData[0].ToString() ?? "").SetFont(regularFont).SetFontSize(10).SetPadding(5)));
              table.AddCell(new Cell(1, 1).Add(new Paragraph(action.FiscalYearData[1].ToString() ?? "").SetFont(regularFont).SetFontSize(10).SetPadding(5)));
              table.AddCell(new Cell(1, 1).Add(new Paragraph(action.FiscalYearData[2].ToString() ?? "").SetFont(regularFont).SetFontSize(10).SetPadding(5)));
              table.AddCell(new Cell(1, 1).Add(new Paragraph(action.FiscalYearData[3].ToString() ?? "").SetFont(regularFont).SetFontSize(10).SetPadding(5)));
              table.AddCell(new Cell(1, 1).Add(new Paragraph(action.FiscalYearData[4].ToString() ?? "").SetFont(regularFont).SetFontSize(10).SetPadding(5)));
            }

            table.AddCell(new Cell(1, 1).Add(new Paragraph(Math.Round(intervention.AverageFiscalYearData.Average(), 2).ToString()).SetFont(regularFont).SetFontSize(10).SetPadding(5)));
          }

          table.AddCell(new Cell(1, 1).Add(new Paragraph(Math.Round(objective.AverageFiscalYearData.Average(), 2).ToString()).SetFont(regularFont).SetFontSize(10).SetPadding(5)));
        }
        document.Add(table);

        document.Close();
      }
      return new MemoryStream(stream.ToArray());
    }


  }
}


