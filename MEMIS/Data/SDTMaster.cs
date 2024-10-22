using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Data
{
  [Table("SDTMaster")]
  public class SDTMaster
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "Service Delivery Timeline")]
    public string? ServiceDeliveryTimeline { get; set; }
    public string? Measure { get; set; }
    [Display(Name = "Evaluation Period")]
    public string? EvaluationPeriod { get; set; }
    public string? Target { get; set; }
    public string? Numerator { get; set; }
    public string? Denominator { get; set; }
    [Display(Name = "Proportion Implemented within Timeline")]
    public string? PropotionWithinTimeline { get; set; }
    [Display(Name = "Department")]
    public virtual Guid? DepartmentId { get; set; }
    [ForeignKey("DepartmentId")]
    public virtual Department? DepartmentFk { get; set; }
    public ICollection<SDTAssessment> SDTAssessments { get; set; } = [];
  }
}

