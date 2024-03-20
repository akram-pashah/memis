using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class annulplan3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentFkintDept",
                table: "AnnualImplemtationPlan",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Risk",
                table: "AnnualImplemtationPlan",
                type: "nvarchar(max)",
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
                name: "IX_AnnualImplemtationPlan_DepartmentFkintDept",
                table: "AnnualImplemtationPlan",
                column: "DepartmentFkintDept");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualImplemtationPlan_intAction",
                table: "AnnualImplemtationPlan",
                column: "intAction");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualImplemtationPlan_intActivity",
                table: "AnnualImplemtationPlan",
                column: "intActivity");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualImplemtationPlan_intFocus",
                table: "AnnualImplemtationPlan",
                column: "intFocus");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualImplemtationPlan_intIntervention",
                table: "AnnualImplemtationPlan",
                column: "intIntervention");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualImplemtationPlan_intObjective",
                table: "AnnualImplemtationPlan",
                column: "intObjective");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_Activity_intActivity",
                table: "AnnualImplemtationPlan",
                column: "intActivity",
                principalTable: "Activity",
                principalColumn: "intActivity",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_Departments_DepartmentFkintDept",
                table: "AnnualImplemtationPlan",
                column: "DepartmentFkintDept",
                principalTable: "Departments",
                principalColumn: "intDept");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_FocusArea_intFocus",
                table: "AnnualImplemtationPlan",
                column: "intFocus",
                principalTable: "FocusArea",
                principalColumn: "intFocus",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_StrategicAction_intAction",
                table: "AnnualImplemtationPlan",
                column: "intAction",
                principalTable: "StrategicAction",
                principalColumn: "intAction",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_StrategicIntervention_intIntervention",
                table: "AnnualImplemtationPlan",
                column: "intIntervention",
                principalTable: "StrategicIntervention",
                principalColumn: "intIntervention",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_StrategicObjective_intObjective",
                table: "AnnualImplemtationPlan",
                column: "intObjective",
                principalTable: "StrategicObjective",
                principalColumn: "intObjective",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_Activity_intActivity",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_Departments_DepartmentFkintDept",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_FocusArea_intFocus",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_StrategicAction_intAction",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_StrategicIntervention_intIntervention",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_StrategicObjective_intObjective",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropIndex(
                name: "IX_AnnualImplemtationPlan_DepartmentFkintDept",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropIndex(
                name: "IX_AnnualImplemtationPlan_intAction",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropIndex(
                name: "IX_AnnualImplemtationPlan_intActivity",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropIndex(
                name: "IX_AnnualImplemtationPlan_intFocus",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropIndex(
                name: "IX_AnnualImplemtationPlan_intIntervention",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropIndex(
                name: "IX_AnnualImplemtationPlan_intObjective",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "DepartmentFkintDept",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "Risk",
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
    }
}
