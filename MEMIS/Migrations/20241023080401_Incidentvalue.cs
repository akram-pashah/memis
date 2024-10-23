using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class Incidentvalue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incident_QuarterlyRiskActions_QuarterlyRiskActionId",
                table: "Incident");

            migrationBuilder.DropForeignKey(
                name: "FK_Incident_RiskIdentification_RiskId",
                table: "Incident");

            migrationBuilder.DropIndex(
                name: "IX_Incident_RiskId",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "RiskId",
                table: "Incident");

            migrationBuilder.AlterColumn<long>(
                name: "QuarterlyRiskActionId",
                table: "Incident",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_QuarterlyRiskActions_QuarterlyRiskActionId",
                table: "Incident",
                column: "QuarterlyRiskActionId",
                principalTable: "QuarterlyRiskActions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incident_QuarterlyRiskActions_QuarterlyRiskActionId",
                table: "Incident");

            migrationBuilder.AlterColumn<long>(
                name: "QuarterlyRiskActionId",
                table: "Incident",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "RiskId",
                table: "Incident",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Incident_RiskId",
                table: "Incident",
                column: "RiskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_QuarterlyRiskActions_QuarterlyRiskActionId",
                table: "Incident",
                column: "QuarterlyRiskActionId",
                principalTable: "QuarterlyRiskActions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_RiskIdentification_RiskId",
                table: "Incident",
                column: "RiskId",
                principalTable: "RiskIdentification",
                principalColumn: "RiskId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
