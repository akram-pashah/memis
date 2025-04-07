using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class RiskEvaluationRelationChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskEvaluation_RiskRegister_RiskRegisterId",
                table: "RiskEvaluation");

            migrationBuilder.RenameColumn(
                name: "RiskRegisterId",
                table: "RiskEvaluation",
                newName: "RiskRefID");

            migrationBuilder.RenameIndex(
                name: "IX_RiskEvaluation_RiskRegisterId_Year_Quarter",
                table: "RiskEvaluation",
                newName: "IX_RiskEvaluation_RiskRefID_Year_Quarter");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskEvaluation_RiskRegister_RiskRefID",
                table: "RiskEvaluation",
                column: "RiskRefID",
                principalTable: "RiskRegister",
                principalColumn: "RiskRefID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskEvaluation_RiskRegister_RiskRefID",
                table: "RiskEvaluation");

            migrationBuilder.RenameColumn(
                name: "RiskRefID",
                table: "RiskEvaluation",
                newName: "RiskRegisterId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskEvaluation_RiskRefID_Year_Quarter",
                table: "RiskEvaluation",
                newName: "IX_RiskEvaluation_RiskRegisterId_Year_Quarter");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskEvaluation_RiskRegister_RiskRegisterId",
                table: "RiskEvaluation",
                column: "RiskRegisterId",
                principalTable: "RiskRegister",
                principalColumn: "RiskRefID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
