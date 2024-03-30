using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class NDPFileUpload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskIdentification_DepartmentPlan_Activity",
                table: "RiskIdentification");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskIdentification_StrategicPlan_FocusArea",
                table: "RiskIdentification");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskIdentification_StrategicPlan_StrategicObjective",
                table: "RiskIdentification");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskRegister_DepartmentPlan_Activity",
                table: "RiskRegister");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskRegister_StrategicPlan_FocusArea",
                table: "RiskRegister");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskRegister_StrategicPlan_StrategicObjective",
                table: "RiskRegister");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileContent",
                table: "NDP",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskIdentification_Activity_Activity",
                table: "RiskIdentification",
                column: "Activity",
                principalTable: "Activity",
                principalColumn: "intActivity",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskIdentification_FocusArea_FocusArea",
                table: "RiskIdentification",
                column: "FocusArea",
                principalTable: "FocusArea",
                principalColumn: "intFocus");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskIdentification_StrategicObjective_StrategicObjective",
                table: "RiskIdentification",
                column: "StrategicObjective",
                principalTable: "StrategicObjective",
                principalColumn: "intObjective");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskRegister_Activity_Activity",
                table: "RiskRegister",
                column: "Activity",
                principalTable: "Activity",
                principalColumn: "intActivity",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskRegister_FocusArea_FocusArea",
                table: "RiskRegister",
                column: "FocusArea",
                principalTable: "FocusArea",
                principalColumn: "intFocus");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskRegister_StrategicObjective_StrategicObjective",
                table: "RiskRegister",
                column: "StrategicObjective",
                principalTable: "StrategicObjective",
                principalColumn: "intObjective");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskIdentification_Activity_Activity",
                table: "RiskIdentification");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskIdentification_FocusArea_FocusArea",
                table: "RiskIdentification");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskIdentification_StrategicObjective_StrategicObjective",
                table: "RiskIdentification");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskRegister_Activity_Activity",
                table: "RiskRegister");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskRegister_FocusArea_FocusArea",
                table: "RiskRegister");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskRegister_StrategicObjective_StrategicObjective",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "FileContent",
                table: "NDP");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskIdentification_DepartmentPlan_Activity",
                table: "RiskIdentification",
                column: "Activity",
                principalTable: "DepartmentPlan",
                principalColumn: "intDeptPlan",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskIdentification_StrategicPlan_FocusArea",
                table: "RiskIdentification",
                column: "FocusArea",
                principalTable: "StrategicPlan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskIdentification_StrategicPlan_StrategicObjective",
                table: "RiskIdentification",
                column: "StrategicObjective",
                principalTable: "StrategicPlan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskRegister_DepartmentPlan_Activity",
                table: "RiskRegister",
                column: "Activity",
                principalTable: "DepartmentPlan",
                principalColumn: "intDeptPlan",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskRegister_StrategicPlan_FocusArea",
                table: "RiskRegister",
                column: "FocusArea",
                principalTable: "StrategicPlan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskRegister_StrategicPlan_StrategicObjective",
                table: "RiskRegister",
                column: "StrategicObjective",
                principalTable: "StrategicPlan",
                principalColumn: "Id");
        }
    }
}
