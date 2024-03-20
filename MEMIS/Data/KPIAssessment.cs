using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
    [Table("KPI_Assessment")]
    public class KPIAssessment
    {
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public virtual int? KPIMasterId {  get; set; }
        [ForeignKey("KPIMasterId")]
        public virtual KPIMaster? KPIMasterFk { get; set; }

        public DateTime AssessmentDate {  get; set; }

        [Display(Name = "Performance indicator")]
        public string? PerformanceIndicator { get; set; }
        [Display(Name = "Frequency of Reporting")]
        public int FrequencyofReporting { get; set; } 
        [Display(Name = "Numerator (Description)")]
        public string? IndicatorFormulae { get; set; }
        [Display(Name = "Denominator (Description)")]
        public string? IndicatorDefinition { get; set; }
        [Display(Name = "Financial Year")]
        public int FY { get; set; }
        [Display(Name = "Target")]
        public double? Target { get; set; }
        [Display(Name = "Numerator")] 
        public double? Numerator { get; set; }
        [Display(Name = "Denominator")]
        public double? Denominator { get; set; }
        [Display(Name = "Performance")]
        public double? Rate { get; set; }
        [Display(Name = "Rating")]
        public string? Achieved { get; set; }
        [Display(Name = "Approval Status")]
        public int? ApprovalStatus { get; set; }=0;
        [Display(Name = "Justification")]
        public string? Justification { get; set; }
    }
}
