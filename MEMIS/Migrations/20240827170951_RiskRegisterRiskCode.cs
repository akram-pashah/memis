using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class RiskRegisterRiskCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RiskCode",
                table: "RiskRegister",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_RiskRegister_RiskIdentification_RiskId",
            //    table: "RiskRegister",
            //    column: "RiskId",
            //    principalTable: "RiskIdentification",
            //    principalColumn: "RiskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskRegister_RiskIdentification_RiskId",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "RiskCode",
                table: "RiskRegister");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskRegister_DeptPlan_RiskId",
                table: "RiskRegister",
                column: "RiskId",
                principalTable: "DeptPlan",
                principalColumn: "intActivity");
        }
    }
}
