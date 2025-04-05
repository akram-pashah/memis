using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class RiskEvaluationColumnsNUllable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskEvaluation_RiskRegister_RiskRefID",
                table: "RiskEvaluation");

            migrationBuilder.DropIndex(
                name: "IX_RiskEvaluation_RiskRefID_Year_Quarter",
                table: "RiskEvaluation");

            migrationBuilder.AlterColumn<int>(
                name: "RiskRefID",
                table: "RiskEvaluation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EvaluationDate",
                table: "RiskEvaluation",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "EvaluatedBy",
                table: "RiskEvaluation",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_RiskEvaluation_RiskRefID_Year_Quarter",
                table: "RiskEvaluation",
                columns: new[] { "RiskRefID", "Year", "Quarter" },
                unique: true,
                filter: "[RiskRefID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskEvaluation_RiskRegister_RiskRefID",
                table: "RiskEvaluation",
                column: "RiskRefID",
                principalTable: "RiskRegister",
                principalColumn: "RiskRefID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskEvaluation_RiskRegister_RiskRefID",
                table: "RiskEvaluation");

            migrationBuilder.DropIndex(
                name: "IX_RiskEvaluation_RiskRefID_Year_Quarter",
                table: "RiskEvaluation");

            migrationBuilder.AlterColumn<int>(
                name: "RiskRefID",
                table: "RiskEvaluation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EvaluationDate",
                table: "RiskEvaluation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EvaluatedBy",
                table: "RiskEvaluation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiskEvaluation_RiskRefID_Year_Quarter",
                table: "RiskEvaluation",
                columns: new[] { "RiskRefID", "Year", "Quarter" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskEvaluation_RiskRegister_RiskRefID",
                table: "RiskEvaluation",
                column: "RiskRefID",
                principalTable: "RiskRegister",
                principalColumn: "RiskRefID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
