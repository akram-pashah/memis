using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class giscsp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassofDrugs",
                table: "ComplianceSupportSupervision");

            migrationBuilder.DropColumn(
                name: "CompAction",
                table: "ComplianceSupportSupervision");

            migrationBuilder.RenameColumn(
                name: "UnregisteredDrugs",
                table: "ComplianceSupportSupervision",
                newName: "Sample_No");

            migrationBuilder.RenameColumn(
                name: "RecordKeeping",
                table: "ComplianceSupportSupervision",
                newName: "PMSActivity");

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

            migrationBuilder.AddColumn<string>(
                name: "Sample_Batch",
                table: "ComplianceSupportSupervision",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sample_ProductName",
                table: "ComplianceSupportSupervision",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Sample_Batch",
                table: "ComplianceSupportSupervision");

            migrationBuilder.DropColumn(
                name: "Sample_ProductName",
                table: "ComplianceSupportSupervision");

            migrationBuilder.RenameColumn(
                name: "Sample_No",
                table: "ComplianceSupportSupervision",
                newName: "UnregisteredDrugs");

            migrationBuilder.RenameColumn(
                name: "PMSActivity",
                table: "ComplianceSupportSupervision",
                newName: "RecordKeeping");

            migrationBuilder.AddColumn<int>(
                name: "ClassofDrugs",
                table: "ComplianceSupportSupervision",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompAction",
                table: "ComplianceSupportSupervision",
                type: "int",
                nullable: true);
        }
    }
}
