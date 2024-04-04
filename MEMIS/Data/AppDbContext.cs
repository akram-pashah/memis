using Microsoft.EntityFrameworkCore;
using MEMIS.Models;
using MEMIS.Data.Risk;
using System.Diagnostics;
using MEMIS.Data.Project;
using MEMIS.Data.Planning;
using MEMIS.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.IO.Compression;

namespace MEMIS.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
       
        public DbSet<FYear>? FYears { get; set; }    
        public DbSet<Register>? Regsiter { get; set; }
        public DbSet<Directorate>? Directorates { get; set; }
        public DbSet<Department>? Departments { get; set; }
        public DbSet<DeptPlan>? DeptPlans { get; set; }
        public DbSet<NDP_HD>? NDP_HD { get; set; }
        public DbSet<NDP>? NDP { get; set; }
    public DbSet<NDPFile>? NDPFile { get; set; }
    public DbSet<ProgramImplementationPlan>? ProgramImplementationPlan { get; set; }
        public DbSet<FocusArea> FocusArea { get; set; }
        public DbSet<StrategicObjective> StrategicObjective { get; set; }
        public DbSet<StrategicIntervention> StrategicIntervention { get; set; }
        public DbSet<StrategicAction> StrategicAction { get; set; }
        public DbSet<StrategicPlan>? StrategicPlan { get; set; }
        public DbSet<Region>? Region { get; set; }
        public DbSet<AnnualImplemtationPlan>? AnnualImplemtationPlan { get; set; }
        public DbSet<WorkPlanSettingsRegion>? WorkPlanSettingsRegion { get; set; }
        public DbSet<DepartmentPlan>? DepartmentPlan { get; set; }
        public DbSet<Master.ImplementationStatus> ImplementationStatus { get; set; }
        public DbSet<ActivityAssessment>? ActivityAssessment { get; set; }

        public DbSet<RiskIdent>RiskIdent { get; set; }
        public DbSet<Cause>? Cause { get; set; }
        public DbSet<RiskIdentification>? RiskIdentifications { get; set; }
        public DbSet<RiskMatrix>? RiskMatrixes { get; set; }
        public DbSet<RiskRegister> RiskRegister { get; set; }
        public DbSet<SDTMaster>? SDTMasters { get; set; }

        public DbSet<KPIMaster>?KPIMasters { get; set; }
        public DbSet<KPIAssessment>? KPIAssessment { get; set; }

        public DbSet<Preinspection>? Preinspections { get; set; }
        public DbSet<PreinspectionPharma>? PreinspectionsPharma { get; set; }
        public DbSet<PreinspectionNewManUnit>? PreinspectionNewManUnit { get;set; }
        public DbSet<RelocationPharma>?RelocationPharma { get; set; }
        public DbSet<RelocationDrugShop>? RelocationDrugShop { get; set; }
        public DbSet<ComplianceSupportSupervision>? ComplianceSupportSupervision { get;set; }
        public DbSet<Renewal>? Renewal { get; set; }
        public DbSet<Enforcement>? Enforcement { get; set; }
        public DbSet<GPP>? GPP { get; set; }
        public DbSet<GDP>? GDP { get; set; }
        public DbSet<HerbalInspection>? HerbalInspection { get; set; }
        public DbSet<District>? Districts { get; set; }

        public DbSet<ProjectInitiation>? ProjectInitiations { get; set; }
        public DbSet<ProjectPayment>? ProjectPayments { get; set; }
        public DbSet<ActivityPlan>? ActivityPlans { get; set; }
        public DbSet<ProjectRiskIdentification>? ProjectRiskIdentifications { get; set; }
        public DbSet<SDTAssessment>? SDTAssessment { get; set; }
        public DbSet<ProjectOthersTab>? ProjectOthersTab { get; set; }

		public DbSet<StakeHolder>? StakeHolder { get; set; }

		public DbSet<MEMIS.Data.Activity>? Activity { get; set; }
        

        ////protected override void OnModelCreating(ModelBuilder modelBuilder)
        ////{
        ////    Boolean achStatus = false;
        ////    SDTAssessmentDto sDTAssessmentDto = new SDTAssessmentDto();
        ////    achStatus = sDTAssessmentDto.ShouldValidateJustification();
        ////    modelBuilder.Entity<SDTAssessmentDto>()
        ////        .Property(s => s.Justification)
        ////        .IsRequired(achStatus);
        ////}

    }
}
