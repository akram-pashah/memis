using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class YearlyTarget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KPITarget",
                table: "M_FYEAR",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KPITarget",
                table: "M_FYEAR");
        }
    }
}
