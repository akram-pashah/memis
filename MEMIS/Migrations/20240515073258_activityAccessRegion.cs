using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class activityAccessRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityAssessRegion",
                columns: table => new
                {
                    intRegionAssess = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    intAssess = table.Column<int>(type: "int", nullable: true),
                    intRegion = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    budgetAmount = table.Column<double>(type: "float", nullable: true),
                    Quarter = table.Column<int>(type: "int", nullable: true),
                    QTarget = table.Column<double>(type: "float", nullable: true),
                    QBudget = table.Column<double>(type: "float", nullable: true),
                    ApprStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityAssessRegion", x => x.intRegionAssess);
                    table.ForeignKey(
                        name: "FK_ActivityAssessRegion_ActivityAssess_intAssess",
                        column: x => x.intAssess,
                        principalTable: "ActivityAssess",
                        principalColumn: "intAssess");
                    table.ForeignKey(
                        name: "FK_ActivityAssessRegion_Region_intRegion",
                        column: x => x.intRegion,
                        principalTable: "Region",
                        principalColumn: "intRegion");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAssessRegion_intAssess",
                table: "ActivityAssessRegion",
                column: "intAssess");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAssessRegion_intRegion",
                table: "ActivityAssessRegion",
                column: "intRegion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityAssessRegion");
        }
    }
}
