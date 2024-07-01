using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
  [Table("DeptPlan")]
  public class DeptPlan
  {
    [Key]
    public int intActivity { get; set; }
    [Display(Name = "Strategic Objective")]
    public virtual int? StrategicObjective { get; set; }

    [ForeignKey("StrategicObjective")]
    public virtual StrategicPlan StrategicPlanFk { get; set; }
    [Display(Name = "Strategic Intervention")]
    public virtual int? strategicIntervention { get; set; }
    [ForeignKey("strategicIntervention")]
    public virtual StrategicPlan StrategicInterventionFk { get; set; }
    [Display(Name = "Strategic Action")]
    public virtual int? StrategicAction { get; set; }
    [ForeignKey("StrategicAction")]
    public virtual StrategicPlan StrategicActionFk { get; set; }
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
    [Display(Name = "Approval Status")]
    public int ApprStatus { get; set; } = 0;
    [Display(Name = "Department")]
    public virtual Guid? DepartmentId { get; set; }
    [ForeignKey("DepartmentId")]
    public virtual Department? DepartmentFk { get; set; }
    public virtual ICollection<QuaterlyPlan> QuaterlyPlans { get; set; }
  }
}
