using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class RegionalAssessmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityAssessRegion_ActivityAssess_intAssess",
                table: "ActivityAssessRegion");

            migrationBuilder.AlterColumn<int>(
                name: "intAssess",
                table: "ActivityAssessRegion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ActivityAssessmentRegion",
                columns: table => new
                {
                    intRegionAssess = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    intAssess = table.Column<int>(type: "int", nullable: false),
                    intRegion = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    budgetAmount = table.Column<double>(type: "float", nullable: true),
                    Quarter = table.Column<int>(type: "int", nullable: true),
                    QTarget = table.Column<double>(type: "float", nullable: true),
                    QAchievement = table.Column<double>(type: "float", nullable: true),
                    QBudget = table.Column<double>(type: "float", nullable: true),
                    QAmtSpent = table.Column<double>(type: "float", nullable: true),
                    QRegJustification = table.Column<double>(type: "float", nullable: true),
                    ApprStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityAssessmentRegion", x => x.intRegionAssess);
                    table.ForeignKey(
                        name: "FK_ActivityAssessmentRegion_ActivityAssess_intAssess",
                        column: x => x.intAssess,
                        principalTable: "ActivityAssess",
                        principalColumn: "intAssess",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityAssessmentRegion_Region_intRegion",
                        column: x => x.intRegion,
                        principalTable: "Region",
                        principalColumn: "intRegion");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAssessmentRegion_intAssess",
                table: "ActivityAssessmentRegion",
                column: "intAssess");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAssessmentRegion_intRegion",
                table: "ActivityAssessmentRegion",
                column: "intRegion");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityAssessRegion_ActivityAssess_intAssess",
                table: "ActivityAssessRegion",
                column: "intAssess",
                principalTable: "ActivityAssess",
                principalColumn: "intAssess",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityAssessRegion_ActivityAssess_intAssess",
                table: "ActivityAssessRegion");

            migrationBuilder.DropTable(
                name: "ActivityAssessmentRegion");

            migrationBuilder.AlterColumn<int>(
                name: "intAssess",
                table: "ActivityAssessRegion",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityAssessRegion_ActivityAssess_intAssess",
                table: "ActivityAssessRegion",
                column: "intAssess",
                principalTable: "ActivityAssess",
                principalColumn: "intAssess");
        }
    }
}
