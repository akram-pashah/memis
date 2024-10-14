using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class UnitCostColAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_Activity_intActivity",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_Departments_intDept",
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

            migrationBuilder.AddColumn<double>(
                name: "UnitCost",
                table: "ProgramImplementationPlan",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "meansofVerification",
                table: "AnnualImplemtationPlan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "intObjective",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "intIntervention",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "intFocus",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "intDept",
                table: "AnnualImplemtationPlan",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "intActivity",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "intAction",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "UnitCost",
                table: "AnnualImplemtationPlan",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "UnitCost",
                table: "ActivityAssessRegion",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "UnitCost",
                table: "ActivityAssessmentRegion",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "UnitCost",
                table: "ActivityAssess",
                type: "float",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_Activity_intActivity",
                table: "AnnualImplemtationPlan",
                column: "intActivity",
                principalTable: "Activity",
                principalColumn: "intActivity");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_Departments_intDept",
                table: "AnnualImplemtationPlan",
                column: "intDept",
                principalTable: "Departments",
                principalColumn: "intDept");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_FocusArea_intFocus",
                table: "AnnualImplemtationPlan",
                column: "intFocus",
                principalTable: "FocusArea",
                principalColumn: "intFocus");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_StrategicAction_intAction",
                table: "AnnualImplemtationPlan",
                column: "intAction",
                principalTable: "StrategicAction",
                principalColumn: "intAction");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_StrategicIntervention_intIntervention",
                table: "AnnualImplemtationPlan",
                column: "intIntervention",
                principalTable: "StrategicIntervention",
                principalColumn: "intIntervention");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_StrategicObjective_intObjective",
                table: "AnnualImplemtationPlan",
                column: "intObjective",
                principalTable: "StrategicObjective",
                principalColumn: "intObjective");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_Activity_intActivity",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_Departments_intDept",
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

            migrationBuilder.DropColumn(
                name: "UnitCost",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropColumn(
                name: "UnitCost",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "UnitCost",
                table: "ActivityAssessRegion");

            migrationBuilder.DropColumn(
                name: "UnitCost",
                table: "ActivityAssessmentRegion");

            migrationBuilder.DropColumn(
                name: "UnitCost",
                table: "ActivityAssess");

            migrationBuilder.AlterColumn<string>(
                name: "meansofVerification",
                table: "AnnualImplemtationPlan",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "intObjective",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "intIntervention",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "intFocus",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "intDept",
                table: "AnnualImplemtationPlan",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "intActivity",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "intAction",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_Activity_intActivity",
                table: "AnnualImplemtationPlan",
                column: "intActivity",
                principalTable: "Activity",
                principalColumn: "intActivity",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_Departments_intDept",
                table: "AnnualImplemtationPlan",
                column: "intDept",
                principalTable: "Departments",
                principalColumn: "intDept",
                onDelete: ReferentialAction.Cascade);

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
    }
}
