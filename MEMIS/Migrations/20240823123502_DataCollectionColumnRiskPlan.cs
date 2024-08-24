using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class DataCollectionColumnRiskPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataCollectionInstrumentMethods",
                table: "RiskTreatmentPlan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeansOfVerification",
                table: "RiskTreatmentPlan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsiblePersons",
                table: "RiskTreatmentPlan",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCollectionInstrumentMethods",
                table: "RiskTreatmentPlan");

            migrationBuilder.DropColumn(
                name: "MeansOfVerification",
                table: "RiskTreatmentPlan");

            migrationBuilder.DropColumn(
                name: "ResponsiblePersons",
                table: "RiskTreatmentPlan");
        }
    }
}
