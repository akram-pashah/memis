using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MEMIS;
namespace MEMIS.Data
{
    [Table("SDTAssessment")]
    public class SDTAssessment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public float Numerator { get; set; }

        [Required]
        public float Denominator { get; set; }

        [Display(Name ="Implemented within Timeline")]
        [Required]
        public float ImplementedTimeline { get; set; }

        [Required]
		[Display(Name = "Average Days")]
		public float Rate { get; set; }

        [Display(Name = "Propotion implemented with Timelines")]
        [Required]
        public float ProportionTimeline { get; set; }

        [Required]
        public int Target { get; set; }

        [Display(Name = "Achivement Status")]
        [Required]
        public string? AchivementStatus { get; set; }

        [Display(Name = "Percentage Variance")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        [Required]
        public string? Variance { get; set; }

        [Required]
        [MaxLength(500)]
        public string? Justification { get; set; }

        [Required]
        public string? Rating { get; set; }

        [Display(Name = "HOD Action")]
        public string? HODAction { get; set; }

        [Display(Name = "HOD Comment")]
        [MaxLength(500)]
        public string? HODComment { get; set; }

        [Display(Name = "HOD Action Date")]
        public DateTime? HODActionDate { get; set; }

        [Display(Name = "Director Action")]
        public string? DirectorAction { get; set; }

        [Display(Name = "Director Comment")]
        [MaxLength(500)]
        public string? DirectorComment { get; set; }

        [Display(Name = "Director Action Date")]
        public DateTime? DirectorActionDate { get; set; }
        [Display(Name = "M & E Officer Action")]
        public string? MEOfficerAction { get; set; }

        [Display(Name = "M & E Officer Comment")]
        [MaxLength(500)]
        public string? MEOfficerComment { get; set; }

        [Display(Name = "M & E Officer Action Date")]
        public DateTime? MEOfficerActionDate { get; set; }
        [Display(Name = "Head BPD Action")]
        public string? HBPDAction { get; set; }

        [Display(Name = "Head BPD Comment")]
        [MaxLength(500)]
        public string? HBPDComment { get; set; }

        [Display(Name = "Head BPD Action Date")]
        public DateTime? HBPDActionDate { get; set; }
        [Display(Name = "Director DCS Action")]
        public string? DDCSAction { get; set; }

        [Display(Name = "Director DCS Comment")]
        [MaxLength(500)]
        public string? DDCSComment { get; set; }

        [Display(Name = "Director DCS Action Date")]
        public DateTime? DDCSActionDate { get; set; }
        [Required]
        public virtual int? SDTMasterId { get; set; }
        [ForeignKey("SDTMasterId")]
        public virtual SDTMaster? SDTMasterFk { get; set; }
        [Display(Name = "Approval Status")]
        public int ApprovalStatusHOD { get; set; }
        [Display(Name = "Approval Status")]
        public int ApprovalStatusDirector { get; set; }
        [Display(Name = "Approval Status")]
        public int ApprovalStatusOfficer { get; set; }
        [Display(Name = "Approval Status")]
        public int ApprovalStatusMEOFfficer { get; set; }
        [Display(Name = "Approval Status")]
        public int ApprovalStatusHBPD { get; set; }
        [Display(Name = "Approval Status")]
        public int ApprovalStatusDDCS { get; set; } 
    }
}
