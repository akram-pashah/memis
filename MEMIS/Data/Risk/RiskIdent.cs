using MEMIS.Data.Planning;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data.Risk
{
    [Table("T_RISKIDENTIFICATION")]
    public class RiskIdent
    {
        [Key]
        public int intRisk { get; set; }
        public DateTime? IdentifiedDate { get; set; }
        public virtual int? intStrategicObjective { get; set; }

        [ForeignKey("intStrategicObjective")]
        public virtual StrategicObjective StrategicObjectiveFk { get; set; }
    }
    public class Cause
    {
        [Key]
        public int intCause { get; set;} 
        public string CauseDescription { get; set;}
        public virtual int RiskId {  get; set;}
        [ForeignKey("intRisk")]
        public virtual RiskIdent RiskIdent { get; set;}
    }
}
