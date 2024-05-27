using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class AddingChildTablesForRI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Events_RiskIdentification_RiskId",
                        column: x => x.RiskId,
                        principalTable: "RiskIdentification",
                        principalColumn: "RiskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskCauses",
                columns: table => new
                {
                    RiskCauseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskCauses", x => x.RiskCauseId);
                    table.ForeignKey(
                        name: "FK_RiskCauses_RiskIdentification_RiskId",
                        column: x => x.RiskId,
                        principalTable: "RiskIdentification",
                        principalColumn: "RiskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskConsequenceDetails",
                columns: table => new
                {
                    RiskConsequenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskConsequenceDetails", x => x.RiskConsequenceId);
                    table.ForeignKey(
                        name: "FK_RiskConsequenceDetails_RiskIdentification_RiskId",
                        column: x => x.RiskId,
                        principalTable: "RiskIdentification",
                        principalColumn: "RiskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskSources",
                columns: table => new
                {
                    RiskSourceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskSources", x => x.RiskSourceId);
                    table.ForeignKey(
                        name: "FK_RiskSources_RiskIdentification_RiskId",
                        column: x => x.RiskId,
                        principalTable: "RiskIdentification",
                        principalColumn: "RiskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_RiskId",
                table: "Events",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskCauses_RiskId",
                table: "RiskCauses",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskConsequenceDetails_RiskId",
                table: "RiskConsequenceDetails",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskSources_RiskId",
                table: "RiskSources",
                column: "RiskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "RiskCauses");

            migrationBuilder.DropTable(
                name: "RiskConsequenceDetails");

            migrationBuilder.DropTable(
                name: "RiskSources");
        }
    }
}
