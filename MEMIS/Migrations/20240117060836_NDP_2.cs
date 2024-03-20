using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class NDP_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FocusArea",
                table: "NDP");

            migrationBuilder.DropColumn(
                name: "FocusAreaObjective",
                table: "NDP");

            migrationBuilder.DropColumn(
                name: "FocusAreaStrategies",
                table: "NDP");

            migrationBuilder.DropColumn(
                name: "Intervention",
                table: "NDP");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FocusArea",
                table: "NDP",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FocusAreaObjective",
                table: "NDP",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FocusAreaStrategies",
                table: "NDP",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Intervention",
                table: "NDP",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
