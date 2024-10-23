using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class IncidentsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incident_QuarterlyRiskActions_QuarterlyRiskActionId",
                table: "Incident");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Incident",
                table: "Incident");

            migrationBuilder.RenameTable(
                name: "Incident",
                newName: "Incidents");

            migrationBuilder.RenameIndex(
                name: "IX_Incident_QuarterlyRiskActionId",
                table: "Incidents",
                newName: "IX_Incidents_QuarterlyRiskActionId");

            migrationBuilder.AlterColumn<long>(
                name: "QuarterlyRiskActionId",
                table: "Incidents",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<double>(
                name: "NoOfIncedents",
                table: "Incidents",
                type: "float",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Incidents",
                table: "Incidents",
                column: "IncidentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_QuarterlyRiskActions_QuarterlyRiskActionId",
                table: "Incidents",
                column: "QuarterlyRiskActionId",
                principalTable: "QuarterlyRiskActions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_QuarterlyRiskActions_QuarterlyRiskActionId",
                table: "Incidents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Incidents",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "NoOfIncedents",
                table: "Incidents");

            migrationBuilder.RenameTable(
                name: "Incidents",
                newName: "Incident");

            migrationBuilder.RenameIndex(
                name: "IX_Incidents_QuarterlyRiskActionId",
                table: "Incident",
                newName: "IX_Incident_QuarterlyRiskActionId");

            migrationBuilder.AlterColumn<long>(
                name: "QuarterlyRiskActionId",
                table: "Incident",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Incident",
                table: "Incident",
                column: "IncidentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_QuarterlyRiskActions_QuarterlyRiskActionId",
                table: "Incident",
                column: "QuarterlyRiskActionId",
                principalTable: "QuarterlyRiskActions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
