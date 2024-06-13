using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class AddRiskTreatmentPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskTreatmentPlan",
                columns: table => new
                {
                    TreatmentPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiskRefID = table.Column<int>(type: "int", nullable: false),
                    TreatmentAction = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IndicatorDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Baseline = table.Column<long>(type: "bigint", nullable: false),
                    CumulativeTarget = table.Column<long>(type: "bigint", nullable: false),
                    FrequencyOfReporting = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskTreatmentPlan", x => x.TreatmentPlanId);
                    table.ForeignKey(
                        name: "FK_RiskTreatmentPlan_RiskRegister_RiskRefID",
                        column: x => x.RiskRefID,
                        principalTable: "RiskRegister",
                        principalColumn: "RiskRefID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskTreatmentPlan_RiskRefID",
                table: "RiskTreatmentPlan",
                column: "RiskRefID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskTreatmentPlan");
        }
    }
}
