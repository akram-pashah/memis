using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class NDP_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProgrammeIntervention",
                table: "NDP",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubProgramme",
                table: "NDP",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubProgrammeObjective",
                table: "NDP",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProgrammeIntervention",
                table: "NDP");

            migrationBuilder.DropColumn(
                name: "SubProgramme",
                table: "NDP");

            migrationBuilder.DropColumn(
                name: "SubProgrammeObjective",
                table: "NDP");
        }
    }
}
