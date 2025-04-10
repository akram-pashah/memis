using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class kpimasteroutcome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "intOutcome",
                table: "KPI",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KPI_intOutcome",
                table: "KPI",
                column: "intOutcome");

            migrationBuilder.AddForeignKey(
                name: "FK_KPI_Outcome_intOutcome",
                table: "KPI",
                column: "intOutcome",
                principalTable: "Outcome",
                principalColumn: "intOutcome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KPI_Outcome_intOutcome",
                table: "KPI");

            migrationBuilder.DropIndex(
                name: "IX_KPI_intOutcome",
                table: "KPI");

            migrationBuilder.DropColumn(
                name: "intOutcome",
                table: "KPI");
        }
    }
}
