using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class riskmultirecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskAdditionalMitigations",
                columns: table => new
                {
                    AdditionalMitigationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskAdditionalMitigations", x => x.AdditionalMitigationId);
                    table.ForeignKey(
                        name: "FK_RiskAdditionalMitigations_RiskIdentification_RiskId",
                        column: x => x.RiskId,
                        principalTable: "RiskIdentification",
                        principalColumn: "RiskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskExistMitigations",
                columns: table => new
                {
                    ExistMitigationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskExistMitigations", x => x.ExistMitigationId);
                    table.ForeignKey(
                        name: "FK_RiskExistMitigations_RiskIdentification_RiskId",
                        column: x => x.RiskId,
                        principalTable: "RiskIdentification",
                        principalColumn: "RiskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskOpportunitys",
                columns: table => new
                {
                    OpportunityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskOpportunitys", x => x.OpportunityId);
                    table.ForeignKey(
                        name: "FK_RiskOpportunitys_RiskIdentification_RiskId",
                        column: x => x.RiskId,
                        principalTable: "RiskIdentification",
                        principalColumn: "RiskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskWeaknesses",
                columns: table => new
                {
                    WeaknessId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskWeaknesses", x => x.WeaknessId);
                    table.ForeignKey(
                        name: "FK_RiskWeaknesses_RiskIdentification_RiskId",
                        column: x => x.RiskId,
                        principalTable: "RiskIdentification",
                        principalColumn: "RiskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskAdditionalMitigations_RiskId",
                table: "RiskAdditionalMitigations",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskExistMitigations_RiskId",
                table: "RiskExistMitigations",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskOpportunitys_RiskId",
                table: "RiskOpportunitys",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskWeaknesses_RiskId",
                table: "RiskWeaknesses",
                column: "RiskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskAdditionalMitigations");

            migrationBuilder.DropTable(
                name: "RiskExistMitigations");

            migrationBuilder.DropTable(
                name: "RiskOpportunitys");

            migrationBuilder.DropTable(
                name: "RiskWeaknesses");
        }
    }
}
