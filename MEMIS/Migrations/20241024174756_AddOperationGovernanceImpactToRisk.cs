using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class AddOperationGovernanceImpactToRisk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_RiskRegister_RiskRegisterRiskRefID",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_RiskRegisterRiskRefID",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "SampleSize",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "RiskRegisterRiskRefID",
                table: "Incidents");

            migrationBuilder.AddColumn<double>(
                name: "OperationGovernanceImpact",
                table: "RiskRegister",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationGovernanceImpact",
                table: "RiskRegister");

            migrationBuilder.AddColumn<int>(
                name: "SampleSize",
                table: "RiskRegister",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RiskRegisterRiskRefID",
                table: "Incidents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_RiskRegisterRiskRefID",
                table: "Incidents",
                column: "RiskRegisterRiskRefID");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_RiskRegister_RiskRegisterRiskRefID",
                table: "Incidents",
                column: "RiskRegisterRiskRefID",
                principalTable: "RiskRegister",
                principalColumn: "RiskRefID");
        }
    }
}
