using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class annulplan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "activity",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "focusArea",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "outputIndicator",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "strategicAction",
                table: "AnnualImplemtationPlan");

            migrationBuilder.RenameColumn(
                name: "strategivIntervention",
                table: "AnnualImplemtationPlan",
                newName: "meansofVerification");

            migrationBuilder.AddColumn<int>(
                name: "ActivityFkintActivity",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentFkintDept",
                table: "AnnualImplemtationPlan",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FocusAreaFkintFocus",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Risk",
                table: "AnnualImplemtationPlan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StrategicActionFkintAction",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StrategicInterventionFkintIntervention",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StrategicObjectiveFkintObjective",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "baseline",
                table: "AnnualImplemtationPlan",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "intAction",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "intActivity",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "intDept",
                table: "AnnualImplemtationPlan",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "intFocus",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "intIntervention",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "intObjective",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "subProgram",
                table: "AnnualImplemtationPlan",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    intActivity = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    activityCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    activityName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    intAction = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.intActivity);
                    table.ForeignKey(
                        name: "FK_Activity_StrategicAction_intAction",
                        column: x => x.intAction,
                        principalTable: "StrategicAction",
                        principalColumn: "intAction");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnualImplemtationPlan_ActivityFkintActivity",
                table: "AnnualImplemtationPlan",
                column: "ActivityFkintActivity");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualImplemtationPlan_DepartmentFkintDept",
                table: "AnnualImplemtationPlan",
                column: "DepartmentFkintDept");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualImplemtationPlan_FocusAreaFkintFocus",
                table: "AnnualImplemtationPlan",
                column: "FocusAreaFkintFocus");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualImplemtationPlan_StrategicActionFkintAction",
                table: "AnnualImplemtationPlan",
                column: "StrategicActionFkintAction");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualImplemtationPlan_StrategicInterventionFkintIntervention",
                table: "AnnualImplemtationPlan",
                column: "StrategicInterventionFkintIntervention");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualImplemtationPlan_StrategicObjectiveFkintObjective",
                table: "AnnualImplemtationPlan",
                column: "StrategicObjectiveFkintObjective");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_intAction",
                table: "Activity",
                column: "intAction");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_Activity_ActivityFkintActivity",
                table: "AnnualImplemtationPlan",
                column: "ActivityFkintActivity",
                principalTable: "Activity",
                principalColumn: "intActivity");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_Departments_DepartmentFkintDept",
                table: "AnnualImplemtationPlan",
                column: "DepartmentFkintDept",
                principalTable: "Departments",
                principalColumn: "intDept");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_FocusArea_FocusAreaFkintFocus",
                table: "AnnualImplemtationPlan",
                column: "FocusAreaFkintFocus",
                principalTable: "FocusArea",
                principalColumn: "intFocus");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_StrategicAction_StrategicActionFkintAction",
                table: "AnnualImplemtationPlan",
                column: "StrategicActionFkintAction",
                principalTable: "StrategicAction",
                principalColumn: "intAction");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_StrategicIntervention_StrategicInterventionFkintIntervention",
                table: "AnnualImplemtationPlan",
                column: "StrategicInterventionFkintIntervention",
                principalTable: "StrategicIntervention",
                principalColumn: "intIntervention");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_StrategicObjective_StrategicObjectiveFkintObjective",
                table: "AnnualImplemtationPlan",
                column: "StrategicObjectiveFkintObjective",
                principalTable: "StrategicObjective",
                principalColumn: "intObjective");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_Activity_ActivityFkintActivity",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_Departments_DepartmentFkintDept",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_FocusArea_FocusAreaFkintFocus",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_StrategicAction_StrategicActionFkintAction",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_StrategicIntervention_StrategicInterventionFkintIntervention",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_StrategicObjective_StrategicObjectiveFkintObjective",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropIndex(
                name: "IX_AnnualImplemtationPlan_ActivityFkintActivity",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropIndex(
                name: "IX_AnnualImplemtationPlan_DepartmentFkintDept",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropIndex(
                name: "IX_AnnualImplemtationPlan_FocusAreaFkintFocus",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropIndex(
                name: "IX_AnnualImplemtationPlan_StrategicActionFkintAction",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropIndex(
                name: "IX_AnnualImplemtationPlan_StrategicInterventionFkintIntervention",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropIndex(
                name: "IX_AnnualImplemtationPlan_StrategicObjectiveFkintObjective",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "ActivityFkintActivity",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "DepartmentFkintDept",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "FocusAreaFkintFocus",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "Risk",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "StrategicActionFkintAction",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "StrategicInterventionFkintIntervention",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "StrategicObjectiveFkintObjective",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "baseline",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "intAction",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "intActivity",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "intDept",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "intFocus",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "intIntervention",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "intObjective",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "subProgram",
                table: "AnnualImplemtationPlan");

            migrationBuilder.RenameColumn(
                name: "meansofVerification",
                table: "AnnualImplemtationPlan",
                newName: "strategivIntervention");

            migrationBuilder.AddColumn<string>(
                name: "activity",
                table: "AnnualImplemtationPlan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "focusArea",
                table: "AnnualImplemtationPlan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "outputIndicator",
                table: "AnnualImplemtationPlan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "strategicAction",
                table: "AnnualImplemtationPlan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
