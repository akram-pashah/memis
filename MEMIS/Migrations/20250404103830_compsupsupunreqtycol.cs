using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class compsupsupunreqtycol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Complaint_Product",
                table: "ComplianceSupportSupervision");

            migrationBuilder.DropColumn(
                name: "Followup_Comment",
                table: "ComplianceSupportSupervision");

            migrationBuilder.DropColumn(
                name: "Followup_Product",
                table: "ComplianceSupportSupervision");

            migrationBuilder.DropColumn(
                name: "Other_Activity",
                table: "ComplianceSupportSupervision");

            migrationBuilder.DropColumn(
                name: "PMSActivity",
                table: "ComplianceSupportSupervision");

            migrationBuilder.DropColumn(
                name: "Sample_Batch",
                table: "ComplianceSupportSupervision");

            migrationBuilder.RenameColumn(
                name: "Sample_ProductName",
                table: "ComplianceSupportSupervision",
                newName: "UnRegDrugQty");

            migrationBuilder.RenameColumn(
                name: "Sample_No",
                table: "ComplianceSupportSupervision",
                newName: "Action");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnRegDrugQty",
                table: "ComplianceSupportSupervision",
                newName: "Sample_ProductName");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "ComplianceSupportSupervision",
                newName: "Sample_No");

            migrationBuilder.AddColumn<string>(
                name: "Complaint_Product",
                table: "ComplianceSupportSupervision",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Followup_Comment",
                table: "ComplianceSupportSupervision",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Followup_Product",
                table: "ComplianceSupportSupervision",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Other_Activity",
                table: "ComplianceSupportSupervision",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PMSActivity",
                table: "ComplianceSupportSupervision",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sample_Batch",
                table: "ComplianceSupportSupervision",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
