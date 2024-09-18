using MEMIS.Data.Project;
using MEMIS.Data.Risk;
using MEMIS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
    public DbSet<NDAFile>? NDAFile { get; set; }
    public DbSet<ProgramImplementationPlan>? ProgramImplementationPlan { get; set; }
    public DbSet<FocusArea> FocusArea { get; set; }
    public DbSet<StrategicObjective> StrategicObjective { get; set; }
    public DbSet<StrategicIntervention> StrategicIntervention { get; set; }
    public DbSet<StrategicAction> StrategicAction { get; set; }
    public DbSet<StrategicPlan>? StrategicPlan { get; set; }
    public DbSet<Region>? Region { get; set; }
    public DbSet<AnnualImplemtationPlan>? AnnualImplemtationPlan { get; set; }
    public DbSet<ActivityAssess> ActivityAssess { get; set; }
    public DbSet<QuaterlyPlan>? QuaterlyPlans { get; set; }
    public DbSet<QuarterlyRiskAction>? QuarterlyRiskActions { get; set; }
    public DbSet<WorkPlanSettingsRegion>? WorkPlanSettingsRegion { get; set; }
    public DbSet<DepartmentPlan>? DepartmentPlan { get; set; }
    public DbSet<Master.ImplementationStatus> ImplementationStatus { get; set; }
    public DbSet<ActivityAssessment>? ActivityAssessment { get; set; }

    public DbSet<ActivityAssessRegion> ActivityAssessRegion { get; set; }
    public DbSet<ActivityAssessmentRegion> ActivityAssessmentRegion { get; set; }

    public DbSet<RiskIdent> RiskIdent { get; set; }
    public DbSet<Cause>? Cause { get; set; }
    public DbSet<RiskCategory>? RiskCategorys { get; set; }
    public DbSet<RiskIdentification>? RiskIdentifications { get; set; }
    public DbSet<RiskMatrix>? RiskMatrixes { get; set; }
    public DbSet<RiskRegister> RiskRegister { get; set; }
    public DbSet<SDTMaster>? SDTMasters { get; set; }

    public DbSet<KPIMaster>? KPIMasters { get; set; }
    public DbSet<KPIAssessment>? KPIAssessment { get; set; }

    public DbSet<Preinspection>? Preinspections { get; set; }
    public DbSet<PreinspectionPharma>? PreinspectionsPharma { get; set; }
    public DbSet<PreinspectionNewManUnit>? PreinspectionNewManUnit { get; set; }
    public DbSet<RelocationPharma>? RelocationPharma { get; set; }
    public DbSet<RelocationDrugShop>? RelocationDrugShop { get; set; }
    public DbSet<ComplianceSupportSupervision>? ComplianceSupportSupervision { get; set; }
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
    public DbSet<ProjectSubmittalTracker> ProjectSubmittalTracker {  get; set; }

    public DbSet<MEMIS.Data.Activity>? Activity { get; set; }
    public DbSet<RiskDetail> RiskDetails { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<RiskSource> RiskSources { get; set; }
    public DbSet<RiskCause> RiskCauses { get; set; }
    public DbSet<RiskConsequenceDetails> RiskConsequenceDetails { get; set; }
    public DbSet<RiskTreatmentPlan> RiskTreatmentPlans { get; set; }
    public DbSet<MonitoringAndControl> MonitoringAndControls { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<StrategicObjective>()
        .HasOne(x => x.FocusArea)
        .WithMany(t => t.StrategicObjectives)
        .HasForeignKey(t => t.intFocus);

      modelBuilder.Entity<StrategicIntervention>()
        .HasOne(z => z.StrategicObjective)
        .WithMany(x => x.StrategicInterventions)
        .HasForeignKey(t => t.intObjective);

      modelBuilder.Entity<StrategicAction>()
        .HasOne(z => z.StrategicIntervention)
        .WithMany(x => x.StrategicActions)
        .HasForeignKey(t => t.intIntervention);

      modelBuilder.Entity<Activity>()
        .HasOne(z => z.StrategicAction)
        .WithMany(x => x.Activities)
        .HasForeignKey(t => t.intAction);

      modelBuilder.Entity<RiskIdentification>()
                .HasMany(r => r.RiskDetails)
                .WithOne(rd => rd.RiskIdentification)
                .HasForeignKey(rd => rd.RiskId);

      modelBuilder.Entity<RiskIdentification>()
                .HasMany(r => r.Events)
                .WithOne(e => e.RiskIdentification)
                .HasForeignKey(e => e.RiskId);

      modelBuilder.Entity<RiskIdentification>()
          .HasMany(r => r.RiskSources)
          .WithOne(rs => rs.RiskIdentification)
          .HasForeignKey(rs => rs.RiskId);

      modelBuilder.Entity<RiskIdentification>()
          .HasMany(r => r.RiskCauses)
          .WithOne(rc => rc.RiskIdentification)
          .HasForeignKey(rc => rc.RiskId);

      modelBuilder.Entity<RiskIdentification>()
          .HasMany(r => r.RiskConsequenceDetails)
          .WithOne(rc => rc.RiskIdentification)
          .HasForeignKey(rc => rc.RiskId);

      modelBuilder.Entity<RiskTreatmentPlan>()
           .HasOne(t => t.RiskRegister)
           .WithMany(r => r.RiskTreatmentPlans)
           .HasForeignKey(t => t.RiskRefID);

      modelBuilder.Entity<QuaterlyPlan>()
           .HasOne(t => t.ActivityAssess)
           .WithMany(r => r.QuaterlyPlans)
           .HasForeignKey(t => t.ActivityAccessId);

      modelBuilder.Entity<QuaterlyPlan>()
           .HasOne(t => t.ActivityAssessRegion)
           .WithMany(r => r.QuaterlyPlans)
           .HasForeignKey(t => t.ActivityAssessRegionId);

      modelBuilder.Entity<QuaterlyPlan>()
           .HasOne(t => t.DeptPlan)
           .WithMany(r => r.QuaterlyPlans)
           .HasForeignKey(t => t.DeptPlanId);

      modelBuilder.Entity<QuaterlyPlan>()
           .HasOne(t => t.ActivityAssessment)
           .WithMany(r => r.QuaterlyPlans)
           .HasForeignKey(t => t.ActivityAssessmentId);

      modelBuilder.Entity<QuaterlyPlan>()
           .HasOne(t => t.ActivityAssessmentRegion)
           .WithMany(r => r.QuaterlyPlans)
           .HasForeignKey(t => t.ActivityAssessmentRegionId);

      modelBuilder.Entity<QuarterlyRiskAction>()
     .HasOne(t => t.RiskTreatmentPlan)
     .WithMany(r => r.QuarterlyRiskActions)
     .HasForeignKey(t => t.TreatmentPlanId);

      modelBuilder.Entity<QuarterlyRiskAction>()
        .HasOne(t => t.ImplementationStatus)
        .WithMany(x => x.QuarterlyRiskActions)
        .HasForeignKey(t => t.ImpStatusId);

      base.OnModelCreating(modelBuilder);

      //Boolean achStatus = false;
      //SDTAssessmentDto sDTAssessmentDto = new SDTAssessmentDto();
      //achStatus = sDTAssessmentDto.ShouldValidateJustification();
      //modelBuilder.Entity<SDTAssessmentDto>()
      //    .Property(s => s.Justification)
      //    .IsRequired(achStatus);
    }

  }
}
