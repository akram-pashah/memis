using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class annulplan2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "annualTarget",
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
                name: "meansofVerification",
                table: "AnnualImplemtationPlan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "annualTarget",
                table: "AnnualImplemtationPlan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                name: "meansofVerification",
                table: "AnnualImplemtationPlan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}
