using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class QuarterlyRiskActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuarterlyRiskActions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quarter = table.Column<int>(type: "int", nullable: false),
                    TreatmentPlanId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoOfIncedents = table.Column<double>(type: "float", nullable: true),
                    RiskDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncidentValue = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuarterlyRiskActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuarterlyRiskActions_RiskTreatmentPlan_TreatmentPlanId",
                        column: x => x.TreatmentPlanId,
                        principalTable: "RiskTreatmentPlan",
                        principalColumn: "TreatmentPlanId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuarterlyRiskActions_TreatmentPlanId",
                table: "QuarterlyRiskActions",
                column: "TreatmentPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuarterlyRiskActions");
        }
    }
}
