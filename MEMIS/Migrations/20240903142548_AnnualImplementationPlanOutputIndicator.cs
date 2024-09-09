using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class AnnualImplementationPlanOutputIndicator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FYEAR",
                table: "ActivityAssessment");

            migrationBuilder.AddColumn<string>(
                name: "outputIndicator",
                table: "AnnualImplemtationPlan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "outputIndicator",
                table: "AnnualImplemtationPlan");

            migrationBuilder.AddColumn<int>(
                name: "FYEAR",
                table: "ActivityAssessment",
                type: "int",
                nullable: true);
        }
    }
}
