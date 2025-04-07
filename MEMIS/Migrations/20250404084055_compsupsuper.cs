using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class compsupsuper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LicenseNo",
                table: "ComplianceSupportSupervision",
                newName: "Other_CategoryPremise");

            migrationBuilder.AddColumn<int>(
                name: "Unlicensed",
                table: "ComplianceSupportSupervision",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unlicensed",
                table: "ComplianceSupportSupervision");

            migrationBuilder.RenameColumn(
                name: "Other_CategoryPremise",
                table: "ComplianceSupportSupervision",
                newName: "LicenseNo");
        }
    }
}
