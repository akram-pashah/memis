using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class Cause : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cause",
                columns: table => new
                {
                    intCause = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CauseDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskId = table.Column<int>(type: "int", nullable: false),
                    intRisk = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cause", x => x.intCause);
                    table.ForeignKey(
                        name: "FK_Cause_T_RISKIDENTIFICATION_intRisk",
                        column: x => x.intRisk,
                        principalTable: "T_RISKIDENTIFICATION",
                        principalColumn: "intRisk",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cause_intRisk",
                table: "Cause",
                column: "intRisk");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cause");
        }
    }
}
