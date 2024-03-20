using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Models
{
    public class SDTMastersCreateEditDto
    {
        public int Id { get; set; }
        [Required]
		[Display(Name = "Service Delivery Timeline")]
		public string? ServiceDeliveryTimeline { get; set; }
        [Required]
        public string? Measure { get; set; }
        [Required]
		[Display(Name = "Evaluation Period")]
		public string? EvaluationPeriod { get; set; }
        [Required]
        public string? Target { get; set; }
        [Required]
        public string? Numerator { get; set; }
        [Required]
        public string? Denominator { get; set; }
        [Required]
        [Display(Name = "Department")]
        public virtual Guid? DepartmentId { get; set; }
    }
}
