using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectRiskIdentificationWithRankCalculation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Consequence",
                table: "ProjectRiskIdentification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Likelihood",
                table: "ProjectRiskIdentification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Mitigation",
                table: "ProjectRiskIdentification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ownership",
                table: "ProjectRiskIdentification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RiskImplementationCost",
                table: "ProjectRiskIdentification",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                table: "ProjectRiskIdentification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Stage",
                table: "ProjectRiskIdentification",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Consequence",
                table: "ProjectRiskIdentification");

            migrationBuilder.DropColumn(
                name: "Likelihood",
                table: "ProjectRiskIdentification");

            migrationBuilder.DropColumn(
                name: "Mitigation",
                table: "ProjectRiskIdentification");

            migrationBuilder.DropColumn(
                name: "Ownership",
                table: "ProjectRiskIdentification");

            migrationBuilder.DropColumn(
                name: "RiskImplementationCost",
                table: "ProjectRiskIdentification");

            migrationBuilder.DropColumn(
                name: "Severity",
                table: "ProjectRiskIdentification");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "ProjectRiskIdentification");
        }
    }
}
