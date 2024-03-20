using MEMIS.Data.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
    public class ActivityAssessment
    {
        [Key]
        public int intDeptPlan { get; set; }
        public string strategicObjective { get; set; }
        [Display(Name = "Strategic Intervention")]
        public string strategicIntervention { get; set; }
        [Display(Name = "Strategic Action")]
        public string StrategicAction { get; set; }
        [Display(Name = "Activity / Initiative")]
        public string activity { get; set; }
        [Display(Name = "Output Indicators")]
        public string outputIndicator { get; set; }
        [Display(Name = "Baseline")]
        public double? baseline { get; set; }
        [Display(Name = "Budget Code")]
        public double? budgetCode { get; set; }
        [Display(Name = "Unit Cost")]
        public double? unitCost { get; set; }
        [Display(Name = "Comparative Target")]
        public double? comparativeTarget { get; set; }
        [Display(Name = "Justification")]
        public string? justification { get; set; }
        [Display(Name = "Budget Amount")]
        public double? budgetAmount { get; set; }
        [Display(Name = "Q1 Target")]
        public double? Q1Target { get; set; }
        [Display(Name = "Q1 Budget")]
        public double? Q1Budget { get; set; }
        [Display(Name = "Q1 Actual")]
        public double? Q1Actual { get; set; }
        [Display(Name = "Q1 Amount Spent")]
        public double? Q1AmtSpent { get; set; }
        [Display(Name = "Q1 Justification")]
        public string? Q1Justification { get; set; }
        [Display(Name = "Q2 Target")]
        public double? Q2Target { get; set; }
        [Display(Name = "Q2 Budget")]
        public double? Q2Budget { get; set; }
        [Display(Name = "Q2 Actual")]
        public double? Q2Actual { get; set; }
        [Display(Name = "Q2 Amount Spent")]
        public double? Q2AmtSpent { get; set; }
        [Display(Name = "Q2 Justification")]
        public string? Q2Justification { get; set; }
        [Display(Name = "Q3 Target")]
        public double? Q3Target { get; set; }
        [Display(Name = "Q3 Budget")]
        public double? Q3Budget { get; set; }
        [Display(Name = "Q3 Actual")]
        public double? Q3Actual { get; set; }
        [Display(Name = "Q3 Amount Spent")]
        public double? Q3AmtSpent { get; set; }
        [Display(Name = "Q3 Justification")]
        public string? Q3Justification { get; set; }
        [Display(Name = "Q4 Target")]
        public double? Q4Target { get; set; }
        [Display(Name = "Q4 Budget")]
        public double? Q4Budget { get; set; }
        [Display(Name = "Q4 Actual")]
        public double? Q4Actual { get; set; }
        [Display(Name = "Q4 Amount Spent")]
        public double? Q4AmtSpent { get; set; }
        [Display(Name = "Q4 Justification")]
        public string? Q4Justification { get; set; }
        [Display(Name = "Annual Achievement")]
        public double? AnnualAchievement { get; set; }
        [Display(Name = "Total Amount Spent")]
        public double? TotAmtSpent { get; set; }

        [ForeignKey("ImplementationStatus")]
        [Display(Name = "Implementation Status")]
        public int ImpStatusId { get; set; }=1 ;
        public virtual ImplementationStatus ImplementationStatus { get; set; }

        [Display(Name = "Justification")]
        public string? AnnualJustification { get; set; }


       
    }
}
