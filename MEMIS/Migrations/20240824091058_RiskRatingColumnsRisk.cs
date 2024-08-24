using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class RiskRatingColumnsRisk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RiskRatingCategory",
                table: "RiskRegister",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RiskRatingColor",
                table: "RiskRegister",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RiskRatingId",
                table: "RiskRegister",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiskRatingCategory",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "RiskRatingColor",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "RiskRatingId",
                table: "RiskRegister");
        }
    }
}
