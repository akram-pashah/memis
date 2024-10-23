using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class Incidents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Incident",
                columns: table => new
                {
                    IncidentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskId = table.Column<int>(type: "int", nullable: false),
                    QuarterlyRiskActionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incident", x => x.IncidentId);
                    table.ForeignKey(
                        name: "FK_Incident_QuarterlyRiskActions_QuarterlyRiskActionId",
                        column: x => x.QuarterlyRiskActionId,
                        principalTable: "QuarterlyRiskActions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Incident_RiskIdentification_RiskId",
                        column: x => x.RiskId,
                        principalTable: "RiskIdentification",
                        principalColumn: "RiskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incident_QuarterlyRiskActionId",
                table: "Incident",
                column: "QuarterlyRiskActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_RiskId",
                table: "Incident",
                column: "RiskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incident");
        }
    }
}
