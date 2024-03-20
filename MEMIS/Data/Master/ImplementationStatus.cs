using System.ComponentModel.DataAnnotations;

namespace MEMIS.Data.Master
{
    public class ImplementationStatus
    {
        [Key]
        public int ImpStatusId { get; set; } 
        public string ImpStatusName { get; set; }
        public virtual ICollection<ActivityAssessment> ActivityAssessments { get; set; }

    }
}
