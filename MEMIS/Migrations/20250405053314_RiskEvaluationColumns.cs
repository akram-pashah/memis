using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class RiskEvaluationColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ResidualRiskConsequence",
                table: "RiskEvaluation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ResidualRiskLikelihood",
                table: "RiskEvaluation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ResidualRiskRank",
                table: "RiskEvaluation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResidualRiskConsequence",
                table: "RiskEvaluation");

            migrationBuilder.DropColumn(
                name: "ResidualRiskLikelihood",
                table: "RiskEvaluation");

            migrationBuilder.DropColumn(
                name: "ResidualRiskRank",
                table: "RiskEvaluation");
        }
    }
}
