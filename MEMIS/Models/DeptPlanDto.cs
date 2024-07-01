using MEMIS.Data;
using MEMIS.Data.Risk;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Models
{
    public class DeptPlanDto
    {

        public int intActivity { get; set; }
        [Required]
        [Display(Name = "Strategic Objective")]
        public int? StrategicObjective { get; set; }
        [Required]
        [Display(Name = "Strategic Intervention")]
        public int? strategicIntervention { get; set; }
        [Display(Name = "Strategic Action")]
        public int? StrategicAction { get; set; }
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
        [Display(Name = "Target")]
        public double? Q1Target { get; set; }
        [Display(Name = "Budget")]
        public double? Q1Budget { get; set; }
        [Display(Name = "Q2 Target")]
        public double? Q2Target { get; set; }
        [Display(Name = "Q2 Budget")]
        public double? Q2Budget { get; set; }
        [Display(Name = "Q3 Target")]
        public double? Q3Target { get; set; }
        [Display(Name = "Q3 Budget")]
        public double? Q3Budget { get; set; }
        [Display(Name = "Q4 Target")]
        public double? Q4Target { get; set; }
        [Display(Name = "Q4 Budget")]
        public double? Q4Budget { get; set; }
        [Display(Name = "Comparative Target")]
        public double? comparativeTarget { get; set; }
        [Display(Name = "Justification")]
        public string? justification { get; set; }
        [Display(Name = "Budget Amount")]
        public double? budgetAmount { get; set; }
        public bool IsVerified { get; set; }
        public int ApprStatus { get; set; } = 0;
        public virtual Guid? DepartmentId { get; set; }
        public List<QuaterlyPlan> QuaterlyPlans { get; set; }
    }
}
