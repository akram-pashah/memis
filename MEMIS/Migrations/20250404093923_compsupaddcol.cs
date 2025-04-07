using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class compsupaddcol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassofDrugs",
                table: "ComplianceSupportSupervision",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseNo",
                table: "ComplianceSupportSupervision",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecordKeeping",
                table: "ComplianceSupportSupervision",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnRegisteredDrug",
                table: "ComplianceSupportSupervision",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassofDrugs",
                table: "ComplianceSupportSupervision");

            migrationBuilder.DropColumn(
                name: "LicenseNo",
                table: "ComplianceSupportSupervision");

            migrationBuilder.DropColumn(
                name: "RecordKeeping",
                table: "ComplianceSupportSupervision");

            migrationBuilder.DropColumn(
                name: "UnRegisteredDrug",
                table: "ComplianceSupportSupervision");
        }
    }
}
