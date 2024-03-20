using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class add_approvalStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatusDirector",
                table: "SDTAssessment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatusHOD",
                table: "SDTAssessment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatusOfficer",
                table: "SDTAssessment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatusDirector",
                table: "SDTAssessment");

            migrationBuilder.DropColumn(
                name: "ApprovalStatusHOD",
                table: "SDTAssessment");

            migrationBuilder.DropColumn(
                name: "ApprovalStatusOfficer",
                table: "SDTAssessment");
        }
    }
}
