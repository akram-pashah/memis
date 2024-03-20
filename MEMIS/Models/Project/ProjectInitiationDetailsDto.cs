using MEMIS.Data;
using MEMIS.Data.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Models
{
    public class ProjectInitiationDetailsDto
    {
        public int ProjectInitId { get; set; }

		public ActivityPlanDto ActivityPlan { get; set; }
		public ProjectPaymentDto ProjectPayment { get; set; }
		public ProjectRiskIdentificationDto RiskIdentification { get; set; }
		public ProjectOthersTab ProjectOthersTab { get; set; }
		public StakeHolder StakeHolder { get; set; }

		public List<ActivityPlan> ActivityPlans { get; set; }
        public List<ProjectPayment> ProjectPayments { get; set; }
        public List<ProjectRiskIdentification> ProjectRiskIdentifications { get; set; }
        public List<ProjectOthersTab> ProjectOthersTabs { get; set; }
		public List<StakeHolder> StakeHolders { get; set; }
	}
}
