using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class RiskRefIDNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskTreatmentPlan_RiskRegister_RiskRefID",
                table: "RiskTreatmentPlan");

            migrationBuilder.DropColumn(
                name: "AdditionalMitigation",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "ExpectedDate",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "ResourcesRequired",
                table: "RiskRegister");

            migrationBuilder.AlterColumn<int>(
                name: "RiskRefID",
                table: "RiskTreatmentPlan",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskTreatmentPlan_RiskRegister_RiskRefID",
                table: "RiskTreatmentPlan",
                column: "RiskRefID",
                principalTable: "RiskRegister",
                principalColumn: "RiskRefID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskTreatmentPlan_RiskRegister_RiskRefID",
                table: "RiskTreatmentPlan");

            migrationBuilder.AlterColumn<int>(
                name: "RiskRefID",
                table: "RiskTreatmentPlan",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalMitigation",
                table: "RiskRegister",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedDate",
                table: "RiskRegister",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourcesRequired",
                table: "RiskRegister",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskTreatmentPlan_RiskRegister_RiskRefID",
                table: "RiskTreatmentPlan",
                column: "RiskRefID",
                principalTable: "RiskRegister",
                principalColumn: "RiskRefID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
