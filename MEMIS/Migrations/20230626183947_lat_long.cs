using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class lat_long : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GPS",
                table: "Preinspection");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Preinspection",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Preinspection",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Preinspection");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Preinspection");

            migrationBuilder.AddColumn<string>(
                name: "GPS",
                table: "Preinspection",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
