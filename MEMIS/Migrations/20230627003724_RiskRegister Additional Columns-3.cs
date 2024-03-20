using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class RiskRegisterAdditionalColumns3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RiskResidualConsequenceId",
                table: "RiskRegister",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RiskResidualLikelihoodId",
                table: "RiskRegister",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RiskResidualRank",
                table: "RiskRegister",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RiskResidualScore",
                table: "RiskRegister",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiskResidualConsequenceId",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "RiskResidualLikelihoodId",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "RiskResidualRank",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "RiskResidualScore",
                table: "RiskRegister");
        }
    }
}
