namespace MEMIS.Models.Report
{
  public class StrategicPlanActivityModels
  {
  }
  public class StrategicObjectiveReport
  {
    public string StrategicObjective { get; set; }
    public List<StrategicInterventionReport> StrategicInterventions { get; set; }
    public double[] AverageFiscalYearData
    {
      get
      {
        double[] sumFiscalYearData = new double[5];
        int interventionCount = StrategicInterventions.Count;

        foreach (var intervention in StrategicInterventions)
        {
          var averageData = intervention.AverageFiscalYearData;
          for (int i = 0; i < 5; i++)
          {
            sumFiscalYearData[i] += averageData[i];
          }
        }

        double[] averageFiscalYearData = new double[5];
        for (int i = 0; i < 5; i++)
        {
          if (interventionCount > 0)
          {
            averageFiscalYearData[i] = sumFiscalYearData[i] / interventionCount;
          }
          else
          {
            averageFiscalYearData[i] = 0;
          }
        }

        return averageFiscalYearData;
      }
    }
  }

  public class StrategicInterventionReport
  {
    public string StrategicIntervention { get; set; }
    public List<StrategicActionReport> StrategicActions { get; set; }
    public double[] AverageFiscalYearData
    {
      get
      {
        double[] sumFiscalYearData = new double[5];
        int actionCount = StrategicActions.Count;

        foreach (var action in StrategicActions)
        {
          for (int i = 0; i < 5; i++)
          {
            sumFiscalYearData[i] += action.FiscalYearData[i];
          }
        }

        double[] averageFiscalYearData = new double[5];
        for (int i = 0; i < 5; i++)
        {
          if (actionCount > 0)
          {
            averageFiscalYearData[i] = sumFiscalYearData[i] / actionCount;
          }
          else
          {
            averageFiscalYearData[i] = 0;
          }
        }

        return averageFiscalYearData;
      }
    }
  }

  public class StrategicActionReport
  {
    public string StrategicAction { get; set; }
    public double[] FiscalYearData { get; set; } // Stores data for FY1, FY2, FY3, FY4, FY5
  }
}
