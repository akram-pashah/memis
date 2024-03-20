using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Data
{
    [Table("KPI")]
    public class KPIMaster
	{
        [Key]
        public int Id { get; set; }
		[Display(Name = "Strategic Objective")]
		public virtual int? StrategicObjective { get; set; }

		[ForeignKey("StrategicObjective")]
		public virtual StrategicPlan StrategicPlanFk { get; set; }
		[Display(Name = "Performance indicator")]
        public string? PerformanceIndicator { get; set; }
		[Display(Name = "Type of Indicator")]
		public int TypeofIndicator { get; set; }
		[Display(Name = "Indicator Formulae")]
		public string? IndicatorFormulae { get; set; }
		[Display(Name = "Indicator Definition")]
		public string? IndicatorDefinition { get; set; }
		[Display(Name = "Original Baseline")]
		public long? OriginalBaseline { get; set; }		
		[Display(Name = "Indicator classification")]
		public int Indicatorclassification { get; set; }
		[Display(Name = "Data Type")]
		public string? DataType { get; set; }
		[Display(Name = "Unit of Measure")]
		public string? Unitofmeasure { get; set; }
		[Display(Name = "Frequency of Reporting")]
		public int FrequencyofReporting { get; set; }
		[Display(Name = "FY1")]
		public double? FY1 { get; set; }
		[Display(Name = "FY2")]
		public double? FY2 { get; set; }
		[Display(Name = "FY3")]
		public double? FY3 { get; set; }
		[Display(Name = "FY4")]
		public double? FY4 { get; set; }
		[Display(Name = "FY5")]
		public double? FY5 { get; set; }
		[Display(Name = "Means of Verification")]
		public string? MeansofVerification { get; set; }
		[Display(Name = "Responsible Party")]
		public string? ResponsibleParty { get; set; }
		 
    }
}
