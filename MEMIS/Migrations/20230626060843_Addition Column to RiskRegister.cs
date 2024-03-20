using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class AdditionColumntoRiskRegister : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "riskTolerence",
                table: "RiskRegister",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "riskTolerenceJustification",
                table: "RiskRegister",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "riskTolerence",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "riskTolerenceJustification",
                table: "RiskRegister");
        }
    }
}
