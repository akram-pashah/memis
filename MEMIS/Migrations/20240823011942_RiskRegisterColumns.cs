using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class RiskRegisterColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ControlEffectiveness",
                table: "RiskRegister",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Effectiveness",
                table: "RiskRegister",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Recommendation",
                table: "RiskRegister",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ControlEffectiveness",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "Effectiveness",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "Recommendation",
                table: "RiskRegister");
        }
    }
}
