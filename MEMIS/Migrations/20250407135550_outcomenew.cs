using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class outcomenew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KPI_StrategicPlan_StrategicObjective",
                table: "KPI");

            migrationBuilder.CreateTable(
                name: "Outcome",
                columns: table => new
                {
                    intOutcome = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutcomeCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OutcomeName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    intObjective = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outcome", x => x.intOutcome);
                    table.ForeignKey(
                        name: "FK_Outcome_StrategicObjective_intObjective",
                        column: x => x.intObjective,
                        principalTable: "StrategicObjective",
                        principalColumn: "intObjective");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Outcome_intObjective",
                table: "Outcome",
                column: "intObjective");

            migrationBuilder.AddForeignKey(
                name: "FK_KPI_StrategicObjective_StrategicObjective",
                table: "KPI",
                column: "StrategicObjective",
                principalTable: "StrategicObjective",
                principalColumn: "intObjective");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KPI_StrategicObjective_StrategicObjective",
                table: "KPI");

            migrationBuilder.DropTable(
                name: "Outcome");

            migrationBuilder.AddForeignKey(
                name: "FK_KPI_StrategicPlan_StrategicObjective",
                table: "KPI",
                column: "StrategicObjective",
                principalTable: "StrategicPlan",
                principalColumn: "Id");
        }
    }
}
